namespace Winfocus.LMS.Application.Services
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Logging;
    using System.Data;
    using Winfocus.LMS.Application.Common.Exceptions;
    using Winfocus.LMS.Application.DTOs;
    using Winfocus.LMS.Application.DTOs.Auth;
    using Winfocus.LMS.Application.Interfaces;
    using Winfocus.LMS.Domain.Entities;
    using static Winfocus.LMS.Application.Common.Helpers.ValidationHelper;

    /// <summary>
    /// Handles authentication business logic.
    /// </summary>
    public sealed class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly ITokenService _tokenService;
        private readonly IUserActivationTokenRepository _activationRepository;
        private readonly IEmailService _emailService;
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
        /// <param name="activationRepository">The user activation token repository.</param>
        /// <param name="emailService">The email service.</param>
        public AuthService(
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            ITokenService tokenService,
            IPasswordHasher<User> passwordHasher,
            ILogger<AuthService> logger,
            IUserActivationTokenRepository activationRepository,
            IEmailService emailService)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _tokenService = tokenService;
            _passwordHasher = passwordHasher;
            _logger = logger;
            _activationRepository = activationRepository;
            _emailService = emailService;
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
            try
            {
                _logger.LogInformation("Registration attempt for {Username}", request.username);

                var validationErrors = new Dictionary<string, string[]>();

                if (string.IsNullOrWhiteSpace(request.username))
                {
                    AddError(validationErrors, "Username", "Username is required.");
                }

                if (string.IsNullOrWhiteSpace(request.email))
                {
                    AddError(validationErrors, "Email", "Email is required.");
                }

                if (validationErrors.Any())
                {
                    throw new ValidationException("Validation failed", validationErrors);
                }

                if (await _userRepository.UsernameExistsAsync(request.username))
                {
                    throw new CustomException(
                        "Username already exists.",
                        StatusCodes.Status409Conflict,
                        "USERNAME_ALREADY_EXISTS");
                }

                List<Role> roles;

                if (request.roleNames != null && request.roleNames.Any())
                {
                    var distinctRoleNames = request.roleNames
                        .Where(r => !string.IsNullOrWhiteSpace(r))
                        .Select(r => r.Trim())
                        .Distinct(StringComparer.OrdinalIgnoreCase)
                        .ToList();

                    roles = (await _userRepository.GetByNamesAsync(distinctRoleNames)).ToList();

                    var invalidRoles = distinctRoleNames
                        .Except(roles.Select(r => r.Name), StringComparer.OrdinalIgnoreCase)
                        .ToList();

                    if (invalidRoles.Any())
                    {
                        throw new CustomException(
                            $"Invalid roles: {string.Join(", ", invalidRoles)}",
                            StatusCodes.Status400BadRequest,
                            "INVALID_ROLE");
                    }
                }
                else
                {
                    roles = (await _userRepository.GetByNamesAsync(new[] { "Student" })).ToList();

                    if (!roles.Any())
                    {
                        throw new CustomException(
                            "Default role 'Student' not configured in system.",
                            StatusCodes.Status500InternalServerError,
                            "DEFAULT_ROLE_NOT_FOUND");
                    }
                }

                var user = new User
                {
                    Id = Guid.NewGuid(),
                    Username = request.username,
                    Email = request.email,
                    CreatedAt = DateTime.UtcNow,
                    IsActive = false,
                };

                user.UserRoles = roles.Select(role => new UserRole
                {
                    UserId = user.Id,
                    RoleId = role.Id,
                }).ToList();

                await _userRepository.AddAsync(user);

                var token = Guid.NewGuid().ToString("N");

                var activationToken = new UserActivationToken
                {
                    Id = Guid.NewGuid(),
                    UserId = user.Id,
                    Token = token,
                    ExpiryDate = DateTime.UtcNow.AddHours(24),
                    IsUsed = false,
                    CreatedAt = DateTime.UtcNow,
                };

                await _activationRepository.AddAsync(activationToken);

                await _emailService.SendActivationEmailAsync(user.Email, user.Username, token);

                _logger.LogInformation("Activation token generated for {UserId}", user.Id);

                return new AuthResponseDto(
                    string.Empty,
                    user.Id,
                    user.Username,
                    user.Email,
                    roles.Select(r => r.Name).ToList());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Registration failed for {Username}", request.username);
                throw;
            }
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

            var user = await _userRepository.GetByUsernameAsync(request.username);

            if (user == null)
            {
                throw new CustomException(
                    "Invalid credentials.",
                    StatusCodes.Status401Unauthorized,
                    "INVALID_CREDENTIALS");
            }

            if (!user.IsActive)
            {
                throw new CustomException(
                    "Account not activated.",
                    StatusCodes.Status403Forbidden,
                    "ACCOUNT_NOT_ACTIVE");
            }

            if (user.PasswordHash == null)
            {
                throw new CustomException(
                    "Account not activated.",
                    StatusCodes.Status403Forbidden,
                    "ACCOUNT_NOT_ACTIVE");
            }

            var result = _passwordHasher.VerifyHashedPassword(
                                                                user,
                                                                user.PasswordHash,
                                                                request.password);

            if (result == PasswordVerificationResult.Failed)
            {
                throw new CustomException(
                    "Invalid credentials.",
                    StatusCodes.Status401Unauthorized,
                    "INVALID_CREDENTIALS");
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

        /// <summary>
        /// Sets the password asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <exception cref="InvalidOperationException">
        /// Invalid token.
        /// or
        /// Token already used.
        /// or
        /// Token expired.
        /// </exception>
        /// <exception cref="KeyNotFoundException">User not found.</exception>
        /// <returns>Task.</returns>
        public async Task SetPasswordAsync(SetPasswordDto request)
        {
            try
            {
                _logger.LogInformation("Password setup attempt for token {Token}", request.token);

                var tokenEntity = await _activationRepository.GetByTokenAsync(request.token)
                                    ?? throw new CustomException(
                                        "Invalid token.",
                                        StatusCodes.Status400BadRequest,
                                        "INVALID_TOKEN");

                if (tokenEntity.IsUsed)
                {
                    throw new CustomException(
                        "Token already used.",
                        StatusCodes.Status400BadRequest,
                        "TOKEN_ALREADY_USED");
                }

                if (tokenEntity.ExpiryDate < DateTime.UtcNow)
                {
                    throw new CustomException(
                        "Token expired.",
                        StatusCodes.Status400BadRequest,
                        "TOKEN_EXPIRED");
                }

                var user = await _userRepository.GetByIdAsync(tokenEntity.UserId)
                    ?? throw new CustomException(
                        "User not found.",
                        StatusCodes.Status404NotFound,
                        "USER_NOT_FOUND");

                user.PasswordHash = _passwordHasher.HashPassword(user, request.password);
                user.IsActive = true;

                tokenEntity.IsUsed = true;

                await _userRepository.UpdateAsync(user);

                _logger.LogInformation("Password set successfully for user {UserId}", user.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Password setup failed for token {Token}", request.token);
                throw;
            }
        }
    }
}
