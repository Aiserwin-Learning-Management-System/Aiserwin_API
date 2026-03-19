namespace Winfocus.LMS.Application.DTOs.Stats
{
    /// <summary>
    /// Question statistics breakdown by status.
    /// </summary>
    public class QuestionStatsDto
    {
        /// <summary>
        /// Gets or sets the total questions assigned to this operator.
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// Gets or sets the number of completed (submitted+) questions.
        /// </summary>
        public int Completed { get; set; }

        /// <summary>
        /// Gets or sets the number of approved questions.
        /// </summary>
        public int Approved { get; set; }

        /// <summary>
        /// Gets or sets the number of rejected questions.
        /// </summary>
        public int Rejected { get; set; }

        /// <summary>
        /// Gets or sets the number of pending (submitted, under review) questions.
        /// </summary>
        public int Pending { get; set; }

        /// <summary>
        /// Gets or sets the number of draft questions.
        /// </summary>
        public int Draft { get; set; }

        /// <summary>
        /// Gets or sets the approval rate percentage.
        /// Calculated as (approved / (approved + rejected)) * 100.
        /// Returns 0 if no approved or rejected questions exist.
        /// </summary>
        public decimal ApprovalRate { get; set; }

        /// <summary>
        /// Gets or sets the rejection rate percentage.
        /// Calculated as (rejected / (approved + rejected)) * 100.
        /// Returns 0 if no approved or rejected questions exist.
        /// </summary>
        public decimal RejectionRate { get; set; }
    }
}
