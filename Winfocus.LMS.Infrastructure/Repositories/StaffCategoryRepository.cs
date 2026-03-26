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
    /// Provides data access operations for <see cref="StaffCategory"/> entities.
    /// </summary>
    public sealed class StaffCategoryRepository : IStaffCategoryRepository
    {
        /// <summary>
        /// The application database context used to access StaffCategory.
        /// </summary>
        private readonly AppDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="StaffCategoryRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public StaffCategoryRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>StaffCategory list.</returns>
        public async Task<IReadOnlyList<StaffCategory>> GetAllAsync()
        {
            return await _dbContext.StaffCategories
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>StaffCategory.</returns>
        public async Task<StaffCategory?> GetByIdAsync(Guid id)
        {
            return await _dbContext.StaffCategories
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        /// <summary>
        /// Adds the asynchronous.
        /// </summary>
        /// <param name="staffCategory">The AcademicYear.</param>
        /// <returns>AcademicYear.</returns>
        public async Task<StaffCategory> AddAsync(StaffCategory staffCategory)
        {
            _dbContext.StaffCategories.Add(staffCategory);
            await _dbContext.SaveChangesAsync();
            return staffCategory;
        }

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="staffCategory">The staff category.</param>
        /// <returns>task.</returns>
        public async Task<StaffCategory> UpdateAsync(StaffCategory staffCategory)
        {
            staffCategory.UpdatedAt = DateTime.UtcNow;
            _dbContext.StaffCategories.Update(staffCategory);
            await _dbContext.SaveChangesAsync();
            return staffCategory;
        }

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>task.</returns>
        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _dbContext.StaffCategories.FindAsync(id);
            if (entity == null)
            {
                return false;
            }

            entity.IsActive = false;

            _dbContext.StaffCategories.Update(entity);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Gets queryable for filtering with full hierarchy.
        /// </summary>
        /// <returns>Queryable staff category.</returns>
        public IQueryable<StaffCategory> Query()
        {
            return _dbContext.StaffCategories
                .AsNoTracking();
        }
    }
}
