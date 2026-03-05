using System;
using System.Collections.Generic;
using System.Text;

namespace Winfocus.LMS.Application.DTOs.Fees
{
    /// <summary>
    /// Represents a request to create a discount for a FeePlan.
    /// </summary>
    public sealed record CreateFeePlanDiscountRequestDto
    (
        Guid id,
        string discountName,
        decimal discountPercent);
}
