namespace Winfocus.LMS.Application.Interfaces
{
    using Winfocus.LMS.Domain.Entities;

    /// <summary>
    /// IStreamRepository.
    /// </summary>
    public interface IStreamRepository
    {
        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>Stream.</returns>
        Task<IReadOnlyList<Streams>> GetAllAsync();

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Stream.</returns>
        Task<Streams?> GetByIdAsync(Guid id);

        /// <summary>
        /// Adds the asynchronous.
        /// </summary>
        /// <param name="streams">The Streams.</param>
        /// <returns>Stream.</returns>
        Task<Streams> AddAsync(Streams streams);

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="streams">The Streams.</param>
        /// <returns>Streams.</returns>
        Task UpdateAsync(Streams streams);

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>task.</returns>
        Task DeleteAsync(Guid id);

        /// <summary>
        /// Existses the by code asynchronous.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <returns>bool.</returns>
        Task<bool> ExistsByCodeAsync(string code);

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="gradeid">The identifier.</param>
        /// <returns>Stream.</returns>
        Task<List<Streams>> GetByGradeIdAsync(Guid gradeid);

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Stream.</returns>
        Task<Streams?> GetByIdWithCoursesAsync(Guid id);
    }
}
