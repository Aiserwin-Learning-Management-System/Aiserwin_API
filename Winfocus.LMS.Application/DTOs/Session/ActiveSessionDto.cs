namespace Winfocus.LMS.Application.DTOs.Session
{
    /// <summary>
    /// DTO representing an active user session.
    /// </summary>
    public class ActiveSessionDto
    {
        /// <summary>
        /// Gets or sets the session record identifier.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets the unique session identifier (JWT JTI).
        /// </summary>
        public string SessionId { get; set; } = null!;

        /// <summary>
        /// Gets or sets the IP address bound to this session.
        /// </summary>
        public string IpAddress { get; set; } = null!;

        /// <summary>
        /// Gets or sets the user agent.
        /// </summary>
        public string? UserAgent { get; set; }

        /// <summary>
        /// Gets or sets the login timestamp.
        /// </summary>
        public DateTimeOffset LoginAt { get; set; }

        /// <summary>
        /// Gets or sets the expiry timestamp.
        /// </summary>
        public DateTimeOffset ExpiresAt { get; set; }
    }
}
