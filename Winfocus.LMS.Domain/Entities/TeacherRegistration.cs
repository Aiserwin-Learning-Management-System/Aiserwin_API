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
        /// Gets or sets the Alternative mobile number.
        /// </summary>
        public string AlternativeMobileNumber { get; set; } = null!;

        /// <summary>
        /// Gets or sets the email address.
        /// </summary>
        public string EmailAddress { get; set; } = null!;

        /// <summary>
        /// Gets or sets the alternative email address.
        /// </summary>
        public string AlternativeEmailAddress { get; set; } = null!;

        /// <summary>
        /// Gets or sets the district name.
        /// </summary>
        public string DistrictOrLocation { get; set; } = null!;

        /// <summary>
        /// Gets or sets the emergency contact number.
        /// </summary>
        public string? RefernceContactNumber { get; set; }

        /// <summary>
        /// Gets or sets the emergency contact name.
        /// </summary>
        public string? RefernceContactName { get; set; }

        /// <summary>
        /// Gets or sets the current residential address.
        /// </summary>
        public string ResidentialAddress { get; set; } = null!;

        /// <summary>
        /// Gets or sets a value indicating whether the teacher is willing to work on weekends.
        /// </summary>
        public bool IsWillingToWorkWeekends { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the teacher has internet and system availability.
        /// </summary>
        public bool HasInternetAndSystemAvailability { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets a is declared.
        /// </summary>
        public bool IsDeclared { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets a value Signed Agreement.
        /// </summary>
        public bool IsSignedAgreement { get; set; }

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
        /// Gets or sets the joining date.
        /// </summary>
        public DateTime? JoiningDate { get; set; }

        /// <summary>
        /// Gets or sets the contract duration.
        /// </summary>
        public decimal ContractDuration { get; set; }

        /// <summary>
        /// Gets or sets the pincode.
        /// </summary>
        public string? Pincode { get; set; }

        /// <summary>
        /// Gets or sets the countryid.
        /// </summary>
        public Guid CountryId { get; set; }

        /// <summary>
        /// Gets or sets the country.
        /// </summary>
        public Country Country { get; set; } = null!;

        /// <summary>
        /// Gets or sets the stateid.
        /// </summary>
        public Guid StateId { get; set; }

        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        public State State { get; set; } = null!;

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

        /// <summary>
        /// Gets or sets the subjects taught earlier by the teacher.
        /// </summary>
        public ICollection<TeacherTaughtSubject> SubjectsTaughtEarlier { get; set; } = new List<TeacherTaughtSubject>();

        /// <summary>
        /// Gets or sets the grades taught earlier by the teacher.
        /// </summary>
        public ICollection<TeacherTaughtGrade> GradesTaughtEarlier { get; set; } = new List<TeacherTaughtGrade>();
    }
}
