namespace Winfocus.LMS.Application.DTOs.DtpAdmin
{
    using Winfocus.LMS.Application.DTOs.Common;

    /// <summary>
    /// Filter request DTO for admin DAR listing with date range and operator filtering.
    /// </summary>
    public class DarFilterRequest : PagedRequest
    {
        /// <summary>
        /// Gets or sets the operator identifier (optional).
        /// If null, returns all operators' DARs.
        /// </summary>
        public Guid? OperatorId { get; set; }

        /// <summary>
        /// Gets or sets the start date for filtering (optional).
        /// </summary>
        public new DateOnly? StartDate { get; set; }

        /// <summary>
        /// Gets or sets the end date for filtering (optional).
        /// </summary>
        public new DateOnly? EndDate { get; set; }
    }
}
