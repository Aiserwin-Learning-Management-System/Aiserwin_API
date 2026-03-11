namespace Winfocus.LMS.Application.Interfaces
{
    using Winfocus.LMS.Application.DTOs;
    using Winfocus.LMS.Application.DTOs.Common;
    using Winfocus.LMS.Application.DTOs.Masters;

    /// <summary>
    /// Defines business operations for <see cref="BatchTimingMTF"/> entities.
    /// </summary>
    public interface IBatchTimingMTFService
    {
        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <param name="centerId">The centerId.</param>
        /// <returns>BatchTimingMTFDto.</returns>
        Task<CommonResponse<List<BatchTimingMTFDto>>> GetAllAsync(Guid centerId);

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>BatchTimingMTFDto.</returns>
        Task<CommonResponse<BatchTimingMTFDto>> GetByIdAsync(Guid id);

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="centerId">The centerId.</param>
        /// <returns>BatchTimingMTFDto.</returns>
        Task<CommonResponse<BatchTimingMTFDto>> GetByIdCenterIdAsync(Guid id, Guid centerId);

        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>BatchTimingMTFDto.</returns>
        Task<BatchTimingMTFDto> CreateAsync(BatchTimingRequest request);

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="request">The request.</param>
        /// <returns>id.</returns>
        Task<BatchTimingMTFDto> UpdateAsync(Guid id, BatchTimingRequest request);

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="centerId">The centerId.</param>
        /// <returns>id.</returns>
        Task<bool> DeleteAsync(Guid id, Guid centerId);

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="subjectid">The identifier.</param>
        /// <returns>BatchTimingMTFDto.</returns>
        Task<List<BatchTimingMTFDto>> GetBySubjectIdAsync(Guid subjectid);

        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>.</returns>
        Task BatchTimingSubjectCreate(SubjectBatchTimingRequest request);

        /// <summary>
        /// Gets filtered batch timing for monday to friday with pagination support.
        /// </summary>
        /// <param name="request">The paged request.</param>
        /// <param name="centerId">The centerId.</param>
        /// <returns>Paginated batch timing for monday to frida result.</returns>
        Task<CommonResponse<PagedResult<BatchTimingMTFDto>>> GetFilteredAsync(PagedRequest request, Guid centerId);
    }
}
