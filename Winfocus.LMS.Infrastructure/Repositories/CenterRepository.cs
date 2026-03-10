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
        public async Task<IReadOnlyList<Center>> GetAllAsync()
        {
            return await _dbContext.Centres
                .Where(x => x.IsActive && !x.IsDeleted)
                .Include(x => x.Country)
                .Include(x => x.State)
                .Include(x => x.modeOfStudy)
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Center.</returns>
        public async Task<Center?> GetByIdAsync(Guid id)
        {
            return await _dbContext.Centres
                .Include(x => x.Country)
                .Include(x => x.State)
                .Include(x => x.modeOfStudy)
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        }

        /// <summary>
        /// Adds the asynchronous.
        /// </summary>
        /// <param name="center">The center.</param>
        /// <returns>center.</returns>
        public async Task<Center> AddAsync(Center center)
        {
         _dbContext.Centres.Add(center);
         await _dbContext.SaveChangesAsync();
         return center;
        }

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="center">The center.</param>
        /// <returns>task.</returns>
        public async Task<Center> UpdateAsync(Center center)
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
            entity.IsDeleted = true;

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
            return await _dbContext.Centres.AnyAsync(x => x.CenterCode == code);
        }

        /// <summary>
        /// Gets centre by country, mode of study and state.
        /// </summary>
        /// <param name="countryId">Country identifier.</param>
        /// <param name="modeOfStudyId">Mode of study identifier.</param>
        /// <param name="stateId">State identifier.</param>
        /// <returns>Centre entity if found; otherwise null.</returns>
        public async Task<List<Center>> GetByFilterAsync(
            Guid? countryId,
            Guid? modeOfStudyId,
            Guid? stateId)
        {
            var query = _dbContext.Centres.Where(x => !x.IsDeleted)
         .AsNoTracking()
         .Include(x => x.Country)
         .Include(x => x.modeOfStudy)
         .Include(x => x.State)
         .AsQueryable();

            if (countryId.HasValue)
                query = query.Where(x => x.CountryId == countryId.Value);

            if (modeOfStudyId.HasValue)
                query = query.Where(x => x.ModeOfStudyId == modeOfStudyId.Value);

            if (stateId.HasValue)
                query = query.Where(x => x.StateId == stateId.Value);

            return await query.ToListAsync();
        }

        /// <summary>
        /// Gets queryable for filtering with full hierarchy.
        /// </summary>
        /// <returns>Queryable center.</returns>
        public IQueryable<Center> Query()
        {
            return _dbContext.Centres.Where(x => !x.IsDeleted)
                .Include(x => x.Country)
                .Include(x => x.State)
                .Include(x => x.modeOfStudy)
                .AsNoTracking();
        }
    }
}
