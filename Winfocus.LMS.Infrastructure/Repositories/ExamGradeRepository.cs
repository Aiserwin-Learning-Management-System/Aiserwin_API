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
    /// ExamGradeRepository.
    /// </summary>
    public sealed class ExamGradeRepository : IExamGradeRepository
    {
        /// <summary>
        /// The application database context used to access persistence.
        /// </summary>
        private readonly AppDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExamGradeRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public ExamGradeRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>ExamGrade list.</returns>
        public async Task<IReadOnlyList<ExamGrade>> GetAllAsync()
        {
            return await _dbContext.ExamGrades
                .Where(x => x.IsActive && !x.IsDeleted)
                .Include(x => x.Syllabus)
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="syllabusId">The Syllabus.</param>
        /// <returns>ExamGrade.</returns>
        public async Task<ExamGrade?> GetByIdAsync(Guid id, Guid syllabusId)
        {
            var res = _dbContext.ExamGrades
                .Include(x => x.Syllabus)
                .Where(x => x.Id == id && !x.IsDeleted);

            if (syllabusId != Guid.Empty)
            {
                res = res.Where(x => x.SyllabusId == syllabusId);
            }

            return await res.FirstOrDefaultAsync();
        }

        /// <summary>
        /// Adds the asynchronous.
        /// </summary>
        /// <param name="ExamGrade">The ExamGrade.</param>
        /// <returns>ExamGrade.</returns>
        public async Task<ExamGrade> AddAsync(ExamGrade ExamGrade)
        {
            _dbContext.ExamGrades.Add(ExamGrade);
            await _dbContext.SaveChangesAsync();
            return ExamGrade;
        }

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="ExamGrade">The ExamGrade.</param>
        /// <returns>task.</returns>
        public async Task<ExamGrade> UpdateAsync(ExamGrade ExamGrade)
        {
            _dbContext.ExamGrades.Update(ExamGrade);
            await _dbContext.SaveChangesAsync();
            return ExamGrade;
        }

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>task.</returns>
        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _dbContext.ExamGrades.FindAsync(id);
            if (entity == null)
            {
                return false;
            }
            entity.IsActive = false;
            entity.IsDeleted = true;

            _dbContext.ExamGrades.Update(entity);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Existses the by code asynchronous.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="syllabusID">The academicYearId.</param>
        /// <returns>bool.</returns>
        public async Task<bool> ExistsByNameAsync(string name, Guid syllabusID)
        {
            name = name.Trim();

            return await _dbContext.ExamGrades
                .AnyAsync(x =>
                    x.SyllabusId == syllabusID &&
                    x.Name.Trim().ToLower() == name.ToLower());
        }

        /// <summary>
        /// Gets centre by country, mode of study and state.
        /// </summary>
        /// <param name="SyllabusID">academicYear identifier.</param>
        /// <returns>Centre entity if found; otherwise null.</returns>
        public async Task<List<ExamGrade>> GetByFilterAsync(
            Guid? SyllabusID)
        {
            var query = _dbContext.ExamGrades.Where(x => !x.IsDeleted)
         .AsNoTracking()
         .Include(x => x.Syllabus)
         .AsQueryable();

            if (SyllabusID.HasValue)
                query = query.Where(x => x.SyllabusId == SyllabusID.Value);

            return await query.ToListAsync();
        }

        /// <summary>
        /// Gets queryable for filtering with full hierarchy.
        /// </summary>
        /// <returns>Queryable ExamGrade.</returns>
        public IQueryable<ExamGrade> Query()
        {
            return _dbContext.ExamGrades.Where(x => !x.IsDeleted)
                .Include(x => x.Syllabus)
                .ThenInclude(x => x.AcademicYear)
                .AsNoTracking();
        }
    }
}
