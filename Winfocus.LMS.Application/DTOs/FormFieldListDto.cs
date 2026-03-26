using System;
using System.Collections.Generic;
using System.Text;

namespace Winfocus.LMS.Application.DTOs
{
    /// <summary>
    /// Lightweight DTO used when listing form fields.
    /// </summary>
    public class FormFieldListDto
    {
        /// <summary>
        /// Gets or sets the field type.
        /// </summary>
        public Guid FieldId { get; set; }

        /// <summary>
        /// Gets or sets the FieldName.
        /// </summary>
        public string FieldName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the DisplayLabel.
        /// </summary>
        public string DisplayLabel { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the field type.
        /// </summary>
        public string FieldType { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the DisplayOrder.
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// Gets or sets the Group.
        /// </summary>
        public FieldGroupDto? Group { get; set; }
    }
}
