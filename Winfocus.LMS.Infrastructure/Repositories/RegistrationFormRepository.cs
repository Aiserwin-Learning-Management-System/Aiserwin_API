using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Winfocus.LMS.Application.Interfaces;
using Winfocus.LMS.Domain.Entities;
using Winfocus.LMS.Infrastructure.Data;

namespace Winfocus.LMS.Infrastructure.Repositories
{
    /// <summary>
    /// Provides database operations for <see cref="RegistrationForm"/> entities.
    /// 
    /// This repository is responsible for:
    /// - Creating registration forms
    /// - Retrieving forms with their associated groups and fields
    /// - Updating form data
    /// - Soft deleting forms
    /// - Checking whether submissions exist for a form
    /// The repository interacts with the database through the <see cref="ApplicationDbContext"/>.
    /// </summary>
    public class RegistrationFormRepository : IRegistrationFormRepository
    {
        /// <summary>
        /// Database context used for accessing application data.
        /// </summary>
        private readonly AppDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistrationFormRepository"/> class.
        /// </summary>
        /// <param name="context">
        /// The application database context used to perform data operations.
        /// </param>
        public RegistrationFormRepository(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Adds a new registration form to the database.
        /// </summary>
        /// <param name="form">
        /// The <see cref="RegistrationForm"/> entity to be created.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <remarks>
        /// This method saves the form immediately by calling <c>SaveChangesAsync()</c>.
        /// </remarks>
        public async Task<RegistrationForm> AddAsync(RegistrationForm form)
        {
            await _context.RegistrationForms.AddAsync(form);
            await _context.SaveChangesAsync();
            return form;
        }

        /// <summary>
        /// Retrieves a registration form by its unique identifier.
        /// </summary>
        /// <param name="id">
        /// The unique identifier of the registration form.
        /// </param>
        /// <returns>
        /// The matching <see cref="RegistrationForm"/> including its
        /// associated groups and fields, or <c>null</c> if not found.
        /// </returns>
        /// <remarks>
        /// This method uses <see cref="EntityFrameworkQueryableExtensions.Include{TEntity, TProperty}"/>
        /// to eagerly load related navigation properties such as:
        /// <list type="bullet">
        /// <item>Form Groups</item>
        /// <item>Form Fields</item>
        /// </list>
        /// </remarks>
        public async Task<RegistrationForm> GetByIdAsync(Guid id)
        {
            //return await _context.RegistrationForms
            //    .Include(x => x.StaffCategory)
            //    .Include(x => x.FormGroups)
            //    .Include(x => x.FormFields)
            //    .FirstOrDefaultAsync(x => x.Id == id);

            return await _context.RegistrationForms
               .Include(x => x.FormGroups)
                   .ThenInclude(g => g.FieldGroup)
               .Include(x => x.FormFields)
                   .ThenInclude(f => f.FormField)
                       .ThenInclude(field => field.FieldOptions)
               .Include(x => x.StaffCategory)
               .FirstOrDefaultAsync(x => x.Id == id);
        }

        /// <summary>
        /// Retrieves all registration forms from the database.
        /// </summary>
        /// <returns>
        /// A list of <see cref="RegistrationForm"/> entities.
        /// </returns>
        /// <remarks>
        /// This method returns all forms without loading related entities.
        /// Use <see cref="GetByIdAsync(Guid)"/> if groups and fields are required.
        /// </remarks>
        public async Task<List<RegistrationForm>> GetAllAsync()
        {
            return await _context.RegistrationForms.ToListAsync();
        }

        /// <summary>
        /// Updates an existing registration form in the database.
        /// </summary>
        /// <param name="form">
        /// The modified <see cref="RegistrationForm"/> entity.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous update operation.
        /// </returns>
        /// <remarks>
        /// The entity must already exist in the database.
        /// Changes are persisted using <c>SaveChangesAsync()</c>.
        /// </remarks>
        public async Task UpdateAsync(RegistrationForm form)
        {
            _context.RegistrationForms.Update(form);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Performs a soft delete of a registration form.
        /// </summary>
        /// <param name="id">
        /// The unique identifier of the form to be deleted.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <remarks>
        /// Instead of removing the record from the database,
        /// this method sets the <c>IsActive</c> flag to <c>false</c>.
        /// This preserves historical data and relationships.
        /// </remarks>
        public async Task DeleteAsync(Guid id)
        {
            var form = await _context.RegistrationForms.FindAsync(id);

            if (form != null)
            {
                form.IsActive = false;
                form.IsDeleted = true;
                await _context.SaveChangesAsync();
            }
        }

        /// <inheritdoc/>
        public async Task AddGroupsAsync(List<RegistrationFormGroup> groups)
        {
            await _context.RegistrationFormGroups.AddRangeAsync(groups);
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task AddFieldsAsync(List<RegistrationFormField> fields)
        {
            await _context.RegistrationFormFields.AddRangeAsync(fields);
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task<List<FormField>> GetByGroupIdAsync(Guid groupId)
        {
            return await _context.FormFields
                .Where(x => x.FieldGroupId == groupId && x.IsActive)
                .ToListAsync();
        }
    }
}
