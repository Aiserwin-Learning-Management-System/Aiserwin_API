namespace Winfocus.LMS.Domain.Entities
{
    using Winfocus.LMS.Domain.Common;

    /// <summary>
    /// Represents a dynamic staff category (e.g., DTP, Teacher, Driver)
    /// used as a reference when building registration forms.
    /// </summary>
    public class StaffCategory : BaseEntity
    {
        /// <summary>
        /// Gets or sets the name of the staff category.
        /// Must be unique among non-deleted records.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the optional description providing additional context
        /// about what this staff category represents.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the placeholder text shown in the input field
        /// on the Create User page. Optional — if null, frontend can
        /// default to "Enter {Name}" or similar.
        /// </summary>
        /// <example>Enter Teacher Name, Select Driver, Enter DTP Operator.</example>
        public string? PlaceholderText { get; set; }
    }
}
