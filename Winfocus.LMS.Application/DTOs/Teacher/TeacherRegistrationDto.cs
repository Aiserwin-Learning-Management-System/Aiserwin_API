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
        /// Gets or sets the date the teacher joined the organization.
        /// </summary>
        public DateTime DateOfJoining { get; set; }

        /// <summary>
        /// Gets or sets the nationality of the teacher.
        /// </summary>
        public string Nationality { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the emergency contact number.
        /// </summary>
        public string? EmergencyContactNumber { get; set; }

        /// <summary>
        /// Gets or sets the permanent address of the teacher.
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
        /// Gets or sets a value indicating whether the teacher has accepted terms and conditions.
        /// </summary>
        public bool IsTermsAccepted { get; set; }

        /// <summary>
        /// Gets or sets the declaration date, if provided.
        /// </summary>
        public DateTime? DeclarationDate { get; set; }

        /// <summary>
        /// Gets or sets the current approval status of the teacher.
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// Gets or sets administrative remarks provided during approval.
        /// </summary>
        public string? AdministrativeRemarks { get; set; }

        /// <summary>
        /// Gets or sets the reporting manager identifier (if set).
        /// </summary>
        public Guid? ReportingManagerId { get; set; }

        /// <summary>
        /// Gets or sets the country identifier of the teacher.
        /// </summary>
        public Guid CountryId { get; set; }

        /// <summary>
        /// Gets or sets the employment type (staff category) identifier.
        /// </summary>
        public Guid EmploymentTypeId { get; set; }
    }
}
