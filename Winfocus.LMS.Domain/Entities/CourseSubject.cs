namespace Winfocus.LMS.Domain.Entities
{
    /// <summary>
    /// CourseSubject.
    /// </summary>
    public class CourseSubject
    {
        /// <summary>
        /// Gets or sets the course identifier.
        /// </summary>
        /// <value>
        /// The course identifier.
        /// </value>
        public Guid CourseId { get; set; }

        /// <summary>
        /// Gets or sets the course.
        /// </summary>
        /// <value>
        /// The course.
        /// </value>
        public Course Course { get; set; } = null!;

        /// <summary>
        /// Gets or sets the subject identifier.
        /// </summary>
        /// <value>
        /// The subject identifier.
        /// </value>
        public Guid SubjectId { get; set; }

        /// <summary>
        /// Gets or sets the subject.
        /// </summary>
        /// <value>
        /// The subject.
        /// </value>
        public Subject Subject { get; set; } = null!;
    }
}
