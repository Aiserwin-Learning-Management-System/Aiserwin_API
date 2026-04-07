namespace Winfocus.LMS.API.Middleware
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using Winfocus.LMS.Application.Interfaces;

    /// <summary>
    /// Middleware that validates active user sessions on every authenticated request.
    /// Ensures the session is not revoked, not expired, and the request IP matches
    /// the session's bound IP address.
    /// </summary>
    public class SessionValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<SessionValidationMiddleware> _logger;

        /// <summary>
        /// Paths that are exempt from session IP validation.
        /// Logout must be allowed even if the user's IP has changed.
        /// </summary>
        private static readonly string[] ExemptPaths = new[]
        {
            "/usersessions/logout",
            "/swagger",
            "/api/auth",
        };

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="SessionValidationMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next middleware in the pipeline.</param>
        /// <param name="logger">The logger.</param>
        public SessionValidationMiddleware(
            RequestDelegate next,
            ILogger<SessionValidationMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// Invokes the middleware.
        /// </summary>
        /// <param name="context">The HTTP context.</param>
        /// <param name="sessionService">
        /// The user session service (resolved per-request from DI).
        /// </param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task InvokeAsync(
            HttpContext context,
            IUserSessionService sessionService)
        {
            // Skip if request is not authenticated
            if (context.User.Identity?.IsAuthenticated != true)
            {
                await _next(context);
                return;
            }

            // Skip for exempt paths (e.g., logout)
            var requestPath = context.Request.Path.Value?.ToLower() ?? string.Empty;
            if (ExemptPaths.Any(p => requestPath.Contains(p)))
            {
                await _next(context);
                return;
            }

            // Extract session_id claim
            // We use a custom "session_id" claim to avoid issues with
            // .NET's default inbound JWT claim type mapping for "jti"
            var sessionId = context.User.FindFirst("session_id")?.Value;

            if (string.IsNullOrEmpty(sessionId))
            {
                _logger.LogWarning(
                    "Authenticated request without session_id claim. " +
                    "Token may have been issued before session tracking was enabled.");

                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(new
                {
                    error = "Session expired. Please login again.",
                    code = "SESSION_NOT_FOUND",
                });
                return;
            }

            // Validate session
            var currentIp = context.Connection.RemoteIpAddress?.ToString();
            var isValid = await sessionService
                .ValidateSessionAsync(sessionId, currentIp);

            if (!isValid)
            {
                _logger.LogWarning(
                    "Session validation failed for SessionId={SessionId}, " +
                    "RequestIP={RequestIp}",
                    sessionId,
                    currentIp);

                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(new
                {
                    error = "Session is invalid, expired, or accessed from " +
                            "a different location. Please login again.",
                    code = "SESSION_INVALID",
                });
                return;
            }

            await _next(context);
        }
    }
}