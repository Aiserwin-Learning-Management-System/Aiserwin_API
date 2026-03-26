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
    /// ExamSyllabusRepository.
    /// </summary>
    public sealed class ExamSyllabusRepository : IExamSyllabusRepository
    {
        /// <summary>
        /// The application database context used to access persistence.
        /// </summary>
        private readonly AppDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExamSyllabusRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public ExamSyllabusRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>ExamSyllabus list.</returns>
        public async Task<IReadOnlyList<ExamSyllabus>> GetAllAsync()
        {
            return await _dbContext.ExamSyllabuses
                .Where(x => x.IsActive && !x.IsDeleted)
                .Include(x => x.AcademicYear)
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="accademicYearId">The accademicYearId.</param>
        /// <returns>ExamSyllabus.</returns>
        public async Task<ExamSyllabus?> GetByIdAsync(Guid id, Guid accademicYearId)
        {
            var res = _dbContext.ExamSyllabuses
                .Include(x => x.AcademicYear)
                .Where(x => x.Id == id && !x.IsDeleted);

            if (accademicYearId != Guid.Empty)
            {
                res = res.Where(x => x.AcademicYearId == accademicYearId);
            }

            return await res.FirstOrDefaultAsync();
        }

        /// <summary>
        /// Adds the asynchronous.
        /// </summary>
        /// <param name="examSyllabus">The examSyllabus.</param>
        /// <returns>ExamSyllabus.</returns>
        public async Task<ExamSyllabus> AddAsync(ExamSyllabus examSyllabus)
        {
            _dbContext.ExamSyllabuses.Add(examSyllabus);
            await _dbContext.SaveChangesAsync();
            return examSyllabus;
        }

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="examSyllabus">The examSyllabus.</param>
        /// <returns>task.</returns>
        public async Task<ExamSyllabus> UpdateAsync(ExamSyllabus examSyllabus)
        {
            _dbContext.ExamSyllabuses.Update(examSyllabus);
            await _dbContext.SaveChangesAsync();
            return examSyllabus;
        }

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>task.</returns>
        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _dbContext.ExamSyllabuses.FindAsync(id);
            if (entity == null)
            {
                return false;
            }
            entity.IsActive = false;
            entity.IsDeleted = true;

            _dbContext.ExamSyllabuses.Update(entity);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Existses the by code asynchronous.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="academicYearId">The academicYearId.</param>
        /// <returns>bool.</returns>
        public async Task<bool> ExistsByNameAsync(string name, Guid academicYearId)
        {
            name = name.Trim();

            return await _dbContext.ExamSyllabuses
                .AnyAsync(x =>
                    x.AcademicYearId == academicYearId &&
                    x.Name.Trim().ToLower() == name.ToLower());
        }

        /// <summary>
        /// Gets centre by country, mode of study and state.
        /// </summary>
        /// <param name="academicYearId">academicYear identifier.</param>
        /// <returns>Centre entity if found; otherwise null.</returns>
        public async Task<List<ExamSyllabus>> GetByFilterAsync(
            Guid? academicYearId)
        {
            var query = _dbContext.ExamSyllabuses.Where(x => !x.IsDeleted)
         .AsNoTracking()
         .Include(x => x.AcademicYear)
         .AsQueryable();

            if (academicYearId.HasValue)
                query = query.Where(x => x.AcademicYear.Id == academicYearId.Value);

            return await query.ToListAsync();
        }

        /// <summary>
        /// Gets queryable for filtering with full hierarchy.
        /// </summary>
        /// <returns>Queryable ExamSyllabus.</returns>
        public IQueryable<ExamSyllabus> Query()
        {
            return _dbContext.ExamSyllabuses.Include(x => x.AcademicYear).Where(x => !x.IsDeleted)
                .AsNoTracking();
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="yearId">The year.</param>
        /// <returns>ExamSyllabus.</returns>
        public async Task<List<ExamSyllabus>> GetByYearIdAsync(Guid yearId)
        {
            var res = _dbContext.ExamSyllabuses
                .Include(x => x.AcademicYear)
                .Where(x => x.AcademicYearId == yearId && !x.IsDeleted);
            return await res.ToListAsync();
        }
    }
}
