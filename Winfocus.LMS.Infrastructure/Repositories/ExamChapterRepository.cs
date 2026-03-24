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
    /// ExamUnitRepository.
    /// </summary>
    public class ExamChapterRepository : IExamChapterRepository
    {
        /// <summary>
        /// The application database context used to access persistence.
        /// </summary>
        private readonly AppDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExamChapterRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public ExamChapterRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>ExamUnit list.</returns>
        public async Task<IReadOnlyList<ExamChapter>> GetAllAsync()
        {
            return await _dbContext.ExamChapters
                .Where(x => x.IsActive && !x.IsDeleted)
                .Include(x => x.Unit)
                .ThenInclude(x => x.Subject)
                .ThenInclude(x => x.Grade)
                .ThenInclude(x => x.Syllabus)
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="unitid">The unitid.</param>
        /// <returns>ExamChapter.</returns>
        public async Task<ExamChapter?> GetByIdAsync(Guid id, Guid unitid)
        {
            var res = _dbContext.ExamChapters
                .Include(x => x.Unit)
                 .ThenInclude(x => x.Subject)
                .ThenInclude(x => x.Grade)
                .ThenInclude(x => x.Syllabus)
                .Where(x => x.Id == id && !x.IsDeleted);

            if (unitid != Guid.Empty)
            {
                res = res.Where(x => x.UnitId == unitid);
            }

            return await res.FirstOrDefaultAsync();
        }

        /// <summary>
        /// Adds the asynchronous.
        /// </summary>
        /// <param name="examchapter">The ExamChapter.</param>
        /// <returns>ExamChapter.</returns>
        public async Task<ExamChapter> AddAsync(ExamChapter examchapter)
        {
            _dbContext.ExamChapters.Add(examchapter);
            await _dbContext.SaveChangesAsync();

            return await _dbContext.ExamChapters
                .Include(x => x.Unit)
                    .ThenInclude(u => u.Subject)
                        .ThenInclude(s => s.Grade)
                            .ThenInclude(g => g.Syllabus)
                .FirstOrDefaultAsync(x => x.Id == examchapter.Id && !x.IsDeleted);
        }

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="examchapter">The ExamUnit.</param>
        /// <returns>task.</returns>
        public async Task<ExamChapter> UpdateAsync(ExamChapter examchapter)
        {
            _dbContext.ExamChapters.Update(examchapter);
            await _dbContext.SaveChangesAsync();
            return examchapter;
        }

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>task.</returns>
        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _dbContext.ExamChapters.FindAsync(id);
            if (entity == null)
            {
                return false;
            }

            entity.IsActive = false;
            entity.IsDeleted = true;

            _dbContext.ExamChapters.Update(entity);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Existses the by code asynchronous.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="unitid">The SubjectID.</param>
        /// <returns>bool.</returns>
        public async Task<bool> ExistsByNameAsync(string name, Guid unitid)
        {
            name = name.Trim();

            return await _dbContext.ExamChapters
                .Include(x => x.Unit)
                .ThenInclude(x => x.Subject)
                .ThenInclude(x => x.Grade)
                .ThenInclude(x => x.Syllabus)
                .AnyAsync(x =>
                    x.UnitId == unitid &&
                    x.Name.Trim().ToLower() == name.ToLower());
        }

        /// <summary>
        /// Gets centre by country, mode of study and state.
        /// </summary>
        /// <param name="unitid">unitid identifier.</param>
        /// <returns>exam unit entity if found; otherwise null.</returns>
        public async Task<List<ExamChapter>> GetByFilterAsync(
            Guid? unitid)
        {
            var query = _dbContext.ExamChapters.Where(x => !x.IsDeleted)
         .AsNoTracking()
         .Include(x => x.Unit)
          .ThenInclude(x => x.Subject)
                .ThenInclude(x => x.Grade)
                .ThenInclude(x => x.Syllabus)
         .AsQueryable();

            if (unitid.HasValue)
                query = query.Where(x => x.UnitId == unitid.Value);

            return await query.ToListAsync();
        }

        /// <summary>
        /// Gets queryable for filtering with full hierarchy.
        /// </summary>
        /// <returns>Queryable ExamUnit.</returns>
        public IQueryable<ExamChapter> Query()
        {
            return _dbContext.ExamChapters.Where(x => !x.IsDeleted)
                .Include(x => x.Unit)
                 .ThenInclude(x => x.Subject)
                .ThenInclude(x => x.Grade)
                .ThenInclude(x => x.Syllabus)
                .AsNoTracking();
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="unitId">The Unit.</param>
        /// <returns>ExamChapter.</returns>
        public async Task<List<ExamChapter>> GetByUnitIdAsync(Guid unitId)
        {
            var res = _dbContext.ExamChapters
                .Include(x => x.Unit)
                .ThenInclude(x => x.Subject)
                .ThenInclude(x => x.Grade)
                .ThenInclude(x => x.Syllabus)
                .Where(x => x.UnitId == unitId && !x.IsDeleted);
            return await res.ToListAsync();
        }

    }
}
