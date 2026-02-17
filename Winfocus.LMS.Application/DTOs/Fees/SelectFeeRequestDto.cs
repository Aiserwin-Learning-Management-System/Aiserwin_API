namespace Winfocus.LMS.Application.DTOs.Fees
{
    /// <summary>
    /// SelectFeeRequestDto.
    /// </summary>
    public sealed class SelectFeeRequestDto
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

        /// <summary>
        /// Gets or sets the manual discount percent.
        /// </summary>
        /// <value>
        /// The manual discount percent.
        /// </value>
        public decimal ManualDiscountPercent { get; set; }
    }
}
