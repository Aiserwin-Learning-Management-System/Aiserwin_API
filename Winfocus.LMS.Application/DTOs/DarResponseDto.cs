namespace Winfocus.LMS.Application.DTOs
{
    /// <summary>
    /// Response DTO for a Daily Activity Report.
    /// </summary>
    public class DarResponseDto
    {
        /// <summary>
        /// Gets or sets the DAR identifier.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the operator name.
        /// </summary>
        public string OperatorName { get; set; } = null!;

        /// <summary>
        /// Gets or sets the report date.
        /// </summary>
        public DateOnly ReportDate { get; set; }

        /// <summary>
        /// Gets or sets the task information.
        /// </summary>
        public DarTaskDto? Task { get; set; }

        /// <summary>
        /// Gets or sets the number of questions typed.
        /// </summary>
        public int QuestionsTyped { get; set; }

        /// <summary>
        /// Gets or sets the time spent in hours.
        /// </summary>
        public decimal TimeSpentHours { get; set; }

        /// <summary>
        /// Gets or sets the issues faced during the work.
        /// </summary>
        public string? IssuesFaced { get; set; }

        /// <summary>
        /// Gets or sets the remarks or comments.
        /// </summary>
        public string? Remarks { get; set; }

        /// <summary>
        /// Gets or sets the status (Draft or Submitted).
        /// </summary>
        public string Status { get; set; } = null!;

        /// <summary>
        /// Gets or sets the creation timestamp.
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }

    /// <summary>
    /// Nested DTO for task information within DAR response.
    /// </summary>
    public class DarTaskDto
    {
        /// <summary>
        /// Gets or sets the task identifier.
        /// </summary>
        public Guid TaskId { get; set; }

        /// <summary>
        /// Gets or sets the task code.
        /// </summary>
        public string TaskCode { get; set; } = null!;

        /// <summary>
        /// Gets or sets the subject name.
        /// </summary>
        public string Subject { get; set; } = null!;
    }
}
