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
    /// Represents a question-typing task assigned to a DTP operator.
    /// Tracks assignment details, progress, and deadline.
    /// </summary>
    public class TaskAssignmentRepository : ITaskAssignmentRepository
    {
        private readonly AppDbContext _db;

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskAssignmentRepository"/> class.
        /// </summary>
        /// <param name="db">The database.</param>
        public TaskAssignmentRepository(AppDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>TaskAssignment list.</returns>
        public async Task<IReadOnlyList<TaskAssignment>> GetAllAsync()
        {
            return await _db.TaskAssignments.Where(x => !x.IsDeleted)
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>TaskAssignment.</returns>
        public async Task<TaskAssignment?> GetByIdAsync(Guid id)
        {
            var query = _db.TaskAssignments
                  .Where(x => x.Id == id && !x.IsDeleted);
            return await query.FirstOrDefaultAsync();
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="operatorid">The identifier.</param>
        /// <returns>TaskAssignment.</returns>
        public async Task<List<TaskAssignment?>> GetByOperatorIdAsync(Guid operatorid)
        {
            var query = _db.TaskAssignments
                  .Where(x => x.OperatorId == operatorid && !x.IsDeleted);
            return await query.ToListAsync();
        }

        /// <summary>
        /// Adds the asynchronous.
        /// </summary>
        /// <param name="tasks">The task.</param>
        /// <returns>task.</returns>
        public async Task<TaskAssignment> AddAsync(TaskAssignment tasks)
        {
            _db.TaskAssignments.Add(tasks);
            await _db.SaveChangesAsync();
            return tasks;
        }

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="tasks">The tasks.</param>
        /// <returns>task.</returns>
        public async Task<TaskAssignment> UpdateAsync(TaskAssignment tasks)
        {
            _db.TaskAssignments.Update(tasks);
            await _db.SaveChangesAsync();
            return tasks;
        }

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>task.</returns>
        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _db.TaskAssignments.FindAsync(id);
            if (entity == null)
            {
                return false;
            }

            entity.IsActive = false;
            entity.IsDeleted = true;

            _db.TaskAssignments.Update(entity);
            await _db.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <param name="centerId">The centerId.</param>
        /// <returns>
        /// tasks.
        /// </returns>
        public IQueryable<TaskAssignment> Query()
        {
            return _db.TaskAssignments.Where(x => !x.IsDeleted)
                .AsNoTracking();
        }

        /// <summary>
        /// Existses the by name asynchronous.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>bool.</returns>
        public async Task<bool> ExistsByNameAsync(string name)
        {
            return await _db.TaskAssignments.AnyAsync(x => x.Operator.StaffCategory.Name == name && !x.IsDeleted);
        }

        /// <inheritdoc/>
        public async Task<List<TaskAssignment>> GetAllForOverviewAsync()
        {
            return await _db.TaskAssignments
                .Include(x => x.Operator)
                .Where(x => !x.IsDeleted)
                .ToListAsync();
        }

        /// <inheritdoc />
        public async Task<int> GetNextSequenceAsync(
            Guid syllabusId,
            Guid academicYearId,
            Guid gradeId,
            Guid subjectId,
            Guid unitId,
            Guid chapterId,
            Guid resourceTypeId,
            Guid questionTypeId,
            Guid operatorId)
        {
            int maxSequence = await _db.TaskAssignments
                .Where(x =>
                    x.SyllabusId == syllabusId &&
                    x.Syllabus.AcademicYearId == academicYearId &&
                    x.GradeId == gradeId &&
                    x.SubjectId == subjectId &&
                    x.UnitId == unitId &&
                    x.ChapterId == chapterId &&
                    x.ResourceTypeId == resourceTypeId &&
                    x.QuestionTypeId == questionTypeId &&
                    x.OperatorId == operatorId &&
                    !x.IsDeleted)
                .MaxAsync(x => (int?)x.SequenceNumber) ?? 0;

            return maxSequence + 1;
        }

        /// <inheritdoc />
        public async Task<bool> CodeExistsAsync(string taskCode)
        {
            return await _db.TaskAssignments
                .AnyAsync(x => x.TaskCode == taskCode && !x.IsDeleted);
        }
    }
}
