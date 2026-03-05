namespace Winfocus.LMS.Domain.Entities
{
    using Winfocus.LMS.Domain.Common;

    /// <summary>
    /// Represents a user entity within the LMS system.
    /// </summary>
    public class User : BaseEntity
    {
        /// <summary>
        /// Gets or sets the username of the user.
        /// </summary>
        public string Username { get; set; } = null!;

        /// <summary>
        /// Gets or sets the email address of the user.
        /// </summary>
        public string Email { get; set; } = null!;

        /// <summary>
        /// Gets or sets the hashed password of the user.
        /// </summary>
        public string? PasswordHash { get; set; } = null!;

        /// <summary>
        /// Gets or sets the identifier of the associated country.
        /// </summary>
        public Guid? CountryId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the associated country.
        /// </summary>
        public Country? Country { get; set; } = null!;

        /// <summary>
        /// Gets or sets the identifier of the associated Center.
        /// </summary>
        public Guid? CenterId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the associated Center.
        /// </summary>
        public Center? Center { get; set; } = null!;

        /// <summary>
        /// Gets or sets the identifier of the Stafftype.
        /// </summary>
        public Guid? StaffTypeId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the StaffType.
        /// </summary>
        public StaffType? StaffType { get; set; } = null!;

        /// <summary>
        /// Gets or sets the collection of UserRole associated with the user.
        /// </summary>
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}
