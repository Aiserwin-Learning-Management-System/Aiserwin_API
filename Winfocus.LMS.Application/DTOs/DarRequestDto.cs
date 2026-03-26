namespace Winfocus.LMS.Application.DTOs
{
    /// <summary>
    /// Request DTO for creating or updating a Daily Activity Report.
    /// </summary>
    public class DarRequestDto
    {
        /// <summary>
        /// Gets or sets the report date.
        /// </summary>
        public DateOnly ReportDate { get; set; }

        /// <summary>
        /// Gets or sets the task identifier (optional).
        /// </summary>
        public Guid? TaskId { get; set; }

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
    }
}
