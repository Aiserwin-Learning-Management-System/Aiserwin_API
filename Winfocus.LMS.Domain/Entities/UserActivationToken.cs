namespace Winfocus.LMS.Domain.Entities
{
    using Winfocus.LMS.Domain.Common;

    /// <summary>
    /// Represents activation token for password setup.
    /// </summary>
    public sealed class UserActivationToken : BaseEntity
    {
        /// <summary>
        /// Gets or sets the related user identifier.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets the activation token string.
        /// </summary>
        public string Token { get; set; } = null!;

        /// <summary>
        /// Gets or sets expiration date.
        /// </summary>
        public DateTime ExpiryDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is used.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is used; otherwise, <c>false</c>.
        /// </value>
        public bool IsUsed { get; set; }
    }
}
