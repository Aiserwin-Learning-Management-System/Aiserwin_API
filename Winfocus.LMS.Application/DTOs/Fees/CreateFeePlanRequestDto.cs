using System;
using System.Collections.Generic;
using System.Text;

namespace Winfocus.LMS.Application.DTOs.Fees
{
    /// <summary>
    /// Represents a request to create a FeePlan.
    /// </summary>
    public sealed record CreateFeePlanRequestDto
    (
        Guid CourseId,
        string PlanName,
        decimal TuitionFee,
        bool IsInstallmentAllowed,
        List<CreateFeePlanDiscountRequestDto>? Discounts, Guid userid
    );
}
