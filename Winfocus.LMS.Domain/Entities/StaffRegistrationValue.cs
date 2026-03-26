namespace Winfocus.LMS.Domain.Entities
{
    /// <summary>
    /// Stores the actual value submitted for a single form field
    /// within a staff registration. Values are stored as NVARCHAR(MAX)
    /// for flexibility across all field types.
    /// </summary>
    public class StaffRegistrationValue
    {
        /// <summary>
        /// Gets or sets the unique identifier.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the parent registration this value belongs to.
        /// </summary>
        public Guid RegistrationId { get; set; }

        /// <summary>
        /// Gets or sets the form field this value is for.
        /// </summary>
        public Guid FieldId { get; set; }

        /// <summary>
        /// Gets or sets the snapshot of the field name at submission time.
        /// Preserved even if the original field is later renamed.
        /// </summary>
        public string FieldName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the submitted value. NULL for empty/skipped optional fields.
        /// File uploads store the file path/URL.
        /// </summary>
        public string? Value { get; set; }

        /// <summary>
        /// Gets or sets the parent registration.
        /// </summary>
        public StaffRegistration StaffRegistration { get; set; } = null!;

        /// <summary>
        /// Gets or sets the form field definition.
        /// </summary>
        public FormField FormField { get; set; } = null!;
    }
}
