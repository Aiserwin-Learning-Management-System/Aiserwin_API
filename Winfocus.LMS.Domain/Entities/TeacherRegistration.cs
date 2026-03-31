namespace Winfocus.LMS.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using Winfocus.LMS.Domain.Common;
    using Winfocus.LMS.Domain.Enums;

    /// <summary>
    /// Represents a teacher registration in the system.
    /// </summary>
    public class TeacherRegistration : BaseEntity
    {
        /// <summary>
        /// Gets or sets the auto-generated employee ID (e.g., TCH-20250001).
        /// </summary>
        public string EmployeeId { get; set; } = null!;

        /// <summary>
        /// Gets or sets the employment type (staff category).
        /// </summary>
        public Guid EmploymentTypeId { get; set; }

        /// <summary>
        /// Gets or sets the date of joining.
        /// </summary>
        public DateTime DateOfJoining { get; set; }

        /// <summary>
        /// Gets or sets the work mode.
        /// </summary>
        public WorkMode WorkMode { get; set; }

        /// <summary>
        /// Gets or sets the reporting manager identifier.
        /// </summary>
        public Guid? ReportingManagerId { get; set; }

        /// <summary>
        /// Gets or sets the full name of the teacher.
        /// </summary>
        public string FullName { get; set; } = null!;

        /// <summary>
        /// Gets or sets the gender.
        /// </summary>
        public Gender Gender { get; set; }

        /// <summary>
        /// Gets or sets the date of birth.
        /// </summary>
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Gets or sets the nationality.
        /// </summary>
        public string Nationality { get; set; } = null!;

        /// <summary>
        /// Gets or sets the mobile number.
        /// </summary>
        public string MobileNumber { get; set; } = null!;

        /// <summary>
        /// Gets or sets the email address.
        /// </summary>
        public string EmailAddress { get; set; } = null!;

        /// <summary>
        /// Gets or sets the emergency contact number.
        /// </summary>
        public string? EmergencyContactNumber { get; set; }

        /// <summary>
        /// Gets or sets the current residential address.
        /// </summary>
        public string Address { get; set; } = null!;

        /// <summary>
        /// Gets or sets the permanent address.
        /// </summary>
        public string PermanentAddress { get; set; } = null!;

        /// <summary>
        /// Gets or sets a value indicating whether the teacher is willing to work on weekends.
        /// </summary>
        public bool IsWillingToWorkWeekends { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the teacher has internet and system availability.
        /// </summary>
        public bool HasInternetAndSystemAvailability { get; set; }

        /// <summary>
        /// Gets or sets the current approval status of the teacher.
        /// </summary>
        public TeacherStatus Status { get; set; } = TeacherStatus.Pending;

        /// <summary>
        /// Gets or sets administrative remarks provided during approval/rejection.
        /// </summary>
        public string? AdministrativeRemarks { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the teacher has accepted terms and conditions.
        /// </summary>
        public bool IsTermsAccepted { get; set; }

        /// <summary>
        /// Gets or sets the declaration date.
        /// </summary>
        public DateTime? DeclarationDate { get; set; }

        /// <summary>
        /// Gets or sets the employment type (staff category).
        /// </summary>
        public StaffCategory EmploymentType { get; set; } = null!;

        /// <summary>
        /// Gets or sets the professional details of the teacher.
        /// </summary>
        public TeacherProfessionalDetail ProfessionalDetail { get; set; } = null!;

        /// <summary>
        /// Gets or sets the teaching schedule details.
        /// </summary>
        public TeacherSchedule Schedule { get; set; } = null!;

        /// <summary>
        /// Gets or sets the document details of the teacher.
        /// </summary>
        public TeacherDocumentInfo Documents { get; set; } = null!;

        /// <summary>
        /// Gets or sets the work history records of the teacher.
        /// </summary>
        public ICollection<TeacherWorkHistory> WorkHistory { get; set; } = new List<TeacherWorkHistory>();
    }
}
