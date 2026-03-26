namespace Winfocus.LMS.Application.DTOs.Stats
{
    /// <summary>
    /// Daily average productivity metrics calculated from DailyActivityReports.
    /// </summary>
    public class DailyAverageDto
    {
        /// <summary>
        /// Gets or sets the average number of questions typed per day.
        /// </summary>
        public decimal QuestionsPerDay { get; set; }

        /// <summary>
        /// Gets or sets the average hours spent per day.
        /// </summary>
        public decimal HoursPerDay { get; set; }
    }
}
