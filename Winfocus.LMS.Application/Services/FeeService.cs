namespace Winfocus.LMS.Application.Services
{
    using Microsoft.Extensions.Logging;
    using Winfocus.LMS.Application.Common.Exceptions;
    using Winfocus.LMS.Application.DTOs.Fees;
    using Winfocus.LMS.Application.Interfaces;
    using Winfocus.LMS.Domain.Entities;
    using Winfocus.LMS.Domain.Enums;

    /// <summary>
    /// FeeService – implements all fee-related business logic including
    /// discount management and fee calculation.
    /// </summary>
    /// <seealso cref="Winfocus.LMS.Application.Interfaces.IFeeService" />
    public sealed class FeeService : IFeeService
    {
        private readonly IFeeRepository _repo;
        private readonly ILogger<FeeService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="FeeService"/> class.
        /// </summary>
        /// <param name="repo">The repository.</param>
        /// <param name="logger">The logger.</param>
        public FeeService(IFeeRepository repo, ILogger<FeeService> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        /// <summary>
        /// Gets the fee page asynchronous.
        /// Builds a pricing table for all courses in the student's grade.
        /// Registration fee is excluded from calculations.
        /// Only active discounts are subtracted.
        /// </summary>
        /// <param name="studentId">The student identifier.</param>
        /// <returns>Task&lt;FeePageResponseDto&gt;.</returns>
        /// <exception cref="AppException">Thrown when student is not found.</exception>
        public async Task<FeePageResponseDto> GetFeePageAsync(Guid studentId)
        {
            _logger.LogInformation(
                "GetFeePageAsync called for StudentId: {StudentId}", studentId);

            var student = await _repo.GetStudentWithCoursesAsync(studentId);

            if (student == null)
            {
                _logger.LogWarning(
                    "Student not found. StudentId: {StudentId}", studentId);
                throw new AppException("Student not found.", 404, "STUDENT_NOT_FOUND");
            }

            var selectedCourseIds = student.StudentAcademicCouses
                .Select(x => x.CourseId)
                .ToHashSet();

            var courses = await _repo.GetCoursesByGradeAsync(student.AcademicDetails.GradeId);

            // Load existing fee selections to show persisted manual discounts
            var existingSelections = await _repo.GetStudentFeeSelectionsByStudentAsync(studentId);
            var selectionLookup = existingSelections
                .ToDictionary(s => (s.CourseId, s.FeePlanId), s => s);

            var pricingTable = courses
                .SelectMany(course => course.FeePlans, (course, feePlan) =>
                {
                    // Base fee = tuition only (no registration fee)
                    var baseFee = feePlan.TuitionFee;

                    // Check if there is an existing selection with persisted discount values
                    selectionLookup.TryGetValue(
                        (course.Id, feePlan.Id), out var existingSelection);

                    var scholarshipPercent = existingSelection?.ScholarshipPercent
                        ?? feePlan.ScholarshipPercent;
                    var isScholarshipActive = existingSelection?.IsScholarshipActive ?? true;

                    var seasonalPercent = existingSelection?.SeasonalPercent
                        ?? feePlan.SeasonalPercent;
                    var isSeasonalActive = existingSelection?.IsSeasonalActive
                        ?? feePlan.IsSeasonalDiscountActive;

                    var manualPercent = existingSelection?.ManualDiscountPercent ?? 0m;
                    var isManualActive = existingSelection?.IsManualDiscountActive ?? false;

                    var feeAfterDiscount = CalculateFinalAmount(
                        baseFee,
                        scholarshipPercent,
                        isScholarshipActive,
                        seasonalPercent,
                        isSeasonalActive,
                        manualPercent,
                        isManualActive);

                    return new FeeRowDto
                    {
                        CourseId = course.Id,
                        FeePlanId = feePlan.Id,
                        CourseName = course.Name,
                        BaseFee = baseFee,
                        PaymentType = feePlan.PlanName,
                        ScholarshipPercent = scholarshipPercent,
                        IsScholarshipActive = isScholarshipActive,
                        SeasonalPercent = seasonalPercent,
                        IsSeasonalActive = isSeasonalActive,
                        ManualDiscountPercent = manualPercent,
                        IsManualDiscountActive = isManualActive,
                        FeeAfterDiscount = feeAfterDiscount,
                        IsSelected = selectedCourseIds.Contains(course.Id),
                    };
                })
                .ToList();

            _logger.LogInformation(
                "Fee page built for StudentId: {StudentId}. Rows: {RowCount}",
                studentId, pricingTable.Count);

            return new FeePageResponseDto
            {
                PricingTable = pricingTable,
            };
        }

        /// <summary>
        /// Selects the fee asynchronous.
        /// Creates a StudentFeeSelection record with all three discount types persisted.
        /// Registration fee is excluded from the base fee.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Task&lt;FeeSummaryDto&gt;.</returns>
        /// <exception cref="AppException">Thrown when student or fee plan is not found.</exception>
        public async Task<FeeSummaryDto> SelectFeeAsync(SelectFeeRequestDto request)
        {
            _logger.LogInformation(
                "SelectFeeAsync called. StudentId: {StudentId}, FeePlanId: {FeePlanId}",
                request.StudentId, request.FeePlanId);

            var student = await _repo.GetStudentWithCoursesAsync(request.StudentId);

            if (student == null)
            {
                _logger.LogWarning(
                    "Student not found. StudentId: {StudentId}", request.StudentId);
                throw new AppException("Student not found.", 404, "STUDENT_NOT_FOUND");
            }

            var courses = await _repo.GetCoursesByGradeAsync(student.AcademicDetails.GradeId);

            var feePlan = courses
                .SelectMany(c => c.FeePlans)
                .FirstOrDefault(x => x.Id == request.FeePlanId);

            if (feePlan == null)
            {
                _logger.LogWarning(
                    "Fee plan not found. FeePlanId: {FeePlanId}", request.FeePlanId);
                throw new AppException("Fee plan not found.", 404, "FEE_PLAN_NOT_FOUND");
            }

            // Base fee = tuition only (no registration fee)
            var baseFee = feePlan.TuitionFee;

            // Use request override or plan default for scholarship
            var scholarshipPercent = request.ScholarshipPercent ?? feePlan.ScholarshipPercent;
            var isScholarshipActive = request.IsScholarshipActive;

            // Seasonal comes from the plan
            var seasonalPercent = feePlan.SeasonalPercent;
            var isSeasonalActive = request.IsSeasonalActive && feePlan.IsSeasonalDiscountActive;

            // Manual from admin
            var manualPercent = request.ManualDiscountPercent;
            var isManualActive = request.IsManualDiscountActive;

            var finalAmount = CalculateFinalAmount(
                baseFee,
                scholarshipPercent,
                isScholarshipActive,
                seasonalPercent,
                isSeasonalActive,
                manualPercent,
                isManualActive);

            var summary = BuildSummary(
                baseFee,
                scholarshipPercent,
                isScholarshipActive,
                seasonalPercent,
                isSeasonalActive,
                manualPercent,
                isManualActive);

            var selection = new StudentFeeSelection(
                request.StudentId,
                feePlan.CourseId,
                feePlan.Id,
                scholarshipPercent,
                isScholarshipActive,
                seasonalPercent,
                isSeasonalActive,
                manualPercent,
                isManualActive,
                baseFee,
                finalAmount);

            await _repo.AddStudentFeeSelectionAsync(selection);
            await _repo.SaveChangesAsync();

            _logger.LogInformation(
                "Fee selected. SelectionId: {SelectionId}, FinalAmount: {FinalAmount}",
                selection.Id, finalAmount);

            return summary;
        }

        /// <summary>
        /// Gets all discount details for a student's fee selections.
        /// Returns 3 entries per selection (one for each discount type).
        /// </summary>
        /// <param name="studentId">The student identifier.</param>
        /// <returns>Task&lt;IReadOnlyList&lt;DiscountDetailDto&gt;&gt;.</returns>
        /// <exception cref="AppException">Thrown when student is not found.</exception>
        public async Task<IReadOnlyList<DiscountDetailDto>> GetDiscountsByStudentAsync(
            Guid studentId)
        {
            _logger.LogInformation(
                "GetDiscountsByStudentAsync called. StudentId: {StudentId}", studentId);

            var student = await _repo.GetStudentWithCoursesAsync(studentId);

            if (student == null)
            {
                throw new AppException("Student not found.", 404, "STUDENT_NOT_FOUND");
            }

            var selections = await _repo.GetStudentFeeSelectionsByStudentAsync(studentId);

            return selections
                .SelectMany(MapSelectionToDiscountDetails)
                .ToList()
                .AsReadOnly();
        }

        /// <summary>
        /// Gets discount details for a specific student fee selection.
        /// Returns 3 entries (one for each discount type).
        /// </summary>
        /// <param name="selectionId">The student fee selection identifier.</param>
        /// <returns>Task&lt;IReadOnlyList&lt;DiscountDetailDto&gt;&gt;.</returns>
        /// <exception cref="AppException">Thrown when selection is not found.</exception>
        public async Task<IReadOnlyList<DiscountDetailDto>> GetDiscountsBySelectionAsync(
            Guid selectionId)
        {
            _logger.LogInformation(
                "GetDiscountsBySelectionAsync called. SelectionId: {SelectionId}", selectionId);

            var selection = await _repo.GetStudentFeeSelectionByIdAsync(selectionId);

            if (selection == null)
            {
                throw new AppException(
                    "Student fee selection not found.",
                    404,
                    "SELECTION_NOT_FOUND");
            }

            return MapSelectionToDiscountDetails(selection).ToList().AsReadOnly();
        }

        /// <summary>
        /// Updates a specific discount on a student fee selection.
        /// Recalculates and persists the new final amount.
        /// </summary>
        /// <param name="request">The update discount request.</param>
        /// <returns>Task&lt;FeeSummaryDto&gt;.</returns>
        /// <exception cref="AppException">Thrown when selection is not found or discount type is invalid.</exception>
        public async Task<FeeSummaryDto> UpdateDiscountAsync(UpdateDiscountRequestDto request)
        {
            _logger.LogInformation(
                "UpdateDiscountAsync called. SelectionId: {SelectionId}, Type: {DiscountType}",
                request.StudentFeeSelectionId, request.DiscountType);

            ValidateDiscountType(request.DiscountType);

            var selection = await _repo.GetStudentFeeSelectionByIdAsync(
                request.StudentFeeSelectionId);

            if (selection == null)
            {
                throw new AppException(
                    "Student fee selection not found.",
                    404,
                    "SELECTION_NOT_FOUND");
            }

            switch (request.DiscountType)
            {
                case DiscountType.Scholarship:
                    selection.UpdateScholarshipDiscount(request.Percent, request.IsActive);
                    break;
                case DiscountType.Seasonal:
                    selection.UpdateSeasonalDiscount(request.Percent, request.IsActive);
                    break;
                case DiscountType.Manual:
                    selection.UpdateManualDiscount(request.Percent, request.IsActive);
                    break;
            }

            await _repo.SaveChangesAsync();

            _logger.LogInformation(
                "Discount updated. SelectionId: {SelectionId}, Type: {DiscountType}, " +
                "NewPercent: {Percent}, Active: {IsActive}, FinalAmount: {FinalAmount}",
                selection.Id,
                request.DiscountType,
                request.Percent,
                request.IsActive,
                selection.FinalAmount);

            return BuildSummary(
                selection.BaseFee,
                selection.ScholarshipPercent,
                selection.IsScholarshipActive,
                selection.SeasonalPercent,
                selection.IsSeasonalActive,
                selection.ManualDiscountPercent,
                selection.IsManualDiscountActive);
        }

        /// <summary>
        /// Removes (deactivates and zeroes) a specific discount from a student fee selection.
        /// Recalculates and persists the new final amount.
        /// </summary>
        /// <param name="selectionId">The student fee selection identifier.</param>
        /// <param name="discountType">The discount type.</param>
        /// <returns>Task&lt;FeeSummaryDto&gt;.</returns>
        /// <exception cref="AppException">Thrown when selection is not found or discount type is invalid.</exception>
        public async Task<FeeSummaryDto> RemoveDiscountAsync(
            Guid selectionId,
            DiscountType discountType)
        {
            _logger.LogInformation(
                "RemoveDiscountAsync called. SelectionId: {SelectionId}, Type: {DiscountType}",
                selectionId, discountType);

            ValidateDiscountType(discountType);

            var selection = await _repo.GetStudentFeeSelectionByIdAsync(selectionId);

            if (selection == null)
            {
                throw new AppException(
                    "Student fee selection not found.",
                    404,
                    "SELECTION_NOT_FOUND");
            }

            switch (discountType)
            {
                case DiscountType.Scholarship:
                    selection.UpdateScholarshipDiscount(0m, false);
                    break;
                case DiscountType.Seasonal:
                    selection.UpdateSeasonalDiscount(0m, false);
                    break;
                case DiscountType.Manual:
                    selection.UpdateManualDiscount(0m, false);
                    break;
            }

            await _repo.SaveChangesAsync();

            _logger.LogInformation(
                "Discount removed. SelectionId: {SelectionId}, Type: {DiscountType}, " +
                "FinalAmount: {FinalAmount}",
                selection.Id, discountType, selection.FinalAmount);

            return BuildSummary(
                selection.BaseFee,
                selection.ScholarshipPercent,
                selection.IsScholarshipActive,
                selection.SeasonalPercent,
                selection.IsSeasonalActive,
                selection.ManualDiscountPercent,
                selection.IsManualDiscountActive);
        }

        /// <summary>
        /// Updates the seasonal discount on a fee plan (plan-level).
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Task.</returns>
        /// <exception cref="AppException">Thrown when fee plan is not found.</exception>
        public async Task UpdateSeasonalDiscountOnPlanAsync(
            UpdateSeasonalDiscountRequestDto request)
        {
            _logger.LogInformation(
                "UpdateSeasonalDiscountOnPlanAsync called. FeePlanId: {FeePlanId}",
                request.FeePlanId);

            var feePlan = await _repo.GetFeePlanByIdAsync(request.FeePlanId);

            if (feePlan == null)
            {
                throw new AppException(
                    "Fee plan not found.", 404, "FEE_PLAN_NOT_FOUND");
            }

            feePlan.UpdateSeasonalDiscount(request.Percent, request.IsActive);
            await _repo.SaveChangesAsync();

            _logger.LogInformation(
                "Seasonal discount updated on plan. FeePlanId: {FeePlanId}, " +
                "Percent: {Percent}, Active: {IsActive}",
                feePlan.Id, request.Percent, request.IsActive);
        }

        /// <summary>
        /// Gets the seasonal discount for a fee plan.
        /// </summary>
        /// <param name="feePlanId">The fee plan identifier.</param>
        /// <returns>Task&lt;DiscountDetailDto&gt;.</returns>
        /// <exception cref="AppException">Thrown when fee plan is not found.</exception>
        public async Task<DiscountDetailDto> GetSeasonalDiscountOnPlanAsync(Guid feePlanId)
        {
            _logger.LogInformation(
                "GetSeasonalDiscountOnPlanAsync called. FeePlanId: {FeePlanId}", feePlanId);

            var feePlan = await _repo.GetFeePlanByIdAsync(feePlanId);

            if (feePlan == null)
            {
                throw new AppException(
                    "Fee plan not found.", 404, "FEE_PLAN_NOT_FOUND");
            }

            return new DiscountDetailDto
            {
                StudentFeeSelectionId = Guid.Empty,
                StudentId = Guid.Empty,
                CourseId = feePlan.CourseId,
                FeePlanId = feePlan.Id,
                DiscountType = DiscountType.Seasonal,
                Percent = feePlan.SeasonalPercent,
                IsActive = feePlan.IsSeasonalDiscountActive,
                BaseFee = feePlan.TuitionFee,
                FinalAmount = 0,
            };
        }

        /// <summary>
        /// Removes the seasonal discount from a fee plan.
        /// </summary>
        /// <param name="feePlanId">The fee plan identifier.</param>
        /// <returns>Task.</returns>
        /// <exception cref="AppException">Thrown when fee plan is not found.</exception>
        public async Task RemoveSeasonalDiscountOnPlanAsync(Guid feePlanId)
        {
            _logger.LogInformation(
                "RemoveSeasonalDiscountOnPlanAsync called. FeePlanId: {FeePlanId}", feePlanId);

            var feePlan = await _repo.GetFeePlanByIdAsync(feePlanId);

            if (feePlan == null)
            {
                throw new AppException(
                    "Fee plan not found.", 404, "FEE_PLAN_NOT_FOUND");
            }

            feePlan.UpdateSeasonalDiscount(0m, false);
            await _repo.SaveChangesAsync();

            _logger.LogInformation(
                "Seasonal discount removed from plan. FeePlanId: {FeePlanId}", feePlanId);
        }

        /// <summary>
        /// Calculates the final amount after applying only active discounts.
        /// Discount application order: Scholarship → Seasonal → Manual (sequential).
        /// </summary>
        /// <param name="baseFee">The base fee.</param>
        /// <param name="scholarshipPercent">The scholarship percent.</param>
        /// <param name="isScholarshipActive">Whether scholarship is active.</param>
        /// <param name="seasonalPercent">The seasonal percent.</param>
        /// <param name="isSeasonalActive">Whether seasonal is active.</param>
        /// <param name="manualPercent">The manual percent.</param>
        /// <param name="isManualActive">Whether manual is active.</param>
        /// <returns>The final amount.</returns>
        private static decimal CalculateFinalAmount(
            decimal baseFee,
            decimal scholarshipPercent,
            bool isScholarshipActive,
            decimal seasonalPercent,
            bool isSeasonalActive,
            decimal manualPercent,
            bool isManualActive)
        {
            var amount = baseFee;

            if (isScholarshipActive && scholarshipPercent > 0)
            {
                amount -= amount * scholarshipPercent / 100m;
            }

            if (isSeasonalActive && seasonalPercent > 0)
            {
                amount -= amount * seasonalPercent / 100m;
            }

            if (isManualActive && manualPercent > 0)
            {
                amount -= amount * manualPercent / 100m;
            }

            return Math.Max(amount, 0);
        }

        /// <summary>
        /// Builds a fee summary with individual discount amounts.
        /// </summary>
        /// <param name="baseFee">The base fee.</param>
        /// <param name="scholarshipPercent">The scholarship percent.</param>
        /// <param name="isScholarshipActive">Whether scholarship is active.</param>
        /// <param name="seasonalPercent">The seasonal percent.</param>
        /// <param name="isSeasonalActive">Whether seasonal is active.</param>
        /// <param name="manualPercent">The manual percent.</param>
        /// <param name="isManualActive">Whether manual is active.</param>
        /// <returns>The fee summary DTO.</returns>
        private static FeeSummaryDto BuildSummary(
            decimal baseFee,
            decimal scholarshipPercent,
            bool isScholarshipActive,
            decimal seasonalPercent,
            bool isSeasonalActive,
            decimal manualPercent,
            bool isManualActive)
        {
            var amount = baseFee;
            decimal scholarshipDiscount = 0;
            decimal seasonalDiscount = 0;
            decimal manualDiscount = 0;

            if (isScholarshipActive && scholarshipPercent > 0)
            {
                scholarshipDiscount = amount * scholarshipPercent / 100m;
                amount -= scholarshipDiscount;
            }

            if (isSeasonalActive && seasonalPercent > 0)
            {
                seasonalDiscount = amount * seasonalPercent / 100m;
                amount -= seasonalDiscount;
            }

            if (isManualActive && manualPercent > 0)
            {
                manualDiscount = amount * manualPercent / 100m;
                amount -= manualDiscount;
            }

            return new FeeSummaryDto
            {
                BaseFee = baseFee,
                ScholarshipDiscount = scholarshipDiscount,
                SeasonalDiscount = seasonalDiscount,
                ManualDiscount = manualDiscount,
                TotalPayable = Math.Max(amount, 0),
            };
        }

        /// <summary>
        /// Maps a single StudentFeeSelection to 3 DiscountDetailDto entries.
        /// </summary>
        /// <param name="selection">The student fee selection.</param>
        /// <returns>Enumerable of DiscountDetailDto.</returns>
        private static IEnumerable<DiscountDetailDto> MapSelectionToDiscountDetails(
            StudentFeeSelection selection)
        {
            yield return new DiscountDetailDto
            {
                StudentFeeSelectionId = selection.Id,
                StudentId = selection.StudentId,
                CourseId = selection.CourseId,
                FeePlanId = selection.FeePlanId,
                DiscountType = DiscountType.Scholarship,
                Percent = selection.ScholarshipPercent,
                IsActive = selection.IsScholarshipActive,
                BaseFee = selection.BaseFee,
                FinalAmount = selection.FinalAmount,
            };

            yield return new DiscountDetailDto
            {
                StudentFeeSelectionId = selection.Id,
                StudentId = selection.StudentId,
                CourseId = selection.CourseId,
                FeePlanId = selection.FeePlanId,
                DiscountType = DiscountType.Seasonal,
                Percent = selection.SeasonalPercent,
                IsActive = selection.IsSeasonalActive,
                BaseFee = selection.BaseFee,
                FinalAmount = selection.FinalAmount,
            };

            yield return new DiscountDetailDto
            {
                StudentFeeSelectionId = selection.Id,
                StudentId = selection.StudentId,
                CourseId = selection.CourseId,
                FeePlanId = selection.FeePlanId,
                DiscountType = DiscountType.Manual,
                Percent = selection.ManualDiscountPercent,
                IsActive = selection.IsManualDiscountActive,
                BaseFee = selection.BaseFee,
                FinalAmount = selection.FinalAmount,
            };
        }

        /// <summary>
        /// Validates that the discount type is one of: Scholarship, Seasonal, Manual.
        /// </summary>
        /// <param name="discountType">The discount type string.</param>
        /// <exception cref="AppException">Thrown when the discount type is invalid.</exception>
        private static void ValidateDiscountType(DiscountType discountType)
        {
            if (discountType == DiscountType.None ||
                !Enum.IsDefined(typeof(DiscountType), discountType))
            {
                throw new AppException(
                    $"Invalid discount type '{discountType}'. " +
                    $"Valid values: Scholarship, Seasonal, Manual.",
                    400,
                    "INVALID_DISCOUNT_TYPE");
            }
        }
    }
}
