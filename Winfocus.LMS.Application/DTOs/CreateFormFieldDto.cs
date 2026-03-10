using System;
using System.Collections.Generic;
using System.Text;
using Winfocus.LMS.Domain.Enums;

namespace Winfocus.LMS.Application.DTOs
{
    /// <summary>
    /// Represents the request to create a new dynamic form field.
    /// The field may optionally belong to a field group.
    /// </summary>
    public class CreateFormFieldDto
    {
        /// <summary>
        /// Gets or sets the optional identifier of the field group.
        /// If null, the field will be created as a standalone (ungrouped) field.
        /// </summary>
        public Guid? FieldGroupId { get; set; }

        /// <summary>
        /// Gets or sets the unique programmatic name of the field.
        /// Used as the key when storing form data.
        /// </summary>
        /// <example>first_name, date_of_birth.</example>
        public string FieldName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the human-readable label displayed on the form.
        /// </summary>
        /// <example>First Name.</example>
        public string DisplayLabel { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the placeholder text displayed inside the input control.
        /// </summary>
        public string? Placeholder { get; set; }

        /// <summary>
        /// Gets or sets the type of form field input.
        /// </summary>
        public FieldType FieldType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this field is required.
        /// </summary>
        public bool IsRequired { get; set; }

        /// <summary>
        /// Gets or sets the optional regular expression used for validation.
        /// </summary>
        public string? ValidationRegex { get; set; }

        /// <summary>
        /// Gets or sets the minimum length allowed for text-based fields.
        /// </summary>
        public int? MinLength { get; set; }

        /// <summary>
        /// Gets or sets the maximum length allowed for text-based fields.
        /// </summary>
        public int? MaxLength { get; set; }

        /// <summary>
        /// Gets or sets the display order of the field inside its group.
        /// Lower numbers appear earlier.
        /// </summary>
        public int? DisplayOrder { get; set; }

        /// <summary>
        /// Gets or sets the selectable options for dropdown, checkbox, or radio fields.
        /// </summary>
        public List<FieldOptionDto>? Options { get; set; }
    }
}
