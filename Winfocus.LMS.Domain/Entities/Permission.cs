using System;
using System.Collections.Generic;
using System.Text;

namespace Winfocus.LMS.Domain.Entities
{
    /// <summary>
    /// Represents a system permission that defines an action a user is allowed to perform.
    /// Example permissions include creating, updating, or deleting students.
    /// </summary>
    public class Permission
    {
        /// <summary>
        /// Gets or sets unique identifier for the permission.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets name of the permission used in authorization policies.
        /// Example: CanCreateStudent, CanUpdateStudent.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Navigation property linking the permission to roles that are allowed to use it.
        /// </summary>
        public ICollection<RolePermission> RolePermissions { get; set; }
    }
}
