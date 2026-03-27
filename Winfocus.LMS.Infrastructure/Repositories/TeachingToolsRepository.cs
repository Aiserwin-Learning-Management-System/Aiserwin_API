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
    /// Provides data access operations for <see cref="Teachingtools"/> entities.
    /// </summary>
    public class TeachingToolsRepository : ITeachingToolsRepository
    {
        /// <summary>
        /// The application database context used to access persistence.
        /// </summary>
        private readonly AppDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="TeachingToolsRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public TeachingToolsRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>AcademicYear list.</returns>
        public async Task<IReadOnlyList<TeachingTools>> GetAllAsync()
        {
            return await _dbContext.TeachingTools.Where(x => !x.IsDeleted)
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>TeachingTools.</returns>
        public async Task<TeachingTools?> GetByIdAsync(Guid id)
        {
            return await _dbContext.TeachingTools
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        }

        /// <summary>
        /// Adds the asynchronous.
        /// </summary>
        /// <param name="tools">The teaching tools.</param>
        /// <returns>teaching tools.</returns>
        public async Task<TeachingTools> AddAsync(TeachingTools tools)
        {
            _dbContext.TeachingTools.Add(tools);
            await _dbContext.SaveChangesAsync();
            return tools;
        }

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="tools">The teaching tools.</param>
        /// <returns>task.</returns>
        public async Task<TeachingTools> UpdateAsync(TeachingTools tools)
        {
            tools.UpdatedAt = DateTime.UtcNow;
            _dbContext.TeachingTools.Update(tools);
            await _dbContext.SaveChangesAsync();
            return tools;
        }

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>task.</returns>
        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _dbContext.TeachingTools.FindAsync(id);
            if (entity == null)
            {
                return false;
            }

            entity.IsActive = false;
            entity.IsDeleted = true;

            _dbContext.TeachingTools.Update(entity);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Gets queryable for filtering with full hierarchy.
        /// </summary>
        /// <returns>Queryable teaching tools.</returns>
        public IQueryable<TeachingTools> Query()
        {
            return _dbContext.TeachingTools.Where(x => !x.IsDeleted)
                .AsNoTracking();
        }

    }
}
