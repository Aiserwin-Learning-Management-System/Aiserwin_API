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
        Task<CommonResponse<SyllabusDto>> CreateAsync(SyllabusRequest request);

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="request">The request.</param>
        /// <returns>id.</returns>
        Task<CommonResponse<SyllabusDto>> UpdateAsync(Guid id, SyllabusRequest request);

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>id.</returns>
        Task<CommonResponse<bool>> DeleteAsync(Guid id);

        /// <summary>
        /// Gets filtered syllabuses with pagination support.
        /// </summary>
        /// <param name="request">The paged request.</param>
        /// <returns>Paginated syllabus result.</returns>
        Task<CommonResponse<PagedResult<SyllabusDto>>> GetFilteredAsync(PagedRequest request);

        /// <summary>
        /// Gets the by center identifier asynchronous.
        /// </summary>
        /// <param name="centerId">The center identifier.</param>
        /// <returns>List of SyllabusDto.</returns>
        Task<CommonResponse<List<SyllabusDto>>> GetByCenterIdAsync(Guid centerId);
    }
}
