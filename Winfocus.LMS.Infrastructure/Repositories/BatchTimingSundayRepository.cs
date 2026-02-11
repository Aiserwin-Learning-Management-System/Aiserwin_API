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
    /// Provides data access operations for <see cref="BatchTimingSunday"/> entities.
    /// </summary>
    public class BatchTimingSundayRepository : IBatchTimingSundayRepository
    {
        /// <summary>
        /// The application database context used to access persistence.
        /// </summary>
        private readonly AppDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="BatchTimingSundayRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public BatchTimingSundayRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>BatchTimingSunday list.</returns>
        public async Task<IReadOnlyList<BatchTimingSunday>> GetAllAsync()
        {
            return await _dbContext.BatchTimingSundays
                .Include(x => x.Subject)
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>BatchTiming.</returns>
        public async Task<BatchTimingSunday?> GetByIdAsync(Guid id)
        {
            return await _dbContext.BatchTimingSundays
                .Include(x => x.Subject)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        /// <summary>
        /// Adds the asynchronous.
        /// </summary>
        /// <param name="batchtiming">The country.</param>
        /// <returns>country.</returns>
        public async Task<BatchTimingSunday> AddAsync(BatchTimingSunday batchtiming)
        {
            batchtiming.CreatedAt = DateTime.UtcNow;
            _dbContext.BatchTimingSundays.Add(batchtiming);
            await _dbContext.SaveChangesAsync();
            return batchtiming;
        }

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="batchtiming">The batchtiming.</param>
        /// <returns>task.</returns>
        public async Task UpdateAsync(BatchTimingSunday batchtiming)
        {
            batchtiming.UpdatedAt = DateTime.UtcNow;
            _dbContext.BatchTimingSundays.Update(batchtiming);
            await _dbContext.SaveChangesAsync();
        }


        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>task.</returns>
        public async Task DeleteAsync(Guid id)
        {
            var entity = await _dbContext.BatchTimingSundays.FindAsync(id);
            if (entity == null)
            {
                return;
            }

            entity.IsActive = false;

            _dbContext.BatchTimingSundays.Update(entity);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="subjectid">The identifier.</param>
        /// <returns>BatchTimings.</returns>
        public async Task<List<BatchTimingSunday>> GetBySubjectIdAsync(Guid subjectid)
        {
            return await _dbContext.BatchTimingSundays
                .Include(x => x.Subject)
                .Where(x => x.SubjectId == subjectid)
                .ToListAsync();
        }
    }
}
