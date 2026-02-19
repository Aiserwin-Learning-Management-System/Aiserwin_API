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
            return await _dbContext.AcademicYears
                .AsNoTracking()
                .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<AcademicYear?> GetByDateAsync(DateTime date)
        {
            return await _dbContext.AcademicYears
                .FirstOrDefaultAsync(x =>
                    x.StartDate <= date &&
                    x.EndDate >= date);
        }
    }
}
