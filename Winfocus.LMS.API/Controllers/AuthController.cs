namespace Winfocus.LMS.Api.Controllers
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Winfocus.LMS.Application.Services;
    using Winfocus.LMS.Domain.Entities;
    using Winfocus.LMS.Infrastructure.Data;

    /// <summary>
    /// Controller for authentication-related actions such as user registration and login.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly JwtService _jwt;
        private readonly IPasswordHasher<User> _hasher;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthController"/> class.
        /// </summary>
        /// <param name="db">The application database context.</param>
        /// <param name="jwt">The JWT service for token generation.</param>
        /// <param name="hasher">The password hasher for user passwords.</param>
        public AuthController(AppDbContext db, JwtService jwt, IPasswordHasher<User> hasher)
        {
            _db = db;
            _jwt = jwt;
            _hasher = hasher;
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="req">The registration request containing username, email, password, and optional role.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the registration.</returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest req)
        {
            // Check if username already exists
            if (await _db.Users.AnyAsync(u => u.Username == req.username))
            {
                return BadRequest("Username already exists");
            }

            // Create new user
            var user = new User
            {
                Username = req.username,
                Email = req.email,
                PasswordHash = _hasher.HashPassword(null!, req.password), // hash password
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            // Determine role (default to "Student" if none provided)
            var roleName = string.IsNullOrEmpty(req.role) ? "Student" : req.role;
            var role = await _db.Roles.FirstOrDefaultAsync(r => r.Name == roleName);

            if (role == null)
            {
                return BadRequest($"Role '{roleName}' does not exist");
            }

            // Assign role to user
            var userRole = new UserRole
            {
                UserId = user.Id,
                RoleId = role.Id,
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
            };

            _db.UserRoles.Add(userRole);
            await _db.SaveChangesAsync();

            return Ok(new { user.Id, user.Username, AssignedRole = role.Name });
        }

        /// <summary>
        /// Authenticates a user and returns a JWT token if successful.
        /// </summary>
        /// <param name="req">The login request containing username and password.</param>
        /// <returns>An <see cref="IActionResult"/> containing the JWT token if authentication is successful.</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest req)
        {
            var user = await _db.Users.Include(u => u.UserRoles).ThenInclude(ur => ur.Role).FirstOrDefaultAsync(u => u.Username == req.username);
            if (user == null)
            {
                return Unauthorized();
            }

            var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, req.password);
            if (result == PasswordVerificationResult.Failed)
            {
                return Unauthorized();
            }

            var token = _jwt.GenerateToken(user);
            return Ok(new { token });
        }
    }

    /// <summary>
    /// Request model for user registration.
    /// </summary>
    /// <param name="username">The username of the new user.</param>
    /// <param name="email">The email address of the new user.</param>
    /// <param name="password">The password for the new user.</param>
    /// <param name="role">The role of the new user (optional).</param>
    public record RegisterRequest(string username, string email, string password, string? role);

    /// <summary>
    /// Request model for user login.
    /// </summary>
    /// <param name="username">The username of the user.</param>
    /// <param name="password">The password of the user.</param>
    public record LoginRequest(string username, string password);
}
