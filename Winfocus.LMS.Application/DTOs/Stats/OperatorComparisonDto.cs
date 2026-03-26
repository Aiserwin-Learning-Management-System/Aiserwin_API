namespace Winfocus.LMS.Application.DTOs.Stats
{
    /// <summary>
    /// Single operator row in the all-operators comparison view.
    /// </summary>
    public class OperatorComparisonDto
    {
        /// <summary>
        /// Gets or sets the operator identifier.
        /// </summary>
        public Guid OperatorId { get; set; }

        /// <summary>
        /// Gets or sets the operator name.
        /// </summary>
        public string Name { get; set; } = default!;

        /// <summary>
        /// Gets or sets the total questions assigned.
        /// </summary>
        public int TotalAssigned { get; set; }

        /// <summary>
        /// Gets or sets the total questions completed.
        /// </summary>
        public int Completed { get; set; }

        /// <summary>
        /// Gets or sets the total approved questions.
        /// </summary>
        public int Approved { get; set; }

        /// <summary>
        /// Gets or sets the total rejected questions.
        /// </summary>
        public int Rejected { get; set; }

        /// <summary>
        /// Gets or sets the completion rate percentage.
        /// </summary>
        public decimal CompletionRate { get; set; }

        /// <summary>
        /// Gets or sets the approval rate percentage.
        /// </summary>
        public decimal ApprovalRate { get; set; }

        /// <summary>
        /// Gets or sets the average questions per day.
        /// </summary>
        public decimal AvgQuestionsPerDay { get; set; }

        /// <summary>
        /// Gets or sets the number of active tasks.
        /// </summary>
        public int ActiveTasks { get; set; }
    }
}
