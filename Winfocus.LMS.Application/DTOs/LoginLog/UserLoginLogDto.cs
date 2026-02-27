namespace Winfocus.LMS.Application.DTOs.LoginLog
{
    using Winfocus.LMS.Application.DTOs.Masters;

    /// <summary>
    /// UserLoginLogDto.
    /// </summary>
    public class UserLoginLogDto : BaseClassDTO
    {
        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        public Guid? UserId { get; set; }

        /// <summary>
        /// Gets or sets the login timestamp.
        /// </summary>
        /// <value>
        /// The login timestamp.
        /// </value>
        public DateTimeOffset LoginTimestamp { get; set; }

        /// <summary>
        /// Gets or sets the ip address.
        /// </summary>
        /// <value>
        /// The ip address.
        /// </value>
        public string? IpAddress { get; set; }

        /// <summary>
        /// Gets or sets the user agent.
        /// </summary>
        /// <value>
        /// The user agent.
        /// </value>
        public string? UserAgent { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is successful.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is successful; otherwise, <c>false</c>.
        /// </value>
        public bool IsSuccessful { get; set; }

        /// <summary>
        /// Gets or sets the failure reason.
        /// </summary>
        /// <value>
        /// The failure reason.
        /// </value>
        public string? FailureReason { get; set; }
    }
}
