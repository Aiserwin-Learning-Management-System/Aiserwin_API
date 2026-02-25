namespace Winfocus.LMS.Application.DTOs.Fees
{
    /// <summary>
    /// FeeSummaryDto – summary returned after fee selection.
    /// </summary>
    public sealed class FeeSummaryDto
    {
        /// <summary>
        /// Gets or sets the base fee (tuition only).
        /// </summary>
        /// <value>The base fee.</value>
        public decimal BaseFee { get; set; }

        /// <summary>
        /// Gets or sets the scholarship discount amount applied.
        /// </summary>
        /// <value>The scholarship discount.</value>
        public decimal ScholarshipDiscount { get; set; }

        /// <summary>
        /// Gets or sets the seasonal discount amount applied.
        /// </summary>
        /// <value>The seasonal discount.</value>
        public decimal SeasonalDiscount { get; set; }

        /// <summary>
        /// Gets or sets the manual discount amount applied.
        /// </summary>
        /// <value>The manual discount.</value>
        public decimal ManualDiscount { get; set; }

        /// <summary>
        /// Gets or sets the total payable after all active discounts.
        /// </summary>
        /// <value>The total payable.</value>
        public decimal TotalPayable { get; set; }
    }
}
