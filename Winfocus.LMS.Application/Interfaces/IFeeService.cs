namespace Winfocus.LMS.Application.Interfaces
{
    using Winfocus.LMS.Application.DTOs;
    using Winfocus.LMS.Application.DTOs.Common;
    using Winfocus.LMS.Application.DTOs.Fees;
    using Winfocus.LMS.Application.DTOs.Masters;
    using Winfocus.LMS.Domain.Enums;

    /// <summary>
    /// IFeeService – contract for fee-related business operations.
    /// </summary>
    public interface IFeeService
    {
        /// <summary>
        /// Gets the fee page asynchronous.
        /// </summary>
        /// <param name="studentId">The student identifier.</param>
        /// <returns>Task&lt;FeePageResponseDto&gt;.</returns>
        Task<FeePageResponseDto> GetFeePageAsync(Guid studentId);

        /// <summary>
        /// Selects the fee asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Task&lt;FeeSummaryDto&gt;.</returns>
        Task<FeeSummaryDto> SelectFeeAsync(SelectFeeRequestDto request);

        /// <summary>
        /// Gets all discount details for a student's fee selections.
        /// </summary>
        /// <param name="studentId">The student identifier.</param>
        /// <returns>Task&lt;IReadOnlyList&lt;DiscountDetailDto&gt;&gt;.</returns>
        Task<IReadOnlyList<DiscountDetailDto>> GetDiscountsByStudentAsync(Guid studentId);

        /// <summary>
        /// Gets discount details for a specific student fee selection.
        /// </summary>
        /// <param name="selectionId">The student fee selection identifier.</param>
        /// <returns>Task&lt;IReadOnlyList&lt;DiscountDetailDto&gt;&gt;.</returns>
        Task<IReadOnlyList<DiscountDetailDto>> GetDiscountsBySelectionAsync(Guid selectionId);

        /// <summary>
        /// Updates a specific discount on a student fee selection.
        /// </summary>
        /// <param name="request">The update discount request.</param>
        /// <returns>Task&lt;FeeSummaryDto&gt;.</returns>
        Task<FeeSummaryDto> UpdateDiscountAsync(UpdateDiscountRequestDto request);

        /// <summary>
        /// Removes (deactivates and zeroes) a specific discount from a student fee selection.
        /// </summary>
        /// <param name="selectionId">The student fee selection identifier.</param>
        /// <param name="discountType">The discount type (Scholarship, Seasonal, Manual).</param>
        /// <returns>Task&lt;FeeSummaryDto&gt;.</returns>
        Task<FeeSummaryDto> RemoveDiscountAsync(Guid selectionId, DiscountType discountType);

        /// <summary>
        /// Updates the seasonal discount on a fee plan (plan-level).
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Task.</returns>
        Task UpdateSeasonalDiscountOnPlanAsync(UpdateSeasonalDiscountRequestDto request);

        /// <summary>
        /// Gets the seasonal discount for a fee plan.
        /// </summary>
        /// <param name="feePlanId">The fee plan identifier.</param>
        /// <returns>Task&lt;DiscountDetailDto&gt;.</returns>
        Task<DiscountDetailDto> GetSeasonalDiscountOnPlanAsync(Guid feePlanId);

        /// <summary>
        /// Removes the seasonal discount from a fee plan.
        /// </summary>
        /// <param name="feePlanId">The fee plan identifier.</param>
        /// <returns>Task.</returns>
        Task RemoveSeasonalDiscountOnPlanAsync(Guid feePlanId);

        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>FeePlanDto.</returns>
        Task<FeePlanDto> CreateAsync(CreateFeePlanRequestDto request);

        /// <summary>
        /// Retrieves all fee plans.
        /// </summary>
        /// <returns>A collection of fee plan DTOs.</returns>
        Task<List<FeePlanDto>> GetAllAsync();

        /// <summary>
        /// Retrieves a fee plan by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the fee plan.</param>
        /// <returns>
        /// The matching fee plan DTO if found; otherwise, <c>null</c>.
        /// </returns>
        Task<FeePlanDto?> GetByIdAsync(Guid id);

        /// <summary>
        /// Updates an existing fee plan.
        /// </summary>
        /// <param name="id">The fee plan identifier.</param>
        /// <param name="request">The fee plan data.</param>
        /// <returns>The updated fee plan DTO.</returns>
        Task<FeePlanDto?> UpdateAsync(Guid id, CreateFeePlanRequestDto request);

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>id.</returns>
        Task<CommonResponse<bool>> DeleteAsync(Guid id);

        /// <summary>
        /// Gets filtered courses with pagination support.
        /// </summary>
        /// <param name="request">The paged request.</param>
        /// <returns>Paginated fee result.</returns>
        Task<CommonResponse<PagedResult<FeePlanDto>>> GetFilteredAsync(PagedRequest request);

        /// <summary>
        /// Gets the fee listing page for student portal.
        /// Shows all available course + payment type + duration combinations.
        /// </summary>
        /// <param name="studentId">The student identifier.</param>
        /// <returns>Fee page data for student portal.</returns>
        Task<CommonResponse<StudentFeePageDto>> GetStudentFeePageAsync(Guid studentId);

        /// <summary>
        /// Student confirms their fee selection.
        /// Creates StudentFeeSelection with applicable discounts auto-applied.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Summary of the confirmed fee selection, including final amount and applied discounts.</returns>
        Task<CommonResponse<FeeSummaryDto>> ConfirmFeeSelectionAsync(
            ConfirmFeeSelectionRequest request);
    }
}
