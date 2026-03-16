namespace Winfocus.LMS.Infrastructure.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Winfocus.LMS.Application.DTOs.Masters;
    using Winfocus.LMS.Application.Interfaces;
    using Winfocus.LMS.Domain.Entities;
    using Winfocus.LMS.Infrastructure.Data;

    /// <summary>
    /// Provides data access operations for <see cref="ModeOfStudy"/> entities.
    /// </summary>
    public sealed class ModeOfStudyRepository : IModeOfStudyRepository
    {
        /// <summary>
        /// The application database context used to access persistence.
        /// </summary>
        private readonly AppDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModeOfStudyRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public ModeOfStudyRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>Mode of study list.</returns>
        public async Task<IReadOnlyList<ModeOfStudy>> GetAllAsync()
        {
            return await _dbContext.ModeOfStudies.Where(x => !x.IsDeleted)
                .Include(x => x.Country)
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="countryId">The countryId.</param>
        /// <returns>Country.</returns>
        public async Task<ModeOfStudy?> GetByIdAsync(Guid id, Guid countryId)
        {
            var query = _dbContext.ModeOfStudies
         .Include(x => x.Country)
         .Where(x => x.Id == id && !x.IsDeleted);

            if (countryId != Guid.Empty)
            {
                query = query.Where(x => x.CountryId == countryId);
            }

            return await query.FirstOrDefaultAsync();
        }

        /// <summary>
        /// Adds the asynchronous.
        /// </summary>
        /// <param name="modeofstudy">The modeofstudy.</param>
        /// <returns>modeofstudy.</returns>
        public async Task<ModeOfStudy> AddAsync(ModeOfStudy modeofstudy)
        {
            _dbContext.ModeOfStudies.Add(modeofstudy);
            await _dbContext.SaveChangesAsync();
            return modeofstudy;
        }

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="modeofstudy">The modeofstudy.</param>
        /// <returns>task.</returns>
        public async Task<ModeOfStudy> UpdateAsync(ModeOfStudy modeofstudy)
        {
            _dbContext.ModeOfStudies.Update(modeofstudy);
            await _dbContext.SaveChangesAsync();
            return modeofstudy;
        }

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="countryId">The countryId.</param>
        /// <returns>task.</returns>
        public async Task<bool> DeleteAsync(Guid id, Guid countryId)
        {
            var entity = await _dbContext.ModeOfStudies.FindAsync(id);
            if (entity == null)
            {
                return false;
            }

            if (countryId != Guid.Empty && entity.CountryId != countryId)
            {
                return false;
            }

            entity.IsActive = false;
            entity.IsDeleted = true;

            _dbContext.ModeOfStudies.Update(entity);
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
            return await _dbContext.ModeOfStudies.AnyAsync(x => x.Name == code && !x.IsDeleted);
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="countryid">The identifier.</param>
        /// <returns>Modeofstudy.</returns>
        public async Task<List<ModeOfStudy>> GetByCountryIdAsync(Guid countryid)
        {
            return await _dbContext.ModeOfStudies
                .Include(x => x.Country)
                .Where(x => x.CountryId == countryid && x.IsActive == true && !x.IsDeleted)
                .ToListAsync();
        }

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <param name="countryId">The identifier.</param>
        /// <returns>
        /// modeofstudy.
        /// </returns>
        public IQueryable<ModeOfStudy> Query(Guid countryId)
        {
           var res = _dbContext.ModeOfStudies.Where(x => !x.IsDeleted)
                .Include(x => x.Country)
                .AsNoTracking();
           if (Guid.Empty != countryId)
            {
                res = res.Where(x => x.CountryId == countryId);
            }

           return res;
        }
    }
}
