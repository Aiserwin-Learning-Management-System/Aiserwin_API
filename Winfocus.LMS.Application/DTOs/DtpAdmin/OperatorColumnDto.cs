namespace Winfocus.LMS.Application.DTOs.DtpAdmin
{
    /// <summary>
    /// Column definition from registration form — frontend builds table headers.
    /// </summary>
    public class OperatorColumnDto
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
        /// Gets or sets the display order.
        /// </summary>
        /// <value>
        /// The display order.
        /// </value>
        public int DisplayOrder { get; set; }
    }
}
