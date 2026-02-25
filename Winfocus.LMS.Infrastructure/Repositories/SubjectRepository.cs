namespace Winfocus.LMS.Infrastructure.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using Winfocus.LMS.Application.Interfaces;
    using Winfocus.LMS.Domain.Entities;
    using Winfocus.LMS.Infrastructure.Data;

    /// <summary>
    /// SubjectRepository.
    /// </summary>
    /// <seealso cref="Winfocus.LMS.Application.Interfaces.ISubjectRepository" />
    public sealed class SubjectRepository : ISubjectRepository
    {
        private readonly AppDbContext _db;

        /// <summary>
        /// Initializes a new instance of the <see cref="SubjectRepository"/> class.
        /// </summary>
        /// <param name="db">The database.</param>
        public SubjectRepository(AppDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>
        /// Subject list.
        /// </returns>
        public async Task<IReadOnlyList<Subject>> GetAllAsync()
            => await _db.Subjects
                .Where(x => x.IsActive)
                .AsNoTracking()
                .ToListAsync();

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// Subject.
        /// </returns>
        public async Task<Subject?> GetByIdAsync(Guid id)
            => await _db.Subjects.FirstOrDefaultAsync(x => x.Id == id && x.IsActive);

        /// <summary>
        /// Gets the by stream asynchronous.
        /// </summary>
        /// <param name="streamId">The stream identifier.</param>
        /// <returns>
        /// Subject list.
        /// </returns>
        public async Task<IReadOnlyList<Subject>> GetByStreamAsync(Guid streamId)
            => await _db.Courses
                .Where(c => c.StreamId == streamId && c.Subject.IsActive && c.IsActive)
                .Select(c => c.Subject)
                .Distinct()
                .AsNoTracking()
                .ToListAsync();

        /// <summary>
        /// Gets the by course ids asynchronous.
        /// </summary>
        /// <param name="courseIds">The course ids.</param>
        /// <returns>
        /// Subject list.
        /// </returns>
        public async Task<IReadOnlyList<Subject>> GetByCourseIdsAsync(List<Guid> courseIds)
        {
            return await _db.Courses
                .Where(c => courseIds.Contains(c.Id) && c.IsActive)
                .Select(c => c.Subject)
                .Distinct()
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Adds the asynchronous.
        /// </summary>
        /// <param name="subject">The subject.</param>
        /// <returns>
        /// Subject.
        /// </returns>
        public async Task<Subject> AddAsync(Subject subject)
        {
            _db.Subjects.Add(subject);
            await _db.SaveChangesAsync();
            return subject;
        }

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="subject">The subject.</param>
        /// <returns>
        /// Task.
        /// </returns>
        public async Task<Subject> UpdateAsync(Subject subject)
        {
            _db.Subjects.Update(subject);
            await _db.SaveChangesAsync();
            return subject;
        }

        /// <summary>
        /// Soft deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// Task.
        /// </returns>
        public async Task<bool> SoftDeleteAsync(Guid id)
        {
            var entity = await _db.Subjects.FindAsync(id);
            if (entity == null)
            {
                return false;
            }

            entity.IsActive = false;
            entity.UpdatedAt = DateTime.UtcNow;

            _db.Subjects.Update(entity);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
