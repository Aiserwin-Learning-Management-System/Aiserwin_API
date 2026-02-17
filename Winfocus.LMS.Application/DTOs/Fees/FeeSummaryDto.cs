namespace Winfocus.LMS.Application.DTOs.Fees
{
    /// <summary>
    /// FeeSummaryDto.
    /// </summary>
    public sealed class FeeSummaryDto
    {
        /// <summary>
        /// Gets or sets the base fee.
        /// </summary>
        /// <value>
        /// The base fee.
        /// </value>
        public decimal BaseFee { get; set; }

        /// <summary>
        /// Gets or sets the total payable.
        /// </summary>
        /// <value>
        /// The total payable.
        /// </value>
        public decimal TotalPayable { get; set; }
    }
}
