namespace Winfocus.LMS.Application.DTOs.Fees
{
    /// <summary>
    /// FeePageResponseDto.
    /// </summary>
    public sealed class FeePageResponseDto
    {
        /// <summary>
        /// Gets or sets the pricing table.
        /// </summary>
        /// <value>
        /// The pricing table.
        /// </value>
        public List<FeeRowDto> PricingTable { get; set; } = null!;
    }
}
