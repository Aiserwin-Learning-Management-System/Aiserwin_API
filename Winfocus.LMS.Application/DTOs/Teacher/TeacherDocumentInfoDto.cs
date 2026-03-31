namespace Winfocus.LMS.Application.DTOs.Teacher
{
    /// <summary>
    /// DTO for uploading teacher document references (paths or identifiers).
    /// </summary>
    public class TeacherDocumentInfoDto
    {
        /// <summary>
        /// Gets or sets the profile photo path.
        /// </summary>
        public string? PhotoPath { get; set; }

        /// <summary>
        /// Gets or sets the ID card document path.
        /// </summary>
        public string? IdCardPath { get; set; }
    }
}
