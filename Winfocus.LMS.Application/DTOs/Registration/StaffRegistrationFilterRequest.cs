namespace Winfocus.LMS.Application.DTOs.Registration
{
    using Winfocus.LMS.Application.DTOs.Common;

    /// <summary>
    /// Paged filter request with registration-specific filters.
    /// </summary>
    public class StaffRegistrationFilterRequest : PagedRequest
    {
        /// <summary>
        /// Gets or sets the staff category identifier.
        /// </summary>
        /// <value>
        /// The staff category identifier.
        /// </value>
        public Guid? StaffCategoryId { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public int? Status { get; set; }
    }
}
