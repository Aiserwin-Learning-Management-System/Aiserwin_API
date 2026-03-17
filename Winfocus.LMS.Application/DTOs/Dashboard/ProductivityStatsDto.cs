namespace Winfocus.LMS.Application.DTOs.Dashboard
{
    /// <summary>
    /// ProductivityStatsDto.
    /// </summary>
    public class ProductivityStatsDto
    {
        /// <summary>
        /// Gets or sets the period.
        /// </summary>
        /// <value>
        /// The period.
        /// </value>
        public string Period { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the total tasks.
        /// </summary>
        /// <value>
        /// The total tasks.
        /// </value>
        public int TotalTasks { get; set; }

        /// <summary>
        /// Gets or sets the questions to enter.
        /// </summary>
        /// <value>
        /// The questions to enter.
        /// </value>
        public int QuestionsToEnter { get; set; }

        /// <summary>
        /// Gets or sets the questions completed.
        /// </summary>
        /// <value>
        /// The questions completed.
        /// </value>
        public int QuestionsCompleted { get; set; }

        /// <summary>
        /// Gets or sets the pending questions.
        /// </summary>
        /// <value>
        /// The pending questions.
        /// </value>
        public int PendingQuestions { get; set; }

        /// <summary>
        /// Gets or sets the rejected questions.
        /// </summary>
        /// <value>
        /// The rejected questions.
        /// </value>
        public int RejectedQuestions { get; set; }

        /// <summary>
        /// Gets or sets the draft questions.
        /// </summary>
        /// <value>
        /// The draft questions.
        /// </value>
        public int DraftQuestions { get; set; }

        /// <summary>
        /// Gets or sets the approved questions.
        /// </summary>
        /// <value>
        /// The approved questions.
        /// </value>
        public int ApprovedQuestions { get; set; }

        /// <summary>
        /// Gets or sets the target achievement.
        /// </summary>
        /// <value>
        /// The target achievement.
        /// </value>
        public decimal TargetAchievement { get; set; }

        /// <summary>
        /// Gets or sets the progress bar.
        /// </summary>
        /// <value>
        /// The progress bar.
        /// </value>
        public int ProgressBar { get; set; }
    }
}
