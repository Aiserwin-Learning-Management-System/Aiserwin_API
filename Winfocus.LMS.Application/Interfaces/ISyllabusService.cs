namespace Winfocus.LMS.Application.Interfaces
{
    using Winfocus.LMS.Application.DTOs;
    using Winfocus.LMS.Application.DTOs.Common;
    using Winfocus.LMS.Application.DTOs.Masters;

    /// <summary>
    /// ISyllabusService.
    /// </summary>
    public interface ISyllabusService
    {
        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>SyllabusDto.</returns>
        Task<CommonResponse<List<SyllabusDto>>> GetAllAsync();

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>SyllabusDto.</returns>
        Task<CommonResponse<SyllabusDto>> GetByIdAsync(Guid id);

        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>SyllabusDto.</returns>
        Task<SyllabusDto> CreateAsync(SyllabusRequest request);

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="request">The request.</param>
        /// <returns>id.</returns>
        Task<SyllabusDto> UpdateAsync(Guid id, SyllabusRequest request);

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>id.</returns>
        Task<bool> DeleteAsync(Guid id);

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="centerid">The identifier.</param>
        /// <returns>SyllabusDto.</returns>
        Task<List<SyllabusDto>> GetByCenterIdAsync(Guid centerid);

        /// <summary>
        /// Gets filtered courses with pagination support.
        /// </summary>
        /// <param name="startDate">Filter courses created after this date.</param>
        /// <param name="endDate">Filter courses created before this date.</param>
        /// <param name="active">Filter by active status.</param>
        /// <param name="searchText">Search keyword.</param>
        /// <param name="limit">Number of records to return.</param>
        /// <param name="offset">Number of records to skip.</param>
        /// <param name="sortOrder">Sorting order (asc or desc).</param>
        /// <returns>Paginated course result.</returns>
        Task<CommonResponse<PagedResult<SyllabusDto>>> GetFilteredAsync(
            DateTime? startDate,
            DateTime? endDate,
            bool? active,
            string? searchText,
            int limit,
            int offset,
            string sortOrder);
    }
}
