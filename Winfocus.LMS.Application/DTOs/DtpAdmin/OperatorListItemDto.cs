namespace Winfocus.LMS.Application.DTOs.DtpAdmin
{
    /// <summary>
    /// A single operator row with dynamic values as dictionary.
    /// </summary>
    public class OperatorListItemDto
    {
        /// <summary>
        /// Gets or sets the registration identifier.
        /// </summary>
        /// <value>
        /// The registration identifier.
        /// </value>
        public Guid RegistrationId { get; set; }

        /// <summary>
        /// Gets or sets the sl no.
        /// </summary>
        /// <value>
        /// The sl no.
        /// </value>
        public int SlNo { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the color of the status.
        /// </summary>
        /// <value>
        /// The color of the status.
        /// </value>
        public string StatusColor { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is active; otherwise, <c>false</c>.
        /// </value>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the values.
        /// </summary>
        /// <value>
        /// The values.
        /// </value>
        public Dictionary<string, string?> Values { get; set; } = new ();

        /// <summary>
        /// Gets or sets the registered at.
        /// </summary>
        /// <value>
        /// The registered at.
        /// </value>
        public DateTime RegisteredAt { get; set; }
    }
}
