namespace Winfocus.LMS.Infrastructure.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Winfocus.LMS.Application.DTOs.Masters;
    using Winfocus.LMS.Application.Interfaces;
    using Winfocus.LMS.Domain.Entities;
    using Winfocus.LMS.Infrastructure.Data;

    /// <summary>
    /// Provides data access operations for <see cref="ModeOfStudy"/> entities.
    /// </summary>
    public sealed class ModeOfStudyRepository : IModeOfStudyRepository
    {
        /// <summary>
        /// The application database context used to access persistence.
        /// </summary>
        private readonly AppDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModeOfStudyRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public ModeOfStudyRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Retrieves an active <see cref="ModeOfStudy"/> by its identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the mode of study.</param>
        /// <returns>
        /// A <see cref="Task{TResult}"/> that resolves to the matching <see cref="ModeOfStudy"/> if found and active; otherwise <c>null</c>.
        /// </returns>
        public Task<ModeOfStudy?> GetByIdAsync(Guid id)
        {
            return _dbContext.ModeOfStudies
                .FirstOrDefaultAsync(r => r.Id == id && r.IsActive);
        }

        /// <summary>
        /// Retrieves all active <see cref="ModeOfStudy"/> entities.
        /// </summary>
        /// <returns>A task resolving to a list of active modes of study.</returns>
        public Task<List<ModeOfStudy>> GetAllAsync()
        {
            return _dbContext.ModeOfStudies
                .Where(m => m.IsActive)
                .ToListAsync();
        }

        /// <summary>
        /// Creates a new <see cref="ModeOfStudy"/> in the database.
        /// </summary>
        /// <param name="mode">The mode of study entity to create.</param>
        /// <returns>The created <see cref="ModeOfStudy"/> with its assigned identifier.</returns>
        public async Task<ModeOfStudy> CreateAsync(ModeOfStudy mode)
        {
            mode.Id = mode.Id == Guid.Empty ? Guid.NewGuid() : mode.Id;
            mode.CreatedAt = DateTime.UtcNow;

            await _dbContext.ModeOfStudies.AddAsync(mode);
            await _dbContext.SaveChangesAsync();

            return mode;
        }

        /// <summary>
        /// Updates an existing active <see cref="ModeOfStudy"/>.
        /// </summary>
        /// <param name="mode">The entity containing updated values. The <see cref="ModeOfStudy.Id"/> must identify an existing active record.</param>
        /// <returns>The updated <see cref="ModeOfStudy"/> if the entity existed and was updated; otherwise <c>null</c>.</returns>
        public async Task<ModeOfStudy?> UpdateAsync(ModeOfStudy mode)
        {
            var existing = await _dbContext.ModeOfStudies
                .FirstOrDefaultAsync(m => m.Id == mode.Id && m.IsActive);

            if (existing is null)
            {
                return null;
            }

            // Preserve immutable/audit fields
            var createdAt = existing.CreatedAt;
            var createdBy = existing.CreatedBy;
            var isActive = existing.IsActive;

            // Apply incoming values
            _dbContext.Entry(existing).CurrentValues.SetValues(mode);

            // Restore preserved fields and set updated timestamp
            existing.CreatedAt = createdAt;
            existing.CreatedBy = createdBy;
            existing.IsActive = isActive;
            existing.UpdatedAt = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync();

            return existing;
        }

        /// <summary>
        /// Soft-deletes an existing <see cref="ModeOfStudy"/> by marking it inactive.
        /// </summary>
        /// <param name="id">The identifier of the mode of study to delete.</param>
        /// <returns><c>true</c> if the item was found and marked inactive; otherwise <c>false</c>.</returns>
        public async Task<bool> DeleteAsync(Guid id)
        {
            var existing = await _dbContext.ModeOfStudies
                .FirstOrDefaultAsync(m => m.Id == id && m.IsActive);

            if (existing is null)
            {
                return false;
            }

            existing.IsActive = false;
            existing.UpdatedAt = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync();

            return true;
        }
    }
}
