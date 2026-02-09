namespace Winfocus.LMS.Infrastructure.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using Winfocus.LMS.Application.Interfaces;
    using Winfocus.LMS.Domain.Entities;
    using Winfocus.LMS.Infrastructure.Data;

    /// <summary>
    /// GradeRepository.
    /// </summary>
    /// <seealso cref="Winfocus.LMS.Application.Interfaces.IStreamRepository" />
    public sealed class StreamRepository : IStreamRepository
    {
        private readonly AppDbContext _db;

        /// <summary>
        /// Initializes a new instance of the <see cref="StreamRepository"/> class.
        /// </summary>
        /// <param name="db">The database.</param>
        public StreamRepository(AppDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>Stream list.</returns>
        public async Task<IReadOnlyList<Streams>> GetAllAsync()
        {
            return await _db.Streams
                .Include(x => x.Grade)
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Streams.</returns>
        public async Task<Streams?> GetByIdAsync(Guid id)
        {
            return await _db.Streams
                .Include(x => x.Grade)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        /// <summary>
        /// Adds the asynchronous.
        /// </summary>
        /// <param name="streams">The Stream.</param>
        /// <returns>Stream.</returns>
        public async Task<Streams> AddAsync(Streams streams)
        {
            _db.Streams.Add(streams);
            await _db.SaveChangesAsync();
            return streams;
        }

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="streams">The Streams.</param>
        /// <returns>task.</returns>
        public async Task UpdateAsync(Streams streams)
        {
            _db.Streams.Update(streams);
            await _db.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>task.</returns>
        public async Task DeleteAsync(Guid id)
        {
            var entity = await _db.Streams.FindAsync(id);
            if (entity == null)
            {
                return;
            }

            entity.IsActive = false;

            _db.Streams.Update(entity);
            await _db.SaveChangesAsync();
        }

        /// <summary>
        /// Existses the by code asynchronous.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <returns>bool.</returns>
        public async Task<bool> ExistsByCodeAsync(string code)
        {
            return await _db.Streams.AnyAsync(x => x.StreamCode == code);
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="gradeid">The identifier.</param>
        /// <returns>Streams.</returns>
        public async Task<Streams?> GetByGradeIdAsync(Guid gradeid)
        {
            return await _db.Streams
                .Include(x => x.Grade)
                .FirstOrDefaultAsync(x => x.GradeId == gradeid);
        }
}
