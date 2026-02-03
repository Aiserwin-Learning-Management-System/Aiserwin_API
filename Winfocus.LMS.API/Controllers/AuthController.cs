using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Winfocus.LMS.Application.Services;
using Winfocus.LMS.Domain.Entities;
using Winfocus.LMS.Infrastructure.Data;

namespace Winfocus.LMS.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly JwtService _jwt;
        private readonly IPasswordHasher<User> _hasher;

        public AuthController(AppDbContext db, JwtService jwt, IPasswordHasher<User> hasher)
        {
            _db = db;
            _jwt = jwt;
            _hasher = hasher;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest req)
        {
            if (await _db.Users.AnyAsync(u => u.Username == req.Username))
                return BadRequest("Username already exists");

            var user = new User { Username = req.Username, Email = req.Email, Role = req.Role ?? "User" };
            user.PasswordHash = _hasher.HashPassword(user, req.Password);
            _db.Users.Add(user);
            await _db.SaveChangesAsync();
            return Ok(new { user.Id, user.Username });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest req)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Username == req.Username);
            if (user == null) return Unauthorized();

            var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, req.Password);
            if (result == PasswordVerificationResult.Failed) return Unauthorized();

            var token = _jwt.GenerateToken(user);
            return Ok(new { token });
        }
    }

    public record RegisterRequest(string Username, string Email, string Password, string? Role);
    public record LoginRequest(string Username, string Password);
}
