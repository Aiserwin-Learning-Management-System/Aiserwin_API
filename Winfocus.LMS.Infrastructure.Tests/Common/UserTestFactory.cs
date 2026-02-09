namespace Winfocus.LMS.Infrastructure.Tests.Common
{
    using Microsoft.AspNetCore.Identity;
    using Winfocus.LMS.Domain.Entities;

    /// <summary>
    /// Factory for creating test users.
    /// </summary>
    internal static class UserTestFactory
    {
        private static readonly PasswordHasher<User> _hasher = new ();

        /// <summary>
        /// Creates the active user.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="email">The email.</param>
        /// <param name="password">The password.</param>
        /// <returns>The active user.</returns>
        public static User CreateActiveUser(
            string username = "testuser",
            string email = "test@winfocus.com",
            string password = "Password@123")
        {
            var user = new User
            {
                Username = username,
                Email = email,
                IsActive = true,
            };

            user.PasswordHash = _hasher.HashPassword(user, password);
            return user;
        }

        /// <summary>
        /// Creates the inactive user.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="email">The email.</param>
        /// <returns>The inactive user.</returns>
        public static User CreateInactiveUser(
            string username = "inactive",
            string email = "inactive@winfocus.com")
        {
            var user = new User
            {
                Username = username,
                Email = email,
                IsActive = false,
            };

            user.PasswordHash = _hasher.HashPassword(user, "Password@123");
            return user;
        }
    }
}
