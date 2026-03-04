namespace Winfocus.LMS.Api.Controllers
{
    using Asp.Versioning;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.RateLimiting;
    using Winfocus.LMS.Application.DTOs.Auth;
    using Winfocus.LMS.Application.Interfaces;

    /// <summary>
    /// Handles authentication endpoints.
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public sealed class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthController"/> class.
        /// </summary>
        /// <param name="authService">The authentication service.</param>
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Registers the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Registered data.</returns>
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(
            [FromBody] RegisterRequestDto request)
        {
            var result = await _authService.RegisterAsync(request);
            return Ok(result);
        }

        /// <summary>
        /// Logs in the user. Captures IP address and User Agent from the
        /// HTTP context to enforce IP-based session locking.
        /// </summary>
        /// <param name="request">The login request.</param>
        /// <returns>Auth response with JWT token.</returns>
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(
            [FromBody] LoginRequestDto request)
        {
            // Ensure IP and UserAgent are captured from the HTTP context
            // so session locking uses the real client IP, not client-provided values
            var enrichedRequest = request with
            {
                ipAddress = HttpContext.Connection
                    .RemoteIpAddress?.ToString() ?? request.ipAddress,
                userAgent = Request.Headers.UserAgent.ToString()
                    is { Length: > 0 } ua ? ua : request.userAgent,
            };

            var result = await _authService.LoginAsync(enrichedRequest);
            return Ok(result);
        }

        /// <summary>
        /// Sets the password.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Task.</returns>
        [AllowAnonymous]
        [EnableRateLimiting("SetPasswordPolicy")]
        [HttpPost("set-password")]
        public async Task<IActionResult> SetPassword(
            [FromBody] SetPasswordDto request)
        {
            await _authService.SetPasswordAsync(request);
            return Ok("Password set successfully.");
        }

        /// <summary>
        /// Forgot password — sends reset link.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Task.</returns>
        [AllowAnonymous]
        [EnableRateLimiting("SetPasswordPolicy")]
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(
            ForgotPasswordDto request)
        {
            await _authService.ForgotPasswordAsync(request);
            return Ok(new
            {
                message = "If the email exists, a reset link has been sent.",
            });
        }

        /// <summary>
        /// Resets the password.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Task.</returns>
        [AllowAnonymous]
        [EnableRateLimiting("SetPasswordPolicy")]
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(
            ResetPasswordDto request)
        {
            await _authService.ResetPasswordAsync(request);
            return Ok(new { message = "Password reset successfully." });
        }

        /// <summary>
        /// Revokes all active sessions for a user using their credentials.
        /// This is the escape hatch for users locked out due to IP change.
        /// Does NOT require a JWT — authentication is via username + password.
        /// </summary>
        /// <param name="request">Username and password.</param>
        /// <returns>Success message.</returns>
        [AllowAnonymous]
        [EnableRateLimiting("SetPasswordPolicy")]
        [HttpPost("revoke-all-sessions")]
        public async Task<IActionResult> RevokeAllSessions(
            [FromBody] RevokeAllSessionsDto request)
        {
            await _authService.RevokeAllSessionsAsync(request);
            return Ok(new
            {
                message = "All sessions revoked successfully. " +
                          "You can now login from any device.",
            });
        }
    }
}
