namespace Winfocus.LMS.Application.DTOs.Fees
{
    using Winfocus.LMS.Application.DTOs.Masters;
    using Winfocus.LMS.Domain.Enums;

    /// <summary>
    /// Data Transfer Object (DTO) representing a fee plan,
    /// including tuition fee, duration, payment type, and related identifiers.
    /// </summary>
    public class FeePlanDto : BaseClassDTO
    {
        /// <summary>Gets or sets the unique identifier of the associated course.</summary>
        public Guid CourseId { get; set; }

        /// <summary>Gets or sets the name of the fee plan.</summary>
        public string PlanName { get; set; } = string.Empty;

        /// <summary>Gets or sets the tuition fee amount for the plan.</summary>
        public decimal TuitionFee { get; set; }

        /// <summary>Gets or sets a value indicating whether installment payments are allowed.</summary>
        public bool IsInstallmentAllowed { get; set; }

        /// <summary>Gets or sets the payment type (e.g., full payment, installment).</summary>
        public PaymentType PaymentType { get; set; }

        /// <summary>Gets or sets the duration of the plan in years.</summary>
        public int DurationinYears { get; set; }

        /// <summary>Gets or sets the unique identifier of the associated subject.</summary>
        public Guid SubjectId { get; set; }

        /// <summary>Gets or sets the unique identifier of the syllabus associated with the plan.</summary>
        public Guid SyllabusId { get; set; }

        /// <summary>Gets or sets the unique identifier of the grade associated with the plan.</summary>
        public Guid GradeId { get; set; }

        /// <summary>Gets or sets the unique identifier of the stream associated with the plan.</summary>
        public Guid StreamId { get; set; }

        /// <summary>Gets or sets the unique identifier of the country associated with the plan.</summary>
        public Guid CountryId { get; set; }

        /// <summary>Gets or sets the unique identifier of the state associated with the plan.</summary>
        public Guid StateId { get; set; }

        /// <summary>Gets or sets the unique identifier of the mode of study associated with the plan.</summary>
        public Guid ModeofstudyId { get; set; }

        /// <summary>Gets or sets the unique identifier of the center associated with the plan.</summary>
        public Guid CenterId { get; set; }

        /// <summary>
        /// Gets or sets the collection of discounts applicable to this fee plan.
        /// </summary>
        public IReadOnlyCollection<FeePlanDiscountDto> Discounts { get; set; }
            = new List<FeePlanDiscountDto>();
    }
}
