namespace Winfocus.LMS.Application.DTOs.Fees
{
    using Winfocus.LMS.Domain.Enums;

    /// <summary>
    /// Data Transfer Object (DTO) representing a student's fee selection record,
    /// including course details, plan information, payment type, status, and dates.
    /// </summary>
    public sealed class StudentFeeSelectionListDto
    {
        /// <summary>Gets or sets the unique identifier of the fee selection.</summary>
        public Guid Id { get; set; }

        /// <summary>Gets or sets the unique identifier of the student.</summary>
        public Guid StudentId { get; set; }

        /// <summary>Gets or sets the name of the student.</summary>
        public string StudentName { get; set; } = string.Empty;

        /// <summary>Gets or sets the registration number of the student.</summary>
        public string RegistrationNumber { get; set; } = string.Empty;

        /// <summary>Gets or sets the name of the course.</summary>
        public string CourseName { get; set; } = string.Empty;

        /// <summary>Gets or sets the name of the fee plan.</summary>
        public string PlanName { get; set; } = string.Empty;

        /// <summary>Gets or sets the yearly fee amount.</summary>
        public decimal YearlyFee { get; set; }

        /// <summary>Gets or sets the duration of the plan in years.</summary>
        public int DurationYears { get; set; }

        /// <summary>Gets or sets the final payable amount after discounts.</summary>
        public decimal FinalAmount { get; set; }

        /// <summary>Gets or sets the payment type (e.g., full payment, installment).</summary>
        public PaymentType PaymentType { get; set; }

        /// <summary>Gets or sets the current status of the fee selection.</summary>
        public FeeSelectionStatus Status { get; set; }

        /// <summary>Gets or sets the start date of the fee plan.</summary>
        public DateTime StartDate { get; set; }

        /// <summary>Gets or sets the end date of the fee plan.</summary>
        public DateTime EndDate { get; set; }

        /// <summary>Gets or sets a value indicating whether the fee selection is active.</summary>
        public bool IsActive { get; set; }

        /// <summary>Gets or sets the date when the fee selection was created.</summary>
        public DateTime CreatedAt { get; set; }
    }
}
