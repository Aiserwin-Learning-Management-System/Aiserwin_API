namespace Winfocus.LMS.Application.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Winfocus.LMS.Domain.Entities; 

    /// <summary>
    /// Defines CRUD operations for <see cref="Centre"/> entities.
    /// </summary>
    public interface ICentreRepository
    {
        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>Center.</returns>
        Task<IReadOnlyList<Centre>> GetAllAsync();

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Center.</returns>
        Task<Centre?> GetByIdAsync(Guid id);

        /// <summary>
        /// Adds the asynchronous.
        /// </summary>
        /// <param name="center">The center.</param>
        /// <returns>Center.</returns>
        Task<Centre> AddAsync(Centre center);

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="center">The center.</param>
        /// <returns>Center.</returns>
        Task UpdateAsync(Centre center);

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
        /// Gets centre by mode of study and state.
        /// </summary>
        /// <param name="modeofid">Mode of study identifier.</param>
        /// <param name="stateid">State identifier.</param>
        /// <returns>Centre entity if found; otherwise null.</returns>
        Task<Centre?> GetByFilterAsync(Guid modeofid, Guid stateid);
    }
}
