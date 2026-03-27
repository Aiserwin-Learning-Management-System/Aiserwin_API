namespace Winfocus.LMS.Application.DTOs.DtpAdmin
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Teacher summary used in grouped lists.
    /// </summary>
    public class TeacherListDto
    {
        /// <summary>
        /// Gets or sets the unique registration identifier of the teacher.
        /// </summary>
        public Guid RegistrationId { get; set; }

        /// <summary>
        /// Gets or sets the full name of the teacher.
        /// </summary>
        public string FullName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the employee ID assigned to the teacher.
        /// </summary>
        public string EmployeeId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the role/designation of the teacher (e.g., Subject Teacher, HOD).
        /// </summary>
        public string Role { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the email address of the teacher.
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Gets or sets the date of birth of the teacher in string format.
        /// </summary>
        public string? DateOfBirth { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the teacher joined the organization.
        /// </summary>
        public DateTime JoinedAt { get; set; }
    }

    /// <summary>
    /// Grouping of teachers under a staff category.
    /// </summary>
    public class TeachersByCategoryDto
    {
        /// <summary>
        /// Gets or sets the name of the staff category (e.g., Teaching Staff, Non-Teaching Staff).
        /// </summary>
        public string CategoryName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the list of teachers belonging to the specified category.
        /// </summary>
        public List<TeacherListDto> Teachers { get; set; } = new List<TeacherListDto>();
    }
}
