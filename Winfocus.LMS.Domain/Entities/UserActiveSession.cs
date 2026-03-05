namespace Winfocus.LMS.Domain.Entities
{
    using Winfocus.LMS.Domain.Common;

    /// <summary>
    /// Represents an active user session bound to a specific IP address.
    /// Used to enforce single-location login and prevent credential sharing.
    /// </summary>
    public class UserActiveSession : BaseEntity
    {
        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets the unique session identifier (maps to JWT JTI claim).
        /// </summary>
        public string SessionId { get; set; } = null!;

        /// <summary>
        /// Gets or sets the IP address from which the session was initiated.
        /// </summary>
        public string IpAddress { get; set; } = null!;

        /// <summary>
        /// Gets or sets the user agent (browser/device info) of the session.
        /// </summary>
        public string? UserAgent { get; set; }

        /// <summary>
        /// Gets or sets the timestamp when the user logged in.
        /// </summary>
        public DateTimeOffset LoginAt { get; set; }

        /// <summary>
        /// Gets or sets the timestamp when the session expires.
        /// </summary>
        public DateTimeOffset ExpiresAt { get; set; }

        /// <summary>
        /// Gets or sets the timestamp when the user logged out.
        /// Null if the session is still active.
        /// </summary>
        public DateTimeOffset? LogoutAt { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this session has been revoked.
        /// </summary>
        public bool IsRevoked { get; set; }
    }
}
