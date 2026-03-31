using System;
using Winfocus.LMS.Application.DTOs.Masters;

namespace Winfocus.LMS.Application.DTOs.Teacher
{
    /// <summary>
    /// Response DTO for teacher registration.
    /// </summary>
    public class TeacherRegistrationDto : BaseClassDTO
    {
        /// <summary>
        /// Gets or sets the employee ID assigned to the teacher.
        /// </summary>
        public string EmployeeId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the full name of the teacher.
        /// </summary>
        public string FullName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the email address of the teacher.
        /// </summary>
        public string EmailAddress { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the mobile number of the teacher.
        /// </summary>
        public string MobileNumber { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the date of birth of the teacher.
        /// </summary>
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the teacher has accepted the terms and conditions.
        /// </summary>
        public bool IsTermsAccepted { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the teacher has accepted the declaration.
        /// </summary>
        public bool IsDeclarationAccepted { get; set; }
    }
}
