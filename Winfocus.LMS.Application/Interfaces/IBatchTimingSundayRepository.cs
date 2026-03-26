namespace Winfocus.LMS.Application.Interfaces
{
    using Winfocus.LMS.Domain.Entities;

    /// <summary>
    /// Defines business operations for <see cref="BatchTimingSunday"/> entities.
    /// </summary>
    public interface IBatchTimingSundayRepository
    {
        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <param name="centerId">The centerId.</param>
        /// <returns>BatchTimingSunday.</returns>
        Task<IReadOnlyList<BatchTimingSunday>> GetAllAsync(Guid centerId);

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>BatchTimingSunday.</returns>
        Task<BatchTimingSunday?> GetByIdAsync(Guid id);

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="centerId">The centerId.</param>
        /// <returns>BatchTimingSunday.</returns>
        Task<BatchTimingSunday?> GetByIdCenterIdAsync(Guid id, Guid centerId);

        /// <summary>
        /// Adds the asynchronous.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <returns>BatchTimingSunday.</returns>
        Task<BatchTimingSunday> AddAsync(BatchTimingSunday state);

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="batchtiming">The state.</param>
        /// <returns>BatchTimingSunday.</returns>
        Task<BatchTimingSunday> UpdateAsync(BatchTimingSunday batchtiming);

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="centerId">The centerId.</param>
        /// <returns>task.</returns>
        Task<bool> DeleteAsync(Guid id, Guid centerId);

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="subjectid">The identifier.</param>
        /// <returns>BatchTiming.</returns>
        Task<List<BatchTimingSunday>> GetBySubjectIdAsync(Guid subjectid);

        /// <summary>
        /// Adds the asynchronous.
        /// </summary>
        /// <param name="batchTimingSunday">The batchTimingMTF.</param>
        /// <returns>batchTimingSunday.</returns>
        Task<SubjectBatchTimingSunday> BatchTimingSubjectCreate(SubjectBatchTimingSunday batchTimingSunday);

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <param name="centerId">The centerId.</param>
        /// <returns>batch timing.</returns>
        IQueryable<BatchTimingSunday> Query(Guid centerId);
    }
}
