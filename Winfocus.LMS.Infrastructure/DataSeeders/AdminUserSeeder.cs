namespace Winfocus.LMS.Infrastructure.DataSeeders
{
    using Microsoft.AspNetCore.Identity;
    using Winfocus.LMS.Domain.Entities;
    using Winfocus.LMS.Infrastructure.Data;

    /// <summary>
    /// Seeds default SuperAdmin and Admin users linked to UAE
    /// so the system is usable immediately after the first migration.
    /// Must run <b>after</b> <see cref="RoleDataSeeder"/>,
    /// <see cref="CountryDataSeeder"/>, and <see cref="CenterDataSeeder"/>.
    /// </summary>
    public static class AdminUserSeeder
    {
        private const string _superAdminEmail = "superadmin@winfocus.com";
        private const string _superAdminPassword = "SuperAdmin@123";

        private const string _adminEmail = "admin@winfocus.com";
        private const string _adminPassword = "Admin@123";

        /// <summary>
        /// Seeds one SuperAdmin and one Admin user linked to UAE
        /// if they do not already exist (matched by email).
        /// </summary>
        /// <param name="db">The application database context.</param>
        public static void Seed(AppDbContext db)
        {
            var passwordHasher = new PasswordHasher<User>();

            var uae = db.Countries
                .FirstOrDefault(c => c.Name.ToLower() == "united arab emirates");

            if (uae == null)
            {
                return;
            }

            var uaeCenter = db.Centres
                .FirstOrDefault(c => c.CountryId == uae.Id && c.Name.ToLower().Contains("ras al khaimah"));

            if (uaeCenter == null)
            {
                uaeCenter = db.Centres.FirstOrDefault(c => c.CountryId == uae.Id);
            }

            SeedUser(
                db,
                passwordHasher,
                roleName: "SuperAdmin",
                username: "superadmin",
                email: _superAdminEmail,
                password: _superAdminPassword,
                country: uae,
                center: uaeCenter);

            SeedUser(
                db,
                passwordHasher,
                roleName: "CountryAdmin",
                username: "admin",
                email: _adminEmail,
                password: _adminPassword,
                country: uae,
                center: uaeCenter);
        }

        /// <summary>
        /// Creates a single user with the specified role if the email
        /// is not already registered.
        /// </summary>
        private static void SeedUser(
            AppDbContext db,
            IPasswordHasher<User> passwordHasher,
            string roleName,
            string username,
            string email,
            string password,
            Country country,
            Center? center)
        {
            if (db.Users.Any(u => u.Email == email))
            {
                return;
            }

            var role = db.Roles.FirstOrDefault(r => r.Name == roleName);
            if (role == null)
            {
                return;
            }

            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = username,
                Email = email,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = Guid.Empty,
                CountryId = country.Id,
                CenterId = center?.Id,
                StaffCategoryId = null,
            };

            user.PasswordHash = passwordHasher.HashPassword(user, password);

            user.UserRoles = new List<UserRole>
            {
                new UserRole
                {
                    UserId = user.Id,
                    RoleId = role.Id,
                },
            };

            db.Users.Add(user);
            db.SaveChanges();
        }
    }
}
