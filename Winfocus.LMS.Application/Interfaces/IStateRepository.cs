namespace Winfocus.LMS.Application.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Winfocus.LMS.Application.DTOs;
    using Winfocus.LMS.Application.DTOs.Masters;
    using Winfocus.LMS.Domain.Entities;

    /// <summary>
    /// Defines CRUD operations for <see cref="State"/> entities.
    /// </summary>
    public interface IStateRepository
    {
     /// <summary>
     /// Gets all asynchronous.
     /// </summary>
     /// <returns>State.</returns>
        Task<IReadOnlyList<State>> GetAllAsync();

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>State.</returns>
        Task<State?> GetByIdAsync(Guid id);

        /// <summary>
        /// Adds the asynchronous.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <returns>State.</returns>
        Task<State> AddAsync(State state);

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <returns>State.</returns>
        Task UpdateAsync(State state);

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
