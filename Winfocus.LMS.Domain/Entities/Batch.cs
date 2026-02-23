namespace Winfocus.LMS.Domain.Entities
{
    using Winfocus.LMS.Domain.Common;

    /// <summary>
    /// Represents a batch (grouping or cohort) in the system.
    /// </summary>
    public class Batch : BaseEntity
    {
        /// <summary>
        /// Gets or sets the display name of the batch.
        /// </summary>
        public string Name { get; set; } = null!;

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
