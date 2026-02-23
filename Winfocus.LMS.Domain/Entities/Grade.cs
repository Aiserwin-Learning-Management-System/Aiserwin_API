namespace Winfocus.LMS.Domain.Entities
{
    using System;
    using Winfocus.LMS.Domain.Common;

    /// <summary>
    /// Represents an academic grade (class/level) within a syllabus.
    /// </summary>
    public class Grade : BaseEntity
    {
        /// <summary>
        /// Gets or sets the optional name for the grade.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Gets or sets the identifier of the associated syllabus.
        /// </summary>
        public Guid SyllabusId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the associated syllabus.
        /// </summary>
        public Syllabus Syllabus { get; set; } = null!;

        /// <summary>
        /// Gets or sets the stream.
        /// </summary>
        /// <value>
        /// The stream.
        /// </value>
        public Streams Stream { get; set; } = null!;
    }
}
