using System;
using System.Collections.Generic;
using System.Text;

namespace Winfocus.LMS.Application.Interfaces
{
    using Winfocus.LMS.Domain.Entities;

    /// <summary>
    /// Defines business operations for <see cref="ModeOfStudy"/> entities.
    /// </summary>
    public interface IModeOfStudyService
    {
        /// <summary>
        /// Retrieves an active <see cref="ModeOfStudy"/> by its identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the ModeOfStudy.</param>
        /// <returns>
        /// A <see cref="Task{TResult}"/> that resolves to the matching <see cref="ModeOfStudy"/> if found and active; otherwise <c>null</c>.
        /// </returns>
        Task<ModeOfStudy?> GetByIdAsync(Guid id);

        /// <summary>
        /// Retrieves all active <see cref="ModeOfStudy"/> entities.
        /// </summary>
        /// <returns>A task resolving to a list of active ModeOfStudy.</returns>
        Task<List<ModeOfStudy>> GetAllAsync();

        /// <summary>
        /// Creates a new <see cref="ModeOfStudy"/>.
        /// </summary>
        /// <param name="mode">The ModeOfStudy to create.</param>
        /// <returns>The created <see cref="ModeOfStudy"/>.</returns>
        Task<ModeOfStudy> CreateAsync(ModeOfStudy mode);

        /// <summary>
        /// Updates an existing active <see cref="ModeOfStudy"/>.
        /// </summary>
        /// <param name="mode">The ModeOfStudy containing updated values.</param>
        /// <returns>The updated <see cref="ModeOfStudy"/> if the operation succeeded; otherwise <c>null</c>.</returns>
        Task<ModeOfStudy?> UpdateAsync(ModeOfStudy mode);

        /// <summary>
        /// Soft-deletes a <see cref="ModeOfStudy"/> by marking it inactive.
        /// </summary>
        /// <param name="id">The identifier of the mode of study to delete.</param>
        /// <returns><c>true</c> if the ModeOfStudy was found and marked inactive; otherwise <c>false</c>.</returns>
        Task<bool> DeleteAsync(Guid id);
    }
}
