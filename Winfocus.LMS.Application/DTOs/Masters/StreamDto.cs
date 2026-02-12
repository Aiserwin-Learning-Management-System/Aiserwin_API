namespace Winfocus.LMS.Application.DTOs.Masters
{
    /// <summary>
    /// Represents an academic stream within a grade (for example, Science or Arts).
    /// </summary>
    public class StreamDto : BaseClassDTO
    {
        /// <summary>
        /// Gets or sets the display name of the stream.
        /// </summary>
        public string StreamName { get; set; } = null!;

        /// <summary>
        /// Gets or sets the optional code for the stream.
        /// </summary>
        public string StreamCode { get; set; } = null!;

        /// <summary>
        /// Gets or sets the identifier of the associated grade.
        /// </summary>
        public Guid GradeId { get; set; }

        // One stream → many courses

        /// <summary>
        /// Gets or sets the courses.
        /// </summary>
        /// <value>
        /// The courses.
        /// </value>
        public IReadOnlyList<CourseDto> Courses { get; set; } = new List<CourseDto>();
    }
}
