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
            return await _db.Syllabuses
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
                .FirstOrDefaultAsync(x => x.Id == id);
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
            return syllabus;
        }

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="syllabus">The syllabus.</param>
        /// <returns>task.</returns>
        public async Task UpdateAsync(Syllabus syllabus)
        {
            _db.Syllabuses.Update(syllabus);
            await _db.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>task.</returns>
        public async Task DeleteAsync(Guid id)
        {
            var entity = await _db.Syllabuses.FindAsync(id);
            if (entity == null)
            {
                return;
            }

            entity.IsActive = false;

            _db.Syllabuses.Update(entity);
            await _db.SaveChangesAsync();
        }

        /// <summary>
        /// Existses the by code asynchronous.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <returns>bool.</returns>
        public async Task<bool> ExistsByCodeAsync(string code)
        {
            return await _db.Syllabuses.AnyAsync(x => x.SyllabusCode == code);
        }
    }
}
