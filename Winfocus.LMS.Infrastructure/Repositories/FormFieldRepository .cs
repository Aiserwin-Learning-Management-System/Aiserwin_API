using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Pqc.Crypto.Lms;
using System;
using System.Collections.Generic;
using System.Text;
using Winfocus.LMS.Application.Interfaces;
using Winfocus.LMS.Domain.Entities;
using Winfocus.LMS.Infrastructure.Data;

namespace Winfocus.LMS.Infrastructure.Repositories
{
    /// <summary>
    /// Repository implementation responsible for data access operations
    /// related to <see cref="FormField"/> entities.
    /// Handles CRUD operations, eager loading of related entities,
    /// and utility queries such as display order calculation.
    /// </summary>
    public class FormFieldRepository : IFormFieldRepository
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="FormFieldRepository"/> class.
        /// </summary>
        /// <param name="context">
        /// The application database context used for querying and persisting data.
        /// </param>
        public FormFieldRepository(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves a form field by its unique identifier.
        /// Includes related <see cref="FieldGroup"/> and <see cref="FieldOption"/> entities.
        /// </summary>
        /// <param name="id">The unique identifier of the form field.</param>
        /// <returns>
        /// The matching <see cref="FormField"/> if found and not deleted; otherwise <c>null</c>.
        /// </returns>
        public async Task<FormField?> GetByIdAsync(Guid id)
        {
            return await _context.FormFields
                .Include(f => f.FieldGroup)
                .Include(f => f.FieldOptions)
                .FirstOrDefaultAsync(f => f.Id == id && !f.IsDeleted);
        }

        /// <summary>
        /// Retrieves all active form fields from the database.
        /// Includes their associated field groups and selectable options.
        /// </summary>
        /// <returns>
        /// A list of <see cref="FormField"/> entities that are not soft-deleted.
        /// </returns>
        public async Task<List<FormField>> GetAllAsync()
        {
            return await _context.FormFields
                .Include(f => f.FieldGroup)
                .Include(f => f.FieldOptions)
                .Where(f => !f.IsDeleted)
                .ToListAsync();
        }

        /// <summary>
        /// Retrieves all standalone form fields that are not assigned to any group.
        /// </summary>
        /// <returns>
        /// A list of <see cref="FormField"/> entities where <see cref="FormField.FieldGroupId"/> is <c>null</c>.
        /// </returns>
        public async Task<List<FormField>> GetUngroupedAsync()
        {
            return await _context.FormFields
                .Include(f => f.FieldOptions)
                .Where(f => f.FieldGroupId == null && !f.IsDeleted)
                .ToListAsync();
        }

        /// <summary>
        /// Adds a new form field to the database.
        /// </summary>
        /// <param name="field">The <see cref="FormField"/> entity to add.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task AddAsync(FormField field)
        {
            await _context.FormFields.AddAsync(field);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Updates an existing form field in the database.
        /// </summary>
        /// <param name="field">The updated <see cref="FormField"/> entity.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task UpdateAsync(FormField field)
        {
            _context.FormFields.Update(field);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Performs a soft delete on a form field by marking it as deleted.
        /// The record remains in the database but is excluded from active queries.
        /// </summary>
        /// <param name="field">The <see cref="FormField"/> entity to delete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task SoftDeleteAsync(FormField field)
        {
            field.IsDeleted = true;
            _context.FormFields.Update(field);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Determines whether a field group exists and is not deleted.
        /// </summary>
        /// <param name="groupId">The unique identifier of the field group.</param>
        /// <returns>
        /// <c>true</c> if the field group exists and is active; otherwise <c>false</c>.
        /// </returns>
        public async Task<bool> FieldGroupExistsAsync(Guid groupId)
        {
            return await _context.FieldGroups
                .AnyAsync(g => g.Id == groupId && !g.IsDeleted);
        }

        /// <summary>
        /// Calculates the next available display order for a form field within a group.
        /// If the group is null, the order is calculated among standalone fields.
        /// </summary>
        /// <param name="groupId">
        /// The identifier of the field group. 
        /// If <c>null</c>, the calculation applies to ungrouped fields.
        /// </param>
        /// <returns>
        /// The next available display order value.
        /// </returns>
        public async Task<int> GetNextDisplayOrderAsync(Guid? groupId)
        {
            var max = await _context.FormFields
                .Where(f => f.FieldGroupId == groupId && !f.IsDeleted)
                .MaxAsync(f => (int?)f.DisplayOrder);

            return (max ?? 0) + 1;
        }

    }
}
