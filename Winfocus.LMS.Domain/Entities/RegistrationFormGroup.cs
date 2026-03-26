namespace Winfocus.LMS.Domain.Entities
{
    /// <summary>
    /// Links a <see cref="FieldGroup"/> to a <see cref="RegistrationForm"/>,
    /// defining which field groups appear on the form and in what order.
    /// Lightweight junction entity — no audit fields needed.
    /// </summary>
    public class RegistrationFormGroup
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
        /// Gets or sets the field group being linked to the form.
        /// </summary>
        public Guid FieldGroupId { get; set; }

        /// <summary>
        /// Gets or sets the display order of this group within the form.
        /// Lower values appear first.
        /// </summary>
        public int DisplayOrder { get; set; } = 0;

        /// <summary>
        /// Gets or sets the parent registration form.
        /// </summary>
        public RegistrationForm RegistrationForm { get; set; } = null!;

        /// <summary>
        /// Gets or sets the linked field group definition.
        /// </summary>
        public FieldGroup FieldGroup { get; set; } = null!;

        /// <summary>
        /// Gets or sets the form fields assigned to this group within the form.
        /// </summary>
        public ICollection<RegistrationFormField> FormFields { get; set; } = new List<RegistrationFormField>();
    }
}
