using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Winfocus.LMS.Application.Interfaces;
using Winfocus.LMS.Domain.Entities;
using Winfocus.LMS.Infrastructure.Data;

namespace Winfocus.LMS.Infrastructure.Repositories
{
    public class CountryRepository : ICountryRepository
    {
        private readonly AppDbContext _db;
        public CountryRepository(AppDbContext db) => _db = db;

        public async Task<Country> AddAsync(Country country)
        {
            _db.Countries.Add(country);
            await _db.SaveChangesAsync();
            return country;
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _db.Countries.FindAsync(id);
            if (entity == null) return;
            _db.Countries.Remove(entity);
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<Country>> GetAllAsync()
            => await _db.Countries.Include(c => c.Centres).ToListAsync();

        public async Task<Country?> GetByIdAsync(Guid id)
            => await _db.Countries.Include(c => c.Centres).FirstOrDefaultAsync(c => c.Id == id);

        public async Task UpdateAsync(Country country)
        {
            _db.Countries.Update(country);
            await _db.SaveChangesAsync();
        }
    }
}
