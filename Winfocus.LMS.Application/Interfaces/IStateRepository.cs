namespace Winfocus.LMS.Application.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Winfocus.LMS.Domain.Entities;

    /// <summary>
    /// Defines CRUD operations for <see cref="State"/> entities.
    /// </summary>
    public interface IStateRepository
    {
        /// <summary>
        /// Retrieves an active <see cref="State"/> by its identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the state.</param>
        /// <returns>
        /// A <see cref="Task{TResult}"/> that resolves to the matching <see cref="State"/> if found and active; otherwise <c>null</c>.
        /// </returns>
        Task<State?> GetByIdAsync(Guid id);

        /// <summary>
        /// Retrieves all active <see cref="State"/> entities.
        /// </summary>
        /// <returns>A task resolving to a read-only list of active states.</returns>
        Task<List<State>> GetAllAsync();

        /// <summary>
        /// Creates a new <see cref="State"/> in the database.
        /// </summary>
        /// <param name="state">The state entity to create.</param>
        /// <returns>The created <see cref="State"/> with its assigned identifier.</returns>
        Task<State> CreateAsync(State state);

        /// <summary>
        /// Updates an existing active <see cref="State"/>.
        /// </summary>
        /// <param name="state">The state entity containing updated values.</param>
        /// <returns>The updated <see cref="State"/> if the entity existed and was updated; otherwise <c>null</c>.</returns>
        Task<State?> UpdateAsync(State state);

        /// <summary>
        /// Soft-deletes an existing <see cref="State"/> by marking it inactive.
        /// </summary>
        /// <param name="id">The identifier of the state to delete.</param>
        /// <returns><c>true</c> if the state was found and marked inactive; otherwise <c>false</c>.</returns>
        Task<bool> DeleteAsync(Guid id);
        /// <summary>
        /// Retrieves an active <see cref="State"/> by its identifier.
        /// </summary>
        /// <param name="name">The unique identifier of the state.</param>
        /// <returns>
        /// A <see cref="Task{TResult}"/> that resolves to the matching <see cref="State"/> if found and active; otherwise <c>null</c>.
        /// </returns>
        public Task<State?> GetByNameAsync(string name);

        /// <summary>
        /// Retrieves an active <see cref="State"/> by its identifier.
        /// </summary>
        /// <param name="countryid">The unique identifier of the state.</param>
        /// <returns>
        /// A <see cref="Task{TResult}"/> that resolves to the matching <see cref="State"/> if found and active; otherwise <c>null</c>.
        /// </returns>
        public Task<State?> GetByCountryAsync(Guid countryid);
    }
}
