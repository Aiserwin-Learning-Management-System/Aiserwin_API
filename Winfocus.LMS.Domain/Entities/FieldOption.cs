namespace Winfocus.LMS.Domain.Entities
{
    /// <summary>
    /// Represents a single selectable option for Dropdown, Checkbox, or Radio fields.
    /// This is a lightweight child entity — does NOT inherit BaseEntity.
    /// Cascade-deleted when the parent FormField is deleted.
    /// </summary>
    public class FieldOption
    {
        /// <summary>
        /// Gets or sets the unique identifier.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the parent form field this option belongs to.
        /// </summary>
        public Guid FieldId { get; set; }

        /// <summary>
        /// Gets or sets the display/stored value of this option.
        /// </summary>
        /// <example>Male, Female, Other.</example>
        public string OptionValue { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the display order among sibling options.
        /// Lower values appear first.
        /// </summary>
        public int DisplayOrder { get; set; } = 0;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is active; otherwise, <c>false</c>.
        /// </value>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Gets or sets the parent form field.
        /// </summary>
        public FormField FormField { get; set; } = null!;
    }
}
