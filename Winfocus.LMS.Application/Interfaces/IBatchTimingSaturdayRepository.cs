namespace Winfocus.LMS.Application.Interfaces
{
    using Winfocus.LMS.Domain.Entities;

    /// <summary>
    /// Defines business operations for <see cref="BatchTimingSaturday"/> entities.
    /// </summary>
    public interface IBatchTimingSaturdayRepository
    {
        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <param name="centerId">The centerId.</param>
        /// <returns>BatchTimingSaturday.</returns>
        Task<IReadOnlyList<BatchTimingSaturday>> GetAllAsync(Guid centerId);

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>BatchTimingSaturday.</returns>
        Task<BatchTimingSaturday?> GetByIdAsync(Guid id);

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="centerId">The centerId.</param>
        /// <returns>BatchTimingSaturday.</returns>
        Task<BatchTimingSaturday?> GetByIdCenterIdAsync(Guid id, Guid centerId);

        /// <summary>
        /// Adds the asynchronous.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <returns>BatchTimingSaturday.</returns>
        Task<BatchTimingSaturday> AddAsync(BatchTimingSaturday state);

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="batchtiming">The state.</param>
        /// <returns>BatchTimingSaturday.</returns>
        Task<BatchTimingSaturday> UpdateAsync(BatchTimingSaturday batchtiming);

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
        Task<List<BatchTimingSaturday>> GetBySubjectIdAsync(Guid subjectid);

        /// <summary>
        /// Adds the asynchronous.
        /// </summary>
        /// <param name="batchTimingSaturday">The batchTimingMTF.</param>
        /// <returns>batchTimingsaturday.</returns>
        Task<SubjectBatchTimingSaturday> BatchTimingSubjectCreate(SubjectBatchTimingSaturday batchTimingSaturday);

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <param name="centerId">The centerId.</param>
        /// <returns>batch timing.</returns>
        IQueryable<BatchTimingSaturday> Query(Guid centerId);
    }
}
