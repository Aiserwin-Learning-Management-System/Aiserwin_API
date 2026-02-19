using Microsoft.EntityFrameworkCore;
using Winfocus.LMS.Application.Interfaces;
using Winfocus.LMS.Domain.Entities;
using Winfocus.LMS.Infrastructure.Data;

namespace Winfocus.LMS.Infrastructure.Repositories
{
    /// <summary>
    /// Provides data access operations for <see cref="StudentAcademicdetails"/> entities.
    /// </summary>
    public sealed class StudentAcademicdetailsRepository : IStudentAcademicdetailsRepository
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
        public async Task<StudentAcademicDetails> UpdateAsync(StudentAcademicDetails studentAcademicDetails)
        {
            studentAcademicDetails.UpdatedAt = DateTime.UtcNow;
            _dbContext.StudentAcademicDetails.Update(studentAcademicDetails);
            await _dbContext.SaveChangesAsync();
            return studentAcademicDetails;
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

        /// <summary>
        /// Adds the asynchronous.
        /// </summary>
        /// <param name="studentDocuments">The studentDocuments.</param>
        /// <returns>StudentDocuments.</returns>
        public async Task<StudentDocuments> AddUploadedDocuments(StudentDocuments studentDocuments)
        {
            studentDocuments.CreatedAt = DateTime.UtcNow;
            _dbContext.StudentDocuments.Add(studentDocuments);
            await _dbContext.SaveChangesAsync();
            return studentDocuments;
        }

        /// <inheritdoc/>
        public async Task AddCourseRangeAsync(List<StudentAcademicCouses> courses)
        {
            await _dbContext.StudentAcademicCouses.AddRangeAsync(courses);
            await _dbContext.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task UpdateCourseRangeAsync(List<StudentAcademicCouses> courses)
        {
            if (courses == null || !courses.Any())
                return;

            var studentId = courses.First().StudentId;

            var incomingCourseIds = courses
                .Select(x => x.CourseId)
                .ToList();

            var existingCourses = await _dbContext.StudentAcademicCouses
                .Where(x => x.StudentId == studentId)
                .ToListAsync();

            var toRemove = existingCourses
                .Where(x => !incomingCourseIds.Contains(x.CourseId))
                .ToList();

            _dbContext.StudentAcademicCouses.RemoveRange(toRemove);

            var existingIds = existingCourses
                .Select(x => x.CourseId)
                .ToList();

            var toAdd = courses
                .Where(x => !existingIds.Contains(x.CourseId))
                .ToList();

            await _dbContext.StudentAcademicCouses.AddRangeAsync(toAdd);

            await _dbContext.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task AddBatchtimingmtfRangeAsync(List<StudentBatchTimingMTF> batchmtfs)
        {
            await _dbContext.StudentBatchTimingMTFs.AddRangeAsync(batchmtfs);
            await _dbContext.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task UpdateBatchtimingmtfRangeAsync(List<StudentBatchTimingMTF> batchmtfs)
        {
            if (batchmtfs == null || !batchmtfs.Any())
                return;

            var studentId = batchmtfs.First().StudentId;

            var incomingBatchIds = batchmtfs
                .Select(x => x.BatchTimingMTFId)
                .ToList();

            var existingBatchTimings = await _dbContext.StudentBatchTimingMTFs
                .Where(x => x.StudentId == studentId)
                .ToListAsync();

            var toRemove = existingBatchTimings
                .Where(x => !incomingBatchIds.Contains(x.BatchTimingMTFId))
                .ToList();

            _dbContext.StudentBatchTimingMTFs.RemoveRange(toRemove);

            var existingIds = existingBatchTimings
                .Select(x => x.BatchTimingMTFId)
                .ToList();

            var toAdd = batchmtfs
                .Where(x => !existingIds.Contains(x.BatchTimingMTFId))
                .ToList();

            await _dbContext.StudentBatchTimingMTFs.AddRangeAsync(toAdd);

            await _dbContext.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task AddBatchtimingsaturdayRangeAsync(List<StudentBatchTimingSaturday> batchsaturdays)
        {
            await _dbContext.StudentBatchTimingSaturdays.AddRangeAsync(batchsaturdays);
            await _dbContext.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task UpdateBatchtimingsaturdayRangeAsync(List<StudentBatchTimingSaturday> batchsaturdays)
        {
            if (batchsaturdays == null || !batchsaturdays.Any())
                return;

            var studentId = batchsaturdays.First().StudentId;

            var incomingBatchIds = batchsaturdays
                .Select(x => x.BatchTimingSaturdayId)
                .ToList();

            var existingBatchTimings = await _dbContext.StudentBatchTimingSaturdays
                .Where(x => x.StudentId == studentId)
                .ToListAsync();

            var toRemove = existingBatchTimings
                .Where(x => !incomingBatchIds.Contains(x.BatchTimingSaturdayId))
                .ToList();

            _dbContext.StudentBatchTimingSaturdays.RemoveRange(toRemove);

            var existingIds = existingBatchTimings
                .Select(x => x.BatchTimingSaturdayId)
                .ToList();

            var toAdd = batchsaturdays
                .Where(x => !existingIds.Contains(x.BatchTimingSaturdayId))
                .ToList();

            await _dbContext.StudentBatchTimingSaturdays.AddRangeAsync(toAdd);

            await _dbContext.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task AddBatchtimingsundayRangeAsync(List<StudentBatchTimingSunday> batchsundays)
        {
            await _dbContext.StudentBatchTimingSundays.AddRangeAsync(batchsundays);
            await _dbContext.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task UpdateBatchtimingsundayRangeAsync(List<StudentBatchTimingSunday> batchsundays)
        {
            if (batchsundays == null || !batchsundays.Any())
                return;

            var studentId = batchsundays.First().StudentId;

            var incomingBatchIds = batchsundays
                .Select(x => x.BatchTimingSundayId)
                .ToList();

            var existingBatchTimings = await _dbContext.StudentBatchTimingSundays
                .Where(x => x.StudentId == studentId)
                .ToListAsync();

            var toRemove = existingBatchTimings
                .Where(x => !incomingBatchIds.Contains(x.BatchTimingSundayId))
                .ToList();

            _dbContext.StudentBatchTimingSundays.RemoveRange(toRemove);

            var existingIds = existingBatchTimings
                .Select(x => x.BatchTimingSundayId)
                .ToList();

            var toAdd = batchsundays
                .Where(x => !existingIds.Contains(x.BatchTimingSundayId))
                .ToList();

            await _dbContext.StudentBatchTimingSundays.AddRangeAsync(toAdd);

            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Student document details.</returns>
        public async Task<StudentDocuments?> DocsGetByIdAsync(Guid id)
        {
            return await _dbContext.StudentDocuments
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        /// <summary>
        /// updates the asynchronous.
        /// </summary>
        /// <param name="studentDocuments">The studentDocuments.</param>
        /// <returns>StudentDocuments.</returns>
        public async Task<StudentDocuments> UpdateUploadedDocuments(StudentDocuments studentDocuments)
        {
            studentDocuments.UpdatedAt = DateTime.UtcNow;
            _dbContext.StudentDocuments.Update(studentDocuments);
            await _dbContext.SaveChangesAsync();
            return studentDocuments;
        }
    }
}
