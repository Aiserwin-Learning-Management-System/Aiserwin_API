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
        Task<RegistrationForm> AddAsync(RegistrationForm form);

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

        /// <summary>
        /// Adds registration form groups.
        /// </summary>
        /// <param name="groups">List of groups to save.</param>
        /// <returns><placeholder>A <see cref="Task"/> representing the asynchronous operation.</placeholder></returns>
        Task AddGroupsAsync(List<RegistrationFormGroup> groups);

        /// <summary>
        /// Adds registration form fields.
        /// </summary>
        /// <param name="fields">List of fields to save.</param>
        /// <returns><placeholder>A <see cref="Task"/> representing the asynchronous operation.</placeholder></returns>
        Task AddFieldsAsync(List<RegistrationFormField> fields);

        /// <summary>
        /// Gets fields belonging to a field group.
        /// Used when auto-populating fields during form creation.
        /// </summary>
        /// <param name="groupId">Field group identifier.</param>
        /// <returns><placeholder>A <see cref="Task"/> representing the asynchronous operation.</placeholder></returns>
        Task<List<FormField>> GetByGroupIdAsync(Guid groupId);

        /// <summary>
        /// Soft deletes a form.
        /// </summary>
        /// <param name="formId">The id.</param>
        /// <returns>.</returns>
        Task DeleteGroupsByFormIdAsync(Guid formId);

        /// <summary>
        /// Soft deletes a form.
        /// </summary>
        /// <param name="formId">The id.</param>
        /// <returns>.</returns>
        Task DeleteFieldsByFormIdAsync(Guid formId);
    }
}
