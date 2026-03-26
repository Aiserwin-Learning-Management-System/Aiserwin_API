namespace Winfocus.LMS.Application.Services
{
    using System.Data;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Winfocus.LMS.Application.Common.Exceptions;
    using Winfocus.LMS.Application.DTOs;
    using Winfocus.LMS.Application.DTOs.Auth;
    using Winfocus.LMS.Application.DTOs.LoginLog;
    using Winfocus.LMS.Application.Interfaces;
    using Winfocus.LMS.Domain.Entities;
    using Winfocus.LMS.Domain.Enums;
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
        private readonly IUsernameGeneratorService _usernameGeneratorService;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly ILogger<AuthService> _logger;
        private readonly IUserLoginLogService _loginLogService;
        private readonly IUserSessionService _userSessionService;
        private readonly IConfiguration _configuration;
        private readonly IStudentRepository _studentRepository;
        private readonly IStudentAcademicdetailsRepository _studacademicrepository;
        private readonly IFileStorageService _fileStorageService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthService" /> class.
        /// </summary>
        /// <param name="userRepository">The user repository.</param>
        /// <param name="roleRepository">The role repository.</param>
        /// <param name="tokenService">The token service.</param>
        /// <param name="passwordHasher">The password hasher.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="activationRepository">The user activation token repository.</param>
        /// <param name="emailService">The email service.</param>
        /// <param name="usernameGeneratorService">The username generator service.</param>
        /// <param name="loginLogService">The user login log service.</param>
        /// <param name="userSessionService">The user session service.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="studentRepository">The studentRepository.</param>
        /// <param name="studacademicrepository">The _studacademicrepository.</param>
        /// <param name="fileStorageService">The file storage service.</param>
        public AuthService(
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            ITokenService tokenService,
            IPasswordHasher<User> passwordHasher,
            ILogger<AuthService> logger,
            IUserActivationTokenRepository activationRepository,
            IEmailService emailService,
            IUsernameGeneratorService usernameGeneratorService,
            IUserLoginLogService loginLogService,
            IUserSessionService userSessionService,
            IConfiguration configuration,
            IStudentRepository studentRepository,
            IStudentAcademicdetailsRepository studacademicrepository,
            IFileStorageService fileStorageService)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _tokenService = tokenService;
            _passwordHasher = passwordHasher;
            _logger = logger;
            _activationRepository = activationRepository;
            _emailService = emailService;
            _usernameGeneratorService = usernameGeneratorService;
            _loginLogService = loginLogService;
            _userSessionService = userSessionService;
            _configuration = configuration;
            _studentRepository = studentRepository;
            _studacademicrepository = studacademicrepository;
            _fileStorageService = fileStorageService;
        }

        /// <summary>
        /// Registers the asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>AuthResponseDto.</returns>
        public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request)
        {
            try
            {
                _logger.LogInformation(
                    "Registration attempt — FullName: '{FullName}', Email: '{Email}'",
                    request.username,
                    request.email);

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

                if (await _userRepository.EmailExistsAsync(request.email))
                {
                    _logger.LogWarning(
                        "Registration rejected: email '{Email}' already exists.",
                        request.email);
                    throw new CustomException(
                        "Email address is already registered.",
                        StatusCodes.Status409Conflict,
                        "EMAIL_ALREADY_EXISTS");
                }

                var generatedUsername = await _usernameGeneratorService
                    .GenerateAsync(request.username, DateTime.UtcNow.Year);

                _logger.LogInformation(
                    "Username generated: '{Username}' for FullName='{FullName}'",
                    generatedUsername,
                    request.username);

                List<Role> roles;

                if (request.roleNames != null &&
                    request.roleNames.Any(r => !string.IsNullOrWhiteSpace(r)))
                {
                    var distinctRoleNames = request.roleNames
                        .Where(r => !string.IsNullOrWhiteSpace(r))
                        .Select(r => r.Trim())
                        .Distinct(StringComparer.OrdinalIgnoreCase)
                        .ToList();

                    roles = (await _userRepository
                        .GetByNamesAsync(distinctRoleNames)).ToList();

                    var invalidRoles = distinctRoleNames
                        .Except(roles.Select(r => r.Name),
                            StringComparer.OrdinalIgnoreCase)
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
                    roles = (await _userRepository
                        .GetByNamesAsync(new[] { "Student" })).ToList();

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
                    Username = generatedUsername,
                    Email = request.email,
                    CreatedAt = DateTime.UtcNow,
                    IsActive = false,
                    CenterId = request.centerid,
                    CountryId = request.countryid,
                    StaffCategoryId = request.staffcategoryid,
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
                    Purpose = TokenPurpose.SetPassword,
                    ExpiryDate = DateTime.UtcNow.AddHours(24),
                    IsUsed = false,
                    CreatedAt = DateTime.UtcNow,
                };

                await _activationRepository.AddAsync(activationToken);

                await _emailService.SendActivationEmailAsync(
                    user.Email, generatedUsername, token);

                _logger.LogInformation(
                    "Activation token generated for {UserId}", user.Id);

                return new AuthResponseDto(
                    string.Empty,
                    user.Id,
                    generatedUsername,
                    user.Email,
                    roles.Select(r => r.Name).ToList(),"");
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Registration failed for FullName='{FullName}', Email='{Email}'",
                    request.username,
                    request.email);
                throw;
            }
        }

        /// <summary>
        /// Logins the asynchronous.
        /// Validates credentials, enforces IP-based session locking,
        /// generates a JWT with embedded session ID, and creates an active session.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>AuthResponseDto.</returns>
        public async Task<AuthResponseDto> LoginAsync(LoginRequestDto request)
        {
            _logger.LogInformation(
                "Login attempt for username: {Username}",
                request.username);

            var ipAddress = request.ipAddress ?? "unknown";
            var userAgent = request.userAgent;

            // ── 1. Validate user exists ──
            var user = await _userRepository
                .GetByUsernameAsync(request.username);

            if (user == null)
            {
                await SafeLogLoginAsync(
                    userId: Guid.Empty,
                    ipAddress: ipAddress,
                    userAgent: userAgent,
                    isSuccessful: false,
                    failureReason: "INVALID_CREDENTIALS");

                throw new CustomException(
                    "Invalid credentials.",
                    StatusCodes.Status401Unauthorized,
                    "INVALID_CREDENTIALS");
            }

            // ── 2. Check account is active ──
            if (!user.IsActive)
            {
                await SafeLogLoginAsync(
                    userId: user.Id,
                    ipAddress: ipAddress,
                    userAgent: userAgent,
                    isSuccessful: false,
                    failureReason: "ACCOUNT_NOT_ACTIVE");

                throw new CustomException(
                    "Account not activated.",
                    StatusCodes.Status403Forbidden,
                    "ACCOUNT_NOT_ACTIVE");
            }

            // ── 3. Check password is set ──
            if (user.PasswordHash == null)
            {
                await SafeLogLoginAsync(
                    userId: user.Id,
                    ipAddress: ipAddress,
                    userAgent: userAgent,
                    isSuccessful: false,
                    failureReason: "ACCOUNT_NOT_ACTIVE");

                throw new CustomException(
                    "Account not activated.",
                    StatusCodes.Status403Forbidden,
                    "ACCOUNT_NOT_ACTIVE");
            }

            // ── 4. Verify password ──
            var result = _passwordHasher.VerifyHashedPassword(
                user,
                user.PasswordHash,
                request.password);

            if (result == PasswordVerificationResult.Failed)
            {
                await SafeLogLoginAsync(
                    userId: user.Id,
                    ipAddress: ipAddress,
                    userAgent: userAgent,
                    isSuccessful: false,
                    failureReason: "INVALID_CREDENTIALS");

                throw new CustomException(
                    "Invalid credentials.",
                    StatusCodes.Status401Unauthorized,
                    "INVALID_CREDENTIALS");
            }

            // ── 5. Enforce IP-based session locking ──
            var expiryDays = int.Parse(
                _configuration["Jwt:SessionExpiryDays"] ?? "1");
            var sessionId = Guid.NewGuid().ToString();
            var expiresAt = DateTimeOffset.UtcNow.AddDays(expiryDays);

            try
            {
                await _userSessionService.CreateSessionAsync(
                    user.Id,
                    sessionId,
                    ipAddress,
                    userAgent,
                    expiresAt);
            }
            catch (CustomException ex) when (ex.ErrorCode == "SESSION_IP_CONFLICT")
            {
                await SafeLogLoginAsync(
                    userId: user.Id,
                    ipAddress: ipAddress,
                    userAgent: userAgent,
                    isSuccessful: false,
                    failureReason: "SESSION_IP_CONFLICT");
                throw;
            }

            // ── 6. Generate JWT with session ID as JTI ──
            var roles = user.UserRoles
                .Where(ur => ur.Role != null)
                .Select(ur => ur.Role!.Name)
                .ToList();
            string profileimage = string.Empty;
            if (roles.Any(r => r.Equals("Student", StringComparison.OrdinalIgnoreCase)))
            {
                Student? studentdata = await _studentRepository.GetByUserIdAsync(user.Id);
                if (studentdata != null
                    && !string.IsNullOrEmpty(
                        studentdata.StudentDocuments?.StudentPhotoPath))
                {
                    profileimage = _fileStorageService.GetFileUrl(
                        studentdata.StudentDocuments.StudentPhotoPath);
                }
            }

            var roleId = user.UserRoles
             .Select(ur => ur.RoleId)
             .FirstOrDefault();

            var permissions = await _userRepository
                .GetByRoleAsync(roleId);

            var jwt = _tokenService.GenerateToken(user, roles, sessionId, permissions);

            // ── 7. Log successful login ──
            await SafeLogLoginAsync(
                userId: user.Id,
                ipAddress: ipAddress,
                userAgent: userAgent,
                isSuccessful: true,
                failureReason: null);

            _logger.LogInformation(
                "Login successful for username {Username} " +
                "(UserId: {UserId}, SessionId: {SessionId}, IP: {IpAddress})",
                user.Username,
                user.Id,
                sessionId,
                ipAddress);

            return new AuthResponseDto(
                jwt,
                user.Id,
                user.Username,
                user.Email,
                roles, profileimage);
        }

        /// <summary>
        /// Sets the password asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Task.</returns>
        public async Task SetPasswordAsync(SetPasswordDto request)
        {
            try
            {
                _logger.LogInformation(
                    "Password setup attempt for token {Token}", request.token);

                var tokenEntity = await _activationRepository
                    .GetByTokenAsync(request.token)
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

                var user = await _userRepository
                    .GetByIdAsync(tokenEntity.UserId)
                    ?? throw new CustomException(
                        "User not found.",
                        StatusCodes.Status404NotFound,
                        "USER_NOT_FOUND");

                user.PasswordHash = _passwordHasher
                    .HashPassword(user, request.password);
                user.IsActive = true;

                tokenEntity.IsUsed = true;

                await _userRepository.UpdateAsync(user);

                _logger.LogInformation(
                    "Password set successfully for user {UserId}", user.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Password setup failed for token {Token}",
                    request.token);
                throw;
            }
        }

        /// <summary>
        /// Forgots the password asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Task.</returns>
        public async Task ForgotPasswordAsync(ForgotPasswordDto request)
        {
            var user = await _userRepository.GetByEmailAsync(request.email);

            if (user == null)
            {
                _logger.LogInformation(
                    "ForgotPassword for non-existing email {Email}",
                    request.email);
                return;
            }

            var purpose = user.IsActive
                ? TokenPurpose.ResetPassword
                : TokenPurpose.SetPassword;

            await _activationRepository
                .InvalidateUserTokensAsync(user.Id, purpose);

            var token = Guid.NewGuid().ToString("N");

            var resetToken = new UserActivationToken
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                Token = token,
                Purpose = purpose,
                ExpiryDate = DateTime.UtcNow.AddHours(24),
                IsUsed = false,
                CreatedAt = DateTime.UtcNow,
            };

            await _activationRepository.AddAsync(resetToken);

            if (purpose == TokenPurpose.SetPassword)
            {
                await _emailService.SendActivationEmailAsync(
                    user.Email, user.Username, token);
            }
            else
            {
                await _emailService.SendResetPasswordEmailAsync(
                    user.Email, user.Username, token);
            }
        }

        /// <summary>
        /// Resets the password asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Task.</returns>
        public async Task ResetPasswordAsync(ResetPasswordDto request)
        {
            var tokenEntity = await _activationRepository
                .GetByTokenAsync(request.token)
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

            var user = await _userRepository
                .GetByIdAsync(tokenEntity.UserId)
                ?? throw new CustomException(
                    "User not found.",
                    StatusCodes.Status404NotFound,
                    "USER_NOT_FOUND");

            user.PasswordHash =
                _passwordHasher.HashPassword(user, request.newPassword);

            if (tokenEntity.Purpose == TokenPurpose.SetPassword)
            {
                user.IsActive = true;
            }

            tokenEntity.IsUsed = true;

            await _userRepository.UpdateAsync(user);
            await _activationRepository.UpdateAsync(tokenEntity);

            // Revoke all existing sessions on password reset
            await _userSessionService.RevokeAllUserSessionsAsync(user.Id);

            _logger.LogInformation(
                "Password reset and all sessions revoked for UserId={UserId}",
                user.Id);
        }

        /// <summary>
        /// Revokes all active sessions for a user using their credentials.
        /// This is the escape hatch for users locked out due to IP change.
        /// </summary>
        /// <param name="request">The username and password.</param>
        /// <returns>Task.</returns>
        public async Task RevokeAllSessionsAsync(RevokeAllSessionsDto request)
        {
            _logger.LogInformation(
                "Revoke all sessions attempt for username: {Username}",
                request.username);

            var user = await _userRepository
                .GetByUsernameAsync(request.username);

            if (user == null)
            {
                throw new CustomException(
                    "Invalid credentials.",
                    StatusCodes.Status401Unauthorized,
                    "INVALID_CREDENTIALS");
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

            await _userSessionService.RevokeAllUserSessionsAsync(user.Id);

            _logger.LogInformation(
                "All sessions revoked for UserId={UserId} via credential-based request",
                user.Id);
        }

        /// <summary>
        /// Safely logs a login attempt. Never throws —
        /// logging failure must not break authentication.
        /// </summary>
        private async Task SafeLogLoginAsync(
            Guid userId,
            string? ipAddress,
            string? userAgent,
            bool isSuccessful,
            string? failureReason)
        {
            try
            {
                await _loginLogService.AddLogAsync(new CreateLoginLogDto
                {
                    UserId = userId.ToString(),
                    IpAddress = ipAddress,
                    UserAgent = userAgent,
                    IsSuccessful = isSuccessful,
                    FailureReason = failureReason,
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to log login attempt for UserId: {UserId}, " + "Successful: {IsSuccessful}",
                    userId,
                    isSuccessful);
            }
        }
    }
}
