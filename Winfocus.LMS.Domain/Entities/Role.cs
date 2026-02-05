namespace Winfocus.LMS.Domain.Entities
{
    using System.Collections.Generic;
    using Winfocus.LMS.Domain.Common;

    /// <summary>
    /// Represents a role within the system.
    /// </summary>
    public class Role : BaseEntity
    {
        /// <summary>
        /// Gets or sets the name of the role.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the collection of user roles associated with this role.
        /// </summary>
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }

}
