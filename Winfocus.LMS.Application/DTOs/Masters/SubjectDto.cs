namespace Winfocus.LMS.Application.DTOs.Masters
{
    /// <summary>
    /// Represents an academic subject taught within a course.
    /// </summary>
    public class SubjectDto : BaseClassDTO
    {
        /// <summary>
        /// Gets or sets the display name of the subject.
        /// </summary>
        public string SubjectName { get; set; } = null!;

        /// <summary>
        /// Gets or sets the optional code for the subject.
        /// </summary>
        public string SubjectCode { get; set; } = null!;

        // One subject → many courses

        /// <summary>
        /// Gets or sets the courses.
        /// </summary>
        /// <value>
        /// The courses.
        /// </value>
        public IReadOnlyList<CourseDto> Courses { get; set; } = new List<CourseDto>();
    }
}
