namespace Winfocus.LMS.Infrastructure.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using Winfocus.LMS.Application.Interfaces;
    using Winfocus.LMS.Domain.Enums;
    using Winfocus.LMS.Infrastructure.Data;

    /// <summary>
    /// Repository implementation for operator productivity statistics data access.
    /// </summary>
    public sealed class OperatorStatsRepository : IOperatorStatsRepository
    {
        private readonly AppDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="OperatorStatsRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public OperatorStatsRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <inheritdoc />
        public async Task<string> GetOperatorNameAsync(Guid operatorId)
        {
            string? name = await _dbContext.StaffRegistrationValues
                .Where(v => v.RegistrationId == operatorId)
                .Where(v => v.FieldName.ToLower().Contains("name") ||
                            v.FieldName.ToLower().Contains("full_name"))
                .Select(v => v.Value)
                .FirstOrDefaultAsync();

            return name ?? "Unknown";
        }

        /// <inheritdoc />
        public async Task<List<Guid>> GetAllOperatorIdsWithTasksAsync()
        {
            return await _dbContext.TaskAssignments
                .Where(t => t.IsActive && !t.IsDeleted)
                .Select(t => t.OperatorId)
                .Distinct()
                .ToListAsync();
        }

        /// <inheritdoc />
        public async Task<int> GetTotalTaskCountAsync(Guid operatorId)
        {
            return await _dbContext.TaskAssignments
                .Where(t => t.OperatorId == operatorId && t.IsActive && !t.IsDeleted)
                .CountAsync();
        }

        /// <inheritdoc />
        public async Task<int> GetActiveTaskCountAsync(Guid operatorId)
        {
            return await _dbContext.TaskAssignments
                .Where(t => t.OperatorId == operatorId && t.IsActive && !t.IsDeleted)
                .Where(t => t.Status == (int)TaskStatus.InProgress)
                .CountAsync();
        }

        /// <inheritdoc />
        public async Task<int> GetCompletedTaskCountAsync(Guid operatorId)
        {
            return await _dbContext.TaskAssignments
                .Where(t => t.OperatorId == operatorId && t.IsActive && !t.IsDeleted)
                .Where(t => t.Status == (int)TaskStatus.Completed)
                .CountAsync();
        }

        /// <inheritdoc />
        public async Task<int> GetOverdueTaskCountAsync(Guid operatorId)
        {
            return await _dbContext.TaskAssignments
                .Where(t => t.OperatorId == operatorId && t.IsActive && !t.IsDeleted)
                .Where(t => t.Status == (int)TaskStatus.Overdue ||
                            (t.Deadline < DateTime.UtcNow && t.Status != (int)TaskStatus.Completed))
                .CountAsync();
        }

        /// <inheritdoc />
        public async Task<int> GetTotalQuestionsAssignedAsync(Guid operatorId)
        {
            return await _dbContext.TaskAssignments
                .Where(t => t.OperatorId == operatorId && t.IsActive && !t.IsDeleted)
                .SumAsync(t => t.TotalQuestions);
        }

        /// <inheritdoc />
        public async Task<Dictionary<int, int>> GetQuestionCountsByStatusAsync(
            Guid operatorId,
            DateTime startDate,
            DateTime endDate)
        {
            return await _dbContext.Questions
                .Where(q => q.OperatorId == operatorId && !q.IsDeleted)
                .Where(q => q.CreatedAt >= startDate && q.CreatedAt <= endDate)
                .GroupBy(q => q.Status)
                .Select(g => new { Status = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.Status, x => x.Count);
        }

        /// <inheritdoc />
        public async Task<(int TotalQuestionsTyped, decimal TotalHoursSpent, int ReportCount)> GetDarAggregatesAsync(
            Guid operatorId,
            DateOnly startDate,
            DateOnly endDate)
        {
            var dars = await _dbContext.DailyActivityReports
                .Where(d => d.OperatorId == operatorId && !d.IsDeleted)
                .Where(d => d.Status == (int)DarStatus.Submitted)
                .Where(d => d.ReportDate >= startDate && d.ReportDate <= endDate)
                .Select(d => new
                {
                    d.QuestionsTyped,
                    d.TimeSpentHours
                })
                .ToListAsync();

            if (!dars.Any())
            {
                return (0, 0, 0);
            }

            return (
                dars.Sum(d => d.QuestionsTyped),
                dars.Sum(d => d.TimeSpentHours),
                dars.Count
            );
        }

        /// <inheritdoc />
        public async Task<List<(DateTime Date, int Count)>> GetDailyTrendAsync(
            Guid operatorId,
            DateTime startDate,
            DateTime endDate)
        {
            var trend = await _dbContext.Questions
                .Where(q => q.OperatorId == operatorId && !q.IsDeleted)
                .Where(q => q.CreatedAt >= startDate && q.CreatedAt <= endDate)
                .GroupBy(q => q.CreatedAt.Date)
                .Select(g => new { Date = g.Key, Count = g.Count() })
                .OrderBy(t => t.Date)
                .ToListAsync();

            return trend.Select(t => (t.Date, t.Count)).ToList();
        }

        /// <inheritdoc />
        public async Task<Guid?> GetOperatorIdByUserIdAsync(Guid userId)
        {
            return await _dbContext.StaffRegistrations
                .Where(r => r.UserId == userId && r.IsActive && !r.IsDeleted)
                .Select(r => r.Id)
                .FirstOrDefaultAsync();
        }
    }
}
