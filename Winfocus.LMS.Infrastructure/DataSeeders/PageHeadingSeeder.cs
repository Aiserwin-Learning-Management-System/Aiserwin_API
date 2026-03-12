namespace Winfocus.LMS.Infrastructure.DataSeeders
{
    using Microsoft.EntityFrameworkCore;
    using Winfocus.LMS.Domain.Entities;
    using Winfocus.LMS.Infrastructure.Data;

    /// <summary>
    /// Seeds default page headings for admin pages.
    /// Must run AFTER <see cref="AdminUserSeeder"/> so SuperAdmin exists.
    /// </summary>
    public static class PageHeadingSeeder
    {
        /// <summary>
        /// Seeds page headings if they don't already exist.
        /// Uses SuperAdmin user as CreatedBy.
        /// </summary>
        /// <param name="db">The application database context.</param>
        public static void Seed(AppDbContext db)
        {
            var superAdmin = db.Users
                .FirstOrDefault(u => u.Username == "superadmin");

            if (superAdmin == null)
            {
                return;
            }

            var createdBy = superAdmin.Id;

            SeedPageHeading(
                db,
                createdBy,
                pageKey: "create_user",
                mainHeading: "User Management",
                subHeading: "Create User",
                moduleName: "User Management");

            SeedPageHeading(
                db,
                createdBy,
                pageKey: "content_management",
                mainHeading: "User Management",
                subHeading: "Content Management",
                moduleName: "User Management");

            SeedPageHeading(
                db,
                createdBy,
                pageKey: "staff_registration_form",
                mainHeading: "User Management",
                subHeading: "Staff Registration Form",
                moduleName: "User Management");

            db.SaveChanges();
        }

        /// <summary>
        /// Seeds a single page heading if PageKey doesn't already exist.
        /// </summary>
        private static void SeedPageHeading(
            AppDbContext db,
            Guid createdBy,
            string pageKey,
            string mainHeading,
            string subHeading,
            string moduleName)
        {
            var exists = db.PageHeadings
                .IgnoreQueryFilters()
                .Any(ph => ph.PageKey == pageKey);

            if (exists)
            {
                return;
            }

            db.PageHeadings.Add(new PageHeading
            {
                PageKey = pageKey,
                MainHeading = mainHeading,
                SubHeading = subHeading,
                ModuleName = moduleName,
                IsActive = true,
                IsDeleted = false,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = createdBy,
            });
        }
    }
}
