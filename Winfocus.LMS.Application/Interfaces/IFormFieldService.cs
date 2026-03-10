using System;
using System.Collections.Generic;
using System.Text;
using Winfocus.LMS.Application.DTOs;

namespace Winfocus.LMS.Application.Interfaces
{
    /// <summary>
    /// Provides business logic for managing dynamic form fields.
    /// Handles validation, grouping logic, ordering, and option synchronization.
    /// </summary>
    public interface IFormFieldService
    {
        /// <summary>
        /// Creates a new dynamic form field.
        /// </summary>
        /// <param name="dto">The field creation request.</param>
        /// <returns>The created field response.</returns>
        Task<FormFieldResponseDto> CreateAsync(CreateFormFieldDto dto);

        /// <summary>
        /// Retrieves all form fields including their group information.
        /// </summary>
        /// <returns>A collection of form field list DTOs.</returns>
        Task<List<FormFieldListDto>> GetAllAsync();

        /// <summary>
        /// Retrieves a specific form field by identifier.
        /// </summary>
        /// <param name="id">The field identifier.</param>
        /// <returns>The field details if found.</returns>
        Task<FormFieldResponseDto?> GetByIdAsync(Guid id);

        /// <summary>
        /// Updates an existing form field.
        /// </summary>
        /// <param name="id">The field identifier.</param>
        /// <param name="dto">The update request.</param>
        /// <returns>The updated field.</returns>
        Task<FormFieldResponseDto> UpdateAsync(Guid id, UpdateFormFieldDto dto);

        /// <summary>
        /// Soft deletes a form field.
        /// </summary>
        /// <param name="id">The field identifier.</param>
        /// <returns>null.</returns>
        Task DeleteAsync(Guid id);

        /// <summary>
        /// Moves a form field to a different group or removes it from a group.
        /// </summary>
        /// <param name="id">The field identifier.</param>
        /// <param name="dto">The group change request.</param>
        /// <returns>null.</returns>
        Task MoveFieldAsync(Guid id, MoveFieldToGroupDto dto);

        /// <summary>
        /// Retrieves all standalone fields that are not assigned to any group.
        /// </summary>
        /// <returns>A collection of ungrouped field DTOs.</returns>
        Task<List<FormFieldListDto>> GetUngroupedAsync();

        /// <summary>
        /// Retrieves all available field types from the <see cref="FieldType"/> enumeration.
        /// </summary>
        /// <returns>A list of field type DTOs.</returns>
        Task<List<FieldTypeDto>> GetFieldTypesAsync();
    }
}
