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
        /// Gets or sets the identifier of the associated stream.
        /// </summary>
        public Guid StreamId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the associated stream.
        /// </summary>
        public StreamDto Stream { get; set; } = null!;

    }
}
