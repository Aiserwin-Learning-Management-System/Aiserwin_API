namespace Winfocus.LMS.Application.DTOs.Dashboard
{
    /// <summary>
    /// CorrectionsSummaryDto.
    /// </summary>
    public class CorrectionsSummaryDto
    {
        /// <summary>
        /// Gets or sets the pending count.
        /// </summary>
        /// <value>
        /// The pending count.
        /// </value>
        public int PendingCount { get; set; }

        /// <summary>
        /// Gets or sets the latest rejection.
        /// </summary>
        /// <value>
        /// The latest rejection.
        /// </value>
        public string? LatestRejection { get; set; }

        /// <summary>
        /// Gets or sets the latest rejection date.
        /// </summary>
        /// <value>
        /// The latest rejection date.
        /// </value>
        public DateTime? LatestRejectionDate { get; set; }

        /// <summary>
        /// Gets or sets the recent rejections.
        /// </summary>
        /// <value>
        /// The recent rejections.
        /// </value>
        public List<RejectedQuestionDto> RecentRejections { get; set; } = new ();
    }
}
