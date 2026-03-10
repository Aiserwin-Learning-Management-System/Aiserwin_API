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
        /// <param name="centerId">The centerId.</param>
        /// <returns>Grade list.</returns>
        public async Task<IReadOnlyList<Grade>> GetAllAsync(Guid centerId)
        {
            return await _db.Grades.Where(x => !x.IsDeleted && x.Syllabus.CenterId == centerId)
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
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="centerId">The centerId.</param>
        /// <returns>Grade.</returns>
        public async Task<Grade?> GetByIdCenterIdAsync(Guid id, Guid centerId)
        {
            return await _db.Grades
                .Include(x => x.Syllabus)
                .ThenInclude(x => x.Center)
                .ThenInclude(x => x.State)
                .ThenInclude(x => x.Country)
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted && x.Syllabus.CenterId == centerId);
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
        /// <param name="centerId">The centerId.</param>
        /// <returns>task.</returns>
        public async Task<bool> DeleteAsync(Guid id, Guid centerId)
        {
            var entity = await _db.Grades.FindAsync(id);
            if (entity == null)
            {
                return false;
            }

            if (entity.Id != centerId)
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
            return await _db.Grades.AnyAsync(x => x.Name == code && !x.IsDeleted);
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
                .Where(x => x.SyllabusId == syllabusid && !x.IsDeleted)
                .ToListAsync();
        }

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <param name="centerId">The centerId.</param>
        /// <returns>
        /// grade.
        /// </returns>
        public IQueryable<Grade> Query(Guid centerId)
        {
            return _db.Grades.Where(x => !x.IsDeleted && x.Syllabus.CenterId == centerId).Include(x => x.Syllabus)
                .ThenInclude(x => x.Center)
                .ThenInclude(x => x.State)
                .ThenInclude(x => x.Country)
                .AsNoTracking();
        }
    }
}
