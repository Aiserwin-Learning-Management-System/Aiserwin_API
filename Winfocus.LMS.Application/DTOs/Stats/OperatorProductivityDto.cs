namespace Winfocus.LMS.Application.DTOs.Stats
{
    /// <summary>
    /// Detailed productivity statistics for a single operator.
    /// </summary>
    public class OperatorProductivityDto
    {
        /// <summary>
        /// Gets or sets the display period label.
        /// Example: "July 2025", "This Week", "Today".
        /// </summary>
        public string Period { get; set; } = default!;

        /// <summary>
        /// Gets or sets the operator identifier.
        /// </summary>
        public Guid OperatorId { get; set; }

        /// <summary>
        /// Gets or sets the operator name.
        /// </summary>
        public string OperatorName { get; set; } = default!;

        /// <summary>
        /// Gets or sets the total number of tasks assigned.
        /// </summary>
        public int TotalTasks { get; set; }

        /// <summary>
        /// Gets or sets the number of active (in-progress) tasks.
        /// </summary>
        public int ActiveTasks { get; set; }

        /// <summary>
        /// Gets or sets the number of completed tasks.
        /// </summary>
        public int CompletedTasks { get; set; }

        /// <summary>
        /// Gets or sets the number of overdue tasks.
        /// </summary>
        public int OverdueTasks { get; set; }

        /// <summary>
        /// Gets or sets the question statistics breakdown.
        /// </summary>
        public QuestionStatsDto Questions { get; set; } = new QuestionStatsDto();

        /// <summary>
        /// Gets or sets the daily average metrics.
        /// </summary>
        public DailyAverageDto DailyAverage { get; set; } = new DailyAverageDto();

        /// <summary>
        /// Gets or sets the target achievement percentage.
        /// Calculated as (completed / total assigned) * 100.
        /// </summary>
        public decimal TargetAchievement { get; set; }

        /// <summary>
        /// Gets or sets the daily trend data for charts.
        /// </summary>
        public List<DailyTrendDto> Trend { get; set; } = new List<DailyTrendDto>();
    }
}
