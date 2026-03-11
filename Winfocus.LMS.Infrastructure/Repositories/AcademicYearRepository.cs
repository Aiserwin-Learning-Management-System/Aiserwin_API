namespace Winfocus.LMS.Infrastructure.Repositories
{
    using System.Collections.Generic;
    using Microsoft.EntityFrameworkCore;
    using Winfocus.LMS.Application.Interfaces;
    using Winfocus.LMS.Domain.Entities;
    using Winfocus.LMS.Infrastructure.Data;

    /// <summary>
    /// Provides data access operations for <see cref="Academicyear"/> entities.
    /// </summary>
    public class AcademicYearRepository : IAcademicYearRepository
    {
        /// <summary>
        /// The application database context used to access persistence.
        /// </summary>
        private readonly AppDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="AcademicYearRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public AcademicYearRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>AcademicYear list.</returns>
        public async Task<IReadOnlyList<AcademicYear>> GetAllAsync()
        {
            return await _dbContext.AcademicYears.Where(predicate => !predicate.IsDeleted)
                .AsNoTracking()
                .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<AcademicYear?> GetByDateAsync(DateTime date)
        {
            return await _dbContext.AcademicYears
                .FirstOrDefaultAsync(x =>
                    x.StartDate <= date &&
                    x.EndDate >= date && !x.IsDeleted);
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Academic year.</returns>
        public async Task<AcademicYear?> GetByIdAsync(Guid id)
        {
            return await _dbContext.AcademicYears
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        }

        /// <summary>
        /// Adds the asynchronous.
        /// </summary>
        /// <param name="academicYear">The AcademicYear.</param>
        /// <returns>AcademicYear.</returns>
        public async Task<AcademicYear> AddAsync(AcademicYear academicYear)
        {
            _dbContext.AcademicYears.Add(academicYear);
            await _dbContext.SaveChangesAsync();
            return academicYear;
        }

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="academicYear">The academicYear.</param>
        /// <returns>task.</returns>
        public async Task<AcademicYear> UpdateAsync(AcademicYear academicYear)
        {
            academicYear.UpdatedAt = DateTime.UtcNow;
            _dbContext.AcademicYears.Update(academicYear);
            await _dbContext.SaveChangesAsync();
            return academicYear;
        }

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>task.</returns>
        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _dbContext.AcademicYears.FindAsync(id);
            if (entity == null)
            {
                return false;
            }

            entity.IsActive = false;
            entity.IsDeleted = true;

            _dbContext.AcademicYears.Update(entity);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Gets queryable for filtering with full hierarchy.
        /// </summary>
        /// <returns>Queryable academicYear.</returns>
        public IQueryable<AcademicYear> Query()
        {
            return _dbContext.AcademicYears.Where(predicate => !predicate.IsDeleted)
                .AsNoTracking();
        }
    }
}
