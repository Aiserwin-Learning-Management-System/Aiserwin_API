using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Winfocus.LMS.API.Controllers
{
    /// <summary>
    /// Base controller that provides common functionality for all API controllers.
    /// </summary>
    /// <remarks>
    /// This controller exposes helper properties such as the authenticated user's identifier
    /// extracted from the JWT Bearer token. All controllers should inherit from this base class
    /// to access shared authentication and utility logic.
    /// </remarks>
    public abstract class BaseController : ControllerBase
    {
        private string userId = "SystemGenerated";

        /// <summary>
        /// Gets the authenticated user's identifier from the JWT token.
        /// Returns empty string if user is not authenticated.
        /// </summary>
        protected Guid UserId
        {
            get
            {
                if (User?.Identity?.IsAuthenticated != true)
                {
                    throw new UnauthorizedAccessException("User is not authenticated.");
                }

                var userIdString =
                    User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value ??
                    User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrWhiteSpace(userIdString))
                {
                    throw new UnauthorizedAccessException("User ID claim is missing.");
                }

                if (!Guid.TryParse(userIdString, out Guid userId))
                {
                    throw new UnauthorizedAccessException("Invalid User ID claim.");
                }

                return userId;
            }
        }
    }
}
