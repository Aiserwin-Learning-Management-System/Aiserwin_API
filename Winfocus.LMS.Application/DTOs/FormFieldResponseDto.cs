using System;
using System.Collections.Generic;
using System.Text;

namespace Winfocus.LMS.Application.DTOs
{
    /// <summary>
    /// Detailed response DTO representing a form field with group and options.
    /// </summary>
    public class FormFieldResponseDto
    {
        /// <summary>
        /// Gets or sets the field identifier.
        /// </summary>
        public Guid FieldId { get; set; }

        /// <summary>
        /// Gets or sets the programmatic field name.
        /// </summary>
        public string FieldName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the display label.
        /// </summary>
        public string DisplayLabel { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the placeholder text.
        /// </summary>
        public string? Placeholder { get; set; }

        /// <summary>
        /// Gets or sets the field type.
        /// </summary>
        public string FieldType { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets whether the field is required.
        /// </summary>
        public bool IsRequired { get; set; }

        /// <summary>
        /// Gets or sets the display order.
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// Gets or sets the group information.
        /// NULL if standalone.
        /// </summary>
        public FieldGroupDto? Group { get; set; }

        /// <summary>
        /// Gets or sets selectable options.
        /// </summary>
        public List<FieldOptionDto>? Options { get; set; }
    }
}
