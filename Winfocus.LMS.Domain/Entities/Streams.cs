namespace Winfocus.LMS.Domain.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Winfocus.LMS.Domain.Common;

    /// <summary>
    /// Represents an academic stream within a grade (for example, Science or Arts).
    /// </summary>
    public class Streams : BaseEntity
    {
        /// <summary>
        /// Gets or sets the display name of the stream.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Gets or sets the identifier of the associated grade.
        /// </summary>
        public Guid GradeId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the associated grade.
        /// </summary>
        public Grade Grade { get; set; } = null!;

        /// <summary>
        /// Gets or sets the courses.
        /// </summary>
        /// <value>
        /// The courses.
        /// </value>
        public ICollection<Course> Courses { get; set; } = new List<Course>();

        /// <summary>
        /// Gets or sets the display code of the stream.
        /// </summary>
        [MaxLength(50)]
        public string StreamCode { get; set; } = null!;
    }
}
