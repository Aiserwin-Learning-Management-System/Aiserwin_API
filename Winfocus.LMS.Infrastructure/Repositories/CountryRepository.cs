namespace Winfocus.LMS.Infrastructure.Repositories
{
    using System;
    using System.Collections.Generic;
    using Microsoft.EntityFrameworkCore;
    using Winfocus.LMS.Application.Interfaces;
    using Winfocus.LMS.Domain.Entities;
    using Winfocus.LMS.Infrastructure.Data;

    /// <summary>
    /// Repository for managing <see cref="Country"/> entities.
    /// </summary>
    public class CountryRepository : ICountryRepository
    {
        private readonly AppDbContext _db;

        /// <summary>
        /// Initializes a new instance of the <see cref="CountryRepository"/> class.
        /// </summary>
        /// <param name="db">The application database context.</param>
        public CountryRepository(AppDbContext db) => _db = db;

        /// <inheritdoc/>
        public async Task<Country> AddAsync(Country country)
        {
            _db.Countries.Add(country);
            await _db.SaveChangesAsync();
            return country;
        }

        /// <inheritdoc/>
        public async Task DeleteAsync(Guid id)
        {
            var entity = await _db.Countries.FindAsync(id);
            if (entity == null)
            {
                return;
            }

            _db.Countries.Remove(entity);
            await _db.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Country>> GetAllAsync()
            => await _db.Countries.Include(c => c.Centres).ToListAsync();

        /// <inheritdoc/>
        public async Task<Country?> GetByIdAsync(Guid id)
            => await _db.Countries.Include(c => c.Centres).FirstOrDefaultAsync(c => c.Id == id);

        /// <inheritdoc/>
        public async Task UpdateAsync(Country country)
        {
            _db.Countries.Update(country);
            await _db.SaveChangesAsync();
        }
    }
}
