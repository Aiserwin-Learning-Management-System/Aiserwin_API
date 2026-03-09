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
    /// Provides data access operations for <see cref="DoubtClearing"/> entities.
    /// </summary>
    public class DoubtClearingRepository : IDoubtClearingRepository
    {
        /// <summary>
        /// The application database context used to access persistence.
        /// </summary>
        private readonly AppDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="DoubtClearingRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public DoubtClearingRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>DoubtClearing list.</returns>
        public async Task<IReadOnlyList<DoubtClearing>> GetAllAsync()
        {
            return await _dbContext.DoubtClearing
               .Include(x => x.Subject)
                  .ThenInclude(s => s.Course)
                     .ThenInclude(s => s.Stream)
                      .ThenInclude(s => s.Grade)
                       .ThenInclude(s => s.Syllabus)
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <param name="date">Date.</param>
        /// <returns> list.</returns>
        public async Task<DoubtClearing?> GetByDateAsync(DateTime date)
        {
            return await _dbContext.DoubtClearing
                .FirstOrDefaultAsync(x =>
                    x.ScheduleTime <= date &&
                    x.ScheduleEndTime >= date);
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Doubt Clearing.</returns>
        public async Task<DoubtClearing?> GetByIdAsync(Guid id)
        {
            return await _dbContext.DoubtClearing
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        /// <summary>
        /// Adds the asynchronous.
        /// </summary>
        /// <param name="doubtclearing">The doubtclearing.</param>
        /// <returns>DoubtClearing .</returns>
        public async Task<DoubtClearing> AddAsync(DoubtClearing doubtclearing)
        {
            _dbContext.DoubtClearing.Add(doubtclearing);
            await _dbContext.SaveChangesAsync();
            return doubtclearing;
        }

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="doubtclearing">The doubtclearing.</param>
        /// <returns>task.</returns>
        public async Task<DoubtClearing> UpdateAsync(DoubtClearing doubtclearing)
        {
            doubtclearing.UpdatedAt = DateTime.UtcNow;
            _dbContext.DoubtClearing.Update(doubtclearing);
            await _dbContext.SaveChangesAsync();
            return doubtclearing;
        }

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>DoubtClearing.</returns>
        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _dbContext.DoubtClearing.FindAsync(id);
            if (entity == null)
            {
                return false;
            }

            entity.IsActive = false;

            _dbContext.DoubtClearing.Update(entity);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="subjectid">The identifier.</param>
        /// <returns>DoubtClearing.</returns>
        public async Task<List<DoubtClearing>> GetBySubjectIdAsync(Guid subjectid)
        {
            return await _dbContext.DoubtClearing
                .Include(x => x.Subject)
                  .ThenInclude(s => s.Course)
                     .ThenInclude(s => s.Stream)
                      .ThenInclude(s => s.Grade)
                       .ThenInclude(s => s.Syllabus)
                .Where(x => x.SubjectId == subjectid)
                .ToListAsync();
        }

        /// <summary>
        /// Gets queryable for filtering with full hierarchy.
        /// </summary>
        /// <returns>Queryable DoubtClearing.</returns>
        public IQueryable<DoubtClearing> Query()
        {
            return _dbContext.DoubtClearing
                .AsNoTracking();
        }

    }
}
