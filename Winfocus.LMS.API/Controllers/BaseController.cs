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

        /// <summary>
        /// Gets the authenticated user's country from the JWT token.
        /// Returns empty string if user is not authenticated.
        /// </summary>
        protected Guid CountryId
        {
            get
            {
                if (User?.Identity?.IsAuthenticated != true)
                {
                    throw new UnauthorizedAccessException("User is not authenticated.");
                }

                var countryIdString = User.FindFirst("countryId")?.Value;

                if (string.IsNullOrWhiteSpace(countryIdString))
                {
                    throw new UnauthorizedAccessException("Country ID claim is missing.");
                }

                if (!Guid.TryParse(countryIdString, out Guid countryId))
                {
                    throw new UnauthorizedAccessException("Invalid Country ID claim.");
                }

                return countryId;
            }
        }

        /// <summary>
        /// Gets the authenticated user's center from the JWT token.
        /// Returns empty string if user is not authenticated.
        /// </summary>
        protected Guid CenterId
        {
            get
            {
                if (User?.Identity?.IsAuthenticated != true)
                {
                    throw new UnauthorizedAccessException("User is not authenticated.");
                }

                var centerIdString = User.FindFirst("centerId")?.Value;

                if (string.IsNullOrWhiteSpace(centerIdString))
                {
                    throw new UnauthorizedAccessException("Center ID claim is missing.");
                }

                if (!Guid.TryParse(centerIdString, out Guid centerId))
                {
                    throw new UnauthorizedAccessException("Invalid Center ID claim.");
                }

                return centerId;
            }
        }

        /// <summary>
        /// Gets the authenticated user's country from the JWT token.
        /// Returns empty string if user is not authenticated.
        /// </summary>
        protected Guid StateId
        {
            get
            {
                //if (User?.Identity?.IsAuthenticated != true)
                //{
                //    throw new UnauthorizedAccessException("User is not authenticated.");
                //}

                var stateIdString = User.FindFirst("stateid")?.Value;

                if (string.IsNullOrWhiteSpace(stateIdString))
                {
                    //throw new UnauthorizedAccessException("State ID claim is missing.");
                    stateIdString = "00000000-0000-0000-0000-000000000000";
                }

                if (!Guid.TryParse(stateIdString, out Guid stateId))
                {
                    throw new UnauthorizedAccessException("Invalid State ID claim.");
                }

                return stateId;
            }
        }
    }
}
