namespace Winfocus.LMS.Application.DTOs.Stats
{
    /// <summary>
    /// All operators comparison statistics for admin view.
    /// </summary>
    public class AllOperatorsStatsDto
    {
        /// <summary>
        /// Gets or sets the display period label.
        /// </summary>
        public string Period { get; set; } = default!;

        /// <summary>
        /// Gets or sets the total operators count.
        /// </summary>
        public int TotalOperators { get; set; }

        /// <summary>
        /// Gets or sets the list of operator comparison stats.
        /// </summary>
        public List<OperatorComparisonDto> Operators { get; set; } = new List<OperatorComparisonDto>();
    }
}
