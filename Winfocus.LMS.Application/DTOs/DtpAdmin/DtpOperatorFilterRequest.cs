namespace Winfocus.LMS.Application.DTOs.DtpAdmin
{
    using Winfocus.LMS.Application.DTOs.Common;

    /// <summary>
    /// Extends PagedRequest with DTP-specific filters.
    /// </summary>
    public class DtpOperatorFilterRequest : PagedRequest
    {
        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public string? Status { get; set; }
    }
}
