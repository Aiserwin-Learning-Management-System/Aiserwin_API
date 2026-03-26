namespace Winfocus.LMS.Infrastructure.Tests.Repositories
{
    using FluentAssertions;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging.Abstractions;
    using Winfocus.LMS.Domain.Entities;
    using Winfocus.LMS.Infrastructure.Repositories;
    using Winfocus.LMS.Infrastructure.Tests.Common;

    /// <summary>
    /// Tests for <see cref="UserRepository"/>.
    /// </summary>
    public sealed class UserRepositoryTests : DbContextTestBase
    {
        /// <summary>
        /// Adds the asynchronous persists user.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Fact]
        public async Task AddAsync_Persists_User()
        {
            using var context = CreateDbContext();
            var repository = new UserRepository(context, NullLogger<UserRepository>.Instance);

            var user = UserTestFactory.CreateActiveUser();

            await repository.AddAsync(user);

            context.Users.Should().ContainSingle();
            context.Users.First().Username.Should().Be("testuser");
        }

        /// <summary>
        /// Usernames the exists asynchronous returns true when user exists.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Fact]
        public async Task UsernameExistsAsync_ReturnsTrue_WhenUserExists()
        {
            using var context = CreateDbContext();
            var repository = new UserRepository(context, NullLogger<UserRepository>.Instance);

            context.Users.Add(UserTestFactory.CreateActiveUser(username: "existing"));

            await context.SaveChangesAsync();

            var result = await repository.UsernameExistsAsync("existing");

            result.Should().BeTrue();
        }

        /// <summary>
        /// Usernames the exists asynchronous returns false when user does not exist.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Fact]
        public async Task UsernameExistsAsync_ReturnsFalse_WhenUserDoesNotExist()
        {
            using var context = CreateDbContext();
            var repository = new UserRepository(context, NullLogger<UserRepository>.Instance);

            var result = await repository.UsernameExistsAsync("missing");

            result.Should().BeFalse();
        }

        /// <summary>
        /// Gets the by username asynchronous returns active user with roles.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Fact]
        public async Task GetByUsernameAsync_Returns_Active_User_With_Roles()
        {
            // Arrange
            using var context = CreateDbContext();
            var repository = new UserRepository(context, NullLogger<UserRepository>.Instance);

            var role = new Role
            {
                Name = "Student",
            };

            var user = UserTestFactory.CreateActiveUser(
                username: "activeuser",
                email: "active@winfocus.com"
            );

            // Create relationship ONLY via navigation properties
            var userRole = new UserRole
            {
                User = user,
                Role = role,
            };

            user.UserRoles.Add(userRole);
            role.UserRoles.Add(userRole);

            context.Users.Add(user);   // EF will cascade add Role & UserRole
            await context.SaveChangesAsync();

            // Act
            var result = await repository.GetByUsernameAsync("activeuser");

            // Assert
            result.Should().NotBeNull();
            result!.UserRoles.Should().HaveCount(1);
            result.UserRoles.First().Role.Name.Should().Be("Student");
        }

        /// <summary>
        /// Gets the by username asynchronous returns null for inactive user.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Fact]
        public async Task GetByUsernameAsync_ReturnsNull_For_Inactive_User()
        {
            using var context = CreateDbContext();
            var repository = new UserRepository(context, NullLogger<UserRepository>.Instance);

            context.Users.Add(UserTestFactory.CreateInactiveUser("inactive"));

            await context.SaveChangesAsync();

            var result = await repository.GetByUsernameAsync("inactive");

            result.Should().BeNull();
        }
    }
}
