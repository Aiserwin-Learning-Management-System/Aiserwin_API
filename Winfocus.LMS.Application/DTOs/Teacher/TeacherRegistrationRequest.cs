namespace Winfocus.LMS.Application.DTOs.Teacher
{
    using System;

    /// <summary>
    /// Request DTO to create a teacher registration.
    /// </summary>
    public class TeacherRegistrationRequest
    {
        /// <summary>
        /// Gets or sets the employment type identifier (e.g., full-time, part-time).
        /// </summary>
        public Guid EmploymentTypeId { get; set; }

        /// <summary>
        /// Gets or sets the work mode of the teacher (e.g., online, offline, hybrid).
        /// </summary>
        public int WorkMode { get; set; }

        /// <summary>
        /// Gets or sets the full name of the teacher.
        /// </summary>
        public string FullName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the email address of the teacher.
        /// </summary>
        public string EmailAddress { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the date of birth of the teacher.
        /// </summary>
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Gets or sets the mobile number of the teacher.
        /// </summary>
        public string MobileNumber { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the residential address of the teacher.
        /// </summary>
        public string Address { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the gender of the teacher.
        /// </summary>
        public int Gender { get; set; }

        /// <summary>
        /// Gets or sets the marital status of the teacher.
        /// </summary>
        public int MaritalStatus { get; set; }

        /// <summary>
        /// Gets or sets the type of identification proof provided by the teacher.
        /// </summary>
        public int IdProofType { get; set; }

        /// <summary>
        /// Gets or sets the identification proof number.
        /// </summary>
        public string IdProofNumber { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the computer literacy level of the teacher.
        /// </summary>
        public int ComputerLiteracy { get; set; }

        /// <summary>
        /// Gets or sets the highest educational qualification of the teacher.
        /// </summary>
        public string HighestQualification { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the salary structure (e.g., fixed, hourly).
        /// </summary>
        public string SalaryStructure { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the payment cycle (e.g., monthly, weekly).
        /// </summary>
        public string PaymentCycle { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the contract duration of the teacher.
        /// </summary>
        public string ContractDuration { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the reporting manager's name or identifier.
        /// </summary>
        public string ReportingManager { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the file path of the teacher's profile photo.
        /// </summary>
        public string? PhotoPath { get; set; }

        /// <summary>
        /// Gets or sets the file path of the teacher's ID card.
        /// </summary>
        public string? IdCardPath { get; set; }

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
