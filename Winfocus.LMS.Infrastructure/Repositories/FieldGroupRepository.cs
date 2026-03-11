using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Winfocus.LMS.Application.DTOs;
using Winfocus.LMS.Application.Interfaces;
using Winfocus.LMS.Domain.Entities;
using Winfocus.LMS.Infrastructure.Data;

namespace Winfocus.LMS.Infrastructure.Repositories
{
    /// <summary>
    /// Provides data access operations for <see cref="FieldGroup"/> entities.
    /// </summary>
    public class FieldGroupRepository : IFieldGroupRepository
    {
        private readonly AppDbContext _db;

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldGroupRepository"/> class.
        /// </summary>
        /// <param name="db">The database.</param>
        public FieldGroupRepository(AppDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>FieldGroup list.</returns>
        public async Task<IReadOnlyList<FieldGroup>> GetAllAsync()
        {
            return await _db.FieldGroups
                .Include(x => x.FormFields)
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>FieldGroup.</returns>
        public async Task<FieldGroup?> GetByIdAsync(Guid id)
        {
            return await _db.FieldGroups
                .Include(x => x.FormFields)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="groupId">The identifier.</param>
        /// <returns>FieldGroup.</returns>
        public async Task<FieldGroupFieldsResponseDto?> GetFieldsByGroupIdAsync(Guid groupId)
        {
            var group = await _db.FieldGroups
             .Include(g => g.FormFields)
             .FirstOrDefaultAsync(g => g.Id == groupId);

            if (group == null)
                return null;

            var fields = group.FormFields
                .OrderBy(f => f.DisplayOrder)
                .Select(f => new FormField_FieldGroupDto
                {
                    FieldId = f.Id,
                    FieldName = f.FieldName,
                    DisplayLabel = f.DisplayLabel,
                    FieldType = f.FieldType,
                    IsRequired = f.IsRequired,
                    DisplayOrder = f.DisplayOrder
                })
                .ToList();

            return new FieldGroupFieldsResponseDto
            {
                GroupName = group.GroupName,
                Description = group.Description,
                DisplayOrder = group.DisplayOrder,
                IsActive = group.IsActive,
                FieldCount = fields.Count,
                Fields = fields
            };
        }

        /// <summary>
        /// Adds the asynchronous.
        /// </summary>
        /// <param name="fieldgroup">The fieldgroup.</param>
        /// <returns>fieldgroup.</returns>
        public async Task<FieldGroup> AddAsync(FieldGroup fieldgroup)
        {
            _db.FieldGroups.Add(fieldgroup);
            await _db.SaveChangesAsync();
            return fieldgroup;
        }

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="fieldgroup">The fieldgroup.</param>
        /// <returns>task.</returns>
        public async Task<FieldGroup> UpdateAsync(FieldGroup fieldgroup)
        {
            _db.FieldGroups.Update(fieldgroup);
            await _db.SaveChangesAsync();
            return fieldgroup;
        }

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>task.</returns>
        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _db.FieldGroups.FindAsync(id);
            if (entity == null)
            {
                return false;
            }

            entity.UpdatedAt = DateTime.UtcNow;
            entity.IsActive = false;

            _db.FieldGroups.Update(entity);
            await _db.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Existses the by code asynchronous.
        /// </summary>
        /// <param name="name">The code.</param>
        /// <returns>bool.</returns>
        public async Task<bool> ExistsByNameAsync(string name)
        {
            return await _db.FieldGroups.AnyAsync(x => x.GroupName == name);
        }

        /// <summary>
        /// Gets queryable for filtering with full hierarchy.
        /// </summary>
        /// <returns>Queryable FieldGroups.</returns>
        public IQueryable<FieldGroup> Query()
        {
            return _db.FieldGroups
                .AsNoTracking();
        }
    }
}
