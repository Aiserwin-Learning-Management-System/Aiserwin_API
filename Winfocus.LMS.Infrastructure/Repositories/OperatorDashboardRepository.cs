namespace Winfocus.LMS.Infrastructure.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using Winfocus.LMS.Application.Interfaces;
    using Winfocus.LMS.Domain.Entities;
    using Winfocus.LMS.Infrastructure.Data;

    /// <summary>
    /// OperatorDashboardRepository.
    /// </summary>
    /// <seealso cref="Winfocus.LMS.Application.Interfaces.IOperatorDashboardRepository" />
    public sealed class OperatorDashboardRepository : IOperatorDashboardRepository
    {
        private readonly AppDbContext _db;

        /// <summary>
        /// Initializes a new instance of the <see cref="OperatorDashboardRepository"/> class.
        /// </summary>
        /// <param name="db">The database.</param>
        public OperatorDashboardRepository(AppDbContext db)
        {
            _db = db;
        }

        /// <inheritdoc/>
        public async Task<StaffRegistration?> GetOperatorRegistrationAsync(Guid userId)
        {
            // Find registration created by this user
            var registration = await _db.StaffRegistrations
                .Include(sr => sr.StaffCategory)
                .Include(sr => sr.RegistrationForm)
                .Include(sr => sr.Values)
                    .ThenInclude(v => v.FormField)
                .AsNoTracking()
                .Where(sr => sr.UserId == userId)
                .OrderByDescending(sr => sr.CreatedAt)
                .FirstOrDefaultAsync();

            if (registration != null) return registration;

            // Fallback: find by User.StaffCategoryId matching DTP categories
            var user = await _db.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user?.StaffCategoryId == null) return null;

            // Find any registration for this staff category created by this user
            return await _db.StaffRegistrations
                .Include(sr => sr.StaffCategory)
                .Include(sr => sr.RegistrationForm)
                .Include(sr => sr.Values)
                    .ThenInclude(v => v.FormField)
                .AsNoTracking()
                .Where(sr => sr.StaffCategoryId == user.StaffCategoryId
                    && sr.UserId == userId)
                .OrderByDescending(sr => sr.CreatedAt)
                .FirstOrDefaultAsync();
        }

        /// <inheritdoc/>
        public async Task<string?> GetUserStaffCategoryAsync(Guid userId)
        {
            var user = await _db.Users
                .Include(u => u.StaffCategory)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == userId);

            return user?.StaffCategory?.Name;
        }

        /// <inheritdoc/>
        public async Task<List<TaskAssignment>> GetActiveTasksAsync(Guid operatorId)
        {
            return await _db.TaskAssignments
                .Include(t => t.ResourceType)
                .Include(t => t.Syllabus)
                .Include(t => t.Grade)
                .Include(t => t.Subject)
                .Include(t => t.Unit)
                .Include(t => t.Chapter)
                .AsNoTracking()
                .Where(t => t.OperatorId == operatorId
                    && (t.Status == 0 || t.Status == 1)) // Pending or InProgress
                .OrderBy(t => t.Deadline)
                .ToListAsync();
        }

        /// <inheritdoc/>
        public IQueryable<TaskAssignment> QueryTasks(Guid operatorId)
        {
            return _db.TaskAssignments
                .Include(t => t.ResourceType)
                .Include(t => t.Syllabus)
                .Include(t => t.Grade)
                .Include(t => t.Subject)
                .Include(t => t.Unit)
                .Include(t => t.Chapter)
                .AsNoTracking()
                .Where(t => t.OperatorId == operatorId);
        }

        /// <inheritdoc/>
        public async Task<Dictionary<int, int>> GetQuestionCountsByStatusAsync(
            Guid operatorId, DateTime? fromDate, DateTime? toDate)
        {
            var query = _db.Questions
                .AsNoTracking()
                .Where(q => q.OperatorId == operatorId);

            if (fromDate.HasValue)
                query = query.Where(q => q.CreatedAt >= fromDate.Value);
            if (toDate.HasValue)
                query = query.Where(q => q.CreatedAt <= toDate.Value);

            return await query
                .GroupBy(q => q.Status)
                .Select(g => new { Status = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.Status, x => x.Count);
        }

        /// <inheritdoc/>
        public async Task<int> GetTotalQuestionsTargetAsync(
            Guid operatorId, DateTime? fromDate, DateTime? toDate)
        {
            var query = _db.TaskAssignments
                .AsNoTracking()
                .Where(t => t.OperatorId == operatorId);

            if (fromDate.HasValue)
                query = query.Where(t => t.CreatedAt >= fromDate.Value);
            if (toDate.HasValue)
                query = query.Where(t => t.CreatedAt <= toDate.Value);

            return await query.SumAsync(t => t.TotalQuestions);
        }

        /// <inheritdoc/>
        public async Task<int> GetTaskCountAsync(
            Guid operatorId, DateTime? fromDate, DateTime? toDate)
        {
            var query = _db.TaskAssignments
                .AsNoTracking()
                .Where(t => t.OperatorId == operatorId);

            if (fromDate.HasValue)
                query = query.Where(t => t.CreatedAt >= fromDate.Value);
            if (toDate.HasValue)
                query = query.Where(t => t.CreatedAt <= toDate.Value);

            return await query.CountAsync();
        }

        /// <inheritdoc/>
        public async Task<List<Question>> GetRejectedQuestionsAsync(
            Guid operatorId, int take = 5)
        {
            return await _db.Questions
                .Include(q => q.Reviews)
                .AsNoTracking()
                .Where(q => q.OperatorId == operatorId && q.Status == 4) // Rejected
                .OrderByDescending(q => q.UpdatedAt ?? q.CreatedAt)
                .Take(take)
                .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<int> GetRegistrationSequenceAsync(Guid registrationId)
        {
            var registration = await _db.StaffRegistrations
                .AsNoTracking()
                .FirstOrDefaultAsync(sr => sr.Id == registrationId);

            if (registration == null) return 0;

            return await _db.StaffRegistrations
                .AsNoTracking()
                .CountAsync(sr => sr.CreatedAt <= registration.CreatedAt);
        }
    }
}
