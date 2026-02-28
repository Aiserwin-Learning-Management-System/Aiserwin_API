namespace Winfocus.LMS.Infrastructure.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using Winfocus.LMS.Application.Interfaces;
    using Winfocus.LMS.Domain.Entities;
    using Winfocus.LMS.Infrastructure.Data;

    /// <summary>
    /// CourseRepository.
    /// </summary>
    /// <seealso cref="Winfocus.LMS.Application.Interfaces.ICourseRepository" />
    public sealed class CourseRepository : ICourseRepository
    {
        private readonly AppDbContext _db;

        /// <summary>
        /// Initializes a new instance of the <see cref="CourseRepository"/> class.
        /// </summary>
        /// <param name="db">The database.</param>
        public CourseRepository(AppDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>
        /// Course.
        /// </returns>
        public async Task<IReadOnlyList<Course>> GetAllAsync()
            => await _db.Courses
                .Include(c => c.Stream)
                    .ThenInclude(s => s.Grade)
                        .ThenInclude(g => g.Syllabus)
                .Include(c => c.Grade)
                    .ThenInclude(g => g.Syllabus)
                .AsNoTracking()
                .ToListAsync();

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// Course.
        /// </returns>
        public async Task<Course?> GetByIdAsync(Guid id)
    => await _db.Courses
            .Include(c => c.Stream)
            .ThenInclude(s => s.Grade)
                .ThenInclude(g => g.Syllabus)
            .Include(c => c.Grade)
                .ThenInclude(g => g.Syllabus)
        .FirstOrDefaultAsync(x => x.Id == id);

        /// <summary>
        /// Gets the by identifier with subjects asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// Course.
        /// </returns>
        public async Task<Course?> GetByIdWithSubjectsAsync(Guid id)
            => await GetByIdAsync(id);

        /// <summary>
        /// Gets the by stream asynchronous.
        /// </summary>
        /// <param name="streamId">The stream identifier.</param>
        /// <returns>
        /// Course.
        /// </returns>
        public async Task<IReadOnlyList<Course>> GetByStreamAsync(Guid streamId)
    => await _db.Courses
        .Where(c => c.StreamId == streamId && c.IsActive)
        .Include(c => c.Grade)
            .ThenInclude(g => g.Syllabus)
        .Include(c => c.Stream)
        .AsNoTracking()
        .ToListAsync();

        /// <summary>
        /// Gets the by subject asynchronous.
        /// </summary>
        /// <param name="subjectId">The subject identifier.</param>
        /// <returns>
        /// Course.
        /// </returns>
        public async Task<IReadOnlyList<Course>> GetBySubjectAsync(Guid subjectId)
    => await _db.Courses
        .Include(c => c.Grade)
            .ThenInclude(g => g.Syllabus)
        .Include(c => c.Stream)
        .AsNoTracking()
        .ToListAsync();

        /// <summary>
        /// Adds the asynchronous.
        /// </summary>
        /// <param name="course">The course.</param>
        /// <returns>
        /// Course.
        /// </returns>
        public async Task<Course> AddAsync(Course course)
        {
            _db.Courses.Add(course);
            await _db.SaveChangesAsync();
            return course;
        }

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="course">The course.</param>
        /// <returns>
        /// Course.
        /// </returns>
        public async Task<Course> UpdateAsync(Course course)
        {
            _db.Courses.Update(course);
            await _db.SaveChangesAsync();
            return course;
        }

        /// <summary>
        /// Softs the delete asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// Task.
        /// </returns>
        public async Task<bool> SoftDeleteAsync(Guid id)
        {
            var entity = await _db.Courses.FindAsync(id);
            if (entity == null || entity.IsActive == false)
            {
                return false;
            }

            entity.IsActive = false;
            entity.UpdatedAt = DateTime.UtcNow;

            _db.Courses.Update(entity);
            await _db.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Gets queryable for filtering with full hierarchy.
        /// </summary>
        /// <returns>Queryable courses.</returns>
        public IQueryable<Course> Query()
        {
            return _db.Courses
                .Include(c => c.Grade)
                    .ThenInclude(g => g.Syllabus)
                .Include(c => c.Stream)
                    .ThenInclude(s => s.Grade)
                        .ThenInclude(g => g.Syllabus)
                .AsNoTracking();
        }
    }
}
