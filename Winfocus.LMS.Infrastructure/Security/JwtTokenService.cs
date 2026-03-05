namespace Winfocus.LMS.Infrastructure.Security
{
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using Microsoft.Extensions.Configuration;
    using Microsoft.IdentityModel.Tokens;
    using Winfocus.LMS.Application.Interfaces;
    using Winfocus.LMS.Domain.Entities;

    /// <summary>
    /// Provides functionality for generating JWT tokens for users.
    /// </summary>
    public sealed class JwtTokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="JwtTokenService"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public JwtTokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Generates the JWT token with the given session identifier as the JTI claim.
        /// Also adds a custom "session_id" claim for reliable retrieval
        /// regardless of inbound claim type mapping.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="roles">The roles.</param>
        /// <param name="sessionId">The session identifier to use as JTI.</param>
        /// <returns>The JWT token string.</returns>
        public string GenerateToken(
            User user,
            IReadOnlyList<string> roles,
            string sessionId)
        {
            var issuer = _configuration["Jwt:Issuer"]!;
            var audience = _configuration["Jwt:Audience"]!;
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] !));
            var expiryDays = int.Parse(
                _configuration["Jwt:SessionExpiryDays"] ?? "1");

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, sessionId),
                new Claim("session_id", sessionId),
            };
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("countryId", user.CountryId.ToString() ?? string.Empty),
            new Claim("centerId", user.CenterId.ToString() ?? string.Empty),
            new Claim("StaffTypeId", user.StaffTypeId?.ToString() ?? ""),
        };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddDays(expiryDays),
                signingCredentials: new SigningCredentials(
                    key,
                    SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
