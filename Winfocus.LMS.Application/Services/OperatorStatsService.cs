namespace Winfocus.LMS.Application.Services
{
    using Microsoft.Extensions.Logging;
    using Winfocus.LMS.Application.DTOs;
    using Winfocus.LMS.Application.DTOs.Stats;
    using Winfocus.LMS.Application.Interfaces;
    using Winfocus.LMS.Domain.Enums;

    /// <summary>
    /// Service implementation for operator productivity and performance statistics.
    /// Aggregates data from TaskAssignments, Questions, and DailyActivityReports via repository.
    /// </summary>
    public sealed class OperatorStatsService : IOperatorStatsService
    {
        private readonly IOperatorStatsRepository _repository;
        private readonly ILogger<OperatorStatsService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="OperatorStatsService"/> class.
        /// </summary>
        /// <param name="repository">The operator stats repository.</param>
        /// <param name="logger">The logger instance.</param>
        public OperatorStatsService(
            IOperatorStatsRepository repository,
            ILogger<OperatorStatsService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task<CommonResponse<OperatorProductivityDto>> GetProductivityAsync(
            Guid operatorId,
            OperatorStatsFilterDto filter)
        {
            try
            {
                _logger.LogInformation(
                    "Fetching productivity stats for OperatorId: {OperatorId}, Period: {Period}",
                    operatorId, filter.Period);

                (DateTime startDate, DateTime endDate, string periodLabel) = ResolvePeriod(filter);

                // ── Operator Name ──
                string operatorName = await _repository.GetOperatorNameAsync(operatorId);

                // ── Task Stats ──
                int totalTasks = await _repository.GetTotalTaskCountAsync(operatorId);
                int activeTasks = await _repository.GetActiveTaskCountAsync(operatorId);
                int completedTasks = await _repository.GetCompletedTaskCountAsync(operatorId);
                int overdueTasks = await _repository.GetOverdueTaskCountAsync(operatorId);
                int totalQuestionsAssigned = await _repository.GetTotalQuestionsAssignedAsync(operatorId);

                // ── Question Stats ──
                Dictionary<int, int> questionCounts = await _repository
                    .GetQuestionCountsByStatusAsync(operatorId, startDate, endDate);

                int draftQuestions = questionCounts.GetValueOrDefault((int)QuestionStatus.Draft, 0);
                int submittedQuestions = questionCounts.GetValueOrDefault((int)QuestionStatus.Submitted, 0)
                    + questionCounts.GetValueOrDefault((int)QuestionStatus.UnderReview, 0);
                int approvedQuestions = questionCounts.GetValueOrDefault((int)QuestionStatus.Approved, 0);
                int rejectedQuestions = questionCounts.GetValueOrDefault((int)QuestionStatus.Rejected, 0);

                int totalQuestions = questionCounts.Values.Sum();
                int completedQuestions = totalQuestions - draftQuestions;

                decimal approvalRate = CalculateRate(approvedQuestions, approvedQuestions + rejectedQuestions);
                decimal rejectionRate = CalculateRate(rejectedQuestions, approvedQuestions + rejectedQuestions);
                decimal targetAchievement = CalculateRate(completedQuestions, totalQuestionsAssigned);

                // ── Daily Averages ──
                (int totalQTyped, decimal totalHours, int darCount) = await _repository
                    .GetDarAggregatesAsync(operatorId, DateOnly.FromDateTime(startDate), DateOnly.FromDateTime(endDate));

                decimal avgQuestionsPerDay = darCount > 0
                    ? Math.Round((decimal)totalQTyped / darCount, 1)
                    : 0;
                decimal avgHoursPerDay = darCount > 0
                    ? Math.Round(totalHours / darCount, 1)
                    : 0;

                // ── Daily Trend ──
                List<(DateTime Date, int Count)> trendData = await _repository
                    .GetDailyTrendAsync(operatorId, startDate, endDate);

                List<DailyTrendDto> trend = trendData
                    .Select(t => new DailyTrendDto
                    {
                        Date = t.Date,
                        QuestionsTyped = t.Count
                    })
                    .ToList();

                // ── Build Response ──
                OperatorProductivityDto result = new OperatorProductivityDto
                {
                    Period = periodLabel,
                    OperatorId = operatorId,
                    OperatorName = operatorName,
                    TotalTasks = totalTasks,
                    ActiveTasks = activeTasks,
                    CompletedTasks = completedTasks,
                    OverdueTasks = overdueTasks,
                    Questions = new QuestionStatsDto
                    {
                        Total = totalQuestionsAssigned,
                        Completed = completedQuestions,
                        Approved = approvedQuestions,
                        Rejected = rejectedQuestions,
                        Pending = submittedQuestions,
                        Draft = draftQuestions,
                        ApprovalRate = approvalRate,
                        RejectionRate = rejectionRate
                    },
                    DailyAverage = new DailyAverageDto
                    {
                        QuestionsPerDay = avgQuestionsPerDay,
                        HoursPerDay = avgHoursPerDay
                    },
                    TargetAchievement = targetAchievement,
                    Trend = trend
                };

                _logger.LogInformation(
                    "Stats fetched for OperatorId: {OperatorId}. Total: {Total}, Approved: {Approved}",
                    operatorId, totalQuestions, approvedQuestions);

                return CommonResponse<OperatorProductivityDto>.SuccessResponse(
                    "Productivity stats fetched successfully.", result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching productivity stats for OperatorId: {OperatorId}", operatorId);
                return CommonResponse<OperatorProductivityDto>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        /// <inheritdoc />
        public async Task<CommonResponse<AllOperatorsStatsDto>> GetAllOperatorsStatsAsync(
            OperatorStatsFilterDto filter)
        {
            try
            {
                _logger.LogInformation("Fetching all operators stats. Period: {Period}", filter.Period);

                (DateTime startDate, DateTime endDate, string periodLabel) = ResolvePeriod(filter);

                List<Guid> operatorIds = await _repository.GetAllOperatorIdsWithTasksAsync();

                List<OperatorComparisonDto> operators = new List<OperatorComparisonDto>();

                foreach (Guid operatorId in operatorIds)
                {
                    string name = await _repository.GetOperatorNameAsync(operatorId);

                    int totalAssigned = await _repository.GetTotalQuestionsAssignedAsync(operatorId);
                    int activeTasks = await _repository.GetActiveTaskCountAsync(operatorId);

                    Dictionary<int, int> questionCounts = await _repository
                        .GetQuestionCountsByStatusAsync(operatorId, startDate, endDate);

                    int completed = questionCounts
                        .Where(kv => kv.Key != (int)QuestionStatus.Draft)
                        .Sum(kv => kv.Value);
                    int approved = questionCounts.GetValueOrDefault((int)QuestionStatus.Approved, 0);
                    int rejected = questionCounts.GetValueOrDefault((int)QuestionStatus.Rejected, 0);

                    (int totalQTyped, decimal _, int darCount) = await _repository
                        .GetDarAggregatesAsync(operatorId, DateOnly.FromDateTime(startDate), DateOnly.FromDateTime(endDate));

                    decimal avgQPerDay = darCount > 0
                        ? Math.Round((decimal)totalQTyped / darCount, 1)
                        : 0;

                    operators.Add(new OperatorComparisonDto
                    {
                        OperatorId = operatorId,
                        Name = name,
                        TotalAssigned = totalAssigned,
                        Completed = completed,
                        Approved = approved,
                        Rejected = rejected,
                        CompletionRate = CalculateRate(completed, totalAssigned),
                        ApprovalRate = CalculateRate(approved, approved + rejected),
                        AvgQuestionsPerDay = avgQPerDay,
                        ActiveTasks = activeTasks
                    });
                }

                AllOperatorsStatsDto result = new AllOperatorsStatsDto
                {
                    Period = periodLabel,
                    TotalOperators = operators.Count,
                    Operators = operators.OrderByDescending(o => o.CompletionRate).ToList()
                };

                _logger.LogInformation("All operators stats fetched. Count: {Count}", operators.Count);

                return CommonResponse<AllOperatorsStatsDto>.SuccessResponse(
                    "All operators stats fetched successfully.", result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all operators stats.");
                return CommonResponse<AllOperatorsStatsDto>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        /// <inheritdoc />
        public async Task<CommonResponse<OperatorProductivityDto>> GetMyProductivityAsync(
            Guid userId,
            OperatorStatsFilterDto filter)
        {
            try
            {
                _logger.LogInformation("Resolving OperatorId for UserId: {UserId}", userId);

                Guid? operatorId = await _repository.GetOperatorIdByUserIdAsync(userId);

                if (!operatorId.HasValue)
                {
                    _logger.LogWarning("No operator registration found for UserId: {UserId}", userId);
                    return CommonResponse<OperatorProductivityDto>.FailureResponse(
                        "Operator registration not found for this user.");
                }

                return await GetProductivityAsync(operatorId.Value, filter);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching stats for UserId: {UserId}", userId);
                return CommonResponse<OperatorProductivityDto>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Resolves the date range and display label from the filter period.
        /// </summary>
        /// <param name="filter">The filter parameters.</param>
        /// <returns>A tuple of (startDate, endDate, periodLabel).</returns>
        private static (DateTime StartDate, DateTime EndDate, string PeriodLabel) ResolvePeriod(
            OperatorStatsFilterDto filter)
        {
            DateTime now = DateTime.UtcNow;

            return filter.Period.ToLower() switch
            {
                "daily" => (
                    now.Date,
                    now.Date.AddDays(1).AddTicks(-1),
                    "Today"),

                "weekly" => (
                    now.Date.AddDays(-(int)now.DayOfWeek),
                    now.Date.AddDays(1).AddTicks(-1),
                    "This Week"),

                "custom" when filter.StartDate.HasValue && filter.EndDate.HasValue => (
                    filter.StartDate.Value.Date,
                    filter.EndDate.Value.Date.AddDays(1).AddTicks(-1),
                    $"{filter.StartDate.Value:dd MMM yyyy} - {filter.EndDate.Value:dd MMM yyyy}"),

                _ => (
                    new DateTime(now.Year, now.Month, 1),
                    now.Date.AddDays(1).AddTicks(-1),
                    now.ToString("MMMM yyyy"))
            };
        }

        /// <summary>
        /// Calculates a percentage rate safely, returning 0 if denominator is 0.
        /// </summary>
        /// <param name="numerator">The numerator value.</param>
        /// <param name="denominator">The denominator value.</param>
        /// <returns>The percentage rate rounded to 2 decimal places.</returns>
        private static decimal CalculateRate(int numerator, int denominator)
        {
            if (denominator == 0)
            {
                return 0;
            }

            return Math.Round((decimal)numerator / denominator * 100, 2);
        }
    }
}
