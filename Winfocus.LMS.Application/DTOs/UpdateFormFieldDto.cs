using System;
using System.Collections.Generic;
using System.Text;
using Winfocus.LMS.Domain.Enums;

namespace Winfocus.LMS.Application.DTOs
{
    /// <summary>
    /// Request DTO used to update an existing form field.
    /// Supports updating group assignment and selectable options.
    /// </summary>
    public class UpdateFormFieldDto
    {
        /// <summary>
        /// Gets or sets the optional field group identifier.
        /// </summary>
        public Guid? FieldGroupId { get; set; }

        /// <summary>
        /// Gets or sets the display label shown on the form.
        /// </summary>
        public string DisplayLabel { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the placeholder text.
        /// </summary>
        public string? Placeholder { get; set; }

        /// <summary>
        /// Gets or sets the field type.
        /// </summary>
        public FieldType FieldType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the field is required.
        /// </summary>
        public bool IsRequired { get; set; }

        /// <summary>
        /// Gets or sets validation regex.
        /// </summary>
        public string? ValidationRegex { get; set; }

        /// <summary>
        /// Gets or sets minimum length validation.
        /// </summary>
        public int? MinLength { get; set; }

        /// <summary>
        /// Gets or sets maximum length validation.
        /// </summary>
        public int? MaxLength { get; set; }

        /// <summary>
        /// Gets or sets the display order.
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// Gets or sets the list of selectable options.
        /// Options will be synced (added/updated/removed).
        /// </summary>
        public List<UpdateFieldOptionDto>? Options { get; set; }
    }
}
