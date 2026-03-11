namespace Winfocus.LMS.Infrastructure.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using Winfocus.LMS.Application.Interfaces;
    using Winfocus.LMS.Domain.Entities;
    using Winfocus.LMS.Infrastructure.Data;

    /// <summary>
    /// StreamRepository.
    /// </summary>
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
        /// Gets all active streams.
        /// </summary>
        /// <param name="centerId">The centerId.</param>
        /// <returns>Stream list.</returns>
        public async Task<IReadOnlyList<Streams>> GetAllAsync(Guid centerId)
        {
            return await _db.Streams
                .Include(x => x.Grade)
                    .ThenInclude(g => g.Syllabus)
                    .ThenInclude(s => s.Center)
                    .ThenInclude(s => s.State)
                    .ThenInclude(s => s.ModeOfStudy)
                    .ThenInclude(s => s.Country)
                .Include(x => x.Courses)
                .Where(x => x.IsActive && !x.IsDeleted && x.Grade.Syllabus.CenterId == centerId)
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
                    .ThenInclude(g => g.Syllabus)
                    .ThenInclude(s => s.Center)
                    .ThenInclude(s => s.State)
                    .ThenInclude(s => s.ModeOfStudy)
                    .ThenInclude(s => s.Country)
                .Include(x => x.Courses)
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="centerId">The centerId.</param>
        /// <returns>Streams.</returns>
        public async Task<Streams?> GetByIdCenterIdAsync(Guid id, Guid centerId)
        {
            return await _db.Streams
                .Include(x => x.Grade)
                    .ThenInclude(g => g.Syllabus)
                    .ThenInclude(s => s.Center)
                    .ThenInclude(s => s.State)
                    .ThenInclude(s => s.ModeOfStudy)
                    .ThenInclude(s => s.Country)
                .Include(x => x.Courses)
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted && x.Grade.Syllabus.CenterId == centerId);
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
        /// <returns>Streams.</returns>
        public async Task<Streams> UpdateAsync(Streams streams)
        {
            _db.Streams.Update(streams);
            await _db.SaveChangesAsync();
            return streams;
        }

        /// <summary>
        /// Soft deletes the stream.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="centerId">The centerId.</param>
        /// <returns>bool.</returns>
        public async Task<bool> DeleteAsync(Guid id, Guid centerId)
        {
            var entity = await _db.Streams
                .Include(x => x.Grade)
                .ThenInclude(x => x.Syllabus)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null)
            {
                return false;
            }

            if (entity.Grade.Syllabus.CenterId != centerId)
            {
                return false;
            }

            entity.IsActive = false;
            entity.IsDeleted = true;
            _db.Streams.Update(entity);
            await _db.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Checks if stream exists by name.
        /// </summary>
        /// <param name="code">The name.</param>
        /// <returns>bool.</returns>
        public async Task<bool> ExistsByCodeAsync(string code)
        {
            return await _db.Streams.AnyAsync(x => x.StreamCode == code && !x.IsDeleted);
        }

        /// <summary>
        /// Gets streams by grade identifier.
        /// </summary>
        /// <param name="gradeid">The grade identifier.</param>
        /// <returns>Stream list.</returns>
        public async Task<List<Streams>> GetByGradeIdAsync(Guid gradeid)
        {
            return await _db.Streams
                .Include(x => x.Grade)
                    .ThenInclude(g => g.Syllabus)
                    .ThenInclude(s => s.Center)
                    .ThenInclude(s => s.State)
                    .ThenInclude(s => s.ModeOfStudy)
                    .ThenInclude(s => s.Country)
                .Include(x => x.Courses)
                .Where(x => x.GradeId == gradeid && x.IsActive && !x.IsDeleted)
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Gets stream with courses by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Streams.</returns>
        public async Task<Streams?> GetByIdWithCoursesAsync(Guid id)
        {
            return await _db.Streams
                .Include(x => x.Courses)
                .ThenInclude(g => g.Grade)
                .ThenInclude(g => g.Syllabus)
                    .ThenInclude(s => s.Center)
                    .ThenInclude(s => s.State)
                    .ThenInclude(s => s.ModeOfStudy)
                    .ThenInclude(s => s.Country)
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        }

        /// <summary>
        /// Gets queryable for filtering with Grade and Syllabus included.
        /// </summary>
        /// <param name="centerId">The centerId.</param>
        /// <returns>Queryable streams.</returns>
        public IQueryable<Streams> Query(Guid centerId)
        {
            return _db.Streams.Where(x => !x.IsDeleted && x.Grade.Syllabus.CenterId == centerId)
                .Include(s => s.Grade)
                    .ThenInclude(g => g.Syllabus)
                    .ThenInclude(s => s.Center)
                    .ThenInclude(s => s.State)
                    .ThenInclude(s => s.ModeOfStudy)
                    .ThenInclude(s => s.Country)
                .AsNoTracking();
        }
    }
}
