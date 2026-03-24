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
                var assignedLookup = assigned.ToLookup(a => a.CourseId);

                var courseBlocks = new List<CourseDiscountBlockDto>();

                foreach (var course in courses)
                {
                    // Get unique discounts across all plans for this course
                    var allPlanDiscounts = course.FeePlans
                        .Where(fp => fp.IsActive)
                        .SelectMany(fp => fp.Discounts.Where(d => d.IsActive))
                        .GroupBy(d => d.DiscountName)
                        .Select(g => g.First())
                        .ToList();

                    var courseAssigned = assignedLookup[course.Id].ToList();
                    var assignedNames = courseAssigned
                        .Where(a => !a.IsManual)
                        .Select(a => a.DiscountName)
                        .ToHashSet(StringComparer.OrdinalIgnoreCase);

                    var baseYearlyFee = course.FeePlans
                        .Where(fp => fp.IsActive)
                        .Select(fp => fp.TuitionFee)
                        .FirstOrDefault();

                    // Calculate active discount total
                    var activeDiscountPercent = courseAssigned
                        .Where(a => a.IsActive)
                        .Sum(a => a.DiscountPercent);
                    activeDiscountPercent = Math.Min(activeDiscountPercent, 100);

                    var feeAfter = baseYearlyFee * (1 - activeDiscountPercent / 100m);

                    var manual = courseAssigned.FirstOrDefault(a => a.IsManual);

                    courseBlocks.Add(new CourseDiscountBlockDto
                    {
                        CourseId = course.Id,
                        CourseName = course.Name,
                        BaseYearlyFee = baseYearlyFee,
                        AvailableDiscounts = allPlanDiscounts.Select(d =>
                        {
                            var isAssigned = assignedNames.Contains(d.DiscountName);
                            return new AvailableDiscountDto
                            {
                                FeePlanDiscountId = d.Id,
                                DiscountName = d.DiscountName,
                                DiscountPercent = d.DiscountPercent,
                                DiscountAmount = baseYearlyFee * d.DiscountPercent / 100m,
                                IsAssigned = isAssigned,
                            };
                        }).ToList(),
                        ManualDiscount = manual == null ? null : new AssignedManualDiscountDto
                        {
                            DiscountName = manual.DiscountName,
                            DiscountPercent = manual.DiscountPercent,
                        },
                        CalculatedFeeAfterDiscounts = Math.Max(feeAfter, 0),
                    });
                }

                var totalPayable = courseBlocks.Sum(c => c.CalculatedFeeAfterDiscounts);

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
                        TotalPayable = totalPayable,
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
                var student = await _repo.GetStudentWithCoursesAsync(request.StudentId);
                if (student == null)
                {
                    return CommonResponse<bool>.FailureResponse("Student not found.");
                }

                // Validate selected discount IDs
                var planDiscounts = request.SelectedDiscountIds.Any()
                    ? await _repo.GetFeePlanDiscountsByIdsAsync(request.SelectedDiscountIds)
                    : new List<FeePlanDiscount>();

                // Verify all IDs are valid and belong to this course
                if (planDiscounts.Count != request.SelectedDiscountIds.Count)
                {
                    return CommonResponse<bool>.FailureResponse(
                        "One or more discount IDs are invalid.");
                }

                if (planDiscounts.Any(d => d.FeePlan.CourseId != request.CourseId))
                {
                    return CommonResponse<bool>.FailureResponse(
                        "Discounts do not belong to the specified course.");
                }

                // Remove existing assignments for student+course
                await _repo.RemoveStudentCourseDiscountsAsync(
                    request.StudentId, request.CourseId);

                // Create new assignments
                var newDiscounts = new List<StudentCourseDiscount>();

                foreach (var pd in planDiscounts)
                {
                    newDiscounts.Add(StudentCourseDiscount.FromPlanDiscount(
                        request.StudentId, request.CourseId, pd, request.UserId));
                }

                if (request.ManualDiscount != null)
                {
                    newDiscounts.Add(StudentCourseDiscount.Manual(
                        request.StudentId,
                        request.CourseId,
                        request.ManualDiscount.DiscountName,
                        request.ManualDiscount.DiscountPercent,
                        request.UserId));
                }

                if (newDiscounts.Any())
                {
                    await _repo.AddStudentCourseDiscountsAsync(newDiscounts);
                }

                await _repo.SaveChangesAsync();

                _logger.LogInformation(
                    "Discounts assigned: Student={StudentId}, Course={CourseId}, Count={Count}",
                    request.StudentId,
                    request.CourseId,
                    newDiscounts.Count);

                return CommonResponse<bool>.SuccessResponse(
                    "Discounts assigned successfully.", true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error assigning discounts");
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
                var assignedByCourse = assigned.ToLookup(a => a.CourseId);
                var existingSelections = await _repo.GetSelectionsByStudentAsync(studentId);
                var selectedPlanIds = existingSelections
                    .Select(s => s.FeePlanId).ToHashSet();

                var feeListings = new List<FeeListingRowDto>();

                foreach (var course in courses)
                {
                    var courseDiscounts = assignedByCourse[course.Id]
                        .Where(a => a.IsActive).ToList();

                    foreach (var plan in course.FeePlans.Where(fp => fp.IsActive))
                    {
                        // Match plan discounts with admin-assigned
                        var appliedDiscounts = new List<DiscountBadgeDto>();
                        decimal totalDiscountPercent = 0;

                        // Non-manual: match by name
                        foreach (var planDiscount in plan.Discounts.Where(d => d.IsActive))
                        {
                            var match = courseDiscounts.FirstOrDefault(
                                cd => !cd.IsManual &&
                                      cd.DiscountName.Equals(
                                          planDiscount.DiscountName,
                                          StringComparison.OrdinalIgnoreCase));
                            if (match != null)
                            {
                                appliedDiscounts.Add(new DiscountBadgeDto
                                {
                                    Name = planDiscount.DiscountName,
                                    Percent = planDiscount.DiscountPercent,
                                });
                                totalDiscountPercent += planDiscount.DiscountPercent;
                            }
                        }

                        // Manual: always apply
                        foreach (var manual in courseDiscounts.Where(cd => cd.IsManual))
                        {
                            appliedDiscounts.Add(new DiscountBadgeDto
                            {
                                Name = manual.DiscountName,
                                Percent = manual.DiscountPercent,
                            });
                            totalDiscountPercent += manual.DiscountPercent;
                        }

                        totalDiscountPercent = Math.Min(totalDiscountPercent, 100);

                        var yearlyFee = plan.TuitionFee;
                        var duration = plan.DurationinYears;
                        var totalBefore = yearlyFee * duration;
                        var discountAmount = totalBefore * totalDiscountPercent / 100m;
                        var feeAfter = Math.Max(totalBefore - discountAmount, 0);

                        var installmentCount = plan.PaymentType == PaymentType.Full
                            ? 1
                            : plan.PaymentType.GetTotalInstallments(plan.DurationinYears);

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
                            TotalDiscountPercent = totalDiscountPercent,
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
                        RegistrationNumber = student.RegistrationNumber ?? "",
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

                // Check duplicate
                var exists = await _repo.HasConfirmedSelectionForCourseAsync(
                    request.StudentId, plan.CourseId);
                if (exists)
                {
                    return CommonResponse<ConfirmFeeResponseDto>.FailureResponse(
                        "A fee plan is already confirmed for this course. Contact admin.");
                }

                // Get admin-assigned discounts
                var courseDiscounts = await _repo.GetStudentCourseDiscountsAsync(
                    request.StudentId, plan.CourseId);

                // Match discounts to this plan
                var matchedDiscounts = new List<(string Name, decimal Percent, bool IsManual)>();

                foreach (var planDiscount in plan.Discounts.Where(d => d.IsActive))
                {
                    var match = courseDiscounts.FirstOrDefault(
                        cd => !cd.IsManual &&
                              cd.DiscountName.Equals(
                                  planDiscount.DiscountName,
                                  StringComparison.OrdinalIgnoreCase));
                    if (match != null)
                    {
                        matchedDiscounts.Add(
                            (planDiscount.DiscountName, planDiscount.DiscountPercent, false));
                    }
                }

                foreach (var manual in courseDiscounts.Where(cd => cd.IsManual))
                {
                    matchedDiscounts.Add(
                        (manual.DiscountName, manual.DiscountPercent, true));
                }

                // Calculate
                var yearlyFee = plan.TuitionFee;
                var duration = plan.DurationinYears;
                var totalBefore = yearlyFee * duration;
                var totalDiscountPercent = Math.Min(
                    matchedDiscounts.Sum(d => d.Percent), 100);
                var totalDiscountAmount = totalBefore * totalDiscountPercent / 100m;
                var finalAmount = Math.Max(totalBefore - totalDiscountAmount, 0);

                var installmentCount = plan.PaymentType == PaymentType.Full
                    ? 1
                    : plan.PaymentType.GetTotalInstallments(duration);

                // Create selection
                var selection = new StudentFeeSelection(
                    request.StudentId,
                    plan.CourseId,
                    plan.Id,
                    yearlyFee,
                    duration,
                    totalBefore,
                    totalDiscountPercent,
                    totalDiscountAmount,
                    finalAmount,
                    plan.PaymentType,
                    installmentCount,
                    request.StartDate,
                    request.EndDate);

                selection.CreatedBy = request.StudentId;

                await _repo.AddStudentFeeSelectionAsync(selection);
                await _repo.SaveChangesAsync(); // get the ID

                // Create discount snapshots
                foreach (var d in matchedDiscounts)
                {
                    var amount = totalBefore * d.Percent / 100m;
                    var snapshot = new StudentFeeDiscount(
                        selection.Id, d.Name, d.Percent, amount, d.IsManual);
                    selection.AppliedDiscounts.Add(snapshot);
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
                        ? perInstallment + remainder  // last gets rounding diff
                        : perInstallment;

                    var installment = new StudentInstallment(
                        selection.Id, i, dueAmount, dueDate);
                    selection.Installments.Add(installment);
                }

                await _repo.SaveChangesAsync();

                _logger.LogInformation(
                    "Fee confirmed: SelectionId={Id}, Final={Amount}, Installments={Count}",
                    selection.Id,
                    finalAmount,
                    installmentCount);

                // Build response
                var response = new ConfirmFeeResponseDto
                {
                    SelectionId = selection.Id,
                    CourseName = plan.Course?.Name ?? string.Empty,
                    PlanName = plan.PlanName,
                    YearlyFee = yearlyFee,
                    DurationYears = duration,
                    TotalBeforeDiscount = totalBefore,
                    TotalDiscountPercent = totalDiscountPercent,
                    TotalDiscountAmount = totalDiscountAmount,
                    FinalAmount = finalAmount,
                    PaymentType = plan.PaymentType,
                    TotalInstallments = installmentCount,
                    Status = selection.Status,
                    AppliedDiscounts = matchedDiscounts.Select(d =>
                        new AppliedDiscountSnapshotDto
                        {
                            Name = d.Name,
                            Percent = d.Percent,
                            Amount = totalBefore * d.Percent / 100m,
                            IsManual = d.IsManual,
                        }).ToList(),
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
                    request.PaidAmount, request.PaidDate, request.Remarks);

                // Update parent selection status
                var selection = await _repo.GetSelectionWithDetailsAsync(
                    installment.StudentFeeSelectionId);

                selection?.RefreshStatus();

                await _repo.SaveChangesAsync();

                _logger.LogInformation(
                    "Payment recorded: InstallmentId={Id}, Amount={Amount}",
                    installmentId, 
                    request.PaidAmount);

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

            foreach (var old in existing)
            {
                if (!incoming.Any(d => d.id != Guid.Empty && d.id == old.Id))
                {
                    _repo.RemoveDiscount(old);
                }
            }

            foreach (var dto in incoming)
            {
                if (dto.id == Guid.Empty)
                {
                    var nd = new FeePlanDiscount(feePlan.Id, dto.discountName, dto.discountPercent);
                    nd.CreatedBy = request.userid;
                    _repo.AddDiscount(nd);
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
            return MapFeePlan(feePlan);
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
            Discounts = fp.Discounts.Select(d => new FeePlanDiscountDto
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
