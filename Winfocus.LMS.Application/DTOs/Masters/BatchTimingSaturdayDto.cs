namespace Winfocus.LMS.Application.DTOs.Masters
{
    /// <summary>
    /// Represents a batch timing entry for Saturday schedule.
    /// </summary>
    public class BatchTimingSaturdayDto : BaseClassDTO
    {
        /// <summary>
        /// Gets or sets the batch time.
        /// </summary>
        public DateTime BatchTime { get; set; }

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
