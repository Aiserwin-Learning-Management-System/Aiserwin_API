namespace Winfocus.LMS.Application.Interfaces
{
    using Winfocus.LMS.Application.DTOs;
    using Winfocus.LMS.Application.DTOs.Common;
    using Winfocus.LMS.Application.DTOs.Masters;

    /// <summary>
    /// Subject service contract.
    /// </summary>
    public interface ISubjectService
    {
        /// <summary>
        /// Gets all active subjects.
        /// </summary>
        /// <param name="centerId">The centerId.</param>
        /// <returns>A list of subject DTOs.</returns>
        Task<CommonResponse<List<SubjectDto>>> GetAllAsync(Guid centerId);

        /// <summary>
        /// Gets a subject by identifier.
        /// </summary>
        /// <param name="id">The subject identifier.</param>
        /// <returns>The subject DTO if found; otherwise, null.</returns>
        Task<CommonResponse<SubjectDto>> GetByIdAsync(Guid id);

        /// <summary>
        /// Gets a subject by identifier.
        /// </summary>
        /// <param name="id">The subject identifier.</param>
        /// <param name="centerId">The centerId.</param>
        /// <returns>The subject DTO if found; otherwise, null.</returns>
        Task<CommonResponse<SubjectDto>> GetByIdCenterIdAsync(Guid id, Guid centerId);

        /// <summary>
        /// Gets subjects by stream identifier.
        /// </summary>
        /// <param name="streamId">The stream identifier.</param>
        /// <returns>A list of subject DTOs associated with the specified stream.</returns>
        Task<CommonResponse<List<SubjectDto>>> GetByStreamAsync(Guid streamId);

        /// <summary>
        /// Gets the by course ids asynchronous.
        /// </summary>
        /// <param name="courseIds">The course ids.</param>
        /// <returns>list.</returns>
        Task<CommonResponse<List<SubjectDto>>> GetByCourseIdsAsync(List<Guid> courseIds);

        /// <summary>
        /// Creates a new subject.
        /// </summary>
        /// <param name="request">The subject creation request.</param>
        /// <returns>The created subject DTO.</returns>
        Task<CommonResponse<SubjectDto>> CreateAsync(SubjectRequest request);

        /// <summary>
        /// Updates an existing subject.
        /// </summary>
        /// <param name="id">The subject identifier.</param>
        /// <param name="request">The subject update request.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task<CommonResponse<SubjectDto>> UpdateAsync(Guid id, SubjectRequest request);

        /// <summary>
        /// Soft deletes a subject.
        /// </summary>
        /// <param name="id">The subject identifier.</param>
        /// <param name="centerId">The centerId.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task<CommonResponse<bool>> DeleteAsync(Guid id, Guid centerId);

        /// <summary>
        /// Gets filtered subjects with pagination support.
        /// </summary>
        /// <param name="request">The paged request.</param>
        /// <param name="centerId">The centerId.</param>
        /// <returns>Paginated subjects result.</returns>
        Task<CommonResponse<PagedResult<SubjectDto>>> GetFilteredAsync(PagedRequest request, Guid centerId);
    }
}
