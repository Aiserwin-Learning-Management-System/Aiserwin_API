namespace Winfocus.LMS.Infrastructure.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using Winfocus.LMS.Application.DTOs;
    using Winfocus.LMS.Application.DTOs.Masters;
    using Winfocus.LMS.Application.Interfaces;
    using Winfocus.LMS.Domain.Entities;
    using Winfocus.LMS.Infrastructure.Data;

    /// <summary>
    /// Provides data access operations for <see cref="State"/> entities.
    /// </summary>
    public sealed class StateRepository : IStateRepository
    {
        /// <summary>
        /// The application database context used to access persistence.
        /// </summary>
        private readonly AppDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="StateRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public StateRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>State list.</returns>
        public async Task<IReadOnlyList<State>> GetAllAsync()
        {
            return await _dbContext.States.Where(x => !x.IsDeleted)
                .Include(x => x.Country)
                .Include(x => x.ModeOfStudy)
                .Include(x => x.Centers)
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="countryId">The countryId.</param>
        /// <returns>State.</returns>
        public async Task<State?> GetByIdAsync(Guid id, Guid countryId)
        {
            return await _dbContext.States
                .Include(x => x.Country)
                .Include(x => x.ModeOfStudy)
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted && x.CountryId == countryId);
        }

        /// <summary>
        /// Adds the asynchronous.
        /// </summary>
        /// <param name="state">The country.</param>
        /// <returns>country.</returns>
        public async Task<State> AddAsync(State state)
        {
            state.CreatedAt = DateTime.UtcNow;
            _dbContext.States.Add(state);
            await _dbContext.SaveChangesAsync();
            return state;
        }

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <returns>task.</returns>
        public async Task<State> UpdateAsync(State state)
        {
            state.UpdatedAt = DateTime.UtcNow;
            _dbContext.States.Update(state);
            await _dbContext.SaveChangesAsync();
            return state;
        }

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="countryId">The countryId.</param>
        /// <returns>task.</returns>
        public async Task<bool> DeleteAsync(Guid id, Guid countryId)
        {
            var entity = await _dbContext.States.FindAsync(id);
            if (entity == null)
            {
                return false;
            }

            if (entity.CountryId != countryId)
            {
                return false;
            }

            entity.IsActive = false;
            entity.IsDeleted = true;

            _dbContext.States.Update(entity);
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
            return await _dbContext.States.AnyAsync(x => x.Name == code && !x.IsDeleted);
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="countryid">The identifier.</param>
        /// <returns>State.</returns>
        public async Task<List<State>> GetByCountryIdAsync(Guid countryid)
        {
            return await _dbContext.States
                .Include(x => x.Country)
                .Include(x => x.ModeOfStudy)
                .Where(x => x.CountryId == countryid && x.IsActive == true && !x.IsDeleted)
                .ToListAsync();
        }

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <param name="countryId">The countryId.</param>
        /// <returns>
        /// state Filter.
        /// </returns>
        public IQueryable<State> Query(Guid countryId)
        {
            return _dbContext.States.Where(x => !x.IsDeleted && x.CountryId == countryId)
                .Include(x => x.Country)
                .Include(x => x.ModeOfStudy)
                .AsNoTracking();
        }
    }
}
