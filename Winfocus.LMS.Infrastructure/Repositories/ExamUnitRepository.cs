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
    public sealed class ExamUnitRepository : IExamUnitRepository
    {
        /// <summary>
        /// The application database context used to access persistence.
        /// </summary>
        private readonly AppDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExamUnitRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public ExamUnitRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>ExamUnit list.</returns>
        public async Task<IReadOnlyList<ExamUnit>> GetAllAsync()
        {
            return await _dbContext.ExamUnits
                .Where(x => x.IsActive && !x.IsDeleted)
                .Include(x => x.Subject)
                .ThenInclude(x => x.Course)
                .ThenInclude(x => x.Grade)
                .ThenInclude(x => x.Syllabus)
                 .ThenInclude(x => x.AcademicYear)
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="SubjectID">The SubjectID.</param>
        /// <returns>ExamUnit.</returns>
        public async Task<ExamUnit?> GetByIdAsync(Guid id, Guid SubjectID)
        {
            var res = _dbContext.ExamUnits
                .Include(x => x.Subject)
                .ThenInclude(x => x.Course)
                .ThenInclude(x => x.Grade)
                .ThenInclude(x => x.Syllabus)
                 .ThenInclude(x => x.AcademicYear)
                .Where(x => x.Id == id && !x.IsDeleted);

            if (SubjectID != Guid.Empty)
            {
                res = res.Where(x => x.SubjectId == SubjectID);
            }

            return await res.FirstOrDefaultAsync();
        }

        /// <summary>
        /// Adds the asynchronous.
        /// </summary>
        /// <param name="ExamUnit">The ExamUnit.</param>
        /// <returns>ExamUnit.</returns>
        public async Task<ExamUnit> AddAsync(ExamUnit ExamUnit)
        {
            _dbContext.ExamUnits.Add(ExamUnit);
            await _dbContext.SaveChangesAsync();

            return await _dbContext.ExamUnits
                .Include(x => x.Subject)
                        .ThenInclude(s => s.Course)
                        .ThenInclude(s => s.Grade)
                            .ThenInclude(g => g.Syllabus)
                             .ThenInclude(x => x.AcademicYear)
                .FirstOrDefaultAsync(x => x.Id == ExamUnit.Id && !x.IsDeleted);
        }

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="ExamUnit">The ExamUnit.</param>
        /// <returns>task.</returns>
        public async Task<ExamUnit> UpdateAsync(ExamUnit ExamUnit)
        {
            _dbContext.ExamUnits.Update(ExamUnit);
            await _dbContext.SaveChangesAsync();
            return ExamUnit;
        }

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>task.</returns>
        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _dbContext.ExamUnits.FindAsync(id);
            if (entity == null)
            {
                return false;
            }
            entity.IsActive = false;
            entity.IsDeleted = true;

            _dbContext.ExamUnits.Update(entity);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Existses the by code asynchronous.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="SubjectID">The SubjectID.</param>
        /// <returns>bool.</returns>
        public async Task<bool> ExistsByNameAsync(string name, Guid SubjectID)
        {
            name = name.Trim();

            return await _dbContext.ExamUnits
                .Include(x => x.Subject)
                .ThenInclude(x => x.Course)
                .ThenInclude(x => x.Grade)
                .ThenInclude(x => x.Syllabus)
                 .ThenInclude(x => x.AcademicYear)
                .AnyAsync(x =>
                    x.Subject.Id == SubjectID &&
                    x.Name.Trim().ToLower() == name.ToLower());
        }

        /// <summary>
        /// Gets centre by country, mode of study and state.
        /// </summary>
        /// <param name="SubjectID">SubjectID identifier.</param>
        /// <returns>Centre entity if found; otherwise null.</returns>
        public async Task<List<ExamUnit>> GetByFilterAsync(
            Guid? SubjectID)
        {
            var query = _dbContext.ExamUnits.Where(x => !x.IsDeleted)
         .AsNoTracking()
         .Include(x => x.Subject)
         .ThenInclude(x => x.Course)
         .ThenInclude(x => x.Grade)
         .ThenInclude(x => x.Syllabus)
          .ThenInclude(x => x.AcademicYear)
         .AsQueryable();

            if (SubjectID.HasValue)
                query = query.Where(x => x.Subject.Id == SubjectID.Value);

            return await query.ToListAsync();
        }

        /// <summary>
        /// Gets queryable for filtering with full hierarchy.
        /// </summary>
        /// <returns>Queryable ExamUnit.</returns>
        public IQueryable<ExamUnit> Query()
        {
            return _dbContext.ExamUnits.Where(x => !x.IsDeleted)
                .Include(x => x.Subject)
                .ThenInclude(x => x.Course)
                .ThenInclude(x => x.Grade)
                .ThenInclude(x => x.Syllabus)
                 .ThenInclude(x => x.AcademicYear)
                .AsNoTracking();
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="subjectId">The Subject.</param>
        /// <returns>ExamUnit.</returns>
        public async Task<List<ExamUnit>> GetBySubjectIdAsync(Guid subjectId)
        {
            var res = _dbContext.ExamUnits
                .Include(x => x.Subject)
                .ThenInclude(x => x.Course)
                .ThenInclude(x => x.Grade)
                .ThenInclude(x => x.Syllabus)
                 .ThenInclude(x => x.AcademicYear)
                .Where(x => x.SubjectId == subjectId && !x.IsDeleted);
            return await res.ToListAsync();
        }
    }
}
