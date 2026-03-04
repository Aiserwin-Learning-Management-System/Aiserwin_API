using System;
using System.Collections.Generic;
using System.Text;
using Winfocus.LMS.Application.DTOs.Masters;

namespace Winfocus.LMS.Application.DTOs.Fees
{
    /// <summary>
    /// Represents a FeePlan with its associated discounts.
    /// </summary>
    public class FeePlanDto : BaseClassDTO
    {

        /// <summary>
        /// Gets the related Course identifier.
        /// </summary>
        public Guid CourseId { get; init; }

        /// <summary>
        /// Gets the name of the fee plan.
        /// </summary>
        public string PlanName { get; init; } = string.Empty;

        /// <summary>
        /// Gets the tuition fee amount.
        /// </summary>
        public decimal TuitionFee { get; init; }

        /// <summary>
        /// Gets a value indicating whether installment payment is allowed.
        /// </summary>
        public bool IsInstallmentAllowed { get; init; }

        /// <summary>
        /// Gets the list of discounts associated with this FeePlan.
        /// </summary>
        public IReadOnlyCollection<FeePlanDiscountDto> Discounts { get; init; }
            = new List<FeePlanDiscountDto>();
    }
}
