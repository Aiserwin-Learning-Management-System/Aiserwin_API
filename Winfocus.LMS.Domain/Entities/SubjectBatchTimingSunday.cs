namespace Winfocus.LMS.Domain.Entities
{
    /// <summary>
    /// Represents the association between a Subject and a batchtiming sunday.
    /// </summary>
    public class SubjectBatchTimingSunday
    {
        /// <summary>
        /// Gets or sets the identifier of the Subject.
        /// </summary>
        public Guid SubjectId { get; set; }

        /// <summary>
        /// Gets or sets the Subject associated with this mapping.
        /// </summary>
        public Subject Subject { get; set; } = null!;

        /// <summary>
        /// Gets or sets the identifier of the BatchTimingSunday.
        /// </summary>
        public Guid BatchTimingId { get; set; }

        /// <summary>
        /// Gets or sets the BatchTimingSunday associated with this mapping.
        /// </summary>
        public BatchTimingSunday BatchTimingSunday { get; set; } = null!;

        /// <summary>
        /// Gets or sets the collection of subject to BatchTimingSundays.
        /// </summary>
        public ICollection<SubjectBatchTimingSunday> SubjectBatchTimingSundays { get; set; } = new List<SubjectBatchTimingSunday>();
    }
}
