using System;
using System.Collections.Generic;
using System.Text;
using Winfocus.LMS.Application.DTOs;
using Winfocus.LMS.Domain.Entities;

namespace Winfocus.LMS.Application.Interfaces
{
    /// <summary>
    /// IFieldGroupRepository.
    /// </summary>
    public interface IFieldGroupRepository
    {
        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>FieldGroup.</returns>
        Task<IReadOnlyList<FieldGroup>> GetAllAsync();

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>FieldGroup.</returns>
        Task<FieldGroup?> GetByIdAsync(Guid id);

        /// <summary>
        /// Adds the asynchronous.
        /// </summary>
        /// <param name="fieldgroup">The fieldgroup.</param>
        /// <returns>FieldGroup.</returns>
        Task<FieldGroup> AddAsync(FieldGroup fieldgroup);

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="fieldgroup">The fieldgroup.</param>
        /// <returns>fieldgroup.</returns>
        Task<FieldGroup> UpdateAsync(FieldGroup fieldgroup);

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>task.</returns>
        Task<bool> DeleteAsync(Guid id);

        /// <summary>
        /// Existses the by code asynchronous.
        /// </summary>
        /// <param name="name">The code.</param>
        /// <returns>bool.</returns>
        Task<bool> ExistsByNameAsync(string name);

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="groupId">The identifier.</param>
        /// <returns>FieldGroup.</returns>
        Task<FieldGroupFieldsResponseDto?> GetFieldsByGroupIdAsync(Guid groupId);

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>fieldgroup.</returns>
        IQueryable<FieldGroup> Query();
    }
}
