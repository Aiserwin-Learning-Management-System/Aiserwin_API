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
        /// <param name="centerId">The centerId.</param>
        /// <returns>
        /// Subject list.
        /// </returns>
        public async Task<IReadOnlyList<Subject>> GetAllAsync(Guid centerId)
            => await _db.Subjects
                .Where(x => x.IsActive && !x.IsDeleted && x.Course.Grade.Syllabus.CenterId == centerId)
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
     => await _db.Subjects
         .Include(s => s.Course)
             .ThenInclude(c => c.Stream)
                 .ThenInclude(st => st.Grade)
                     .ThenInclude(g => g.Syllabus)
                      .ThenInclude(x => x.Center)
                         .ThenInclude(x => x.State)
                         .ThenInclude(x => x.Country)
         .FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted);


        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="centerId">The centerId.</param>
        /// <returns>
        /// Subject.
        /// </returns>
        public async Task<Subject?> GetByCenterIdIdAsync(Guid id, Guid centerId)
     => await _db.Subjects
         .Include(s => s.Course)
             .ThenInclude(c => c.Stream)
                 .ThenInclude(st => st.Grade)
                     .ThenInclude(g => g.Syllabus)
                      .ThenInclude(x => x.Center)
                         .ThenInclude(x => x.State)
                         .ThenInclude(x => x.Country)
         .FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted && s.Course.Grade.Syllabus.CenterId == centerId);

        /// <summary>
        /// Gets the by stream asynchronous.
        /// </summary>
        /// <param name="streamId">The stream identifier.</param>
        /// <returns>
        /// Subject list.
        /// </returns>
        public async Task<IReadOnlyList<Subject>> GetByStreamAsync(Guid streamId)
            => await _db.Subjects
                .Include(s => s.Course)
                .Where(s => s.Course.StreamId == streamId && s.IsActive && !s.IsDeleted)
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
            return await _db.Subjects.
                Include(s => s.Course)
                .ThenInclude(c => c.Stream)
                    .ThenInclude(st => st.Grade)
                        .ThenInclude(g => g.Syllabus)
                         .ThenInclude(x => x.Center)
                            .ThenInclude(x => x.State)
                            .ThenInclude(x => x.Country)
                .Where(c => courseIds.Contains(c.CourseId) && c.IsActive)
            return await _db.Subjects
                .Where(c => courseIds.Contains(c.Id) && c.IsActive && !c.IsDeleted)
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
        /// <param name="centerId">The centerId.</param>
        /// <returns>
        /// Task.
        /// </returns>
        public async Task<bool> SoftDeleteAsync(Guid id, Guid centerId)
        {
            var entity = await _db.Subjects
                .Include(x => x.Course)
                .ThenInclude(x => x.Grade)
                .ThenInclude(x => x.SyllabusId)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null)
            {
                return false;
            }

            if (entity.Course.Grade.Syllabus.CenterId != centerId)
            {
                return false;
            }

            entity.IsActive = false;
            entity.UpdatedAt = DateTime.UtcNow;
            entity.IsDeleted = true;

            _db.Subjects.Update(entity);
            await _db.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Gets queryable for filtering with full hierarchy.
        /// </summary>
        /// <returns>Queryable subjects.</returns>
        public IQueryable<Subject> Query()
        {
            return _db.Subjects.Where(x => !x.IsDeleted)
                .Include(c => c.Course)
                    .ThenInclude(g => g.Stream)
                     .ThenInclude(s => s.Grade)
                       .ThenInclude(g => g.Syllabus)
                         .ThenInclude(x => x.Center)
                             .ThenInclude(x => x.State)
                             .ThenInclude(x => x.Country)
                .AsNoTracking();
        }

        /// <summary>
        /// Existses the by code asynchronous.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <returns>bool.</returns>
        public async Task<bool> ExistsByCodeAsync(string code)
        {
            return await _db.Subjects.AnyAsync(x => x.SubjectCode == code && !x.IsDeleted);
        }
    }
}
