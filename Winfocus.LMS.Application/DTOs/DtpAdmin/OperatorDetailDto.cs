using System;
using System.Collections.Generic;
using System.Text;
using Winfocus.LMS.Application.DTOs.Dashboard;

namespace Winfocus.LMS.Application.DTOs.DtpAdmin
{
    /// <summary>
    /// Paginated operator list with dynamic column data.
    /// </summary>
    public class OperatorDetailDto
    {
        /// <summary>
        /// Gets or sets unique identifier of the operator registration.
        /// </summary>
        public Guid RegistrationId { get; set; }

        /// <summary>
        /// Gets or sets current status of the registration.
        /// Example: Submitted, Approved, Rejected, CorrectionRequested.
        /// </summary>
        public string Status { get; set; } = null!;

        /// <summary>
        /// Gets or sets date and time when the operator registered.
        /// </summary>
        public DateTime RegisteredAt { get; set; }

        /// <summary>
        /// Gets or sets extracted profile details of the operator.
        /// Includes name, email, phone, role, and profile photo.
        /// </summary>
        public ProfileDto? Profile { get; set; }

        /// <summary>
        /// Gets or sets dynamic grouped sections of the registration form.
        /// Each section represents a field group (e.g., Personal Information, Address).
        /// </summary>
        public List<OperatorSectionDto>? Sections { get; set; }

        /// <summary>
        /// Gets or sets list of uploaded documents extracted from file-type fields.
        /// Includes file name and downloadable URL.
        /// </summary>
        public List<OperatorDocumentDto> Documents { get; set; }

        /// <summary>
        /// Gets or sets task-related statistics for the operator.
        /// Includes total, active, completed tasks and question counts.
        /// </summary>
        public OperatorTaskStatsDto TaskStats { get; set; }
    }

    /// <summary>
    /// Represents the basic profile information of an operator
    /// extracted dynamically from registration form values.
    /// </summary>
    public class ProfileDto
    {
        /// <summary>
        /// Gets or sets full name of the operator.
        /// Extracted from a field with name containing "name"
        /// or first text field in the form.
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Gets or sets email address of the operator.
        /// Extracted from a field with FieldType = Email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets phone number of the operator.
        /// Extracted from a field with FieldType = Phone.
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Gets or sets role assigned to the operator.
        /// Typically static (e.g., "DTP Operator") or derived from staff category.
        /// </summary>
        public string Role { get; set; }

        /// <summary>
        /// Gets or sets uRL of the operator's profile photo.
        /// Extracted from a FileUpload field where the field name contains "photo".
        /// Returned as a full URL using file storage service.
        /// </summary>
        public string ProfilePhoto { get; set; }
    }

    /// <summary>
    /// Represents a document uploaded by the operator during registration.
    /// Extracted from file-type dynamic form fields.
    /// </summary>
    public class OperatorDocumentDto
    {
        /// <summary>
        /// Gets or sets unique field identifier name from the registration form 
        /// (e.g., "id_proof", "resume", "photo").
        /// Used internally to map the document to its form field.
        /// </summary>
        public string? FieldName { get; set; }

        /// <summary>
        /// Gets or sets display label of the field shown in the UI 
        /// (e.g., "ID Proof", "Resume Upload").
        /// </summary>
        public string? Label { get; set; }

        /// <summary>
        /// Gets or sets fully qualified URL used to download or view the file.
        /// Generated using the file storage service.
        /// </summary>
        public string? FileUrl { get; set; }

        /// <summary>
        /// Gets or sets original file name extracted from the uploaded file path.
        /// </summary>
        public string FileName { get; set; }
    }

    /// <summary>
    /// Represents task-related statistics for a DTP operator.
    /// </summary>
    public class OperatorTaskStatsDto
    {
        /// <summary>
        /// Gets or sets total number of tasks assigned to the operator.
        /// </summary>
        public int TotalTasks { get; set; }

        /// <summary>
        /// Gets or sets number of tasks that are currently active (in progress).
        /// </summary>
        public int ActiveTasks { get; set; }

        /// <summary>
        /// Gets or sets number of tasks that have been completed by the operator.
        /// </summary>
        public int CompletedTasks { get; set; }

        /// <summary>
        /// Gets or sets total number of questions assigned across all tasks.
        /// </summary>
        public int TotalQuestionsAssigned { get; set; }

        /// <summary>
        /// Gets or sets total number of questions completed by the operator.
        /// </summary>
        public int TotalQuestionsCompleted { get; set; }
    }

    /// <summary>
    /// Request DTO used to verify (approve/reject/request correction) a DTP operator registration.
    /// </summary>
    public class VerifyOperatorDto
    {
        /// <summary>
        /// Gets or sets action to be performed on the registration.
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// Gets or sets additional remarks or comments provided by the admin during verification.
        /// </summary>
        public string Remarks { get; set; }
    }

    /// <summary>
    /// Request DTO used.
    /// </summary>
    public class OperatorFieldDto
    {
        /// <summary>
        /// Gets or sets action.
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Gets or sets action.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets action.
        /// </summary>
        public string FieldType { get; set; }

        /// <summary>
        /// Gets or sets action.
        /// </summary>
        public string FileUrl { get; set; }
    }

    /// <summary>
    /// Gets or sets action.
    /// </summary>
    public class OperatorSectionDto
    {
        /// <summary>
        /// Gets or sets action to be performed on the registration.
        /// </summary>
        public string? GroupName { get; set; }

        /// <summary>
        /// Gets or sets action to be performed on the registration.
        /// </summary>
        public List<OperatorFieldDto>? Fields { get; set; }
    }
}
