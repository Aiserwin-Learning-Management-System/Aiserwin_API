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
        /// <returns>BatchTimingSunday.</returns>
        Task<IReadOnlyList<BatchTimingSunday>> GetAllAsync();

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>BatchTimingSunday.</returns>
        Task<BatchTimingSunday?> GetByIdAsync(Guid id);

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
        Task UpdateAsync(BatchTimingSunday batchtiming);

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>task.</returns>
        Task DeleteAsync(Guid id);

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="subjectid">The identifier.</param>
        /// <returns>BatchTiming.</returns>
        Task<List<BatchTimingSunday>> GetBySubjectIdAsync(Guid subjectid);
    }
}
