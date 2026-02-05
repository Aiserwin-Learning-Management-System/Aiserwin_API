namespace Winfocus.LMS.Infrastructure.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Winfocus.LMS.Application.Interfaces;
    using Winfocus.LMS.Domain.Entities;
    using Winfocus.LMS.Infrastructure.Data;

    /// <summary>
    /// Provides data access operations for <see cref="Centre"/> entities.
    /// </summary>
    public sealed class CentreRepository : ICentreRepository
    {
        /// <summary>
        /// The application database context used to access persistence.
        /// </summary>
        private readonly AppDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="CentreRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public CentreRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Retrieves an active <see cref="Centre"/> by its identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the centre.</param>
        /// <returns>
        /// A <see cref="Task{TResult}"/> that resolves to the matching <see cref="Centre"/> if found and active; otherwise <c>null</c>.
        /// </returns>
        public Task<Centre?> GetByIdAsync(Guid id)
        {
            return _dbContext.Centres
                .FirstOrDefaultAsync(c => c.Id == id && c.IsActive);
        }

        /// <summary>
        /// Retrieves all active <see cref="Centre"/> entities.
        /// </summary>
        /// <returns>A task resolving to a list of active centres.</returns>
        public Task<List<Centre>> GetAllAsync()
        {
            return _dbContext.Centres
                .Where(c => c.IsActive)
                .ToListAsync();
        }

        /// <summary>
        /// Creates a new <see cref="Centre"/> in the database.
        /// </summary>
        /// <param name="centre">The centre entity to create.</param>
        /// <returns>The created <see cref="Centre"/> with its assigned identifier.</returns>
        public async Task<Centre> CreateAsync(Centre centre)
        {
            centre.Id = centre.Id == Guid.Empty ? Guid.NewGuid() : centre.Id;
            centre.CreatedAt = DateTime.UtcNow;

            await _dbContext.Centres.AddAsync(centre);
            await _dbContext.SaveChangesAsync();

            return centre;
        }

        /// <summary>
        /// Updates an existing active <see cref="Centre"/>.
        /// </summary>
        /// <param name="centre">The entity containing updated values. The <see cref="Centre.Id"/> must identify an existing active record.</param>
        /// <returns>The updated <see cref="Centre"/> if the entity existed and was updated; otherwise <c>null</c>.</returns>
        public async Task<Centre?> UpdateAsync(Centre centre)
        {
            var existing = await _dbContext.Centres
                .FirstOrDefaultAsync(c => c.Id == centre.Id && c.IsActive);

            if (existing is null)
            {
                return null;
            }

            // Preserve immutable/audit fields
            var createdAt = existing.CreatedAt;
            var createdBy = existing.CreatedBy;
            var isActive = existing.IsActive;

            // Apply incoming values
            _dbContext.Entry(existing).CurrentValues.SetValues(centre);

            // Restore preserved fields and set updated timestamp
            existing.CreatedAt = createdAt;
            existing.CreatedBy = createdBy;
            existing.IsActive = isActive;
            existing.UpdatedAt = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync();

            return existing;
        }

        /// <summary>
        /// Soft-deletes an existing <see cref="Centre"/> by marking it inactive.
        /// </summary>
        /// <param name="id">The identifier of the centre to delete.</param>
        /// <returns><c>true</c> if the centre was found and marked inactive; otherwise <c>false</c>.</returns>
        public async Task<bool> DeleteAsync(Guid id)
        {
            var existing = await _dbContext.Centres
                .FirstOrDefaultAsync(c => c.Id == id && c.IsActive);

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
