namespace Winfocus.LMS.Application.DTOs.Masters
{
    /// <summary>
    /// Represents a batch timing entry for Sunday schedule.
    /// </summary>
    public class BatchTimingSundayDto : BaseClassDTO
    {
        /// <summary>
        /// Gets or sets the batch time in ISO 8601 UTC format.
        /// </summary>
        public string BatchTime { get; set; } = null!;

        /// <summary>
        /// Gets or sets the batch time formatted for display (e.g., "02:30 AM").
        /// </summary>
        public string BatchTimeDisplay { get; set; } = null!;

        /// <summary>
        /// Gets or sets the identifier of the associated Subject.
        /// </summary>
        public Guid SubjectId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the associated Subject.
        /// </summary>
        public SubjectDto Subject { get; set; } = null!;
    }
}
