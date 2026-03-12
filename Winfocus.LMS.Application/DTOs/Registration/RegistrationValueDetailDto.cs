namespace Winfocus.LMS.Application.DTOs.Registration
{
    /// <summary>
    /// A single field value with full label and type information.
    /// </summary>
    public class RegistrationValueDetailDto
    {

        /// <summary>
        /// Gets or sets the field identifier.
        /// </summary>
        /// <value>
        /// The field identifier.
        /// </value>
        public Guid FieldId { get; set; }

        /// <summary>
        /// Gets or sets the name of the field.
        /// </summary>
        /// <value>
        /// The name of the field.
        /// </value>
        public string FieldName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the display label.
        /// </summary>
        /// <value>
        /// The display label.
        /// </value>
        public string DisplayLabel { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the type of the field.
        /// </summary>
        /// <value>
        /// The type of the field.
        /// </value>
        public string FieldType { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public string? Value { get; set; }

        /// <summary>
        /// Gets or sets the file URL.
        /// </summary>
        /// <value>
        /// The file URL.
        /// </value>
        public string? FileUrl { get; set; }
    }
}
