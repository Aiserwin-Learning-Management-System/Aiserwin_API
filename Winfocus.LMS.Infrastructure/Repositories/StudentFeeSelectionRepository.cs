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
    /// StudentFeeSelectionRepository.
    /// </summary>
    public sealed class StudentFeeSelectionRepository : IStudentFeeSelectionRepository
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="StudentFeeSelectionRepository"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        public StudentFeeSelectionRepository(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets the fee plan by identifier asynchronous.
        /// </summary>
        /// <param name="feeselectionId">The student fee selection identifier.</param>
        /// <returns>Task&lt;FeePlan?&gt;.</returns>
        public async Task<StudentFeeSelection?> GetFeePlanByIdAsync(Guid feeselectionId)
        {
            return await _context.StudentFeeSelections
                .FirstOrDefaultAsync(x => x.Id == feeselectionId && !x.IsDeleted);
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>StudentFeeSelection.</returns>
        public async Task<StudentFeeSelection?> GetByIdAsync(Guid id)
        {
            return await _context.StudentFeeSelections
                .Include(x => x.Student)
                .Include(x => x.FeePlan)
                .Include(x => x.Course)
                .FirstOrDefaultAsync(x => x.Student.Id == id && !x.IsDeleted);
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <returns>StudentFeeSelection.</returns>
        public async Task<List<StudentFeeSelection>> GetAllAsync()
        {
            return await _context.StudentFeeSelections.Where(x => !x.IsDeleted)
                .Include(x => x.Student)
                .Include(x => x.FeePlan)
                .Include(x => x.Course)
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Adds the asynchronous.
        /// </summary>
        /// <param name="studentfeeselection">The StudentFeeSelection.</param>
        /// <returns>StudentFeeSelection.</returns>
        public async Task<StudentFeeSelection> AddAsync(StudentFeeSelection studentfeeselection)
        {
            _context.StudentFeeSelections.Add(studentfeeselection);
            await _context.SaveChangesAsync();
            return studentfeeselection;
        }

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="studentfeeselection">The studentfeeselection.</param>
        /// <returns>StudentFeeSelection.</returns>
        public async Task<StudentFeeSelection> UpdateAsync(StudentFeeSelection studentfeeselection)
        {
            studentfeeselection.UpdatedAt = DateTime.UtcNow;
            _context.StudentFeeSelections.Update(studentfeeselection);
            await _context.SaveChangesAsync();
            return studentfeeselection;
        }

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>StudentFeeSelection.</returns>
        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _context.StudentFeeSelections.FindAsync(id);
            if (entity == null)
            {
                return false;
            }

            entity.IsActive = false;
            entity.IsDeleted = true;

            _context.StudentFeeSelections.Update(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Gets queryable for filtering with full hierarchy.
        /// </summary>
        /// <returns>Queryable StudentFeeSelection.</returns>
        public IQueryable<StudentFeeSelection> Query()
        {
            return _context.StudentFeeSelections.Where(x => !x.IsDeleted).Include(x => x.Student)
                  .Include(s => s.FeePlan)
                .Include(x => x.Course)
                .AsNoTracking();
        }

    }
}
