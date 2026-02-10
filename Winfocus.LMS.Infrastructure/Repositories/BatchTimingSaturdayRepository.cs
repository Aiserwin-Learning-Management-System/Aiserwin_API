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
    /// Provides data access operations for <see cref="BatchTimingSaturday"/> entities.
    /// </summary>
    public class BatchTimingSaturdayRepository : IBatchTimingSaturdayRepository
    {
        /// <summary>
        /// The application database context used to access persistence.
        /// </summary>
        private readonly AppDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="BatchTimingSaturdayRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public BatchTimingSaturdayRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>BatchTimingSaturday list.</returns>
        public async Task<IReadOnlyList<BatchTimingSaturday>> GetAllAsync()
        {
            return await _dbContext.BatchTimingSaturdays
                .Include(x => x.Subject)
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>BatchTimingSaturday.</returns>
        public async Task<BatchTimingSaturday?> GetByIdAsync(Guid id)
        {
            return await _dbContext.BatchTimingSaturdays
                .Include(x => x.Subject)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        /// <summary>
        /// Adds the asynchronous.
        /// </summary>
        /// <param name="batchtiming">The country.</param>
        /// <returns>BatchTimingSaturday.</returns>
        public async Task<BatchTimingSaturday> AddAsync(BatchTimingSaturday batchtiming)
        {
            batchtiming.CreatedAt = DateTime.UtcNow;
            _dbContext.BatchTimingSaturdays.Add(batchtiming);
            await _dbContext.SaveChangesAsync();
            return batchtiming;
        }

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="batchtiming">The batchtiming.</param>
        /// <returns>task.</returns>
        public async Task UpdateAsync(BatchTimingSaturday batchtiming)
        {
            batchtiming.UpdatedAt = DateTime.UtcNow;
            _dbContext.BatchTimingSaturdays.Update(batchtiming);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>task.</returns>
        public async Task DeleteAsync(Guid id)
        {
            var entity = await _dbContext.BatchTimingSaturdays.FindAsync(id);
            if (entity == null)
            {
                return;
            }

            entity.IsActive = false;

            _dbContext.BatchTimingSaturdays.Update(entity);
            await _dbContext.SaveChangesAsync();
        }
    }
}
