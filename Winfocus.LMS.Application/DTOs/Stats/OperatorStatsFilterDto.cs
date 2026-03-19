namespace Winfocus.LMS.Application.DTOs.Stats
{
    /// <summary>
    /// Filter parameters for operator productivity statistics.
    /// </summary>
    public class OperatorStatsFilterDto
    {
        /// <summary>
        /// Gets or sets the period filter.
        /// Values: "daily", "weekly", "monthly", "custom".
        /// Default: "monthly".
        /// </summary>
        public string Period { get; set; } = "monthly";

        /// <summary>
        /// Gets or sets the custom start date. Used when Period is "custom".
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Gets or sets the custom end date. Used when Period is "custom".
        /// </summary>
        public DateTime? EndDate { get; set; }
    }
}
