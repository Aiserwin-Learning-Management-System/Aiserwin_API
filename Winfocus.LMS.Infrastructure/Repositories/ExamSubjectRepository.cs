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
    /// ExamSubjectRepository.
    /// </summary>
    public sealed class ExamSubjectRepository : IExamSubjectRepository
    {
        /// <summary>
        /// The application database context used to access persistence.
        /// </summary>
        private readonly AppDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExamSubjectRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public ExamSubjectRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>ExamSubject list.</returns>
        public async Task<IReadOnlyList<ExamSubject>> GetAllAsync()
        {
            return await _dbContext.ExamSubjects
                .Where(x => x.IsActive && !x.IsDeleted)
                .Include(x => x.Grade)
                .ThenInclude(x => x.Syllabus)
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="gradeID">The gradeID.</param>
        /// <returns>ExamSubject.</returns>
        public async Task<ExamSubject?> GetByIdAsync(Guid id, Guid gradeID)
        {
            var res = _dbContext.ExamSubjects
                .Include(x => x.Grade)
                .ThenInclude(x => x.Syllabus)
                .Where(x => x.Id == id && !x.IsDeleted);

            if (gradeID != Guid.Empty)
            {
                res = res.Where(x => x.GradeId == gradeID);
            }

            return await res.FirstOrDefaultAsync();
        }

        /// <summary>
        /// Adds the asynchronous.
        /// </summary>
        /// <param name="ExamSubject">The ExamSubject.</param>
        /// <returns>ExamSubject.</returns>
        public async Task<ExamSubject> AddAsync(ExamSubject ExamSubject)
        {
            _dbContext.ExamSubjects.Add(ExamSubject);
            await _dbContext.SaveChangesAsync();
            return ExamSubject;
        }

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="ExamSubject">The ExamSubject.</param>
        /// <returns>task.</returns>
        public async Task<ExamSubject> UpdateAsync(ExamSubject ExamSubject)
        {
            _dbContext.ExamSubjects.Update(ExamSubject);
            await _dbContext.SaveChangesAsync();
            return ExamSubject;
        }

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>task.</returns>
        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _dbContext.ExamSubjects.FindAsync(id);
            if (entity == null)
            {
                return false;
            }
            entity.IsActive = false;
            entity.IsDeleted = true;

            _dbContext.ExamSubjects.Update(entity);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Existses the by code asynchronous.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="gradeID">The gradeID.</param>
        /// <returns>bool.</returns>
        public async Task<bool> ExistsByNameAsync(string name, Guid gradeID)
        {
            name = name.Trim();

            return await _dbContext.ExamSubjects
                .Include(x => x.Grade)
                .ThenInclude(x => x.Syllabus)
                .AnyAsync(x =>
                    x.Grade.Id == gradeID &&
                    x.Name.Trim().ToLower() == name.ToLower());
        }

        /// <summary>
        /// Gets centre by country, mode of study and state.
        /// </summary>
        /// <param name="gradeID">gradeID identifier.</param>
        /// <returns>Centre entity if found; otherwise null.</returns>
        public async Task<List<ExamSubject>> GetByFilterAsync(
            Guid? gradeID)
        {
            var query = _dbContext.ExamSubjects.Where(x => !x.IsDeleted)
         .AsNoTracking()
         .Include(x => x.Grade)
         .ThenInclude(x => x.Syllabus)
         .AsQueryable();

            if (gradeID.HasValue)
                query = query.Where(x => x.Grade.Id == gradeID.Value);

            return await query.ToListAsync();
        }

        /// <summary>
        /// Gets queryable for filtering with full hierarchy.
        /// </summary>
        /// <returns>Queryable ExamSubject.</returns>
        public IQueryable<ExamSubject> Query()
        {
            return _dbContext.ExamSubjects.Where(x => !x.IsDeleted)
                .Include(x => x.Grade)
                .ThenInclude(x => x.Syllabus)
                .AsNoTracking();
        }
    }
}
