namespace Winfocus.LMS.Application.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Winfocus.LMS.Domain.Entities;

    /// <summary>
    /// Defines business operations for <see cref="State"/> entities.
    /// </summary>
    public interface IStateService
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
        /// Retrieves an active <see cref="State"/> by its identifier.
        /// </summary>
        /// <param name="countryid">The unique identifier of the state.</param>
        /// <returns>
        /// A <see cref="Task{TResult}"/> that resolves to the matching <see cref="State"/> if found and active; otherwise <c>null</c>.
        /// </returns>
        Task<State?> GetByCountryIdAsync(Guid countryid);

        /// <summary>
        /// Retrieves all active <see cref="State"/> entities.
        /// </summary>
        /// <returns>A task resolving to a list of active states.</returns>
        Task<List<State>> GetAllAsync();

        /// <summary>
        /// Creates a new <see cref="State"/>.
        /// </summary>
        /// <param name="state">The state to create.</param>
        /// <returns>The created <see cref="State"/>.</returns>
        Task<State> CreateAsync(State state);

        /// <summary>
        /// Updates an existing active <see cref="State"/>.
        /// </summary>
        /// <param name="state">The state containing updated values.</param>
        /// <returns>The updated <see cref="State"/> if the operation succeeded; otherwise <c>null</c>.</returns>
        Task<State?> UpdateAsync(State state);

        /// <summary>
        /// Soft-deletes a <see cref="State"/> by marking it inactive.
        /// </summary>
        /// <param name="id">The identifier of the state to delete.</param>
        /// <returns><c>true</c> if the state was found and marked inactive; otherwise <c>false</c>.</returns>
        Task<bool> DeleteAsync(Guid id);
    }
}
