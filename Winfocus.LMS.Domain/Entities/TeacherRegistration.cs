namespace Winfocus.LMS.Domain.Entities
{
    using System;
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

        // Employment Details

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
        /// Gets or sets the working days (JSON string for checkbox group).
        /// </summary>
        public string WorkingDays { get; set; } = null!;

        /// <summary>
        /// Gets or sets the working time start.
        /// </summary>
        public TimeSpan WorkingTimeStart { get; set; }

        /// <summary>
        /// Gets or sets the working time end.
        /// </summary>
        public TimeSpan WorkingTimeEnd { get; set; }

        /// <summary>
        /// Gets or sets the reporting manager.
        /// </summary>
        public string ReportingManager { get; set; } = null!;

        /// <summary>
        /// Gets or sets the salary structure.
        /// </summary>
        public string SalaryStructure { get; set; } = null!;

        /// <summary>
        /// Gets or sets the payment cycle.
        /// </summary>
        public string PaymentCycle { get; set; } = null!;

        /// <summary>
        /// Gets or sets the contract duration.
        /// </summary>
        public string ContractDuration { get; set; } = null!;

        // Personal Details

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
        /// Gets or sets the current address.
        /// </summary>
        public string Address { get; set; } = null!;

        /// <summary>
        /// Gets or sets the permanent address.
        /// </summary>
        public string PermanentAddress { get; set; } = null!;

        /// <summary>
        /// Gets or sets the emergency contact number.
        /// </summary>
        public string EmergencyContactNumber { get; set; } = null!;

        /// <summary>
        /// Gets or sets the marital status.
        /// </summary>
        public MaritalStatus MaritalStatus { get; set; }

        /// <summary>
        /// Gets or sets the ID proof type.
        /// </summary>
        public IdProofType IdProofType { get; set; }

        /// <summary>
        /// Gets or sets the ID proof number.
        /// </summary>
        public string IdProofNumber { get; set; } = null!;

        // Academic Details

        /// <summary>
        /// Gets or sets the highest qualification.
        /// </summary>
        public string HighestQualification { get; set; } = null!;

        /// <summary>
        /// Gets or sets the subject specialization.
        /// </summary>
        public string SubjectSpecialization { get; set; } = null!;

        /// <summary>
        /// Gets or sets the university/institution.
        /// </summary>
        public string University { get; set; } = null!;

        /// <summary>
        /// Gets or sets the year of passing.
        /// </summary>
        public string YearOfPassing { get; set; } = null!;

        /// <summary>
        /// Gets or sets a value indicating whether the teacher has teaching certification.
        /// </summary>
        public bool HasTeachingCertification { get; set; }

        /// <summary>
        /// Gets or sets the additional courses.
        /// </summary>
        public string AdditionalCourses { get; set; } = null!;

        /// <summary>
        /// Gets or sets the computer literacy level.
        /// </summary>
        public ComputerLiteracy ComputerLiteracy { get; set; }

        /// <summary>
        /// Gets or sets the current status.
        /// </summary>
        public string CurrentStatus { get; set; } = null!;

        // Professional Details

        /// <summary>
        /// Gets or sets the total teaching experience.
        /// </summary>
        public string TotalTeachingExperience { get; set; } = null!;

        /// <summary>
        /// Gets or sets a value indicating whether the teacher has online teaching experience.
        /// </summary>
        public bool HasOnlineTeachingExperience { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the teacher has offline teaching experience.
        /// </summary>
        public bool HasOfflineTeachingExperience { get; set; }

        /// <summary>
        /// Gets or sets the previous institutions.
        /// </summary>
        public string PreviousInstitutions { get; set; } = null!;

        /// <summary>
        /// Gets or sets a value indicating whether the teacher is available for demo class.
        /// </summary>
        public bool IsAvailableForDemoClass { get; set; }

        /// <summary>
        /// Gets or sets the preferred teaching time (JSON for time slots).
        /// </summary>
        public string PreferredTeachingTime { get; set; } = null!;

        /// <summary>
        /// Gets or sets a value indicating whether the teacher is willing to work weekends.
        /// </summary>
        public bool IsWillingToWorkWeekends { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the teacher has internet and system availability.
        /// </summary>
        public bool HasInternetAndSystemAvailability { get; set; }

        /// <summary>
        /// Gets or sets the reference name.
        /// </summary>
        public string? ReferenceName { get; set; }

        /// <summary>
        /// Gets or sets the reference contact.
        /// </summary>
        public string? ReferenceContact { get; set; }

        // File Uploads

        /// <summary>
        /// Gets or sets the path to the uploaded photo.
        /// </summary>
        public string? PhotoPath { get; set; }

        /// <summary>
        /// Gets or sets the path to the uploaded ID card.
        /// </summary>
        public string? IdCardPath { get; set; }

        // Declaration

        /// <summary>
        /// Gets or sets a value indicating whether terms and conditions are accepted.
        /// </summary>
        public bool IsTermsAccepted { get; set; }

        /// <summary>
        /// Gets or sets the declaration date.
        /// </summary>
        public DateTime? DeclarationDate { get; set; }

        /// <summary>
        /// Gets or sets the administrative remarks.
        /// </summary>
        public string? AdministrativeRemarks { get; set; }

        /// <summary>
        /// Gets or sets the approval status.
        /// </summary>
        public string ApprovalStatus { get; set; } = "Pending";

        // Navigation Properties

        /// <summary>
        /// Gets or sets the employment type (staff category).
        /// </summary>
        public StaffCategory EmploymentType { get; set; } = null!;

        /// <summary>
        /// Gets or sets the preferred grades.
        /// </summary>
        public ICollection<TeacherPreferredGrade> PreferredGrades { get; set; } = new List<TeacherPreferredGrade>();

        /// <summary>
        /// Gets or sets the preferred subjects.
        /// </summary>
        public ICollection<TeacherPreferredSubject> PreferredSubjects { get; set; } = new List<TeacherPreferredSubject>();

        /// <summary>
        /// Gets or sets the boards handled.
        /// </summary>
        public ICollection<TeacherSyllabus> BoardsHandled { get; set; } = new List<TeacherSyllabus>();

        /// <summary>
        /// Gets or sets the grades taught earlier.
        /// </summary>
        public ICollection<TeacherTaughtGrade> GradesTaughtEarlier { get; set; } = new List<TeacherTaughtGrade>();

        /// <summary>
        /// Gets or sets the subjects taught earlier.
        /// </summary>
        public ICollection<TeacherTaughtSubject> SubjectsTaughtEarlier { get; set; } = new List<TeacherTaughtSubject>();

        /// <summary>
        /// Gets or sets the LMS/tools known.
        /// </summary>
        public ICollection<TeacherTool> ToolsKnown { get; set; } = new List<TeacherTool>();

        /// <summary>
        /// Gets or sets the languages known.
        /// </summary>
        public ICollection<TeacherLanguage> LanguagesKnown { get; set; } = new List<TeacherLanguage>();

        /// <summary>
        /// Gets or sets the academic records.
        /// </summary>
        public ICollection<TeacherAcademicRecord> AcademicRecords { get; set; } = new List<TeacherAcademicRecord>();

        /// <summary>
        /// Gets or sets the work history.
        /// </summary>
        public ICollection<TeacherWorkHistory> WorkHistory { get; set; } = new List<TeacherWorkHistory>();
    }
}