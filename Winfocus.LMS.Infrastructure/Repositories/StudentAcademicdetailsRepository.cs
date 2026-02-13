using Microsoft.EntityFrameworkCore;
using Winfocus.LMS.Application.Interfaces;
using Winfocus.LMS.Domain.Entities;
using Winfocus.LMS.Infrastructure.Data;

namespace Winfocus.LMS.Infrastructure.Repositories
{
    /// <summary>
    /// Provides data access operations for <see cref="StudentAcademicdetails"/> entities.
    /// </summary>
    public sealed class StudentAcademicdetailsRepository : IStudentAcademicdeatilsRepository
    {
        /// <summary>
        /// The application database context used to access persistence.
        /// </summary>
        private readonly AppDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="StudentAcademicdetailsRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public StudentAcademicdetailsRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>StudentAcademicdetails list.</returns>
        public async Task<IReadOnlyList<StudentAcademicDetails>> GetAllAsync()
        {
            return await _dbContext.StudentAcademicDetails
                .Include(x => x.Country)
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>StudentAcademicdetails.</returns>
        public async Task<StudentAcademicDetails?> GetByIdAsync(Guid id)
        {
            return await _dbContext.StudentAcademicDetails
                .Include(x => x.Country)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        /// <summary>
        /// Adds the asynchronous.
        /// </summary>
        /// <param name="studentAcademicDetails">The studentAcademicDetails.</param>
        /// <returns>StudentAcademicDetails.</returns>
        public async Task<StudentAcademicDetails> AddAsync(StudentAcademicDetails studentAcademicDetails)
        {
            studentAcademicDetails.CreatedAt = DateTime.UtcNow;
            _dbContext.StudentAcademicDetails.Add(studentAcademicDetails);
            await _dbContext.SaveChangesAsync();
            return studentAcademicDetails;
        }

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="studentAcademicDetails">The StudentAcademicDetails.</param>
        /// <returns>task.</returns>
        public async Task UpdateAsync(StudentAcademicDetails studentAcademicDetails)
        {
            studentAcademicDetails.UpdatedAt = DateTime.UtcNow;
            _dbContext.StudentAcademicDetails.Update(studentAcademicDetails);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>task.</returns>
        public async Task DeleteAsync(Guid id)
        {
            var entity = await _dbContext.StudentAcademicDetails.FindAsync(id);
            if (entity == null)
            {
                return;
            }

            entity.IsActive = false;

            _dbContext.StudentAcademicDetails.Update(entity);
            await _dbContext.SaveChangesAsync();
        }
    }
}
