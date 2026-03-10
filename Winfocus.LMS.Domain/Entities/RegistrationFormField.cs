namespace Winfocus.LMS.Domain.Entities
{
    /// <summary>
    /// Links a <see cref="FormField"/> to a <see cref="RegistrationForm"/>,
    /// optionally within a <see cref="RegistrationFormGroup"/>.
    /// Allows per-form override of the IsRequired setting.
    /// Lightweight junction entity — no audit fields needed.
    /// </summary>
    public class RegistrationFormField
    {
        /// <summary>
        /// Gets or sets the unique identifier.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the parent registration form.
        /// </summary>
        public Guid FormId { get; set; }

        /// <summary>
        /// Gets or sets the reusable form field being linked.
        /// </summary>
        public Guid FieldId { get; set; }

        /// <summary>
        /// Gets or sets the optional group this field belongs to within the form.
        /// NULL means the field is standalone (ungrouped) on the form.
        /// </summary>
        public Guid? FormGroupId { get; set; }

        /// <summary>
        /// Gets or sets the display order within its group (or globally if ungrouped).
        /// Lower values appear first.
        /// </summary>
        public int DisplayOrder { get; set; } = 0;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is required.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is required; otherwise, <c>false</c>.
        /// </value>
        public bool IsRequired { get; set; } = false;

        /// <summary>
        /// Gets or sets the parent registration form.
        /// </summary>
        public RegistrationForm RegistrationForm { get; set; } = null!;

        /// <summary>
        /// Gets or sets the reusable form field definition.
        /// </summary>
        public FormField FormField { get; set; } = null!;

        /// <summary>
        /// Gets or sets the optional parent group within the form.
        /// NULL if this field is standalone on the form.
        /// </summary>
        public RegistrationFormGroup? RegistrationFormGroup { get; set; }
    }
}
