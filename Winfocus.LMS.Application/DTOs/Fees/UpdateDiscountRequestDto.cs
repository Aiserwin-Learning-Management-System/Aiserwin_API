using Winfocus.LMS.Domain.Enums;

namespace Winfocus.LMS.Application.DTOs.Fees
{
    /// <summary>
    /// UpdateDiscountRequestDto – request body for updating a specific discount
    /// on an existing StudentFeeSelection.
    /// </summary>
    public sealed class UpdateDiscountRequestDto
    {
        /// <summary>
        /// Gets or sets the student fee selection identifier.
        /// </summary>
        /// <value>The student fee selection identifier.</value>
        public Guid StudentFeeSelectionId { get; set; }

        /// <summary>
        /// Gets or sets the discount type. Valid values: Scholarship, Seasonal, Manual.
        /// </summary>
        /// <value>The discount type.</value>
        public DiscountType DiscountType { get; set; }

        /// <summary>
        /// Gets or sets the new discount percent.
        /// </summary>
        /// <value>The percent.</value>
        public decimal Percent { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this discount is active.
        /// </summary>
        /// <value><c>true</c> if active; otherwise, <c>false</c>.</value>
        public bool IsActive { get; set; }
    }
}
