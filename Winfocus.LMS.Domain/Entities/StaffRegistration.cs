namespace Winfocus.LMS.Domain.Entities
{
    using Winfocus.LMS.Domain.Enums;

    /// <summary>
    /// Represents a single staff registration submission against a registration form.
    /// Tracks the submission lifecycle from draft to approval/rejection.
    /// </summary>
    public class StaffRegistration
    {
        /// <summary>
        /// Gets or sets the unique identifier.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the registration form this submission is for.
        /// </summary>
        public Guid FormId { get; set; }

        /// <summary>
        /// Gets or sets the staff category of the registrant.
        /// </summary>
        public Guid StaffCategoryId { get; set; }

        /// <summary>
        /// Gets or sets the current status of the registration.
        /// </summary>
        public RegistrationStatus Status { get; set; } = RegistrationStatus.Draft;

        /// <summary>
        /// Gets or sets optional remarks (e.g., rejection reason).
        /// </summary>
        public string? Remarks { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the registration was submitted.
        /// NULL if still in draft.
        /// </summary>
        public DateTime? SubmittedAt { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the registration was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the user who created the registration.
        /// </summary>
        public Guid CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the registration form this submission belongs to.
        /// </summary>
        public RegistrationForm RegistrationForm { get; set; } = null!;

        /// <summary>
        /// Gets or sets the staff category of the registrant.
        /// </summary>
        public StaffCategory StaffCategory { get; set; } = null!;

        /// <summary>
        /// Gets or sets the submitted field values.
        /// </summary>
        public ICollection<StaffRegistrationValue> Values { get; set; } = new List<StaffRegistrationValue>();
    }
}
