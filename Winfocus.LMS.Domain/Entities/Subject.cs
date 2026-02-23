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
        public string Name { get; set; } = null!;

        // One subject → many courses

        /// <summary>
        /// Gets or sets the courses.
        /// </summary>
        /// <value>
        /// The courses.
        /// </value>
        public ICollection<Course> Courses { get; set; } = new List<Course>();
    }
}
