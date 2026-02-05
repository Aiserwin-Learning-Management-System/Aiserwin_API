namespace Winfocus.LMS.Application.Services
{
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using Microsoft.Extensions.Configuration;
    using Microsoft.IdentityModel.Tokens;
    using Winfocus.LMS.Domain.Entities;

    /// <summary>
    /// Provides functionality for generating JWT tokens for users.
    /// </summary>
    public class JwtService
    {
        private readonly IConfiguration _config;

        /// <summary>
        /// Initializes a new instance of the <see cref="JwtService"/> class.
        /// </summary>
        /// <param name="config">The application configuration.</param>
        public JwtService(IConfiguration config) => _config = config;

        /// <summary>
        /// Generates a JWT token for the specified user.
        /// </summary>
        /// <param name="user">The user for whom to generate the token.</param>
        /// <returns>A JWT token string.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the JWT key configuration is missing or empty.</exception>
        public string GenerateToken(User user)
        {
            var issuer = _config["Jwt:Issuer"] ?? throw new InvalidOperationException("Jwt:Issuer is not configured.");
            var audience = _config["Jwt:Audience"] ?? throw new InvalidOperationException("Jwt:Audience is not configured.");

            var jwtKey = _config["Jwt:Key"] ?? throw new InvalidOperationException("JWT key is missing.");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            // Add multiple roles if applicable
            foreach (var role in user.UserRoles.Select(ur => ur.Role.Name))
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2), // shorter expiry
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
