namespace Winfocus.LMS.Infrastructure.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using Winfocus.LMS.Application.Interfaces;
    using Winfocus.LMS.Domain.Entities;
    using Winfocus.LMS.Infrastructure.Data;

    /// <summary>
    /// CountryRepository.
    /// </summary>
    /// <seealso cref="Winfocus.LMS.Application.Interfaces.ICountryRepository" />
    public sealed class CountryRepository : ICountryRepository
    {
        private readonly AppDbContext _db;

        /// <summary>
        /// Initializes a new instance of the <see cref="CountryRepository"/> class.
        /// </summary>
        /// <param name="db">The database.</param>
        public CountryRepository(AppDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>Country list.</returns>
        public async Task<IReadOnlyList<Country>> GetAllAsync()
        {
            return await _db.Countries
                .Include(x => x.Centres)
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Country.</returns>
        public async Task<Country?> GetByIdAsync(Guid id)
        {
            return await _db.Countries
                .Include(x => x.Centres)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        /// <summary>
        /// Adds the asynchronous.
        /// </summary>
        /// <param name="country">The country.</param>
        /// <returns>country.</returns>
        public async Task<Country> AddAsync(Country country)
        {
            _db.Countries.Add(country);
            await _db.SaveChangesAsync();
            return country;
        }

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="country">The country.</param>
        /// <returns>task.</returns>
        public async Task UpdateAsync(Country country)
        {
            _db.Countries.Update(country);
            await _db.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>task.</returns>
        public async Task DeleteAsync(Guid id)
        {
            var entity = await _db.Countries.FindAsync(id);
            if (entity == null)
            {
                return;
            }

            entity.IsActive = false;

            _db.Countries.Update(entity);
            await _db.SaveChangesAsync();
        }

        /// <summary>
        /// Existses the by code asynchronous.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <returns>bool.</returns>
        public async Task<bool> ExistsByCodeAsync(string code)
        {
            return await _db.Countries.AnyAsync(x => x.Code == code);
        }
    }
}
