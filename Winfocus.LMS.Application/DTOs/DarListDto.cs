namespace Winfocus.LMS.Application.DTOs
{
    /// <summary>
    /// DTO for listing Daily Activity Reports.
    /// </summary>
    public class DarListDto
    {
        /// <summary>
        /// Gets or sets the DAR identifier.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the report date.
        /// </summary>
        public DateOnly ReportDate { get; set; }

        /// <summary>
        /// Gets or sets the number of questions typed.
        /// </summary>
        public int QuestionsTyped { get; set; }

        /// <summary>
        /// Gets or sets the time spent in hours.
        /// </summary>
        public decimal TimeSpentHours { get; set; }

        /// <summary>
        /// Gets or sets the status (Draft or Submitted).
        /// </summary>
        public string Status { get; set; } = null!;

        /// <summary>
        /// Gets or sets the creation timestamp.
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }
}
