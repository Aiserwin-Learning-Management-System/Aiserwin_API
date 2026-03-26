using System;
using System.Collections.Generic;
using System.Text;

namespace Winfocus.LMS.Domain.Entities
{
    /// <summary>
    /// Represents the relationship between roles and permissions.
    /// This table defines which permissions are assigned to a specific role.
    /// </summary>
    public class RolePermission
    {
        /// <summary>
        /// Gets or sets identifier of the role associated with the permission.
        /// </summary>
        public Guid RoleId { get; set; }

        /// <summary>
        /// Gets or sets identifier of the permission assigned to the role.
        /// </summary>
        public Guid PermissionId { get; set; }

        /// <summary>
        /// Gets or sets navigation property for the role.
        /// </summary>
        public Role Role { get; set; }

        /// <summary>
        /// Gets or sets navigation property for the permission.
        /// </summary>
        public Permission Permission { get; set; }
    }
}
