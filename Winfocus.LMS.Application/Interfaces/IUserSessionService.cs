namespace Winfocus.LMS.Application.Interfaces
{
    using Winfocus.LMS.Application.DTOs.Session;

    /// <summary>
    /// Service interface for user session management.
    /// Enforces IP-based session locking to prevent credential sharing.
    /// </summary>
    public interface IUserSessionService
    {
        /// <summary>
        /// Creates a new session for the user. If an active session exists
        /// from the same IP, the old session is revoked. If an active session
        /// exists from a different IP, a <c>SESSION_IP_CONFLICT</c> exception is thrown.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="sessionId">The unique session identifier (JWT JTI).</param>
        /// <param name="ipAddress">The IP address of the login request.</param>
        /// <param name="userAgent">The user agent string.</param>
        /// <param name="expiresAt">When the session should expire.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task CreateSessionAsync(
            Guid userId,
            string sessionId,
            string ipAddress,
            string? userAgent,
            DateTimeOffset expiresAt);

        /// <summary>
        /// Validates whether a session is still active, not revoked,
        /// not expired, and bound to the given IP address.
        /// </summary>
        /// <param name="sessionId">The session identifier (JWT JTI).</param>
        /// <param name="currentIpAddress">The current request IP address.</param>
        /// <returns><c>true</c> if the session is valid; otherwise, <c>false</c>.</returns>
        Task<bool> ValidateSessionAsync(string sessionId, string? currentIpAddress);

        /// <summary>
        /// Revokes a specific session (logout).
        /// </summary>
        /// <param name="sessionId">The session identifier to revoke.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task RevokeSessionAsync(string sessionId);

        /// <summary>
        /// Revokes all active sessions for a specific user (admin force-logout or self-revoke).
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task RevokeAllUserSessionsAsync(Guid userId);

        /// <summary>
        /// Gets the active session details for a user (admin view).
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>The active session DTO if found; otherwise, <c>null</c>.</returns>
        Task<ActiveSessionDto?> GetActiveSessionAsync(Guid userId);
    }
}
