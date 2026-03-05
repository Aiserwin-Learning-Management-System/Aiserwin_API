using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using Winfocus.LMS.Application.Interfaces;

namespace Winfocus.LMS.Application.Services
{
    /// <summary>
    /// Service responsible for extracting scope-related information
    /// (Role, CountryId, CenterId) from the authenticated user's JWT claims.
    /// 
    /// This enables centralized scope filtering for data access throughout the system.
    /// </summary>
    public class UserScopeService : IUserScopeService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserScopeService"/> class.
        /// </summary>
        /// <param name="httpContextAccessor">
        /// Provides access to the current HTTP context to read user claims.
        /// </param>
        public UserScopeService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Gets the role of the authenticated user from JWT claims.
        /// </summary>
        public string Role =>
            _httpContextAccessor.HttpContext?.User
                .FindFirst(ClaimTypes.Role)?.Value;

        /// <summary>
        /// Gets the CountryId claim from the authenticated user.
        /// Returns null if the claim is missing or invalid.
        /// </summary>
        public Guid? CountryId =>
            Guid.TryParse(
                _httpContextAccessor.HttpContext?.User
                    .FindFirst("CountryId")?.Value,
                out var id)
            ? id : null;

        /// <summary>
        /// Gets the CenterId claim from the authenticated user.
        /// Returns null if the claim is missing or invalid.
        /// </summary>
        public Guid? CenterId =>
            Guid.TryParse(
                _httpContextAccessor.HttpContext?.User
                    .FindFirst("CenterId")?.Value,
                out var id)
            ? id : null;
    }
}
