namespace Winfocus.LMS.Application.DTOs.Teacher
{
    /// <summary>
    /// DTO representing a work history item for a teacher.
    /// </summary>
    public class TeacherWorkHistoryDto
    {
        /// <summary>
        /// Gets or sets the duration of employment.
        /// </summary>
        public string Duration { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the job profile.
        /// </summary>
        public string JobProfile { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the institution name.
        /// </summary>
        public string Institution { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        public string Location { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the reason for leaving.
        /// </summary>
        public string ReasonForLeaving { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the employment status.
        /// </summary>
        public string EmploymentStatus { get; set; } = string.Empty;
    }
}
