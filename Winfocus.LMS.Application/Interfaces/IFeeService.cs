namespace Winfocus.LMS.Application.Interfaces
{
    using Winfocus.LMS.Application.DTOs;
    using Winfocus.LMS.Application.DTOs.Common;
    using Winfocus.LMS.Application.DTOs.Fees;

    /// <summary>
    /// Defines the contract for fee-related business logic operations,
    /// including fee plan management, student fee workflows, discount assignment,
    /// payment tracking, and filtered selections.
    /// </summary>
    public interface IFeeService
    {
        // ── FeePlan CRUD (keep) ──

        /// <summary>
        /// Creates a new fee plan asynchronously.
        /// </summary>
        /// <param name="request">The request containing fee plan details.</param>
        /// <returns>A task whose result contains the created <see cref="FeePlanDto"/>.</returns>
        Task<FeePlanDto> CreateAsync(CreateFeePlanRequestDto request);

        /// <summary>
        /// Retrieves all fee plans asynchronously.
        /// </summary>
        /// <returns>A task whose result contains a list of <see cref="FeePlanDto"/> entities.</returns>
        Task<List<FeePlanDto>> GetAllAsync();

        /// <summary>
        /// Retrieves a fee plan by its unique identifier asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the fee plan.</param>
        /// <returns>A task whose result contains the <see cref="FeePlanDto"/> if found; otherwise, null.</returns>
        Task<FeePlanDto?> GetByIdAsync(Guid id);

        /// <summary>
        /// Updates an existing fee plan asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the fee plan.</param>
        /// <param name="request">The request containing updated fee plan details.</param>
        /// <returns>A task whose result contains the updated <see cref="FeePlanDto"/> if successful; otherwise, null.</returns>
        Task<FeePlanDto?> UpdateAsync(Guid id, CreateFeePlanRequestDto request);

        /// <summary>
        /// Deletes a fee plan by its unique identifier asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the fee plan.</param>
        /// <returns>A task whose result contains a <see cref="CommonResponse{T}"/> indicating success or failure.</returns>
        Task<CommonResponse<bool>> DeleteAsync(Guid id);

        /// <summary>
        /// Retrieves a filtered list of fee plans with pagination support asynchronously.
        /// </summary>
        /// <param name="request">The pagination and filter request.</param>
        /// <returns>A task whose result contains a paged list of <see cref="FeePlanDto"/> entities.</returns>
        Task<CommonResponse<PagedResult<FeePlanDto>>> GetFilteredAsync(PagedRequest request);

        // ── Admin student fee management ──

        /// <summary>
        /// Retrieves the admin student fee management page details for a specific student asynchronously.
        /// </summary>
        /// <param name="studentId">The unique identifier of the student.</param>
        /// <returns>A task whose result contains the <see cref="AdminStudentFeePageDto"/>.</returns>
        Task<CommonResponse<AdminStudentFeePageDto>> GetAdminStudentFeePageAsync(Guid studentId);

        /// <summary>
        /// Assigns discounts to a student for a specific course asynchronously.
        /// Replaces all previous discount assignments.
        /// </summary>
        /// <param name="request">The request containing discount assignment details.</param>
        /// <returns>A task whose result contains a <see cref="CommonResponse{T}"/> indicating success or failure.</returns>
        Task<CommonResponse<bool>> AssignDiscountsAsync(AssignDiscountsRequestDto request);

        /// <summary>
        /// Removes all discounts assigned to a student for a specific course asynchronously.
        /// </summary>
        /// <param name="studentId">The unique identifier of the student.</param>
        /// <param name="courseId">The unique identifier of the course.</param>
        /// <returns>A task whose result contains a <see cref="CommonResponse{T}"/> indicating success or failure.</returns>
        Task<CommonResponse<bool>> RemoveDiscountsAsync(Guid studentId, Guid courseId);

        // ── Student portal ──

        /// <summary>
        /// Retrieves the student fee page details for a specific student asynchronously.
        /// </summary>
        /// <param name="studentId">The unique identifier of the student.</param>
        /// <returns>A task whose result contains the <see cref="StudentFeePageDto"/>.</returns>
        Task<CommonResponse<StudentFeePageDto>> GetStudentFeePageAsync(Guid studentId);

        /// <summary>
        /// Confirms a student's fee plan selection asynchronously.
        /// </summary>
        /// <param name="request">The request containing fee confirmation details.</param>
        /// <returns>A task whose result contains the <see cref="ConfirmFeeResponseDto"/>.</returns>
        Task<CommonResponse<ConfirmFeeResponseDto>> ConfirmFeeSelectionAsync(ConfirmFeeRequestDto request);

        // ── Payment tracking ──

        /// <summary>
        /// Retrieves all installment schedules for a specific fee selection asynchronously.
        /// </summary>
        /// <param name="selectionId">The unique identifier of the fee selection.</param>
        /// <returns>A task whose result contains a list of <see cref="InstallmentScheduleDto"/> entities.</returns>
        Task<CommonResponse<List<InstallmentScheduleDto>>> GetInstallmentsAsync(Guid selectionId);

        /// <summary>
        /// Records a payment against a specific installment asynchronously.
        /// </summary>
        /// <param name="installmentId">The unique identifier of the installment.</param>
        /// <param name="request">The request containing payment details.</param>
        /// <returns>A task whose result contains the updated <see cref="InstallmentScheduleDto"/>.</returns>
        Task<CommonResponse<InstallmentScheduleDto>> RecordPaymentAsync(Guid installmentId, RecordPaymentRequestDto request);

        /// <summary>
        /// Retrieves a payment summary for a specific student asynchronously.
        /// </summary>
        /// <param name="studentId">The unique identifier of the student.</param>
        /// <returns>A task whose result contains the <see cref="PaymentSummaryDto"/>.</returns>
        Task<CommonResponse<PaymentSummaryDto>> GetPaymentSummaryAsync(Guid studentId);

        // ── Selections filter ──

        /// <summary>
        /// Retrieves a filtered list of student fee selections with pagination support asynchronously.
        /// </summary>
        /// <param name="request">The pagination and filter request.</param>
        /// <returns>A task whose result contains a paged list of <see cref="StudentFeeSelectionListDto"/> entities.</returns>
        Task<CommonResponse<PagedResult<StudentFeeSelectionListDto>>> GetSelectionsFilteredAsync(PagedRequest request);
    }
}
