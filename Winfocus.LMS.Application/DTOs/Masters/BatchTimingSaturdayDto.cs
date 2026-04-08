namespace Winfocus.LMS.Application.DTOs.Masters
{
    /// <summary>
    /// Represents a batch timing entry for Saturday schedule.
    /// </summary>
    public class BatchTimingSaturdayDto : BaseClassDTO
    {
        /// <summary>
        /// Gets or sets the textual representation of the batch time (for example, "09:00-10:00").
        /// </summary>
        public string BatchTime { get; set; } = null!;

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
