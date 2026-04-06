namespace Winfocus.LMS.Application.Services
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Winfocus.LMS.Application.DTOs;
    using Winfocus.LMS.Application.DTOs.Common;
    using Winfocus.LMS.Application.DTOs.Fees;
    using Winfocus.LMS.Application.Interfaces;
    using Winfocus.LMS.Domain.Entities;
    using Winfocus.LMS.Domain.Enums;
    using Winfocus.LMS.Domain.Extensions;

    /// <summary>
    /// FeeService.
    /// </summary>
    /// <seealso cref="Winfocus.LMS.Application.Interfaces.IFeeService" />
    public sealed class FeeService : IFeeService
    {
        private readonly IFeeRepository _repo;
        private readonly ILogger<FeeService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="FeeService"/> class.
        /// </summary>
        /// <param name="repo">The repo.</param>
        /// <param name="logger">The logger.</param>
        public FeeService(IFeeRepository repo, ILogger<FeeService> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<CommonResponse<AdminStudentFeePageDto>> GetAdminStudentFeePageAsync(
    Guid studentId)
        {
            try
            {
                var student = await _repo.GetStudentWithCoursesAsync(studentId);
                if (student == null)
                {
                    return CommonResponse<AdminStudentFeePageDto>.FailureResponse(
                        "Student not found.");
                }

                var gradeId = student.AcademicDetails?.GradeId ?? Guid.Empty;
                var courses = await _repo.GetCoursesByGradeAsync(gradeId);
                var assigned = await _repo.GetStudentCourseDiscountsAsync(studentId);

                // Only ONE active discount per course
                var assignedByCourse = assigned
                    .GroupBy(a => a.CourseId)
                    .ToDictionary(g => g.Key, g => g.First());

                var courseBlocks = new List<CourseDiscountBlockDto>();

                foreach (var course in courses)
                {
                    var allPlanDiscounts = course.FeePlans
                        .Where(fp => fp.IsActive)
                        .SelectMany(fp => fp.Discounts.Where(d => d.IsActive))
                        .GroupBy(d => d.DiscountName)
                        .Select(g => g.First())
                        .ToList();

                    assignedByCourse.TryGetValue(course.Id, out var currentAssigned);

                    var baseYearlyFee = course.FeePlans
                        .Where(fp => fp.IsActive)
                        .Select(fp => fp.TuitionFee)
                        .FirstOrDefault();

                    // Calculate with single discount
                    var discountPercent = currentAssigned?.DiscountPercent ?? 0;
                    var feeAfter = baseYearlyFee * (1 - Math.Min(discountPercent, 100) / 100m);

                    courseBlocks.Add(new CourseDiscountBlockDto
                    {
                        CourseId = course.Id,
                        CourseName = course.Name,
                        BaseYearlyFee = baseYearlyFee,
                        AvailableDiscounts = allPlanDiscounts.Select(d =>
                            new AvailableDiscountDto
                            {
                                FeePlanDiscountId = d.Id,
                                DiscountName = d.DiscountName,
                                DiscountPercent = d.DiscountPercent,
                                DiscountAmount = baseYearlyFee * d.DiscountPercent / 100m,
                                IsAssigned = currentAssigned != null
                                    && !currentAssigned.IsManual
                                    && currentAssigned.DiscountName.Equals(
                                        d.DiscountName,
                                        StringComparison.OrdinalIgnoreCase),
                            }).ToList(),
                        AssignedDiscount = currentAssigned == null
                            ? null
                            : new AssignedDiscountDto
                            {
                                DiscountName = currentAssigned.DiscountName,
                                DiscountPercent = currentAssigned.DiscountPercent,
                                IsManual = currentAssigned.IsManual
                            },
                        CalculatedFeeAfterDiscount = Math.Max(feeAfter, 0),
                    });
                }

                return CommonResponse<AdminStudentFeePageDto>.SuccessResponse(
                    "Admin fee page loaded.",
                    new AdminStudentFeePageDto
                    {
                        StudentId = studentId,
                        StudentName = student.StudentPersonalDetails?.FullName ?? "Unknown",
                        RegistrationNumber = student.RegistrationNumber ?? "",
                        GradeName = student.AcademicDetails?.Grade?.Name ?? "N/A",
                        SyllabusName = student.AcademicDetails?.Grade?.Syllabus?.Name ?? "N/A",
                        CourseDiscounts = courseBlocks,
                        TotalPayable = courseBlocks.Sum(c => c.CalculatedFeeAfterDiscount)
                    });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading admin fee page for {StudentId}", studentId);
                return CommonResponse<AdminStudentFeePageDto>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        /// <inheritdoc/>
        public async Task<CommonResponse<bool>> AssignDiscountsAsync(
                                                        AssignDiscountsRequestDto request)
        {
            try
            {
                // ── Validation: only ONE discount allowed ──
                if (request.SelectedDiscountId.HasValue && request.ManualDiscount != null)
                {
                    return CommonResponse<bool>.FailureResponse(
                        "Only one discount can be applied at a time. " +
                        "Choose either a plan discount OR a manual discount, not both.");
                }

                if (!request.SelectedDiscountId.HasValue && request.ManualDiscount == null)
                {
                    return CommonResponse<bool>.FailureResponse(
                        "Please select a plan discount or provide a manual discount.");
                }

                var student = await _repo.GetStudentWithCoursesAsync(request.StudentId);
                if (student == null)
                {
                    return CommonResponse<bool>.FailureResponse("Student not found.");
                }

                await _repo.RemoveStudentCourseDiscountsAsync(
                    request.StudentId, request.CourseId);

                StudentCourseDiscount newDiscount;

                if (request.SelectedDiscountId.HasValue)
                {
                    // Plan discount selected
                    var planDiscounts = await _repo.GetFeePlanDiscountsByIdsAsync(
                        new[] { request.SelectedDiscountId.Value });

                    if (!planDiscounts.Any())
                    {
                        return CommonResponse<bool>.FailureResponse(
                            "Selected discount not found.");
                    }

                    var planDiscount = planDiscounts.First();

                    if (planDiscount.FeePlan.CourseId != request.CourseId)
                    {
                        return CommonResponse<bool>.FailureResponse(
                            "Discount does not belong to the specified course.");
                    }

                    newDiscount = StudentCourseDiscount.FromPlanDiscount(
                        request.StudentId, request.CourseId,
                        planDiscount, request.UserId);
                }
                else
                {
                    // Manual discount
                    newDiscount = StudentCourseDiscount.Manual(
                        request.StudentId,
                        request.CourseId,
                        request.ManualDiscount!.DiscountName,
                        request.ManualDiscount.DiscountPercent,
                        request.UserId);
                }

                await _repo.AddStudentCourseDiscountsAsync(new[] { newDiscount });
                await _repo.SaveChangesAsync();

                _logger.LogInformation(
                    "Discount assigned: Student={StudentId}, Course={CourseId}, " +
                    "Discount={Name} ({Percent}%), IsManual={IsManual}",
                    request.StudentId, request.CourseId,
                    newDiscount.DiscountName, newDiscount.DiscountPercent,
                    newDiscount.IsManual);

                return CommonResponse<bool>.SuccessResponse(
                    "Discount assigned successfully.", true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error assigning discount");
                return CommonResponse<bool>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        /// <inheritdoc/>
        public async Task<CommonResponse<bool>> RemoveDiscountsAsync(
            Guid studentId, Guid courseId)
        {
            try
            {
                await _repo.RemoveStudentCourseDiscountsAsync(studentId, courseId);
                await _repo.SaveChangesAsync();
                return CommonResponse<bool>.SuccessResponse(
                    "Discounts removed.", true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing discounts");
                return CommonResponse<bool>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        /// <inheritdoc/>
        public async Task<CommonResponse<StudentFeePageDto>> GetStudentFeePageAsync(
    Guid studentId)
        {
            try
            {
                var student = await _repo.GetStudentWithCoursesAsync(studentId);
                if (student == null)
                {
                    return CommonResponse<StudentFeePageDto>.FailureResponse(
                        "Student not found.");
                }

                var gradeId = student.AcademicDetails?.GradeId ?? Guid.Empty;
                var courses = await _repo.GetCoursesByGradeAsync(gradeId);
                var assigned = await _repo.GetStudentCourseDiscountsAsync(studentId);

                // ONE discount per course
                var assignedByCourse = assigned
                    .GroupBy(a => a.CourseId)
                    .ToDictionary(g => g.Key, g => g.First());

                var existingSelections = await _repo.GetSelectionsByStudentAsync(studentId);
                var selectedPlanIds = existingSelections
                    .Select(s => s.FeePlanId).ToHashSet();

                var feeListings = new List<FeeListingRowDto>();

                foreach (var course in courses)
                {
                    // Get the single assigned discount for this course
                    assignedByCourse.TryGetValue(course.Id, out var courseDiscount);

                    foreach (var plan in course.FeePlans.Where(fp => fp.IsActive))
                    {
                        decimal appliedDiscountPercent = 0;
                        var appliedDiscounts = new List<DiscountBadgeDto>();

                        if (courseDiscount != null)
                        {
                            if (courseDiscount.IsManual)
                            {
                                // Manual discount always applies
                                appliedDiscountPercent = courseDiscount.DiscountPercent;
                                appliedDiscounts.Add(new DiscountBadgeDto
                                {
                                    Name = courseDiscount.DiscountName,
                                    Percent = courseDiscount.DiscountPercent,
                                });
                            }
                            else
                            {
                                // Plan discount: verify it exists in THIS plan
                                var matchInPlan = plan.Discounts.FirstOrDefault(
                                    d => d.IsActive &&
                                         d.DiscountName.Equals(
                                             courseDiscount.DiscountName,
                                             StringComparison.OrdinalIgnoreCase));

                                if (matchInPlan != null)
                                {
                                    appliedDiscountPercent = matchInPlan.DiscountPercent;
                                    appliedDiscounts.Add(new DiscountBadgeDto
                                    {
                                        Name = matchInPlan.DiscountName,
                                        Percent = matchInPlan.DiscountPercent,
                                    });
                                }
                            }
                        }

                        appliedDiscountPercent = Math.Min(appliedDiscountPercent, 100);

                        var yearlyFee = plan.TuitionFee;
                        var duration = plan.DurationinYears;
                        var totalBefore = yearlyFee * duration;
                        var discountAmount = totalBefore * appliedDiscountPercent / 100m;
                        var feeAfter = Math.Max(totalBefore - discountAmount, 0);

                        var installmentCount = plan.PaymentType.GetTotalInstallments(duration);

                        var perInstallment = installmentCount > 0
                            ? Math.Round(feeAfter / installmentCount, 2)
                            : feeAfter;

                        feeListings.Add(new FeeListingRowDto
                        {
                            FeePlanId = plan.Id,
                            CourseId = course.Id,
                            CourseName = course.Name,
                            YearlyFee = yearlyFee,
                            PaymentType = plan.PaymentType,
                            DurationInYears = duration,
                            TotalDiscountPercent = appliedDiscountPercent,
                            TotalBeforeDiscount = totalBefore,
                            FeeAfterDiscount = feeAfter,
                            InstallmentCount = installmentCount,
                            PerInstallment = perInstallment,
                            IsSelected = selectedPlanIds.Contains(plan.Id),
                            AppliedDiscounts = appliedDiscounts,
                        });
                    }
                }

                feeListings = feeListings
                    .OrderBy(f => f.CourseName)
                    .ThenBy(f => f.PaymentType)
                    .ThenBy(f => f.DurationInYears)
                    .ToList();

                var selectedPlanId = existingSelections
                    .OrderByDescending(s => s.CreatedAt)
                    .Select(s => (Guid?)s.FeePlanId)
                    .FirstOrDefault();

                return CommonResponse<StudentFeePageDto>.SuccessResponse(
                    "Fee page loaded.",
                    new StudentFeePageDto
                    {
                        StudentId = studentId,
                        StudentName = student.StudentPersonalDetails?.FullName ?? "Unknown",
                        RegistrationNumber = student.RegistrationNumber ?? string.Empty,
                        GradeId = gradeId,
                        GradeName = student.AcademicDetails?.Grade?.Name ?? "N/A",
                        SyllabusId = student.AcademicDetails?.Grade?.SyllabusId ?? Guid.Empty,
                        SyllabusName = student.AcademicDetails?.Grade?.Syllabus?.Name ?? "N/A",
                        FeeListings = feeListings,
                        SelectedFeePlanId = selectedPlanId,
                    });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading student fee page for {StudentId}", studentId);
                return CommonResponse<StudentFeePageDto>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        /// <inheritdoc/>
        public async Task<CommonResponse<ConfirmFeeResponseDto>> ConfirmFeeSelectionAsync(
                    ConfirmFeeRequestDto request)
        {
            try
            {
                if (!request.DeclarationAccepted)
                {
                    return CommonResponse<ConfirmFeeResponseDto>.FailureResponse(
                        "You must accept the declaration to proceed.");
                }

                var student = await _repo.GetStudentWithCoursesAsync(request.StudentId);
                if (student == null)
                {
                    return CommonResponse<ConfirmFeeResponseDto>.FailureResponse(
                        "Student not found.");
                }

                var plan = await _repo.GetFeePlanWithDiscountsAsync(request.FeePlanId);
                if (plan == null)
                {
                    return CommonResponse<ConfirmFeeResponseDto>.FailureResponse(
                        "Fee plan not found.");
                }

                var exists = await _repo.HasConfirmedSelectionForCourseAsync(
                    request.StudentId, plan.CourseId);
                if (exists)
                {
                    return CommonResponse<ConfirmFeeResponseDto>.FailureResponse(
                        "A fee plan is already confirmed for this course. Contact admin.");
                }

                // Get the ONE assigned discount for this course
                var courseDiscounts = await _repo.GetStudentCourseDiscountsAsync(
                    request.StudentId, plan.CourseId);
                var assignedDiscount = courseDiscounts.FirstOrDefault();

                // Resolve the applied discount
                string? discountName = null;
                decimal discountPercent = 0;
                bool isManual = false;

                if (assignedDiscount != null)
                {
                    if (assignedDiscount.IsManual)
                    {
                        discountName = assignedDiscount.DiscountName;
                        discountPercent = assignedDiscount.DiscountPercent;
                        isManual = true;
                    }
                    else
                    {
                        // Verify discount exists in THIS plan
                        var matchInPlan = plan.Discounts.FirstOrDefault(
                            d => d.IsActive &&
                                 d.DiscountName.Equals(
                                     assignedDiscount.DiscountName,
                                     StringComparison.OrdinalIgnoreCase));

                        if (matchInPlan != null)
                        {
                            discountName = matchInPlan.DiscountName;
                            discountPercent = matchInPlan.DiscountPercent;
                            isManual = false;
                        }
                    }
                }

                discountPercent = Math.Min(discountPercent, 100);

                // Calculate
                var yearlyFee = plan.TuitionFee;
                var duration = plan.DurationinYears;
                var totalBefore = yearlyFee * duration;
                var totalDiscountAmount = totalBefore * discountPercent / 100m;
                var finalAmount = Math.Max(totalBefore - totalDiscountAmount, 0);

                var installmentCount = plan.PaymentType.GetTotalInstallments(duration);

                // Create selection
                var selection = new StudentFeeSelection(
                    request.StudentId,
                    plan.CourseId,
                    plan.Id,
                    yearlyFee,
                    duration,
                    totalBefore,
                    discountPercent,
                    totalDiscountAmount,
                    finalAmount,
                    plan.PaymentType,
                    installmentCount,
                    request.StartDate,
                    request.EndDate);

                selection.CreatedBy = request.StudentId;

                await _repo.AddStudentFeeSelectionAsync(selection);
                await _repo.SaveChangesAsync();

                // Create single discount snapshot (if any)
                var appliedDiscountSnapshots = new List<AppliedDiscountSnapshotDto>();

                if (discountName != null && discountPercent > 0)
                {
                    var snapshot = new StudentFeeDiscount(
                        selection.Id,
                        discountName,
                        discountPercent,
                        totalDiscountAmount,
                        isManual);
                    selection.AppliedDiscounts.Add(snapshot);

                    appliedDiscountSnapshots.Add(new AppliedDiscountSnapshotDto
                    {
                        Name = discountName,
                        Percent = discountPercent,
                        Amount = totalDiscountAmount,
                        IsManual = isManual,
                    });
                }

                // Generate installments
                var perInstallment = installmentCount > 0
                    ? Math.Floor(finalAmount / installmentCount * 100) / 100
                    : finalAmount;

                var remainder = finalAmount - (perInstallment * installmentCount);
                var monthsBetween = plan.PaymentType.GetMonthsBetween();

                for (int i = 1; i <= installmentCount; i++)
                {
                    var dueDate = i == 1
                        ? request.StartDate
                        : request.StartDate.AddMonths(monthsBetween * (i - 1));

                    var dueAmount = i == installmentCount
                        ? perInstallment + remainder
                        : perInstallment;

                    var installment = new StudentInstallment(
                        selection.Id, i, dueAmount, dueDate);
                    selection.Installments.Add(installment);
                }

                await _repo.SaveChangesAsync();

                _logger.LogInformation(
                    "Fee confirmed: SelectionId={Id}, Discount={Name} ({Percent}%), Final={Amount}",
                    selection.Id, discountName ?? "None", discountPercent, finalAmount);

                var response = new ConfirmFeeResponseDto
                {
                    SelectionId = selection.Id,
                    CourseName = plan.Course?.Name ?? "",
                    PlanName = plan.PlanName,
                    YearlyFee = yearlyFee,
                    DurationYears = duration,
                    TotalBeforeDiscount = totalBefore,
                    TotalDiscountPercent = discountPercent,
                    TotalDiscountAmount = totalDiscountAmount,
                    FinalAmount = finalAmount,
                    PaymentType = plan.PaymentType,
                    TotalInstallments = installmentCount,
                    Status = selection.Status,
                    AppliedDiscounts = appliedDiscountSnapshots,
                    Installments = selection.Installments.Select(MapInstallment).ToList(),
                };

                return CommonResponse<ConfirmFeeResponseDto>.SuccessResponse(
                    "Fee selection confirmed successfully.", response);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error confirming fee for Student={StudentId}",
                    request.StudentId);
                return CommonResponse<ConfirmFeeResponseDto>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        /// <inheritdoc/>
        public async Task<CommonResponse<List<InstallmentScheduleDto>>> GetInstallmentsAsync(
            Guid selectionId)
        {
            try
            {
                var installments = await _repo.GetInstallmentsBySelectionAsync(selectionId);
                if (!installments.Any())
                {
                    return CommonResponse<List<InstallmentScheduleDto>>.FailureResponse(
                        "No installments found.");
                }

                return CommonResponse<List<InstallmentScheduleDto>>.SuccessResponse(
                    "Installments loaded.",
                    installments.Select(MapInstallment).ToList());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading installments for {SelectionId}", selectionId);
                return CommonResponse<List<InstallmentScheduleDto>>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        /// <inheritdoc/>
        public async Task<CommonResponse<InstallmentScheduleDto>> RecordPaymentAsync(
            Guid installmentId, RecordPaymentRequestDto request)
        {
            try
            {
                var installment = await _repo.GetInstallmentByIdAsync(installmentId);
                if (installment == null)
                {
                    return CommonResponse<InstallmentScheduleDto>.FailureResponse(
                        "Installment not found.");
                }

                installment.RecordPayment(
                    request.AmountPaid, request.PaymentDate, request.Remarks);

                // Update parent selection status
                var selection = await _repo.GetSelectionWithDetailsAsync(
                    installment.StudentFeeSelectionId);

                selection?.RefreshStatus();

                await _repo.SaveChangesAsync();

                _logger.LogInformation(
                    "Payment recorded: InstallmentId={Id}, Amount={Amount}",
                    installmentId,
                    request.AmountPaid);

                return CommonResponse<InstallmentScheduleDto>.SuccessResponse(
                    "Payment recorded successfully.",
                    MapInstallment(installment));
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex, 
                    "Error recording payment for {InstallmentId}",
                    installmentId);
                return CommonResponse<InstallmentScheduleDto>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        /// <inheritdoc/>
        public async Task<CommonResponse<PaymentSummaryDto>> GetPaymentSummaryAsync(
            Guid studentId)
        {
            try
            {
                var student = await _repo.GetStudentWithCoursesAsync(studentId);
                if (student == null)
                {
                    return CommonResponse<PaymentSummaryDto>.FailureResponse(
                        "Student not found.");
                }

                var selections = await _repo.GetSelectionsByStudentAsync(studentId);
                var selectionDtos = new List<SelectionPaymentDto>();

                foreach (var sel in selections)
                {
                    var installments = await _repo.GetInstallmentsBySelectionAsync(sel.Id);

                    var totalPaid = installments.Sum(i => i.PaidAmount);
                    var totalRemaining = sel.FinalAmount - totalPaid;

                    var nextDue = installments
                        .Where(i => i.Status != InstallmentStatus.Paid)
                        .OrderBy(i => i.DueDate)
                        .FirstOrDefault();

                    selectionDtos.Add(new SelectionPaymentDto
                    {
                        SelectionId = sel.Id,
                        CourseName = sel.Course?.Name ?? "",
                        PlanName = sel.FeePlan?.PlanName ?? "",
                        PaymentType = sel.PaymentType,
                        TotalFee = sel.FinalAmount,
                        TotalPaid = totalPaid,
                        TotalRemaining = Math.Max(totalRemaining, 0),
                        Status = sel.Status,
                        NextDueDate = nextDue?.DueDate,
                        NextDueAmount = nextDue?.BalanceAmount ?? 0,
                        Installments = installments.Select(MapInstallment).ToList(),
                    });
                }

                return CommonResponse<PaymentSummaryDto>.SuccessResponse(
                    "Payment summary loaded.",
                    new PaymentSummaryDto
                    {
                        StudentId = studentId,
                        StudentName = student.StudentPersonalDetails?.FullName ?? "Unknown",
                        Selections = selectionDtos,
                        GrandTotal = selectionDtos.Sum(s => s.TotalFee),
                        GrandPaid = selectionDtos.Sum(s => s.TotalPaid),
                        GrandRemaining = selectionDtos.Sum(s => s.TotalRemaining),
                    });
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error loading payment summary for {StudentId}",
                    studentId);
                return CommonResponse<PaymentSummaryDto>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        /// <inheritdoc/>
        public async Task<CommonResponse<PagedResult<StudentFeeSelectionListDto>>>
            GetSelectionsFilteredAsync(PagedRequest request)
        {
            try
            {
                var query = _repo.QuerySelections();

                if (request.Active.HasValue)
                {
                    query = query.Where(x => x.IsActive == request.Active.Value);
                }

                if (request.StartDate.HasValue)
                {
                    query = query.Where(x => x.CreatedAt >= request.StartDate.Value);
                }

                if (request.EndDate.HasValue)
                {
                    query = query.Where(x => x.CreatedAt <= request.EndDate.Value);
                }

                if (!string.IsNullOrWhiteSpace(request.SearchText))
                {
                    var term = request.SearchText.Trim().ToLower();
                    query = query.Where(x =>
                            x.Student.RegistrationNumber.ToLower().Contains(term) ||
                            x.FeePlan.PlanName.ToLower().Contains(term) ||
                            x.Course.Name.ToLower().Contains(term));
                }

                var totalCount = await query.CountAsync();

                if (totalCount == 0)
                {
                    return CommonResponse<PagedResult<StudentFeeSelectionListDto>>
                        .SuccessResponse(
                            "No selections found.",
                            new PagedResult<StudentFeeSelectionListDto>(
                                new List<StudentFeeSelectionListDto>(),
                                0,
                                request.Limit,
                                request.Offset));
                }

                var isDesc = request.SortOrder
                    .Equals("desc", StringComparison.OrdinalIgnoreCase);

                query = request.SortBy.ToLower() switch
                {
                    "studentname" => isDesc
                        ? query.OrderByDescending(x => x.Student.RegistrationNumber)
                        : query.OrderBy(x => x.Student.RegistrationNumber),
                    "coursename" => isDesc
                        ? query.OrderByDescending(x => x.Course.Name)
                        : query.OrderBy(x => x.Course.Name),
                    "finalamount" => isDesc
                        ? query.OrderByDescending(x => x.FinalAmount)
                        : query.OrderBy(x => x.FinalAmount),
                    "status" => isDesc
                        ? query.OrderByDescending(x => x.Status)
                        : query.OrderBy(x => x.Status),
                    _ => isDesc
                        ? query.OrderByDescending(x => x.CreatedAt)
                        : query.OrderBy(x => x.CreatedAt),
                };

                var items = await query
                    .Skip(request.Offset)
                    .Take(request.Limit)
                    .ToListAsync();

                var dtoList = items.Select(s => new StudentFeeSelectionListDto
                {
                    Id = s.Id,
                    StudentId = s.StudentId,
                    StudentName = s.Student?.StudentPersonalDetails?.FullName ?? string.Empty,
                    RegistrationNumber = s.Student?.RegistrationNumber ?? string.Empty,
                    CourseName = s.Course?.Name ?? string.Empty,
                    PlanName = s.FeePlan?.PlanName ?? string.Empty,
                    YearlyFee = s.YearlyFee,
                    DurationYears = s.SelectedDurationYears,
                    FinalAmount = s.FinalAmount,
                    PaymentType = s.PaymentType,
                    Status = s.Status,
                    StartDate = s.StartDate,
                    EndDate = s.EndDate,
                    IsActive = s.IsActive,
                    CreatedAt = s.CreatedAt,
                }).ToList();

                return CommonResponse<PagedResult<StudentFeeSelectionListDto>>
                    .SuccessResponse(
                        "Selections fetched.",
                        new PagedResult<StudentFeeSelectionListDto>(
                            dtoList, totalCount, request.Limit, request.Offset));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching filtered selections.");
                return CommonResponse<PagedResult<StudentFeeSelectionListDto>>
                    .FailureResponse($"An error occurred: {ex.Message}");
            }
        }

        /// <inheritdoc/>
        public async Task<FeePlanDto> CreateAsync(CreateFeePlanRequestDto request)
        {
            var feePlan = new FeePlan(
                request.CourseId,
                request.PlanName,
                request.TuitionFee,
                request.IsInstallmentAllowed,
                request.PaymentType,
                request.DurationinYears,
                request.SubjectId);

            feePlan.CreatedBy = request.userid;

            if (request.Discounts != null)
            {
                foreach (var d in request.Discounts)
                {
                    var discount = new FeePlanDiscount(feePlan.Id, d.discountName, d.discountPercent);
                    discount.CreatedBy = request.userid;
                    feePlan.Discounts.Add(discount);
                }
            }

            await _repo.AddAsync(feePlan);
            await _repo.SaveChangesAsync();
            return MapFeePlan(feePlan);
        }

        /// <inheritdoc/>
        public async Task<List<FeePlanDto>> GetAllAsync()
        {
            var plans = await _repo.GetAllAsync();
            return plans.Select(MapFeePlan).ToList();
        }

        /// <inheritdoc/>
        public async Task<FeePlanDto?> GetByIdAsync(Guid id)
        {
            var plan = await _repo.GetByIdAsync(id);
            return plan == null ? null : MapFeePlan(plan);
        }

        /// <inheritdoc/>
        public async Task<FeePlanDto?> UpdateAsync(Guid id, CreateFeePlanRequestDto request)
        {
            var feePlan = await _repo.GetByIdAsync(id);
            if (feePlan == null)
            {
                return null;
            }

            feePlan.Update(
                request.PlanName,
                request.TuitionFee,
                request.IsInstallmentAllowed,
                request.PaymentType,
                request.DurationinYears,
                request.SubjectId);

            feePlan.UpdatedBy = request.userid;

            var existing = feePlan.Discounts.ToList();
            var incoming = request.Discounts ?? new List<CreateFeePlanDiscountRequestDto>();

            // ── Remove discounts not in incoming ──
            foreach (var old in existing)
            {
                var stillExists = incoming.Any(d => d.id != Guid.Empty && d.id == old.Id);
                if (!stillExists)
                {
                    feePlan.Discounts.Remove(old);
                    _repo.RemoveDiscount(old);
                }
            }

            // ── Add new / update existing ──
            foreach (var dto in incoming)
            {
                if (dto.id == Guid.Empty)
                {
                    var newDiscount = new FeePlanDiscount(
                        feePlan.Id, dto.discountName, dto.discountPercent);
                    newDiscount.CreatedBy = request.userid;
                    feePlan.Discounts.Add(newDiscount);
                    _repo.AddDiscount(newDiscount);
                }
                else
                {
                    var ex = existing.FirstOrDefault(d => d.Id == dto.id);
                    if (ex != null)
                    {
                        ex.Update(dto.discountName, dto.discountPercent);
                        ex.UpdatedBy = request.userid;
                    }
                }
            }

            await _repo.SaveChangesAsync();

            var refreshed = await _repo.GetByIdAsync(id);
            return MapFeePlan(refreshed!);
        }

        /// <inheritdoc/>
        public async Task<CommonResponse<bool>> DeleteAsync(Guid id)
        {
            try
            {
                var result = await _repo.DeleteAsync(id);
                return result
                    ? CommonResponse<bool>.SuccessResponse("Fee deleted.", true)
                    : CommonResponse<bool>.FailureResponse("Fee not found.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting Fee {Id}", id);
                return CommonResponse<bool>.FailureResponse($"Error: {ex.Message}");
            }
        }

        /// <inheritdoc/>
        public async Task<CommonResponse<PagedResult<FeePlanDto>>> GetFilteredAsync(
            PagedRequest request)
        {
            try
            {
                var query = _repo.Query();

                if (request.Active.HasValue)
                {
                    query = query.Where(x => x.IsActive == request.Active.Value);
                }

                if (request.StartDate.HasValue)
                {
                    query = query.Where(x => x.CreatedAt >= request.StartDate.Value);
                }

                if (request.EndDate.HasValue)
                {
                    query = query.Where(x => x.CreatedAt <= request.EndDate.Value);
                }

                if (!string.IsNullOrWhiteSpace(request.SearchText))
                {
                    var term = request.SearchText.Trim().ToLower();
                    query = query.Where(x =>
                        x.PlanName.ToLower().Contains(term) ||
                        x.Course.Name.ToLower().Contains(term));
                }

                var totalCount = await query.CountAsync();

                if (totalCount == 0)
                {
                    return CommonResponse<PagedResult<FeePlanDto>>.SuccessResponse(
                        "No fees found.",
                        new PagedResult<FeePlanDto>(
                            new List<FeePlanDto>(), 0, request.Limit, request.Offset));
                }

                var isDesc = request.SortOrder
                    .Equals("desc", StringComparison.OrdinalIgnoreCase);

                query = request.SortBy.ToLower() switch
                {
                    "name" => isDesc
                        ? query.OrderByDescending(x => x.PlanName)
                        : query.OrderBy(x => x.PlanName),
                    "coursename" => isDesc
                        ? query.OrderByDescending(x => x.Course.Name)
                        : query.OrderBy(x => x.Course.Name),
                    _ => isDesc
                        ? query.OrderByDescending(x => x.CreatedAt)
                        : query.OrderBy(x => x.CreatedAt),
                };

                var items = await query
                    .Skip(request.Offset).Take(request.Limit).ToListAsync();

                return CommonResponse<PagedResult<FeePlanDto>>.SuccessResponse(
                    "Fees fetched.",
                    new PagedResult<FeePlanDto>(
                        items.Select(MapFeePlan).ToList(), totalCount,
                        request.Limit, request.Offset));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching filtered fees.");
                return CommonResponse<PagedResult<FeePlanDto>>.FailureResponse(
                    $"Error: {ex.Message}");
            }
        }

        // ═══════════════════════════════════════════════════════════
        //  PRIVATE MAPPERS
        // ═══════════════════════════════════════════════════════════
        private static InstallmentScheduleDto MapInstallment(StudentInstallment i) => new()
        {
            InstallmentId = i.Id,
            No = i.InstallmentNo,
            DueDate = i.DueDate,
            DueAmount = i.DueAmount,
            PaidAmount = i.PaidAmount,
            BalanceAmount = i.BalanceAmount,
            Status = i.Status,
            Remarks = i.Remarks,
            PaidDate = i.PaidDate,
        };

        private static FeePlanDto MapFeePlan(FeePlan fp) => new()
        {
            Id = fp.Id,
            CourseId = fp.CourseId,
            PlanName = fp.PlanName,
            TuitionFee = fp.TuitionFee,
            IsInstallmentAllowed = fp.IsInstallmentAllowed,
            IsActive = fp.IsActive,
            PaymentType = fp.PaymentType,
            DurationinYears = fp.DurationinYears,
            SubjectId = fp.SubjectId,
            StreamId = fp.Course?.StreamId ?? Guid.Empty,
            GradeId = fp.Course?.GradeId ?? Guid.Empty,
            SyllabusId = fp.Course?.Grade?.SyllabusId ?? Guid.Empty,
            CountryId = fp.Course?.Grade?.Syllabus?.Center?.CountryId ?? Guid.Empty,
            StateId = fp.Course?.Grade?.Syllabus?.Center?.StateId ?? Guid.Empty,
            ModeofstudyId = fp.Course?.Grade?.Syllabus?.Center?.State?.ModeOfStudyId
                            ?? Guid.Empty,
            CenterId = fp.Course?.Grade?.Syllabus?.CenterId ?? Guid.Empty,
            Discounts = fp.Discounts.Where(d => !d.IsDeleted && d.IsActive).Select(d => new FeePlanDiscountDto
            {
                Id = d.Id,
                FeePlanId = d.FeePlanId,
                DiscountName = d.DiscountName,
                DiscountPercent = d.DiscountPercent,
                CreatedAt = d.CreatedAt,
                UpdatedAt = d.UpdatedAt,
            }).ToList(),
        };
    }
}
