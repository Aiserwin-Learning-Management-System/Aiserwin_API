namespace Winfocus.LMS.Application.Tests.Services
{
    using FluentAssertions;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Logging.Abstractions;
    using Moq;
    using Winfocus.LMS.Application.Common.Exceptions;
    using Winfocus.LMS.Application.DTOs.Auth;
    using Winfocus.LMS.Application.Interfaces;
    using Winfocus.LMS.Application.Services;
    using Winfocus.LMS.Domain.Entities;

    /// <summary>
    /// Unit tests for <see cref="AuthService"/>.
    /// </summary>
    public sealed class AuthServiceTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IRoleRepository> _roleRepositoryMock;
        private readonly Mock<ITokenService> _tokenServiceMock;
        private readonly Mock<IEmailService> _emailServiceMock;
        private readonly Mock<IUserActivationTokenRepository> _userActivationTokenRepositoryMock;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly AuthService _authService;
        private readonly Mock<IUsernameGeneratorService> _usernameGeneratorService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthServiceTests"/> class.
        /// </summary>
        public AuthServiceTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _roleRepositoryMock = new Mock<IRoleRepository>();
            _tokenServiceMock = new Mock<ITokenService>();
            _emailServiceMock = new Mock<IEmailService>();
            _userActivationTokenRepositoryMock = new Mock<IUserActivationTokenRepository>();
            _passwordHasher = new PasswordHasher<User>();
            _usernameGeneratorService = new Mock<IUsernameGeneratorService>();

            _authService = new AuthService(
                _userRepositoryMock.Object,
                _roleRepositoryMock.Object,
                _tokenServiceMock.Object,
                _passwordHasher,
                NullLogger<AuthService>.Instance,
                _userActivationTokenRepositoryMock.Object,
                _emailServiceMock.Object,
                _usernameGeneratorService.Object);
        }

        /// <summary>
        /// Registers the asynchronous with valid request returns authentication response.
        /// </summary>
        /// <returns>AuthResponseDto.</returns>
        [Fact]
        public async Task RegisterAsync_WithValidRequest_ReturnsAuthResponse()
        {
            var request = new RegisterRequestDto("testuser", "test@winfocus.com", new List<string> { "Student" });

            _userRepositoryMock
                .Setup(r => r.GetByNamesAsync(It.IsAny<IReadOnlyList<string>>()))
                .ReturnsAsync(new List<Role> { new Role { Name = "Student" } });

            _usernameGeneratorService
                .Setup(u => u.GenerateAsync("testuser", It.IsAny<int>()))
                .ReturnsAsync("testuser");

            var result = await _authService.RegisterAsync(request);

            Assert.NotNull(result);
            Assert.Equal("testuser", result.username);
            Assert.Equal("test@winfocus.com", result.email);
            Assert.Contains("Student", result.roles);
        }

        /// <summary>
        /// Logins the asynchronous with valid credentials returns authentication response.
        /// </summary>
        /// <returns>AuthResponseDto.</returns>
        [Fact]
        public async Task LoginAsync_WithValidCredentials_ReturnsAuthResponse()
        {
            // Arrange
            var password = "Password@123";

            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = "testuser",
                Email = "test@winfocus.com",
                IsActive = true,
            };

            user.PasswordHash = _passwordHasher.HashPassword(user, password);

            user.UserRoles.Add(new UserRole
            {
                Role = new Role { Name = "Student" },
            });

            var request = new LoginRequestDto(
                username: "testuser",
                password: password
            );

            _userRepositoryMock
                .Setup(r => r.GetByUsernameAsync("testuser"))
                .ReturnsAsync(user);

            _tokenServiceMock
                .Setup(t => t.GenerateToken(user, It.IsAny<List<string>>()))
                .Returns("mock-jwt-token");

            // Act
            var result = await _authService.LoginAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("testuser", result.username);
            Assert.Equal("mock-jwt-token", result.accessToken);
            Assert.Contains("Student", result.roles);
        }

        /// <summary>
        /// Registers with invalid role throws exception.
        /// </summary>
        /// <returns>Task.</returns>
        [Fact]
        public async Task RegisterAsync_WithInvalidRole_ThrowsInvalidOperationException()
        {
            // Arrange
            var request = new RegisterRequestDto(
                username: "testuser",
                email: "test@winfocus.com",
                roleNames: new List<string> { "InvalidRole" }
            );

            _userRepositoryMock
                .Setup(r => r.GetByUsernameAsync("testuser"))
                .ReturnsAsync((User?)null);

            _userRepositoryMock .Setup(r => r.GetByNamesAsync(It.IsAny<IReadOnlyList<string>>())) .ReturnsAsync(new List<Role>());

            // Act
            Func<Task> act = async () => await _authService.RegisterAsync(request);

            // Assert
            await act.Should().ThrowAsync<CustomException>().WithMessage("Invalid roles: InvalidRole");
        }

        /// <summary>
        /// Login with invalid password throws unauthorized exception.
        /// </summary>
        /// <returns>Task.</returns>
        [Fact]
        public async Task LoginAsync_WithInvalidPassword_ThrowsUnauthorizedAccessException()
        {
            // Arrange
            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = "testuser",
                Email = "test@winfocus.com",
                IsActive = true,
            };

            user.PasswordHash = _passwordHasher.HashPassword(user, "CorrectPassword@123");

            var request = new LoginRequestDto(
                username: "testuser",
                password: "WrongPassword@123"
            );

            _userRepositoryMock
                .Setup(r => r.GetByUsernameAsync("testuser"))
                .ReturnsAsync(user);

            // Act
            Func<Task> act = async () => await _authService.LoginAsync(request);

            // Assert
            await act
                .Should()
                .ThrowAsync<CustomException>()
                .WithMessage("Invalid credentials.");
        }

        /// <summary>
        /// Login with non-existing user throws unauthorized exception.
        /// </summary>
        /// <returns>Task.</returns>
        [Fact]
        public async Task LoginAsync_WithNonExistingUser_ThrowsUnauthorizedAccessException()
        {
            // Arrange
            var request = new LoginRequestDto(
                username: "unknown",
                password: "Password@123"
            );

            _userRepositoryMock
                .Setup(r => r.GetByUsernameAsync("unknown"))
                .ReturnsAsync((User?)null);

            // Act
            Func<Task> act = async () => await _authService.LoginAsync(request);

            // Assert
            await act
                .Should()
                .ThrowAsync<CustomException>()
                .WithMessage("Invalid credentials.");
        }

        /// <summary>
        /// Register with existing active user throws exception.
        /// </summary>
        /// <returns>Task.</returns>
        [Fact]
        public async Task RegisterAsync_WithExistingUser_ThrowsInvalidOperationException()
        {
            // Arrange
            var existingUser = new User
            {
                Username = "testuser",
                IsActive = true,
            };

            var request = new RegisterRequestDto(
                username: "testuser",
                email: "test@winfocus.com",
                roleNames: null
            );

            _userRepositoryMock
                .Setup(r => r.EmailExistsAsync("test@winfocus.com"))
                .ReturnsAsync(true);

            // Act
            Func<Task> act = async () => await _authService.RegisterAsync(request);

            // Assert
            await act.Should().ThrowAsync<CustomException>()
                .WithMessage("Email address is already registered.");
        }

        /// <summary>
        /// Register without roles assigns default Student role.
        /// </summary>
        /// <returns>Task.</returns>
        [Fact]
        public async Task RegisterAsync_WithoutRoles_AssignsStudentRole()
        {
            // Arrange
            var request = new RegisterRequestDto(
                username: "student1",
                email: "student@winfocus.com",
                roleNames: null
            );

            _userRepositoryMock
                .Setup(r => r.GetByUsernameAsync("student1"))
                .ReturnsAsync((User?)null);

            _userRepositoryMock
                .Setup(r => r.GetByNamesAsync(It.Is<IReadOnlyList<string>>(roles => roles.Contains("Student"))))
                .ReturnsAsync(new List<Role> { new Role { Name = "Student" } });


            _tokenServiceMock
                .Setup(t => t.GenerateToken(It.IsAny<User>(), It.IsAny<List<string>>()))
                .Returns("mock-jwt-token");

            // Act
            var result = await _authService.RegisterAsync(request);

            // Assert
            result.roles.Should().ContainSingle()
                .Which.Should().Be("Student");
        }

        /// <summary>
        /// Register passes all roles to token generation.
        /// </summary>
        /// <returns>Task.</returns>
        [Fact]
        public async Task RegisterAsync_WithMultipleRoles_PassesAllRolesToToken()
        {
            // Arrange
            var request = new RegisterRequestDto(
                "adminuser",
                "admin@winfocus.com",
                new List<string> { "Admin", "Teacher" }
            );

            _userRepositoryMock
                .Setup(r => r.GetByNamesAsync(It.IsAny<IReadOnlyList<string>>()))
                .ReturnsAsync(new List<Role>
                {
                    new Role { Name = "Admin" },
                    new Role { Name = "Teacher" }
                });

            _usernameGeneratorService
                .Setup(u => u.GenerateAsync("adminuser", It.IsAny<int>()))
                .ReturnsAsync("adminuser");

            // Act
            var result = await _authService.RegisterAsync(request);

            // Assert
            result.roles.Should().BeEquivalentTo("Admin", "Teacher");
        }

    }
}
