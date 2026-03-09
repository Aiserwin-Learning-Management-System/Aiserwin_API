namespace Winfocus.LMS.Infrastructure.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using Winfocus.LMS.Application.Interfaces;
    using Winfocus.LMS.Domain.Entities;
    using Winfocus.LMS.Infrastructure.Data;

    /// <summary>
    /// GradeRepository.
    /// </summary>
    /// <seealso cref="Winfocus.LMS.Application.Interfaces.IGradeRepository" />
    public sealed class GradeRepository : IGradeRepository
    {
        private readonly AppDbContext _db;

        /// <summary>
        /// Initializes a new instance of the <see cref="GradeRepository"/> class.
        /// </summary>
        /// <param name="db">The database.</param>
        public GradeRepository(AppDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>Grade list.</returns>
        public async Task<IReadOnlyList<Grade>> GetAllAsync()
        {
            return await _db.Grades
                .Include(x => x.Syllabus)
                .ThenInclude(x => x.Center)
                .ThenInclude(x => x.State)
                .ThenInclude(x => x.Country)
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Grade.</returns>
        public async Task<Grade?> GetByIdAsync(Guid id)
        {
            return await _db.Grades
                .Include(x => x.Syllabus)
                .ThenInclude(x => x.Center)
                .ThenInclude(x => x.State)
                .ThenInclude(x => x.Country)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        /// <summary>
        /// Adds the asynchronous.
        /// </summary>
        /// <param name="grade">The Grade.</param>
        /// <returns>Grade.</returns>
        public async Task<Grade> AddAsync(Grade grade)
        {
            _db.Grades.Add(grade);
            await _db.SaveChangesAsync();
            return grade;
        }

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="grade">The Grade.</param>
        /// <returns>task.</returns>
        public async Task<Grade> UpdateAsync(Grade grade)
        {
            _db.Grades.Update(grade);
            await _db.SaveChangesAsync();
            return grade;
        }

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>task.</returns>
        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _db.Grades.FindAsync(id);
            if (entity == null)
            {
                return false;
            }

            entity.IsActive = false;
            entity.IsDeleted = true;

            _db.Grades.Update(entity);
            await _db.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Existses the by code asynchronous.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <returns>bool.</returns>
        public async Task<bool> ExistsByCodeAsync(string code)
        {
            return await _db.Grades.AnyAsync(x => x.Name == code);
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="syllabusid">The identifier.</param>
        /// <returns>Grade.</returns>
        public async Task<List<Grade>> GetBySyllabusIdAsync(Guid syllabusid)
        {
            return await _db.Grades
                .Include(x => x.Syllabus)
                .ThenInclude(x => x.Center)
                .ThenInclude(x => x.State)
                .ThenInclude(x => x.Country)
                .Where(x => x.SyllabusId == syllabusid)
                .ToListAsync();
        }

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>
        /// grade.
        /// </returns>
        public IQueryable<Grade> Query()
        {
            return _db.Grades.Include(x => x.Syllabus)
                .ThenInclude(x => x.Center)
                .ThenInclude(x => x.State)
                .ThenInclude(x => x.Country)
                .AsNoTracking();
        }
    }
}
