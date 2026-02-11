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
    }
}
