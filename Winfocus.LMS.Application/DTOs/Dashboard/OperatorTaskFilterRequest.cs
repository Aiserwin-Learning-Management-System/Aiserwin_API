namespace Winfocus.LMS.Application.DTOs.Dashboard
{
    using Winfocus.LMS.Application.DTOs.Common;

    /// <summary>
    /// OperatorTaskFilterRequest.
    /// </summary>
    /// <seealso cref="Winfocus.LMS.Application.DTOs.Common.PagedRequest" />
    public class OperatorTaskFilterRequest : PagedRequest
    {
        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public int? Status { get; set; }

        /// <summary>
        /// Gets or sets the priority.
        /// </summary>
        /// <value>
        /// The priority.
        /// </value>
        public int? Priority { get; set; }
    }
}
