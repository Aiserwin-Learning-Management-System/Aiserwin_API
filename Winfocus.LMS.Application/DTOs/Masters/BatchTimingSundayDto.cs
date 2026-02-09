namespace Winfocus.LMS.Application.DTOs.Masters
{
    /// <summary>
    /// Represents a batch timing entry for Sunday schedule.
    /// </summary>
    public class BatchTimingSundayDto : BaseClassDTO
    {
        /// <summary>
        /// Gets or sets the textual representation of the batch time (for example, "09:00-10:00").
        /// </summary>
        public string BatchTime { get; set; } = null!;
    }
}
