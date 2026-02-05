namespace Winfocus.LMS.Domain.Entities
{
    using Winfocus.LMS.Domain.Common;

    /// <summary>
    /// Represents the association between a user and a role.
    /// </summary>
    public class UserRole
    {
        /// <summary>
        /// Gets or sets the user ID.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets the user associated with this user role.
        /// </summary>
        public User User { get; set; } = null!;

        /// <summary>
        /// Gets or sets the role ID.
        /// </summary>
        public Guid RoleId { get; set; }

        /// <summary>
        /// Gets or sets the role associated with this user role.
        /// </summary>
        public Role Role { get; set; } = null!;
    }

}
