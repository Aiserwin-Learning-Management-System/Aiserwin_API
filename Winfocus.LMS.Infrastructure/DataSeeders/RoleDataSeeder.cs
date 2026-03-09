namespace Winfocus.LMS.Infrastructure.DataSeeders
{
    using Winfocus.LMS.Domain.Entities;
    using Winfocus.LMS.Infrastructure.Data;

    /// <summary>
    /// Provides methods to seed role data into the database.
    /// </summary>
    public static class RoleDataSeeder
    {
        /// <summary>
        /// Seeds the <see cref="Role"/> entities into the database if none exist.
        /// </summary>
        /// <param name="db">The application database context.</param>
        public static void Seed(AppDbContext db)
        {
            if (!db.Roles.Any())
            {
                db.Roles.AddRange(
                new Role { Name = "SuperAdmin", CreatedAt = DateTime.UtcNow, IsActive = true },
                new Role { Name = "CountryAdmin", CreatedAt = DateTime.UtcNow, IsActive = true },
                new Role { Name = "CenterAdmin", CreatedAt = DateTime.UtcNow, IsActive = true },
                new Role { Name = "Staff", CreatedAt = DateTime.UtcNow, IsActive = true });
                db.SaveChanges();
            }
        }
    }
}
