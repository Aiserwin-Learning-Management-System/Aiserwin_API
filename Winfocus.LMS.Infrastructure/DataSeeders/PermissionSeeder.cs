using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Winfocus.LMS.Domain.Entities;
using Winfocus.LMS.Infrastructure.Data;

namespace Winfocus.LMS.Infrastructure.DataSeeders
{
    /// <summary>
    /// Seeds default permissions and role-permission mappings into the database.
    /// This ensures required authorization data exists when the application starts.
    /// </summary>
    public class PermissionSeeder
    {
        /// <summary>
        /// Inserts default permissions and role-permission mappings if they do not already exist.
        /// </summary>
        /// <param name="db">The application database context.</param>
        public static void Seed(AppDbContext db)
        {
            if (!db.Permissions.Any())
            {
                var permissions = new List<Permission>
            {
                new Permission { Id = Guid.NewGuid(), Name = "CanCreateStudent" },
                new Permission { Id = Guid.NewGuid(), Name = "CanUpdateStudent" },
                new Permission { Id = Guid.NewGuid(), Name = "CanDeleteStudent" },
                new Permission { Id = Guid.NewGuid(), Name = "CanViewStudent" },
            };

                db.Permissions.AddRangeAsync(permissions);
                db.SaveChangesAsync();
            }

            SeedRolePermissions(db);
        }

        /// <summary>
        /// Seeds role-permission mappings.
        /// </summary>
        private static void SeedRolePermissions(AppDbContext context)
        {
            var superAdminRole = context.Roles.FirstOrDefault(r => r.Name == "SuperAdmin");
            var adminRole = context.Roles.FirstOrDefault(r => r.Name == "Admin");

            var permissions = context.Permissions.ToList();

            // Assign all permissions to SuperAdmin
            if (superAdminRole != null)
            {
                foreach (var permission in permissions)
                {
                    bool exists = context.RolePermissions
                        .Any(rp => rp.RoleId == superAdminRole.Id && rp.PermissionId == permission.Id);

                    if (!exists)
                    {
                        context.RolePermissions.Add(new RolePermission
                        {
                            RoleId = superAdminRole.Id,
                            PermissionId = permission.Id
                        });
                    }
                }
            }

            // Assign permissions to Admin
            if (adminRole != null)
            {
                foreach (var permission in permissions)
                {
                    bool exists = context.RolePermissions
                        .Any(rp => rp.RoleId == adminRole.Id && rp.PermissionId == permission.Id);

                    if (!exists)
                    {
                        context.RolePermissions.Add(new RolePermission
                        {
                            RoleId = adminRole.Id,
                            PermissionId = permission.Id
                        });
                    }
                }
            }

            context.SaveChanges();
        }
    }
}
