namespace Winfocus.LMS.Application.DTOs.Stats
{
    /// <summary>
    /// Daily trend data point for productivity charts.
    /// </summary>
    public class DailyTrendDto
    {
        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the number of questions typed on this date.
        /// </summary>
        public int QuestionsTyped { get; set; }
    }
}
