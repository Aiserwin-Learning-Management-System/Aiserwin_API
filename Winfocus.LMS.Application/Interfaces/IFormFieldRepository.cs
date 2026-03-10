using System;
using System.Collections.Generic;
using System.Text;
using Winfocus.LMS.Domain.Entities;

namespace Winfocus.LMS.Application.Interfaces
{
    /// <summary>
    /// Repository abstraction for managing <see cref="FormField"/> entities.
    /// Handles data access operations including retrieval, creation,
    /// updates, soft deletion, and ordering logic.
    /// </summary>
    public interface IFormFieldRepository
    {
        /// <summary>
        /// Retrieves a form field by its unique identifier.
        /// Includes related <see cref="FieldGroup"/> and <see cref="FieldOption"/> entities.
        /// </summary>
        /// <param name="id">The form field identifier.</param>
        /// <returns>
        /// The matching <see cref="FormField"/> if found; otherwise <c>null</c>.
        /// </returns>
        Task<FormField?> GetByIdAsync(Guid id);

        /// <summary>
        /// Retrieves all non-deleted form fields including group and options.
        /// </summary>
        /// <returns>A collection of <see cref="FormField"/>.</returns>
        Task<List<FormField>> GetAllAsync();

        /// <summary>
        /// Retrieves all standalone fields that are not assigned to any group.
        /// </summary>
        /// <returns>A collection of ungrouped <see cref="FormField"/> entities.</returns>
        Task<List<FormField>> GetUngroupedAsync();

        /// <summary>
        /// Adds a new form field to the data store.
        /// </summary>
        /// <param name="field">The form field entity to create.</param>
        /// <returns>null.</returns>
        Task AddAsync(FormField field);

        /// <summary>
        /// Updates an existing form field.
        /// </summary>
        /// <param name="field">The form field entity to update.</param>
        /// <returns>null.</returns>
        Task UpdateAsync(FormField field);

        /// <summary>
        /// Performs a soft delete on the specified form field.
        /// </summary>
        /// <param name="field">The form field entity to delete.</param>
        /// <returns>null.</returns>
        Task SoftDeleteAsync(FormField field);

        /// <summary>
        /// Determines whether the specified field group exists.
        /// </summary>
        /// <param name="groupId">The field group identifier.</param>
        /// <returns>
        /// <c>true</c> if the group exists and is not deleted; otherwise <c>false</c>.
        /// </returns>
        Task<bool> FieldGroupExistsAsync(Guid groupId);

        /// <summary>
        /// Retrieves the next display order value within the specified group.
        /// </summary>
        /// <param name="groupId">
        /// The group identifier. If <c>null</c>, the order is calculated
        /// among standalone fields.
        /// </param>
        /// <returns>The next available display order.</returns>
        Task<int> GetNextDisplayOrderAsync(Guid? groupId);
    }
}
