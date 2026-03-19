namespace Winfocus.LMS.Application.DTOs.Fees
{
    /// <summary>
    /// Student confirms their fee selection from the fee page.
    /// </summary>
    public class ConfirmFeeSelectionRequest
    {
        /// <summary>
        /// Gets or sets the student identifier.
        /// </summary>
        /// <value>
        /// The student identifier.
        /// </value>
        public Guid StudentId { get; set; }

        /// <summary>
        /// Gets or sets the fee plan identifier.
        /// </summary>
        /// <value>
        /// The fee plan identifier.
        /// </value>
        public Guid FeePlanId { get; set; }
    }
}
