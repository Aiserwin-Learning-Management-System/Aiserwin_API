namespace Winfocus.LMS.Application.DTOs.Teacher
{
    using System;
    using Winfocus.LMS.Domain.Enums;

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
        public Guid CountryId { get; set; }

        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        public Guid StateId { get; set; }

        /// <summary>
        /// Gets or sets the pincode.
        /// </summary>
        public string Pincode { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the current DistrictOrLocation.
        /// </summary>
        public string DistrictOrLocation { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the primary mobile number.
        /// </summary>
        public string MobileNumber { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the alternative mobile number.
        /// </summary>
        public string AlternativeMobileNumber { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the primary email address.
        /// </summary>
        public string EmailAddress { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the alternative email address.
        /// </summary>
        public string AlternativeEmailAddress { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the emergency contact number (optional).
        /// </summary>
        public string ReferenceContactNumber { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the emergency contact number (optional).
        /// </summary>
        public string EmergencyContactName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the current residential address.
        /// </summary>
        public string ResidentialAddress { get; set; } = string.Empty;

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
        /// Gets or sets the contract duration.
        /// </summary>
        public decimal ContractDuration { get; set; }

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

        /// <summary>
        /// <summary>
        /// Gets or sets the preferred subjects (list of exam subject ids).
        /// </summary>
        public List<Guid>? PreferredSubjectIds { get; set; }

        /// <summary>
        /// <summary>
        /// Gets or sets the preferred grades (list of exam grade ids).
        /// </summary>
        public List<Guid>? PreferredGradeIds { get; set; }

        /// <summary>
        /// <summary>
        /// Gets or sets the preferred syllabuses (list of syllabus ids).
        /// </summary>
        public List<Guid>? PreferredSyllabusIds { get; set; }

        /// <summary>
        /// <summary>
        /// Gets or sets the languages known by the teacher (enum values as ints).
        /// </summary>
        public List<int>? Languages { get; set; }

        /// <summary>
        /// <summary>
        /// Gets or sets the teaching tools known — if a tool does not exist it will be created.
        /// </summary>
        public List<TeachingToolsDto>? Tools { get; set; }

        /// <summary>
        /// <summary>
        /// Gets or sets the subjects taught earlier (exam subject ids).
        /// </summary>
        public List<Guid>? TaughtSubjectIds { get; set; }

        /// <summary>
        /// <summary>
        /// Gets or sets the grades taught earlier (exam grade ids).
        /// </summary>
        public List<Guid>? TaughtGradeIds { get; set; }
    }
}
