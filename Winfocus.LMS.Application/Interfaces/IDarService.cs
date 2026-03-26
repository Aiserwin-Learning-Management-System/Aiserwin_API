namespace Winfocus.LMS.Application.Interfaces
{
    using Winfocus.LMS.Application.DTOs;
    using Winfocus.LMS.Application.DTOs.Common;
    using Winfocus.LMS.Application.DTOs.DtpAdmin;

    /// <summary>
    /// Service interface for Daily Activity Report business logic.
    /// </summary>
    public interface IDarService
    {
        /// <summary>
        /// Creates a new Daily Activity Report asynchronously.
        /// </summary>
        /// <param name="operatorId">The operator identifier.</param>
        /// <param name="request">The DAR request DTO.</param>
        /// <returns>CommonResponse containing the created DAR response DTO.</returns>
        Task<CommonResponse<DarResponseDto>> CreateAsync(Guid operatorId, DarRequestDto request);

        /// <summary>
        /// Updates a Draft Daily Activity Report asynchronously.
        /// </summary>
        /// <param name="operatorId">The operator identifier.</param>
        /// <param name="darId">The DAR identifier.</param>
        /// <param name="request">The DAR request DTO.</param>
        /// <returns>CommonResponse containing the updated DAR response DTO.</returns>
        Task<CommonResponse<DarResponseDto>> UpdateAsync(Guid operatorId, Guid darId, DarRequestDto request);

        /// <summary>
        /// Gets a Daily Activity Report by ID asynchronously.
        /// </summary>
        /// <param name="operatorId">The operator identifier.</param>
        /// <param name="darId">The DAR identifier.</param>
        /// <returns>CommonResponse containing the DAR response DTO.</returns>
        Task<CommonResponse<DarResponseDto>> GetByIdAsync(Guid operatorId, Guid darId);

        /// <summary>
        /// Gets paginated Daily Activity Reports for an operator asynchronously.
        /// </summary>
        /// <param name="operatorId">The operator identifier.</param>
        /// <param name="request">The pagination request.</param>
        /// <returns>CommonResponse containing paginated DAR list DTOs.</returns>
        Task<CommonResponse<PagedResult<DarListDto>>> GetListAsync(Guid operatorId, PagedRequest request);

        /// <summary>
        /// Gets today's Daily Activity Report or empty template asynchronously.
        /// </summary>
        /// <param name="operatorId">The operator identifier.</param>
        /// <returns>CommonResponse containing today's DAR DTO (exists flag + template).</returns>
        Task<CommonResponse<DarTodayDto>> GetTodayAsync(Guid operatorId);

        /// <summary>
        /// Submits a Draft Daily Activity Report (changes status to Submitted) asynchronously.
        /// </summary>
        /// <param name="operatorId">The operator identifier.</param>
        /// <param name="darId">The DAR identifier.</param>
        /// <returns>CommonResponse indicating success or failure.</returns>
        Task<CommonResponse<bool>> SubmitAsync(Guid operatorId, Guid darId);

        /// <summary>
        /// Gets all Daily Activity Reports for admin view asynchronously.
        /// </summary>
        /// <param name="filter">The filter request with optional operator ID and date range.</param>
        /// <returns>CommonResponse containing paginated DAR response DTOs.</returns>
        Task<CommonResponse<PagedResult<DarResponseDto>>> GetAllDarsAdminAsync(DarFilterRequest filter);

        /// <summary>
        /// Gets Daily Activity Reports for a specific operator (admin view) asynchronously.
        /// </summary>
        /// <param name="operatorId">The operator identifier.</param>
        /// <param name="filter">The filter request with date range.</param>
        /// <returns>CommonResponse containing paginated DAR response DTOs.</returns>
        Task<CommonResponse<PagedResult<DarResponseDto>>> GetOperatorDarsAdminAsync(
            Guid operatorId, DarFilterRequest filter);
    }
}
