namespace Winfocus.LMS.Application.DTOs.Dashboard
{
    /// <summary>
    /// Profile info extracted from dynamic registration values.
    /// </summary>
    public class OperatorProfileDto
    {
        /// <summary>
        /// Gets or sets the registration identifier.
        /// </summary>
        /// <value>
        /// The registration identifier.
        /// </value>
        public Guid RegistrationId { get; set; }

        /// <summary>
        /// Gets or sets the full name.
        /// </summary>
        /// <value>
        /// The full name.
        /// </value>
        public string FullName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the employee identifier.
        /// </summary>
        /// <value>
        /// The employee identifier.
        /// </value>
        public string EmployeeId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the role.
        /// </summary>
        /// <value>
        /// The role.
        /// </value>
        public string Role { get; set; } = "DTP Operator";

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        public string? Email { get; set; }

        /// <summary>
        /// Gets or sets the phone.
        /// </summary>
        /// <value>
        /// The phone.
        /// </value>
        public string? Phone { get; set; }

        /// <summary>
        /// Gets or sets the profile photo.
        /// </summary>
        /// <value>
        /// The profile photo.
        /// </value>
        public string? ProfilePhoto { get; set; }

        /// <summary>
        /// Gets or sets the photo URL.
        /// </summary>
        /// <value>
        /// The photo URL.
        /// </value>
        public string? PhotoUrl { get; set; }

        /// <summary>
        /// Gets or sets the staff category.
        /// </summary>
        /// <value>
        /// The staff category.
        /// </value>
        public string StaffCategory { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the joined at.
        /// </summary>
        /// <value>
        /// The joined at.
        /// </value>
        public DateTime JoinedAt { get; set; }
    }
}
