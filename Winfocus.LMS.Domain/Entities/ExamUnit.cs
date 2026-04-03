namespace Winfocus.LMS.Domain.Entities
{
    using Winfocus.LMS.Domain.Common;

    /// <summary>
    /// Represents a unit/topic group under a subject.
    /// Example: "Electrostatics" under Physics.
    /// </summary>
    public class ExamUnit : BaseEntity
    {
        /// <summary>
        /// Gets or sets the subject identifier.
        /// </summary>
        /// <value>
        /// The subject identifier.
        /// </value>
        public Guid SubjectId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; } = default!;

        /// <summary>
        /// Gets or sets the unit number.
        /// </summary>
        /// <value>
        /// The unit number.
        /// </value>
        public int UnitNumber { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the subject.
        /// </summary>
        /// <value>
        /// The subject.
        /// </value>
        public Subject Subject { get; set; } = default!;
    }
}
