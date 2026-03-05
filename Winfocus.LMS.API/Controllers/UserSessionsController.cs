namespace Winfocus.LMS.API.Controllers
{
    using Asp.Versioning;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Winfocus.LMS.Application.DTOs.Session;
    using Winfocus.LMS.Application.Interfaces;

    /// <summary>
    /// Manages user sessions — logout, force-logout, and session inspection.
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class UserSessionsController : ControllerBase
    {
        private readonly IUserSessionService _sessionService;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="UserSessionsController"/> class.
        /// </summary>
        /// <param name="sessionService">The session service.</param>
        public UserSessionsController(IUserSessionService sessionService)
        {
            _sessionService = sessionService;
        }

        /// <summary>
        /// Logs out the current user by revoking their active session.
        /// This endpoint is exempt from IP validation in the middleware,
        /// so users can logout even if their IP has changed.
        /// </summary>
        /// <returns>A success message.</returns>
        [HttpPost("logout")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Logout()
        {
            var sessionId = User.FindFirst("session_id")?.Value;

            if (string.IsNullOrEmpty(sessionId))
            {
                return Unauthorized(new
                {
                    error = "No active session found.",
                    code = "SESSION_NOT_FOUND",
                });
            }

            await _sessionService.RevokeSessionAsync(sessionId);

            return Ok(new { message = "Logged out successfully." });
        }

        /// <summary>
        /// Force-logs out a user by revoking all their active sessions.
        /// Restricted to Admin and SuperAdmin roles.
        /// </summary>
        /// <param name="userId">The user identifier to force-logout.</param>
        /// <returns>A success message.</returns>
        [HttpPost("force-logout/{userId:guid}")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> ForceLogout(Guid userId)
        {
            await _sessionService.RevokeAllUserSessionsAsync(userId);

            return Ok(new
            {
                message = $"All sessions revoked for user {userId}.",
            });
        }

        /// <summary>
        /// Gets the active session information for a specific user.
        /// Restricted to Admin and SuperAdmin roles.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>The active session details, or 404 if no active session.</returns>
        [HttpGet("active/{userId:guid}")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        [ProducesResponseType(typeof(ActiveSessionDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetActiveSession(Guid userId)
        {
            var session = await _sessionService
                .GetActiveSessionAsync(userId);

            if (session == null)
            {
                return NotFound(new
                {
                    message = "No active session found for this user.",
                });
            }

            return Ok(session);
        }
    }
}
