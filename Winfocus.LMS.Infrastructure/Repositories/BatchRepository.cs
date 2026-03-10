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
    /// Provides data access operations for <see cref="Batch"/> entities.
    /// </summary>
    public class BatchRepository : IBatchRepository
    {
        /// <summary>
        /// The application database context used to access persistence.
        /// </summary>
        private readonly AppDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="BatchRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public BatchRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>Batch list.</returns>
        public async Task<IReadOnlyList<Batch>> GetAllAsync()
        {
            return await _dbContext.Batches
                .Where(x => x.IsActive && !x.IsDeleted)
                .Include(x => x.Subject)
                  .ThenInclude(s => s.Course)
                     .ThenInclude(s => s.Stream)
                      .ThenInclude(s => s.Grade)
                       .ThenInclude(s => s.Syllabus)
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Batch.</returns>
        public async Task<Batch?> GetByIdAsync(Guid id)
        {
            return await _dbContext.Batches
                .Include(x => x.Subject)
                  .ThenInclude(s => s.Course)
                     .ThenInclude(s => s.Stream)
                      .ThenInclude(s => s.Grade)
                       .ThenInclude(s => s.Syllabus)
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        }

        /// <summary>
        /// Adds the asynchronous.
        /// </summary>
        /// <param name="batch">The AcademicYear.</param>
        /// <returns>BatchTimingSaturday.</returns>
        public async Task<Batch> AddAsync(Batch batch)
        {
            _dbContext.Batches.Add(batch);
            await _dbContext.SaveChangesAsync();
            return batch;
        }

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="batch">The batchtiming.</param>
        /// <returns>task.</returns>
        public async Task<Batch> UpdateAsync(Batch batch)
        {
            batch.UpdatedAt = DateTime.UtcNow;
            _dbContext.Batches.Update(batch);
            await _dbContext.SaveChangesAsync();
            return batch;
        }

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>task.</returns>
        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _dbContext.Batches.FindAsync(id);
            if (entity == null)
            {
                return false;
            }

            entity.IsActive = false;
            entity.IsDeleted = true;

            _dbContext.Batches.Update(entity);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="subjectid">The identifier.</param>
        /// <returns>BatchTimings.</returns>
        public async Task<List<Batch>> GetBySubjectIdAsync(Guid subjectid)
        {
            return await _dbContext.Batches
                .Include(x => x.Subject)
                   .ThenInclude(s => s.Course)
                      .ThenInclude(s => s.Stream)
                       .ThenInclude(s => s.Grade)
                        .ThenInclude(s => s.Syllabus)
                .Where(x => x.SubjectId == subjectid && x.IsActive && !x.IsDeleted)
                .ToListAsync();
        }

        /// <summary>
        /// Gets queryable for filtering with full hierarchy.
        /// </summary>
        /// <returns>Queryable batches.</returns>
        public IQueryable<Batch> Query()
        {
            return _dbContext.Batches.Where(x => !x.IsDeleted)
               .Include(x => x.Subject)
                   .ThenInclude(s => s.Course)
                      .ThenInclude(s => s.Stream)
                       .ThenInclude(s => s.Grade)
                        .ThenInclude(s => s.Syllabus)
                .AsNoTracking();
        }
    }
}
