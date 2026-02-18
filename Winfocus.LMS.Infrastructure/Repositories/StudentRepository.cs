using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Winfocus.LMS.Application.Interfaces;
using Winfocus.LMS.Domain.Entities;
using Winfocus.LMS.Infrastructure.Data;

namespace Winfocus.LMS.Infrastructure.Repositories
{
    /// <summary>
    /// Provides data access operations for <see cref="Student"/> entities.
    /// </summary>
    public sealed class StudentRepository : IStudentRepository
    {
        /// <summary>
        /// The application database context used to access persistence.
        /// </summary>
        private readonly AppDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="StudentRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public StudentRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>Student list.</returns>
        public async Task<IReadOnlyList<Student>> GetAllAsync()
        {
            return await _dbContext.Students
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Student.</returns>
        public async Task<Student?> GetByIdAsync(Guid id)
        {
            return await _dbContext.Students
        .Include(x => x.AcademicDetails)
           .ThenInclude(ad => ad.Country)
        .Include(x => x.AcademicDetails)
           .ThenInclude(ad => ad.State)
        .Include(x => x.AcademicDetails)
           .ThenInclude(ad => ad.ModeOfStudy)
        .Include(x => x.AcademicDetails)
           .ThenInclude(ad => ad.Center)
        .Include(x => x.AcademicDetails)
           .ThenInclude(ad => ad.Syllabus)
        .Include(x => x.AcademicDetails)
           .ThenInclude(ad => ad.Grade)
        .Include(x => x.AcademicDetails)
           .ThenInclude(ad => ad.Stream)
        .Include(x => x.AcademicDetails)
           .ThenInclude(ad => ad.Subject)
        .Include(x => x.StudentPersonalDetails)
        .Include(x => x.StudentDocuments)

        .Include(x => x.StudentAcademicCouses)
            .ThenInclude(sc => sc.Course)

        .Include(x => x.StudentBatchTimingMTFs)
        .Include(x => x.StudentBatchTimingSaturdays)
        .Include(x => x.StudentBatchTimingSundays)

        .FirstOrDefaultAsync(x => x.Id == id);
        }

        /// <summary>
        /// Adds the asynchronous.
        /// </summary>
        /// <param name="student">The student.</param>
        /// <returns>Student.</returns>
        public async Task<Student> AddAsync(Student student)
        {
            student.CreatedAt = DateTime.UtcNow;
            _dbContext.Students.Add(student);
            await _dbContext.SaveChangesAsync();
            return student;
        }

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="student">The student.</param>
        /// <returns>task.</returns>
        public async Task UpdateAsync(Student student)
        {
            student.UpdatedAt = DateTime.UtcNow;
            _dbContext.Students.Update(student);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>task.</returns>
        public async Task DeleteAsync(Guid id)
        {
            var entity = await _dbContext.Students.FindAsync(id);
            if (entity == null)
            {
                return;
            }

            entity.IsActive = false;

            _dbContext.Students.Update(entity);
            await _dbContext.SaveChangesAsync();
        }
    }
}
