namespace Winfocus.LMS.Domain.Entities
{
    using Winfocus.LMS.Domain.Common;

    /// <summary>
    /// Represents a grade/class level under a syllabus.
    /// Example: 10th, 11th, 12th under CBSE.
    /// </summary>
    public class ExamGrade : BaseEntity
    {
        /// <summary>
        /// Gets or sets the syllabus identifier.
        /// </summary>
        /// <value>
        /// The syllabus identifier.
        /// </value>
        public Guid SyllabusId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; } = default!;

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the syllabus.
        /// </summary>
        /// <value>
        /// The syllabus.
        /// </value>
        public ExamSyllabus Syllabus { get; set; } = default!;
    }
}
