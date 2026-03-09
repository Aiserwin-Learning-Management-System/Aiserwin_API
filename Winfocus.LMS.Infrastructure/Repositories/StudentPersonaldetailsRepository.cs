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
    /// Provides data access operations for <see cref="StudentPersonaldetails"/> entities.
    /// </summary>
    public sealed class StudentPersonaldetailsRepository : IStudentPersonaldetailsRepository
    {
        /// <summary>
        /// The application database context used to access persistence.
        /// </summary>
        private readonly AppDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="StudentPersonaldetailsRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public StudentPersonaldetailsRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>StudentPersonalDetails list.</returns>
        public async Task<IReadOnlyList<StudentPersonalDetails>> GetAllAsync()
        {
            return await _dbContext.StudentPersonalDetails
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>StudentPersonalDetails.</returns>
        public async Task<StudentPersonalDetails?> GetByIdAsync(Guid id)
        {
            return await _dbContext.StudentPersonalDetails
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        /// <summary>
        /// Adds the asynchronous.
        /// </summary>
        /// <param name="studentPersonalDetails">The studentAcademicDetails.</param>
        /// <returns>studentPersonalDetails.</returns>
        public async Task<StudentPersonalDetails> AddAsync(StudentPersonalDetails studentPersonalDetails)
        {
            studentPersonalDetails.CreatedAt = DateTime.UtcNow;
            _dbContext.StudentPersonalDetails.Add(studentPersonalDetails);
            await _dbContext.SaveChangesAsync();
            return studentPersonalDetails;
        }

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="studentPersonalDetails">The studentPersonalDetails.</param>
        /// <returns>task.</returns>
        public async Task<StudentPersonalDetails> UpdateAsync(StudentPersonalDetails studentPersonalDetails)
        {
            studentPersonalDetails.UpdatedAt = DateTime.UtcNow;
            _dbContext.StudentPersonalDetails.Update(studentPersonalDetails);
            await _dbContext.SaveChangesAsync();
            return studentPersonalDetails;
        }

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>task.</returns>
        public async Task DeleteAsync(Guid id)
        {
            var entity = await _dbContext.StudentPersonalDetails.FindAsync(id);
            if (entity == null)
            {
                return;
            }

            entity.IsActive = false;
            entity.IsDeleted = true;

            _dbContext.StudentPersonalDetails.Update(entity);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="email">The identifier.</param>
        /// <returns>StudentPersonalDetails.</returns>
        public async Task<StudentPersonalDetails?> GetByEmailAsync(string email)
        {
            return await _dbContext.StudentPersonalDetails
                .FirstOrDefaultAsync(x => x.EmailAddress == email);
        }
    }
}
