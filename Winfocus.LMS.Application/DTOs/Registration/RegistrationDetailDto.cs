namespace Winfocus.LMS.Application.DTOs.Registration
{
    /// <summary>
    /// Full detail view of a registration with all submitted values.
    /// </summary>
    public class RegistrationDetailDto
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the form identifier.
        /// </summary>
        /// <value>
        /// The form identifier.
        /// </value>
        public Guid FormId { get; set; }

        /// <summary>
        /// Gets or sets the name of the form.
        /// </summary>
        /// <value>
        /// The name of the form.
        /// </value>
        public string FormName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the staff category.
        /// </summary>
        /// <value>
        /// The staff category.
        /// </value>
        public string StaffCategory { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the remarks.
        /// </summary>
        /// <value>
        /// The remarks.
        /// </value>
        public string? Remarks { get; set; }

        /// <summary>
        /// Gets or sets the submitted at.
        /// </summary>
        /// <value>
        /// The submitted at.
        /// </value>
        public DateTime? SubmittedAt { get; set; }

        /// <summary>
        /// Gets or sets the created at.
        /// </summary>
        /// <value>
        /// The created at.
        /// </value>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the values.
        /// </summary>
        /// <value>
        /// The values.
        /// </value>
        public List<RegistrationValueDetailDto> Values { get; set; } = new();
    }
}
