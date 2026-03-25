namespace Winfocus.LMS.Application.DTOs
{
    /// <summary>
    /// DTO for today's Daily Activity Report or empty template.
    /// </summary>
    public class DarTodayDto
    {
        /// <summary>
        /// Gets or sets a value indicating whether a DAR exists for today.
        /// </summary>
        public bool Exists { get; set; }

        /// <summary>
        /// Gets or sets the template (existing DAR or empty template).
        /// </summary>
        public DarTodayTemplateDto Template { get; set; } = null!;
    }

    /// <summary>
    /// Template data for today's DAR.
    /// </summary>
    public class DarTodayTemplateDto
    {
        /// <summary>
        /// Gets or sets the report date (today's date).
        /// </summary>
        public DateOnly ReportDate { get; set; }

        /// <summary>
        /// Gets or sets the task identifier (null for new DAR).
        /// </summary>
        public Guid? TaskId { get; set; }

        /// <summary>
        /// Gets or sets the number of questions typed (default 0 for new DAR).
        /// </summary>
        public int QuestionsTyped { get; set; } = 0;

        /// <summary>
        /// Gets or sets the time spent in hours (default 0 for new DAR).
        /// </summary>
        public decimal TimeSpentHours { get; set; } = 0;

        /// <summary>
        /// Gets or sets the issues faced (empty for new DAR).
        /// </summary>
        public string? IssuesFaced { get; set; }

        /// <summary>
        /// Gets or sets the remarks (empty for new DAR).
        /// </summary>
        public string? Remarks { get; set; }
    }
}
