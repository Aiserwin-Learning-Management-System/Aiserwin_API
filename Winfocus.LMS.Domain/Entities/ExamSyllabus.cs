namespace Winfocus.LMS.Domain.Entities
{
    using System;
    using Winfocus.LMS.Domain.Common;

    /// <summary>
    /// Represents a syllabus .
    /// </summary>
    public class ExamSyllabus : BaseEntity
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; } = default!;

        /// <summary>
        /// Gets or sets the academic year identifier.
        /// </summary>
        /// <value>
        /// The academic year identifier.
        /// </value>
        public Guid AcademicYearId { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the academic year.
        /// </summary>
        /// <value>
        /// The academic year.
        /// </value>
        public AcademicYear AcademicYear { get; set; } = default!;
    }
}
