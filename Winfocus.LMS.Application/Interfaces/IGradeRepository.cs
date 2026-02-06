using Winfocus.LMS.Domain.Entities;

namespace Winfocus.LMS.Application.Interfaces
{
    /// <summary>
    /// IGradeRepository.
    /// </summary>
    public interface IGradeRepository
    {
        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>Grade.</returns>
        Task<IReadOnlyList<Grade>> GetAllAsync();

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Grade.</returns>
        Task<Grade?> GetByIdAsync(Guid id);

        /// <summary>
        /// Adds the asynchronous.
        /// </summary>
        /// <param name="grade">The grade.</param>
        /// <returns>Grade.</returns>
        Task<Grade> AddAsync(Grade grade);

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="grade">The Grade.</param>
        /// <returns>Grade.</returns>
        Task UpdateAsync(Grade grade);

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
    }
}
