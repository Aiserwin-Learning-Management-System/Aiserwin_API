namespace Winfocus.LMS.Application.Interfaces
{
    using Winfocus.LMS.Application.DTOs;
    using Winfocus.LMS.Application.DTOs.Common;
    using Winfocus.LMS.Application.DTOs.Masters;

    /// <summary>
    /// Defines business operations for <see cref="BatchTimingSaturday"/> entities.
    /// </summary>
    public interface IBatchTimingSaturdayService
    {
        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>BatchTimingSaturdayDto.</returns>
        Task<CommonResponse<List<BatchTimingSaturdayDto>>> GetAllAsync();

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>BatchTimingSaturdayDto.</returns>
        Task<CommonResponse<BatchTimingSaturdayDto>> GetByIdAsync(Guid id);

        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>BatchTimingSaturdayDto.</returns>
        Task<CommonResponse<BatchTimingSaturdayDto>> CreateAsync(BatchTimingRequest request);

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="request">The request.</param>
        /// <returns>id.</returns>
        Task<CommonResponse<BatchTimingSaturdayDto>> UpdateAsync(Guid id, BatchTimingRequest request);

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>id.</returns>
        Task<CommonResponse<bool>> DeleteAsync(Guid id);

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="subjectid">The identifier.</param>
        /// <returns>BatchTimingSaturdayDto.</returns>
        Task<CommonResponse<List<BatchTimingSaturdayDto>>> GetBySubjectIdAsync(Guid subjectid);

        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>.</returns>
        Task BatchTimingSubjectCreate(SubjectBatchTimingRequest request);

        /// <summary>
        /// Gets filtered batch timing for saturday with pagination support.
        /// </summary>
        /// <param name="request">The paged request.</param>
        /// <returns>Paginated batch timing for saturday result.</returns>
        Task<CommonResponse<PagedResult<BatchTimingSaturdayDto>>> GetFilteredAsync(PagedRequest request);
    }
}
