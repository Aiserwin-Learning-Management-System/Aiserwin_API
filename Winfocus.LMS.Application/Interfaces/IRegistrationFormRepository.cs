using System;
using System.Collections.Generic;
using System.Text;
using Winfocus.LMS.Domain.Entities;

namespace Winfocus.LMS.Application.Interfaces
{
    /// <summary>
    /// Repository contract for managing registration forms.
    /// </summary>
    public interface IRegistrationFormRepository
    {
        /// <summary>
        /// Creates a new registration form.
        /// </summary>
        /// <param name="form">The form.</param>
        /// <returns>.</returns>
        Task AddAsync(RegistrationForm form);

        /// <summary>
        /// Gets a registration form by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>.</returns>
        Task<RegistrationForm> GetByIdAsync(Guid id);

        /// <summary>
        /// Returns all registration forms.
        /// </summary>
        /// <param name="form">The form.</param>
        /// <returns>.</returns>
        Task<List<RegistrationForm>> GetAllAsync();

        /// <summary>
        /// Updates an existing form.
        /// </summary>
        /// <param name="form">The form.</param>
        /// <returns>.</returns>
        Task UpdateAsync(RegistrationForm form);

        /// <summary>
        /// Soft deletes a form.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>.</returns>
        Task DeleteAsync(Guid id);
    }
}
