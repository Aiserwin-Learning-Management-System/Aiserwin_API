namespace Winfocus.LMS.Application.Services
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using Winfocus.LMS.Application.Common.Exceptions;
    using Winfocus.LMS.Application.DTOs.Session;
    using Winfocus.LMS.Application.Interfaces;
    using Winfocus.LMS.Domain.Entities;

    /// <summary>
    /// Service implementation for user session management.
    /// Enforces IP-based session locking to prevent credential sharing.
    /// </summary>
    public sealed class UserSessionService : IUserSessionService
    {
        private readonly IUserSessionRepository _repository;
        private readonly ILogger<UserSessionService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserSessionService"/> class.
        /// </summary>
        /// <param name="repository">The session repository.</param>
        /// <param name="logger">The logger.</param>
        public UserSessionService(
            IUserSessionRepository repository,
            ILogger<UserSessionService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task CreateSessionAsync(
            Guid userId,
            string sessionId,
            string ipAddress,
            string? userAgent,
            DateTimeOffset expiresAt)
        {
            _logger.LogInformation(
                "Creating session for UserId={UserId}, IP={IpAddress}",
                userId,
                ipAddress);

            // 1. Check for existing active session
            var existingSession = await _repository
                .GetActiveSessionByUserIdAsync(userId);

            if (existingSession != null)
            {
                if (!string.Equals(
                    existingSession.IpAddress,
                    ipAddress,
                    StringComparison.OrdinalIgnoreCase))
                {
                    // Different IP — block login
                    _logger.LogWarning(
                        "Session IP conflict for UserId={UserId}. " +
                        "Active session IP={ActiveIp}, Requested IP={RequestedIp}",
                        userId,
                        existingSession.IpAddress,
                        ipAddress);

                    throw new CustomException(
                        "Your account is currently active on another device/location. " +
                        "Please logout from the other device, wait for session expiry, " +
                        "or use 'Revoke All Sessions' to clear active sessions.",
                        StatusCodes.Status403Forbidden,
                        "SESSION_IP_CONFLICT");
                }

                // Same IP — revoke old session and allow new login
                _logger.LogInformation(
                    "Revoking old session {OldSessionId} for UserId={UserId} (same IP re-login)",
                    existingSession.SessionId,
                    userId);

                existingSession.IsRevoked = true;
                existingSession.LogoutAt = DateTimeOffset.UtcNow;
                existingSession.UpdatedAt = DateTime.UtcNow;
                await _repository.UpdateAsync(existingSession);
            }

            // 2. Create new session
            var session = new UserActiveSession
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                SessionId = sessionId,
                IpAddress = ipAddress,
                UserAgent = userAgent,
                LoginAt = DateTimeOffset.UtcNow,
                ExpiresAt = expiresAt,
                IsRevoked = false,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = userId,
                IsActive = true,
            };

            await _repository.AddAsync(session);

            _logger.LogInformation(
                "Session {SessionId} created for UserId={UserId}, IP={IpAddress}, Expires={ExpiresAt}",
                sessionId,
                userId,
                ipAddress,
                expiresAt);
        }

        /// <inheritdoc />
        public async Task<bool> ValidateSessionAsync(
            string sessionId, string? currentIpAddress)
        {
            var session = await _repository.GetBySessionIdAsync(sessionId);

            if (session == null)
            {
                _logger.LogDebug("Session {SessionId} not found", sessionId);
                return false;
            }

            if (!session.IsActive)
            {
                _logger.LogDebug("Session {SessionId} is inactive", sessionId);
                return false;
            }

            if (session.IsRevoked)
            {
                _logger.LogDebug("Session {SessionId} is revoked", sessionId);
                return false;
            }

            if (session.ExpiresAt < DateTimeOffset.UtcNow)
            {
                _logger.LogDebug("Session {SessionId} has expired", sessionId);
                return false;
            }

            if (!string.Equals(
                session.IpAddress,
                currentIpAddress,
                StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogWarning(
                    "Session {SessionId} IP mismatch. " +
                    "Session IP={SessionIp}, Request IP={RequestIp}",
                    sessionId,
                    session.IpAddress,
                    currentIpAddress);
                return false;
            }

            return true;
        }

        /// <inheritdoc />
        public async Task RevokeSessionAsync(string sessionId)
        {
            var session = await _repository.GetBySessionIdAsync(sessionId);

            if (session == null || session.IsRevoked)
            {
                _logger.LogDebug(
                    "Session {SessionId} not found or already revoked", sessionId);
                return;
            }

            session.IsRevoked = true;
            session.LogoutAt = DateTimeOffset.UtcNow;
            session.UpdatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(session);

            _logger.LogInformation(
                "Session {SessionId} revoked for UserId={UserId}",
                sessionId,
                session.UserId);
        }

        /// <inheritdoc />
        public async Task RevokeAllUserSessionsAsync(Guid userId)
        {
            _logger.LogInformation(
                "Revoking all active sessions for UserId={UserId}", userId);

            await _repository.RevokeAllUserSessionsAsync(userId);

            _logger.LogInformation(
                "All sessions revoked for UserId={UserId}", userId);
        }

        /// <inheritdoc />
        public async Task<ActiveSessionDto?> GetActiveSessionAsync(Guid userId)
        {
            var session = await _repository
                .GetActiveSessionByUserIdAsync(userId);

            if (session == null)
            {
                return null;
            }

            return new ActiveSessionDto
            {
                Id = session.Id,
                UserId = session.UserId,
                SessionId = session.SessionId,
                IpAddress = session.IpAddress,
                UserAgent = session.UserAgent,
                LoginAt = session.LoginAt,
                ExpiresAt = session.ExpiresAt,
            };
        }
    }
}
