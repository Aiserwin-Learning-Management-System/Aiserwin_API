using System;
using System.Collections.Generic;
using System.Text;
using Winfocus.LMS.Application.DTOs.Masters;

namespace Winfocus.LMS.Application.DTOs.Fees
{
    /// <summary>
    /// Represents a discount associated with a FeePlan.
    /// </summary>
    public class FeePlanDiscountDto : BaseClassDTO
    {
        /// <summary>
        /// Gets the identifier of the related FeePlan.
        /// </summary>
        public Guid FeePlanId { get; init; }

        /// <summary>
        /// Gets the name of the discount.
        /// </summary>
        public string DiscountName { get; init; } = string.Empty;

        /// <summary>
        /// Gets the percentage value of the discount.
        /// </summary>
        public decimal DiscountPercent { get; init; }
    }
}
