namespace Winfocus.LMS.Application.Interfaces
{
    using Winfocus.LMS.Application.DTOs;
    using Winfocus.LMS.Application.DTOs.Common;
    using Winfocus.LMS.Application.DTOs.Masters;

    /// <summary>
    /// ICountryService.
    /// </summary>
    public interface IGradeService
    {
        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <param name="centerId">The centerId.</param>
        /// <returns>GradeDto.</returns>
        Task<CommonResponse<List<GradeDto>>> GetAllAsync(Guid centerId);

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>GradeDto.</returns>
        Task<CommonResponse<GradeDto>> GetByIdAsync(Guid id);

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="centerId">The centerId.</param>
        /// <returns>GradeDto.</returns>
        Task<CommonResponse<GradeDto>> GetByIdCenterIdAsync(Guid id, Guid centerId);

        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>GradeDto.</returns>
        Task<CommonResponse<GradeDto>> CreateAsync(GradeRequest request);

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="request">The request.</param>
        /// <returns>id.</returns>
        Task<CommonResponse<GradeDto>> UpdateAsync(Guid id, GradeRequest request);

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>id.</returns>
        Task<CommonResponse<bool>> DeleteAsync(Guid id);

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="syllabusid">The identifier.</param>
        /// <returns>GradeDto.</returns>
        Task<CommonResponse<List<GradeDto>>> GetBySyllabusIdAsync(Guid syllabusid);

        /// <summary>
        /// Gets filtered grades with pagination support.
        /// </summary>
        /// <param name="request">The paged request.</param>
        /// <param name="centerId">The centerId.</param>
        /// <returns>Paginated grade result.</returns>
        Task<CommonResponse<PagedResult<GradeDto>>> GetFilteredAsync(PagedRequest request, Guid centerId);
    }
}
