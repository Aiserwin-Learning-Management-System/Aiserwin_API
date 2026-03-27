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
        /// Gets or sets the work mode.
        /// </summary>
        public WorkMode WorkMode { get; set; }

        // Personal Details
        /// <summary>
        /// Gets or sets the full name of the teacher.
        /// </summary>
        public string FullName { get; set; } = null!;

        /// <summary>
        /// Gets or sets the email address.
        /// </summary>
        public string EmailAddress { get; set; } = null!;

        /// <summary>
        /// Gets or sets the date of birth.
        /// </summary>
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Gets or sets the mobile number.
        /// </summary>
        public string MobileNumber { get; set; } = null!;

        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        public string Address { get; set; } = null!;

        /// <summary>
        /// Gets or sets the gender.
        /// </summary>
        public Gender Gender { get; set; }

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

        /// <summary>
        /// Gets or sets the computer literacy level.
        /// </summary>
        public ComputerLiteracy ComputerLiteracy { get; set; }

        // Academic Details
        /// <summary>
        /// Gets or sets the highest qualification.
        /// </summary>
        public string HighestQualification { get; set; } = null!;

        // Professional Details
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

        /// <summary>
        /// Gets or sets the reporting manager.
        /// </summary>
        public string ReportingManager { get; set; } = null!;

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
        /// Gets or sets a value indicating whether the declaration is accepted.
        /// </summary>
        public bool IsDeclarationAccepted { get; set; }

        // Navigation Properties
        /// <summary>
        /// Gets or sets the employment type (staff category).
        /// </summary>
        public StaffCategory EmploymentType { get; set; } = null!;

        /// <summary>
        /// Gets or sets the preferred grades.
        /// </summary>
        public ICollection<TeacherGrade> PreferredGrades { get; set; } = new List<TeacherGrade>();

        /// <summary>
        /// Gets or sets the preferred subjects.
        /// </summary>
        public ICollection<TeacherSubject> PreferredSubjects { get; set; } = new List<TeacherSubject>();

        /// <summary>
        /// Gets or sets the boards handled.
        /// </summary>
        public ICollection<TeacherSyllabus> BoardsHandled { get; set; } = new List<TeacherSyllabus>();

        /// <summary>
        /// Gets or sets the grades taught earlier.
        /// </summary>
        public ICollection<TeacherGrade> GradesTaughtEarlier { get; set; } = new List<TeacherGrade>();

        /// <summary>
        /// Gets or sets the subjects taught earlier.
        /// </summary>
        public ICollection<TeacherSubject> SubjectsTaughtEarlier { get; set; } = new List<TeacherSubject>();

        /// <summary>
        /// Gets or sets the LMS/tools known.
        /// </summary>
        public ICollection<TeacherTool> ToolsKnown { get; set; } = new List<TeacherTool>();

        /// <summary>
        /// Gets or sets the languages known.
        /// </summary>
        public ICollection<TeacherLanguage> LanguagesKnown { get; set; } = new List<TeacherLanguage>();
    }
}