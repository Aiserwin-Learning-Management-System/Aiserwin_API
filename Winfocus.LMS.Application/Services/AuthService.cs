namespace Winfocus.LMS.Application.Services
{
    using Microsoft.AspNetCore.Identity;
    using Winfocus.LMS.Application.DTOs;
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

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthService"/> class.
        /// </summary>
        /// <param name="userRepository">The user repository.</param>
        /// <param name="roleRepository">The role repository.</param>
        /// <param name="tokenService">The token service.</param>
        /// <param name="passwordHasher">The password hasher.</param>
        public AuthService(
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            ITokenService tokenService,
            IPasswordHasher<User> passwordHasher)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _tokenService = tokenService;
            _passwordHasher = passwordHasher;
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
            if (await _userRepository.UsernameExistsAsync(request.username))
            {
                throw new InvalidOperationException("Username already exists.");
            }

            var role = await _roleRepository.GetByNameAsync(request.roleName)
                       ?? throw new InvalidOperationException("Invalid role.");

            var user = new User
            {
                Username = request.username,
                Email = request.email,
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
            };

            user.PasswordHash = _passwordHasher.HashPassword(user, request.password);

            user.UserRoles.Add(new UserRole
            {
                RoleId = role.Id,
                UserId = user.Id,
            });

            await _userRepository.AddAsync(user);

            var token = _tokenService.GenerateToken(user, new[] { role.Name });

            return new AuthResponseDto(token);
        }

        /// <summary>
        /// Logins the asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>AuthResponseDto.</returns>
        /// <exception cref="UnauthorizedAccessException">Invalid credentials.</exception>
        public async Task<AuthResponseDto> LoginAsync(LoginRequestDto request)
        {
            var user = await _userRepository.GetByUsernameAsync(request.username)
                       ?? throw new UnauthorizedAccessException("Invalid credentials.");

            var verificationResult = _passwordHasher.VerifyHashedPassword(
                user,
                user.PasswordHash,
                request.password);

            if (verificationResult == PasswordVerificationResult.Failed)
            {
                throw new UnauthorizedAccessException("Invalid credentials.");
            }

            var roles = user.UserRoles.Select(r => r.Role.Name).ToList();

            var token = _tokenService.GenerateToken(user, roles);

            return new AuthResponseDto(token);
        }
    }
}
