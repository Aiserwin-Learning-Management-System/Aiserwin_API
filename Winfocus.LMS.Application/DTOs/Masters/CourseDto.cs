namespace Winfocus.LMS.Application.DTOs.Masters
{
    /// <summary>
    /// Represents an educational course.
    /// </summary>
    public class CourseDto : BaseClassDTO
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
        /// Gets or sets the stream identifier.
        /// </summary>
        /// <value>
        /// The stream identifier.
        /// </value>
        public Guid StreamId { get; set; }

        // One course → one subject

        /// <summary>
        /// Gets or sets the subject.
        /// </summary>
        /// <value>
        /// The subject.
        /// </value>
        public SubjectDto Subject { get; set; } = null!;
    }
}
