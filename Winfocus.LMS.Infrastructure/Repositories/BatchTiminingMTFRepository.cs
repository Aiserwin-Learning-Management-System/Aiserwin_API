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
    /// Provides data access operations for <see cref="BatchTimingMTF"/> entities.
    /// </summary>
    public class BatchTiminingMTFRepository : IBatchTimingMTFRepository
    {
        /// <summary>
        /// The application database context used to access persistence.
        /// </summary>
        private readonly AppDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="BatchTiminingMTFRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public BatchTiminingMTFRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>BatchTimingMTF list.</returns>
        public async Task<IReadOnlyList<BatchTimingMTF>> GetAllAsync()
        {
            return await _dbContext.BatchTimingMTFs
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
        /// <returns>BatchTiming.</returns>
        public async Task<BatchTimingMTF?> GetByIdAsync(Guid id)
        {
            return await _dbContext.BatchTimingMTFs
                .Include(x => x.Subject)
                  .ThenInclude(s => s.Course)
                     .ThenInclude(s => s.Stream)
                      .ThenInclude(s => s.Grade)
                       .ThenInclude(s => s.Syllabus)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        /// <summary>
        /// Adds the asynchronous.
        /// </summary>
        /// <param name="batchtiming">The country.</param>
        /// <returns>country.</returns>
        public async Task<BatchTimingMTF> AddAsync(BatchTimingMTF batchtiming)
        {
            _dbContext.BatchTimingMTFs.Add(batchtiming);
            await _dbContext.SaveChangesAsync();
            return batchtiming;
        }

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="batchtiming">The batchtiming.</param>
        /// <returns>task.</returns>
        public async Task<BatchTimingMTF> UpdateAsync(BatchTimingMTF batchtiming)
        {
            batchtiming.UpdatedAt = DateTime.UtcNow;
            _dbContext.BatchTimingMTFs.Update(batchtiming);
            await _dbContext.SaveChangesAsync();
            return batchtiming;
        }

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>task.</returns>
        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _dbContext.BatchTimingMTFs.FindAsync(id);

            if (entity == null)
            {
                return false;
            }

            entity.IsActive = false;
            entity.IsDeleted = true;

            _dbContext.BatchTimingMTFs.Update(entity);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="subjectid">The identifier.</param>
        /// <returns>BatchTimings.</returns>
        public async Task<List<BatchTimingMTF>> GetBySubjectIdAsync(Guid subjectid)
        {
            return await _dbContext.BatchTimingMTFs
                .Include(x => x.Subject)
                  .ThenInclude(s => s.Course)
                     .ThenInclude(s => s.Stream)
                      .ThenInclude(s => s.Grade)
                       .ThenInclude(s => s.Syllabus)
                .Where(x => x.SubjectId == subjectid)
                .ToListAsync();
        }

        /// <summary>
        /// Adds the asynchronous.
        /// </summary>
        /// <param name="batchtiming">The country.</param>
        /// <returns>country.</returns>
        public async Task<SubjectBatchTimingMTF> BatchTimingSubjectCreate(SubjectBatchTimingMTF batchtiming)
        {
            _dbContext.SubjectBatchTimingMTFs.Add(batchtiming);
            await _dbContext.SaveChangesAsync();
            return batchtiming;
        }

        /// <summary>
        /// Gets queryable for filtering with full hierarchy.
        /// </summary>
        /// <returns>Queryable batches.</returns>
        public IQueryable<BatchTimingMTF> Query()
        {
            return _dbContext.BatchTimingMTFs
               .Include(x => x.Subject)
                   .ThenInclude(s => s.Course)
                      .ThenInclude(s => s.Stream)
                       .ThenInclude(s => s.Grade)
                        .ThenInclude(s => s.Syllabus)
                .AsNoTracking();
        }
    }
}
