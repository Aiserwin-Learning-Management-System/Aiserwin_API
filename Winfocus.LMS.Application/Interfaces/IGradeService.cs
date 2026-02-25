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
        /// <returns>GradeDto.</returns>
        Task<CommonResponse<List<GradeDto>>> GetAllAsync();

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>GradeDto.</returns>
        Task<CommonResponse<GradeDto>> GetByIdAsync(Guid id);

        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>GradeDto.</returns>
        Task<GradeDto> CreateAsync(GradeRequest request);

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="request">The request.</param>
        /// <returns>id.</returns>
        Task<GradeDto> UpdateAsync(Guid id, GradeRequest request);

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>id.</returns>
        Task<bool> DeleteAsync(Guid id);

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="syllabusid">The identifier.</param>
        /// <returns>GradeDto.</returns>
        Task<CommonResponse<List<GradeDto>>> GetBySyllabusIdAsync(Guid syllabusid);

        /// <summary>
        /// Gets filtered grades with pagination support.
        /// </summary>
        /// <param name="syllabusId">Syllabus identifier.</param>
        /// <param name="startDate">Filter grades created after this date.</param>
        /// <param name="endDate">Filter grades created before this date.</param>
        /// <param name="active">Filter by active status.</param>
        /// <param name="searchText">Search keyword.</param>
        /// <param name="limit">Number of records to return.</param>
        /// <param name="offset">Number of records to skip.</param>
        /// <param name="sortOrder">Sorting order (asc or desc).</param>
        /// <returns>Paginated grade result.</returns>
        Task<CommonResponse<PagedResult<GradeDto>>> GetFilteredAsync(
            Guid? syllabusId,
            DateTime? startDate,
            DateTime? endDate,
            bool? active,
            string? searchText,
            int limit,
            int offset,
            string sortOrder);
    }
}
