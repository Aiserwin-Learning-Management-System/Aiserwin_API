namespace Winfocus.LMS.Domain.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Winfocus.LMS.Domain.Common;

    /// <summary>
    /// Represents an academic subject taught within a course.
    /// </summary>
    public class Subject : BaseEntity
    {
        /// <summary>
        /// Gets or sets the display name of the subject.
        /// </summary>
        public string SubjectName { get; set; } = null!;

        /// <summary>
        /// Gets or sets the optional code for the subject.
        /// </summary>
        public string SubjectCode { get; set; } = null!;

        /// <summary>
        /// Gets or sets the identifier of the associated course.
        /// </summary>
        public Guid CourseId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the associated course.
        /// </summary>
        public Course Course { get; set; } = null!;
    }
}
