namespace Winfocus.LMS.Application.DTOs.Fees
{
    using Winfocus.LMS.Domain.Enums;

    public sealed record CreateFeePlanRequestDto(
        Guid CourseId,
        string PlanName,
        decimal TuitionFee,
        bool IsInstallmentAllowed,
        List<CreateFeePlanDiscountRequestDto>? Discounts,
        Guid userid,
        PaymentType PaymentType,
        int DurationinYears,
        Guid SubjectId);
}
