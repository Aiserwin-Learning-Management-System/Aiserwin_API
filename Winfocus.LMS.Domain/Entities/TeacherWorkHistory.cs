namespace Winfocus.LMS.Domain.Entities
{
    using Winfocus.LMS.Domain.Common;

    /// <summary>
    /// Represents a work history entry for a teacher registration.
    /// </summary>
    public class TeacherWorkHistory : BaseEntity
    {
        /// <summary>
        /// Gets or sets the identifier of the teacher registration.
        /// </summary>
        public Guid TeacherRegistrationId { get; set; }

        /// <summary>
        /// Gets or sets the duration of employment.
        /// </summary>
        public string Duration { get; set; } = null!;

        /// <summary>
        /// Gets or sets the job profile.
        /// </summary>
        public string JobProfile { get; set; } = null!;

        /// <summary>
        /// Gets or sets the institution name.
        /// </summary>
        public string Institution { get; set; } = null!;

        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        public string Location { get; set; } = null!;

        /// <summary>
        /// Gets or sets the reason for leaving.
        /// </summary>
        public string ReasonForLeaving { get; set; } = null!;

        /// <summary>
        /// Gets or sets the employment status.
        /// </summary>
        public string EmploymentStatus { get; set; } = null!;

        /// <summary>
        /// Gets or sets the teacher registration associated with this work history.
        /// </summary>
        public TeacherRegistration TeacherRegistration { get; set; } = null!;
    }
}