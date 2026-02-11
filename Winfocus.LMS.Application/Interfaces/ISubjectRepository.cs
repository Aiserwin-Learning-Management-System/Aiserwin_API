namespace Winfocus.LMS.Application.Interfaces
{
    using Winfocus.LMS.Domain.Entities;

    /// <summary>
    /// ISubjectRepository.
    /// </summary>
    public interface ISubjectRepository
    {
        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>Subject list.</returns>
        Task<IReadOnlyList<Subject>> GetAllAsync();

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Subject.</returns>
        Task<Subject?> GetByIdAsync(Guid id);

        /// <summary>
        /// Gets the by stream asynchronous.
        /// </summary>
        /// <param name="streamId">The stream identifier.</param>
        /// <returns>Subject list.</returns>
        Task<IReadOnlyList<Subject>> GetByStreamAsync(Guid streamId);

        /// <summary>
        /// Adds the asynchronous.
        /// </summary>
        /// <param name="subject">The subject.</param>
        /// <returns>Subject.</returns>
        Task<Subject> AddAsync(Subject subject);

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="subject">The subject.</param>
        /// <returns>Task.</returns>
        Task UpdateAsync(Subject subject);

        /// <summary>
        /// Soft deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Task.</returns>
        Task SoftDeleteAsync(Guid id);
    }
}
