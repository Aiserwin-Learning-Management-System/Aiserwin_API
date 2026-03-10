namespace Winfocus.LMS.Domain.Entities
{
    using Winfocus.LMS.Domain.Common;
    using Winfocus.LMS.Domain.Enums;

    /// <summary>
    /// Represents a single reusable dynamic form field with validation rules.
    /// Can optionally belong to a <see cref="FieldGroup"/> or exist standalone.
    /// </summary>
    public class FormField : BaseEntity
    {
        /// <summary>
        /// Gets or sets the optional field group this field belongs to.
        /// NULL means the field is standalone (ungrouped).
        /// </summary>
        public Guid? FieldGroupId { get; set; }

        /// <summary>
        /// Gets or sets the unique programmatic name of the field.
        /// Used as the form control name / JSON key.
        /// </summary>
        /// <example>first_name, date_of_birth, profile_photo.</example>
        public string FieldName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the human-readable label displayed on the form.
        /// </summary>
        /// <example>First Name, Date of Birth, Profile Photo.</example>
        public string DisplayLabel { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the optional placeholder text shown inside the input.
        /// </summary>
        public string? Placeholder { get; set; }

        /// <summary>
        /// Gets or sets the input type of this field.
        /// Stored as INT in the database.
        /// </summary>
        public FieldType FieldType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is required.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is required; otherwise, <c>false</c>.
        /// </value>
        public bool IsRequired { get; set; } = false;

        /// <summary>
        /// Gets or sets the optional regex pattern for custom validation.
        /// </summary>
        /// <example>^[a-zA-Z]+$, ^\d{10}$.</example>
        public string? ValidationRegex { get; set; }

        /// <summary>
        /// Gets or sets the minimum allowed length (for text-based fields).
        /// </summary>
        public int? MinLength { get; set; }

        /// <summary>
        /// Gets or sets the maximum allowed length (for text-based fields).
        /// </summary>
        public int? MaxLength { get; set; }

        /// <summary>
        /// Gets or sets the display order within its group (or globally if ungrouped).
        /// Lower values appear first.
        /// </summary>
        public int DisplayOrder { get; set; } = 0;

        /// <summary>
        /// Gets or sets the parent field group. NULL if standalone.
        /// </summary>
        public FieldGroup? FieldGroup { get; set; }

        /// <summary>
        /// Gets or sets the selectable options for Dropdown, Checkbox, and Radio fields.
        /// </summary>
        public ICollection<FieldOption> FieldOptions { get; set; } = new List<FieldOption>();
    }
}
