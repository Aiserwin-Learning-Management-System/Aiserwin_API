using System;
using System.Collections.Generic;
using System.Text;
using Winfocus.LMS.Domain.Common;

namespace Winfocus.LMS.Domain.Entities
{
    /// <summary>
    /// FeePlanDiscount – Represents a discount configured for a specific FeePlan.
    /// A FeePlan can have multiple discounts (e.g., Seasonal, Early Bird, Festival).
    /// </summary>
    /// <seealso cref="BaseEntity"/>
    public class FeePlanDiscount : BaseEntity
    {
        /// <summary>
        /// Gets the associated FeePlan identifier.
        /// </summary>
        /// <value>
        /// The unique identifier of the FeePlan to which this discount belongs.
        /// </value>
        public Guid FeePlanId { get; private set; }

        /// <summary>
        /// Gets the name of the discount.
        /// </summary>
        /// <value>
        /// A descriptive name for the discount.
        /// </value>
        public string DiscountName { get; private set; } = null!;

        /// <summary>
        /// Gets the percentage value of the discount.
        /// </summary>
        /// <value>
        /// The discount percentage applied to the tuition fee.
        /// Must be between 0 and 100.
        /// </value>
        public decimal DiscountPercent { get; private set; }

        /// <summary>
        /// Gets the FeePlan navigation property.
        /// </summary>
        /// <value>
        /// The FeePlan entity that this discount belongs to.
        /// </value>
        public FeePlan FeePlan { get; private set; } = null!;

        /// <summary>
        /// Required by EF Core.
        /// </summary>
        private FeePlanDiscount()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FeePlanDiscount"/> class.
        /// </summary>
        /// <param name="feePlanId">The associated FeePlan identifier.</param>
        /// <param name="discountName">The name of the discount.</param>
        /// <param name="discountPercent">The percentage value of the discount.</param>
        public FeePlanDiscount(
            Guid feePlanId,
            string discountName,
            decimal discountPercent)
        {
            if (string.IsNullOrWhiteSpace(discountName))
                throw new ArgumentException("Discount name is required.");

            if (discountPercent < 0 || discountPercent > 100)
                throw new ArgumentException("Discount percent must be between 0 and 100.");

            FeePlanId = feePlanId;
            DiscountName = discountName;
            DiscountPercent = discountPercent;
            CreatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Updates the discount details.
        /// </summary>
        /// <param name="discountName">The updated discount name.</param>
        /// <param name="discountPercent">The updated discount percentage.</param>
        public void Update(string discountName, decimal discountPercent)
        {
            if (string.IsNullOrWhiteSpace(discountName))
                throw new ArgumentException("Discount name is required.");

            if (discountPercent < 0 || discountPercent > 100)
                throw new ArgumentException("Discount percent must be between 0 and 100.");

            DiscountName = discountName;
            DiscountPercent = discountPercent;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
