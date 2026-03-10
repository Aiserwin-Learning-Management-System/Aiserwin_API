using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Winfocus.LMS.Infrastructure.Repositories
{
    using Winfocus.LMS.Application.Interfaces;
    using Winfocus.LMS.Domain.Entities;
    using Winfocus.LMS.Infrastructure.Data;

    /// <summary>
    /// CountryRepository.
    /// </summary>
    /// <seealso cref="Winfocus.LMS.Application.Interfaces.ISyllabusRepository" />
    public sealed class SyllabusRepository : ISyllabusRepository
    {
        private readonly AppDbContext _db;

        /// <summary>
        /// Initializes a new instance of the <see cref="SyllabusRepository"/> class.
        /// </summary>
        /// <param name="db">The database.</param>
        public SyllabusRepository(AppDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>Syllabus list.</returns>
        public async Task<IReadOnlyList<Syllabus>> GetAllAsync()
        {
            return await _db.Syllabuses.Where(x => !x.IsDeleted)
                 .Include(x => x.Center)
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Syllabus.</returns>
        public async Task<Syllabus?> GetByIdAsync(Guid id)
        {
            return await _db.Syllabuses
                 .Include(x => x.Center)
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        }

        /// <summary>
        /// Adds the asynchronous.
        /// </summary>
        /// <param name="syllabus">The syllabus.</param>
        /// <returns>syllabus.</returns>
        public async Task<Syllabus> AddAsync(Syllabus syllabus)
        {
            _db.Syllabuses.Add(syllabus);
            await _db.SaveChangesAsync();

            return await _db.Syllabuses
                .Include(x => x.Center)
                    .ThenInclude(c => c.Country)
                .Include(x => x.Center)
                    .ThenInclude(c => c.State)
                .Include(x => x.Center)
                    .ThenInclude(c => c.modeOfStudy)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == syllabus.Id);
        }

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="syllabus">The syllabus.</param>
        /// <returns>task.</returns>
        public async Task<Syllabus> UpdateAsync(Syllabus syllabus)
        {
            _db.Syllabuses.Update(syllabus);
            await _db.SaveChangesAsync();
            return syllabus;
        }

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>task.</returns>
        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _db.Syllabuses.FindAsync(id);
            if (entity == null)
            {
                return false;
            }

            entity.IsActive = false;
            entity.IsDeleted = true;

            _db.Syllabuses.Update(entity);
            await _db.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Existses the by name asynchronous.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>bool.</returns>
        public async Task<bool> ExistsByNameAsync(string name)
        {
            return await _db.Syllabuses.AnyAsync(x => x.Name == name && !x.IsDeleted);
        }

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>
        /// syllabuses.
        /// </returns>
        public IQueryable<Syllabus> Query()
        {
            return _db.Syllabuses.Where(x => !x.IsDeleted)
                 .Include(x => x.Center)
                 .Include(x => x.Center.Country)
                .Include(x => x.Center.modeOfStudy)
                .Include(x => x.Center.State)
                .AsNoTracking();
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="centerId">The identifier.</param>
        /// <returns>Syllabus.</returns>
        public async Task<List<Syllabus>> GetByCenterIdAsync(Guid centerId)
        {
            return await _db.Syllabuses
                .Include(x => x.Center)
                .Where(x => x.CenterId == centerId && !x.IsDeleted)
                .ToListAsync();
        }
    }
}
