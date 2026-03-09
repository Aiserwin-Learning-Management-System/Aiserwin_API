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
                .Include(x => x.Centers)
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
                .Include(x => x.Centers)
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
        public async Task<Country> UpdateAsync(Country country)
        {
            _db.Countries.Update(country);
            await _db.SaveChangesAsync();
            return country;
        }

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>task.</returns>
        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _db.Countries.FindAsync(id);
            if (entity == null)
            {
                return false;
            }

            entity.UpdatedAt = DateTime.UtcNow;
            entity.IsActive = false;
            entity.IsDeleted = true;

            _db.Countries.Update(entity);
            await _db.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Existses the by code asynchronous.
        /// </summary>
        /// <param name="name">The code.</param>
        /// <returns>bool.</returns>
        public async Task<bool> ExistsByNameAsync(string name)
        {
            return await _db.Countries.AnyAsync(x => x.Name == name);
        }

        /// <summary>
        /// Gets queryable for filtering with full hierarchy.
        /// </summary>
        /// <returns>Queryable Countries.</returns>
        public IQueryable<Country> Query()
        {
            return _db.Countries
                .AsNoTracking();
        }
    }
}
