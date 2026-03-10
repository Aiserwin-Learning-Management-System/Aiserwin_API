namespace Winfocus.LMS.Domain.Entities
{
    using Winfocus.LMS.Domain.Common;

    /// <summary>
    /// Represents a logical grouping of form fields (e.g., "Personal Info", "Address").
    /// Used to organize fields into sections when building registration forms.
    /// </summary>
    public class FieldGroup : BaseEntity
    {
        /// <summary>
        /// Gets or sets the unique name of this field group.
        /// Must be unique among non-deleted records.
        /// </summary>
        /// <example>Personal Information, Address Details, Emergency Contact.</example>
        public string GroupName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the optional description of what this group contains.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the display order for rendering groups on the form.
        /// Lower values appear first.
        /// </summary>
        public int DisplayOrder { get; set; } = 0;

        /// <summary>
        /// Gets or sets the collection of form fields belonging to this group.
        /// </summary>
        public ICollection<FormField> FormFields { get; set; } = new List<FormField>();
    }
}
