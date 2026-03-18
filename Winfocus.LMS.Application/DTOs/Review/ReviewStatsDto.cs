namespace Winfocus.LMS.Application.DTOs.Review
{
    /// <summary>
    /// ReviewStatsDto.
    /// </summary>
    public class ReviewStatsDto
    {
        /// <summary>
        /// Gets or sets the total pending.
        /// </summary>
        /// <value>
        /// The total pending.
        /// </value>
        public int TotalPending { get; set; }

        /// <summary>
        /// Gets or sets the approved today.
        /// </summary>
        /// <value>
        /// The approved today.
        /// </value>
        public int ApprovedToday { get; set; }

        /// <summary>
        /// Gets or sets the rejected today.
        /// </summary>
        /// <value>
        /// The rejected today.
        /// </value>
        public int RejectedToday { get; set; }

        /// <summary>
        /// Gets or sets the total reviewed today.
        /// </summary>
        /// <value>
        /// The total reviewed today.
        /// </value>
        public int TotalReviewedToday { get; set; }

        /// <summary>
        /// Gets or sets the total approved all time.
        /// </summary>
        /// <value>
        /// The total approved all time.
        /// </value>
        public int TotalApprovedAllTime { get; set; }

        /// <summary>
        /// Gets or sets the total rejected all time.
        /// </summary>
        /// <value>
        /// The total rejected all time.
        /// </value>
        public int TotalRejectedAllTime { get; set; }
    }
}
