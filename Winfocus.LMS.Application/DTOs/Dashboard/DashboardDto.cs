namespace Winfocus.LMS.Application.DTOs.Dashboard
{
    /// <summary>
    /// Complete dashboard response — single API call returns everything.
    /// </summary>
    public class DashboardDto
    {
        /// <summary>
        /// Gets or sets the profile.
        /// </summary>
        /// <value>
        /// The profile.
        /// </value>
        public OperatorProfileDto Profile { get; set; } = new ();

        /// <summary>
        /// Gets or sets the productivity.
        /// </summary>
        /// <value>
        /// The productivity.
        /// </value>
        public ProductivityStatsDto Productivity { get; set; } = new ();

        /// <summary>
        /// Gets or sets the active tasks.
        /// </summary>
        /// <value>
        /// The active tasks.
        /// </value>
        public List<ActiveTaskDto> ActiveTasks { get; set; } = new ();

        /// <summary>
        /// Gets or sets the corrections.
        /// </summary>
        /// <value>
        /// The corrections.
        /// </value>
        public CorrectionsSummaryDto Corrections { get; set; } = new ();
    }
}
