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
        /// <param name="centerId">The centerId.</param>
        /// <returns>Stream.</returns>
        Task<IReadOnlyList<Streams>> GetAllAsync(Guid centerId);

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Stream.</returns>
        Task<Streams?> GetByIdAsync(Guid id);

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="centerId">The centerId.</param>
        /// <returns>Stream.</returns>
        Task<Streams?> GetByIdCenterIdAsync(Guid id, Guid centerId);

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
        Task<Streams> UpdateAsync(Streams streams);

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="centerId">The centerId.</param>
        /// <returns>task.</returns>
        Task<bool> DeleteAsync(Guid id, Guid centerId);

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

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <param name="centerId">The centerId.</param>
        /// <returns>Streams.</returns>
        IQueryable<Streams> Query(Guid centerId);
    }
}
