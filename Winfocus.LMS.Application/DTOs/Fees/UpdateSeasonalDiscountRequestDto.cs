namespace Winfocus.LMS.Application.DTOs.Fees
{
    /// <summary>
    /// UpdateSeasonalDiscountRequestDto – request body for updating seasonal discount on a fee plan.
    /// </summary>
    public sealed class UpdateSeasonalDiscountRequestDto
    {
        /// <summary>
        /// Gets or sets the fee plan identifier.
        /// </summary>
        /// <value>The fee plan identifier.</value>
        public Guid FeePlanId { get; set; }

        /// <summary>
        /// Gets or sets the seasonal discount percent.
        /// </summary>
        /// <value>The percent.</value>
        public decimal Percent { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the seasonal discount is active.
        /// </summary>
        /// <value><c>true</c> if active; otherwise, <c>false</c>.</value>
        public bool IsActive { get; set; }
    }
}
