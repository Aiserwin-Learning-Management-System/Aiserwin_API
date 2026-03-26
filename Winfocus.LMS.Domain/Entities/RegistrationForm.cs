namespace Winfocus.LMS.Domain.Entities
{
    using Winfocus.LMS.Domain.Common;

    /// <summary>
    /// Represents a dynamic registration form assigned to a specific staff category.
    /// Only one active form is allowed per staff category at any time.
    /// </summary>
    public class RegistrationForm : BaseEntity
    {
        /// <summary>
        /// Gets or sets the staff category this form is designed for.
        /// </summary>
        public Guid StaffCategoryId { get; set; }

        /// <summary>
        /// Gets or sets the display name of the registration form.
        /// </summary>
        /// <example>Teacher Registration Form 2025.</example>
        public string FormName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the optional description of the form's purpose.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the staff category this form belongs to.
        /// </summary>
        public StaffCategory StaffCategory { get; set; } = null!;

        /// <summary>
        /// Gets or sets the field groups assigned to this form.
        /// </summary>
        public ICollection<RegistrationFormGroup> FormGroups { get; set; } = new List<RegistrationFormGroup>();

        /// <summary>
        /// Gets or sets the individual fields assigned to this form.
        /// </summary>
        public ICollection<RegistrationFormField> FormFields { get; set; } = new List<RegistrationFormField>();

        /// <summary>
        /// Gets or sets the registrations submitted against this form.
        /// </summary>
        public ICollection<StaffRegistration> Registrations { get; set; } = new List<StaffRegistration>();
    }
}
