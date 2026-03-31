namespace Winfocus.LMS.Application.DTOs.Teacher
{
    using System;

    /// <summary>
    /// Request DTO to create or update a teacher registration.
    /// </summary>
    public class TeacherRegistrationRequest
    {
        /// <summary>
        /// Gets or sets Employment type identifier (foreign key to staff category).
        /// </summary>
        public Guid EmploymentTypeId { get; set; }

        /// <summary>
        /// Gets or sets Work mode (enum value).
        /// </summary>
        public int WorkMode { get; set; }

        /// <summary>
        /// Gets or sets Date the teacher joined the organization.
        /// </summary>
        public DateTime DateOfJoining { get; set; }

        /// <summary>
        ///  Gets or sets Reporting manager identifier (optional).
        /// </summary>
        public Guid? ReportingManagerId { get; set; }

        /// <summary>
        /// Gets or sets Full name of the teacher.
        /// </summary>
        public string FullName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets Gender (enum value).
        /// </summary>
        public int Gender { get; set; }

        /// <summary>
        /// Gets or sets the date of birth.
        /// </summary>
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Gets or sets the nationality.
        /// </summary>
        public string Nationality { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the primary mobile number.
        /// </summary>
        public string MobileNumber { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the primary email address.
        /// </summary>
        public string EmailAddress { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the emergency contact number (optional).
        /// </summary>
        public string? EmergencyContactNumber { get; set; }

        /// <summary>
        /// Gets or sets the current residential address.
        /// </summary>
        public string Address { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the permanent address.
        /// </summary>
        public string PermanentAddress { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether the teacher is willing to work on weekends.
        /// </summary>
        public bool IsWillingToWorkWeekends { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the teacher has internet and system availability.
        /// </summary>
        public bool HasInternetAndSystemAvailability { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether terms and conditions are accepted.
        /// </summary>
        public bool IsTermsAccepted { get; set; }

        /// <summary>
        /// Gets or sets the declaration date (optional).
        /// </summary>
        public DateTime? DeclarationDate { get; set; }

        /// <summary>
        /// Gets or sets the professional details for the teacher.
        /// </summary>
        public TeacherProfessionalDetailDto? ProfessionalDetail { get; set; }

        /// <summary>
        /// Gets or sets the schedule preferences for the teacher.
        /// </summary>
        public TeacherScheduleDto? Schedule { get; set; }

        /// <summary>
        /// Gets or sets the document references (paths) for the teacher.
        /// </summary>
        public TeacherDocumentInfoDto? Documents { get; set; }

        /// <summary>
        /// Gets or sets the work history entries for the teacher.
        /// </summary>
        public List<TeacherWorkHistoryDto>? WorkHistory { get; set; }
    }
}
