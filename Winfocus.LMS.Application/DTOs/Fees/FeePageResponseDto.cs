namespace Winfocus.LMS.Application.DTOs.Fees
{
    /// <summary>
    /// FeePageResponseDto – response for the fee page endpoint.
    /// </summary>
    public sealed class FeePageResponseDto
    {
        /// <summary>
        /// Gets or sets the pricing table rows.
        /// </summary>
        /// <value>The pricing table.</value>
        public List<FeeRowDto> PricingTable { get; set; } = null!;
    }
}
