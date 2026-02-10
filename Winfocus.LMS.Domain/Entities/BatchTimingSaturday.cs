namespace Winfocus.LMS.Domain.Entities
{
    using Winfocus.LMS.Domain.Common;

    /// <summary>
    /// Represents a batch timing entry for Saturday schedule.
    /// </summary>
    public class BatchTimingSaturday : BaseEntity
    {
        /// <summary>
        /// Gets or sets the textual representation of the batch time (for example, "09:00-10:00").
        /// </summary>
        public DateTime BatchTime { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the associated subject.
        /// </summary>
        public Guid SubjectId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the associated subject.
        /// </summary>
        public Subject Subject { get; set; } = null!;
    }
}
