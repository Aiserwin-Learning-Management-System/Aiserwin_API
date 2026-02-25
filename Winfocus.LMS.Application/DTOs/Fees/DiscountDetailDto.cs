using Winfocus.LMS.Domain.Enums;

namespace Winfocus.LMS.Application.DTOs.Fees
{
    /// <summary>
    /// DiscountDetailDto – represents a single discount entry for read operations.
    /// </summary>
    public sealed class DiscountDetailDto
    {
        /// <summary>
        /// Gets or sets the student fee selection identifier.
        /// </summary>
        /// <value>The student fee selection identifier.</value>
        public Guid StudentFeeSelectionId { get; set; }

        /// <summary>
        /// Gets or sets the student identifier.
        /// </summary>
        /// <value>The student identifier.</value>
        public Guid StudentId { get; set; }

        /// <summary>
        /// Gets or sets the course identifier.
        /// </summary>
        /// <value>The course identifier.</value>
        public Guid CourseId { get; set; }

        /// <summary>
        /// Gets or sets the fee plan identifier.
        /// </summary>
        /// <value>The fee plan identifier.</value>
        public Guid FeePlanId { get; set; }

        /// <summary>
        /// Gets or sets the discount type (Scholarship, Seasonal, Manual).
        /// </summary>
        /// <value>The discount type.</value>
        public DiscountType DiscountType { get; set; }

        /// <summary>
        /// Gets or sets the discount percent.
        /// </summary>
        /// <value>The percent.</value>
        public decimal Percent { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this discount is active.
        /// </summary>
        /// <value><c>true</c> if active; otherwise, <c>false</c>.</value>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the base fee.
        /// </summary>
        /// <value>The base fee.</value>
        public decimal BaseFee { get; set; }

        /// <summary>
        /// Gets or sets the final amount after all active discounts.
        /// </summary>
        /// <value>The final amount.</value>
        public decimal FinalAmount { get; set; }
    }
}
