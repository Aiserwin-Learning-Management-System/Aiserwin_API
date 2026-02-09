namespace Winfocus.LMS.Application.Services
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Logging;
    using System.Data;
    using Winfocus.LMS.Application.DTOs;
    using Winfocus.LMS.Application.DTOs.Auth;
    using Winfocus.LMS.Application.Interfaces;
    using Winfocus.LMS.Domain.Entities;

    /// <summary>
    /// Handles authentication business logic.
    /// </summary>
    public sealed class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly ITokenService _tokenService;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly ILogger<AuthService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthService"/> class.
        /// </summary>
        /// <param name="userRepository">The user repository.</param>
        /// <param name="roleRepository">The role repository.</param>
        /// <param name="tokenService">The token service.</param>
        /// <param name="passwordHasher">The password hasher.</param>
        /// <param name="logger">The logger.</param>
        public AuthService(
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            ITokenService tokenService,
            IPasswordHasher<User> passwordHasher,
            ILogger<AuthService> logger)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _tokenService = tokenService;
            _passwordHasher = passwordHasher;
            _logger = logger;
        }

        /// <summary>
        /// Registers the asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>AuthResponseDto.</returns>
        /// <exception cref="InvalidOperationException">
        /// Username already exists.
        /// or
        /// Invalid role.
        /// </exception>
        public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request)
        {
            _logger.LogInformation(
                "Registration attempt for Username: {Username}",
                request.username);

            var existingUser = await _userRepository.GetByUsernameAsync(request.username);

            if (existingUser != null && existingUser.IsActive)
            {
                _logger.LogWarning(
                    "Registration failed. Invalid username or inactive user: {Username}",
                    request.username);

                throw new InvalidOperationException("Invalid username or password.");
            }

            var user = new User
            {
                Username = request.username,
                Email = request.email,
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
            };

            user.PasswordHash = _passwordHasher.HashPassword(user, request.password);

            // If no roles provided, default to "Student"
            var roleNames = (request.roleNames == null || !request.roleNames.Any())
                            ? new List<string> { "Student" }
                            : request.roleNames;

            var roles = new List<string>();

            // Loop through all requested roles
            foreach (var roleName in roleNames)
            {
                var role = await _roleRepository.GetByNameAsync(roleName)
                           ?? throw new InvalidOperationException($"Invalid role: {roleName}");

                user.UserRoles.Add(new UserRole
                {
                    RoleId = role.Id,
                    UserId = user.Id,
                    Role = role,
                });

                roles.Add(roleName);
            }

            await _userRepository.AddAsync(user);

            // Generate token with all roles
            var token = _tokenService.GenerateToken(user, roles);

            _logger.LogInformation(
                   "User registered successfully. Username: {Username}, UserId: {UserId}, Roles: {Roles}",
                   user.Username,
                   user.Id,
                   string.Join(",", roles));

            return new AuthResponseDto(
                token,
                user.Id,
                user.Username,
                user.Email,
                roles);
        }

        /// <summary>
        /// Logins the asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>AuthResponseDto.</returns>
        /// <exception cref="UnauthorizedAccessException">Invalid credentials.</exception>
        public async Task<AuthResponseDto> LoginAsync(LoginRequestDto request)
        {
            _logger.LogInformation(
                "Login attempt for username: {Username}",
                request.username);

            var user = await _userRepository.GetByUsernameAsync(request.username)
                       ?? throw new UnauthorizedAccessException("Invalid credentials.");

            if (user == null || !user.IsActive)
            {
                _logger.LogWarning(
                    "Login failed for username {Username}: user not found or inactive",
                    request.username);

                throw new UnauthorizedAccessException("Invalid username or password");
            }

            var verificationResult = _passwordHasher.VerifyHashedPassword(
                user,
                user.PasswordHash,
                request.password);

            if (verificationResult == PasswordVerificationResult.Failed)
            {
                _logger.LogWarning(
                    "Login failed for username {Username}: invalid password",
                    request.username);
                throw new UnauthorizedAccessException("Invalid credentials.");
            }

            var roles = user.UserRoles
                            .Where(ur => ur.Role != null)
                            .Select(ur => ur.Role!.Name)
                            .ToList();

            var token = _tokenService.GenerateToken(user, roles);

            _logger.LogInformation(
                "Login successful for username {Username} (UserId: {UserId})",
                user.Username,
                user.Id);

            return new AuthResponseDto(
                token,
                user.Id,
                user.Username,
                user.Email,
                roles);
        }
    }
}
