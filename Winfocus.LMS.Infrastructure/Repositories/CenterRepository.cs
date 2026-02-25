namespace Winfocus.LMS.Infrastructure.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Winfocus.LMS.Application.DTOs.Masters;
    using Winfocus.LMS.Application.Interfaces;
    using Winfocus.LMS.Domain.Entities;
    using Winfocus.LMS.Infrastructure.Data;

    /// <summary>
    /// Provides data access operations for <see cref="Centre"/> entities.
    /// </summary>
    public sealed class CentreRepository : ICentreRepository
    {
        /// <summary>
        /// The application database context used to access persistence.
        /// </summary>
        private readonly AppDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="CentreRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public CentreRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>Center list.</returns>
        public async Task<IReadOnlyList<Centre>> GetAllAsync()
        {
            return await _dbContext.Centres
                .Include(x => x.modeOfStudy)
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Center.</returns>
        public async Task<Centre?> GetByIdAsync(Guid id)
        {
            return await _dbContext.Centres
                .Include(x => x.modeOfStudy)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        /// <summary>
        /// Adds the asynchronous.
        /// </summary>
        /// <param name="center">The center.</param>
        /// <returns>center.</returns>
        public async Task<Centre> AddAsync(Centre center)
        {
         var modeOfStudy = await _dbContext.ModeOfStudies
         .FirstOrDefaultAsync(x => x.Id == center.ModeOfStudyId);
         if (modeOfStudy == null)
            {
                throw new Exception("Invalid ModeOfStudyId");
            }

         center.StateId = modeOfStudy.StateId;
         var state = await _dbContext.States
         .FirstOrDefaultAsync(x => x.Id == center.StateId);
         if (state == null)
            {
                throw new Exception("Invalid StateId");
            }

         center.CreatedAt = DateTime.UtcNow;
         center.CountryId = state.CountryId;
         _dbContext.Centres.Add(center);
         await _dbContext.SaveChangesAsync();
         return center;
        }

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="center">The center.</param>
        /// <returns>task.</returns>
        public async Task<Centre> UpdateAsync(Centre center)
        {
            _dbContext.Centres.Update(center);
            await _dbContext.SaveChangesAsync();
            return center;
        }

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>task.</returns>
        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _dbContext.Centres.FindAsync(id);
            if (entity == null)
            {
                return false;
            }

            entity.IsActive = false;

            _dbContext.Centres.Update(entity);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Existses the by code asynchronous.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <returns>bool.</returns>
        public async Task<bool> ExistsByCodeAsync(string code)
        {
            return await _dbContext.Centres.AnyAsync(x => x.Name == code);
        }

        /// <summary>
        /// Gets centre by mode of study and state.
        /// </summary>
        /// <param name="modeofid">Mode of study identifier.</param>
        /// <param name="stateid">State identifier.</param>
        /// <returns>Centre entity if found; otherwise null.</returns>
        public async Task<Centre?> GetByFilterAsync(Guid modeofid, Guid stateid)
        {
            return await _dbContext.Centres
                .AsNoTracking()
                .Include(x => x.modeOfStudy)
                .Include(x => x.State)
                .FirstOrDefaultAsync(x =>
                    x.ModeOfStudyId == modeofid &&
                    x.StateId == stateid);
        }

    }
}
