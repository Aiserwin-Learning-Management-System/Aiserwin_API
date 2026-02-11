namespace Winfocus.LMS.Domain.Entities
{
    using System;
    using Winfocus.LMS.Domain.Common;

    /// <summary>
    /// Represents an educational course.
    /// </summary>
    public class Course : BaseEntity
    {
        /// <summary>
        /// Gets or sets the display name of the course.
        /// </summary>
        public string CourseName { get; set; } = null!;

        /// <summary>
        /// Gets or sets the optional code for the course.
        /// </summary>
        public string CourseCode { get; set; } = null!;

        /// <summary>
        /// Gets or sets the identifier of the associated stream.
        /// </summary>
        public Guid StreamId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the associated stream.
        /// </summary>
        public Streams Stream { get; set; } = null!;

        /// <summary>
        /// Gets or sets the Streams mapped to this Course.
        /// </summary>
        public ICollection<StreamCourse> StreamCourses { get; set; } = new List<StreamCourse>();

        /// <summary>
        /// Gets or sets the course subjects.
        /// </summary>
        /// <value>
        /// The course subjects.
        /// </value>
        public ICollection<CourseSubject> CourseSubjects { get; set; }
            = new List<CourseSubject>();
    }
}
