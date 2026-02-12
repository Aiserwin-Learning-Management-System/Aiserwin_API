namespace Winfocus.LMS.Application.Interfaces
{
    using Winfocus.LMS.Application.DTOs;
    using Winfocus.LMS.Application.DTOs.Masters;

    /// <summary>
    /// Defines business operations for <see cref="BatchTimingMTF"/> entities.
    /// </summary>
    public interface IBatchTimingMTFService
    {
        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>BatchTimingMTFDto.</returns>
        Task<IReadOnlyList<BatchTimingMTFDto>> GetAllAsync();

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>BatchTimingMTFDto.</returns>
        Task<BatchTimingMTFDto?> GetByIdAsync(Guid id);

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
        Task UpdateAsync(Guid id, BatchTimingRequest request);

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>id.</returns>
        Task DeleteAsync(Guid id);

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
    }
}
