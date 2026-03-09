using Microsoft.EntityFrameworkCore;
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
                .Where(x => x.IsActive)
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
        public async Task<BatchTimingSunday?> GetByIdAsync(Guid id)
        {
            return await _dbContext.BatchTimingSundays
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
        public async Task<BatchTimingSunday> UpdateAsync(BatchTimingSunday batchtiming)
        {
            batchtiming.UpdatedAt = DateTime.UtcNow;
            _dbContext.BatchTimingSundays.Update(batchtiming);
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
            var entity = await _dbContext.BatchTimingSundays.FindAsync(id);
            if (entity == null)
            {
                return false;
            }

            entity.IsActive = false;
            entity.IsDeleted = true;

            _dbContext.BatchTimingSundays.Update(entity);
            await _dbContext.SaveChangesAsync();
            return true;
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
        /// <param name="batchtiming">The batchtiming.</param>
        /// <returns>.</returns>
        public async Task<SubjectBatchTimingSunday> BatchTimingSubjectCreate(SubjectBatchTimingSunday batchtiming)
        {
            _dbContext.SubjectBatchTimingSundays.Add(batchtiming);
            await _dbContext.SaveChangesAsync();
            return batchtiming;
        }

        /// <summary>
        /// Gets queryable for filtering with full hierarchy.
        /// </summary>
        /// <returns>Queryable batches.</returns>
        public IQueryable<BatchTimingSunday> Query()
        {
            return _dbContext.BatchTimingSundays
               .Include(x => x.Subject)
                   .ThenInclude(s => s.Course)
                      .ThenInclude(s => s.Stream)
                       .ThenInclude(s => s.Grade)
                        .ThenInclude(s => s.Syllabus)
                .AsNoTracking();
        }
    }
}
