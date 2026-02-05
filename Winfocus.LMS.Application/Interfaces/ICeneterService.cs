using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Winfocus.LMS.Domain.Entities;

namespace Winfocus.LMS.Application.Interfaces
{
    /// <summary>
    /// Defines business operations for <see cref="Centre"/> entities.
    /// </summary>
    public interface ICentreService
    {
        /// <summary>
        /// Retrieves an active <see cref="Centre"/> by its identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the centre.</param>
        /// <returns>
        /// A <see cref="Task{TResult}"/> that resolves to the matching <see cref="Centre"/> if found and active; otherwise <c>null</c>.
        /// </returns>
        Task<Centre?> GetByIdAsync(Guid id);

        /// <summary>
        /// Retrieves all active <see cref="Centre"/> entities.
        /// </summary>
        /// <returns>A task resolving to a list of active centres.</returns>
        Task<List<Centre>> GetAllAsync();

        /// <summary>
        /// Creates a new <see cref="Centre"/>.
        /// </summary>
        /// <param name="centre">The centre to create.</param>
        /// <returns>The created <see cref="Centre"/>.</returns>
        Task<Centre> CreateAsync(Centre centre);

        /// <summary>
        /// Updates an existing active <see cref="Centre"/>.
        /// </summary>
        /// <param name="centre">The centre containing updated values.</param>
        /// <returns>The updated <see cref="Centre"/> if the operation succeeded; otherwise <c>null</c>.</returns>
        Task<Centre?> UpdateAsync(Centre centre);

        /// <summary>
        /// Soft-deletes a <see cref="Centre"/> by marking it inactive.
        /// </summary>
        /// <param name="id">The identifier of the centre to delete.</param>
        /// <returns><c>true</c> if the centre was found and marked inactive; otherwise <c>false</c>.</returns>
        Task<bool> DeleteAsync(Guid id);
    }
}
