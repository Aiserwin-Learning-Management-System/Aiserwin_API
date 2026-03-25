namespace Winfocus.LMS.Application.DTOs.Fees
{
    using Winfocus.LMS.Domain.Enums;

    /// <summary>
    /// Data Transfer Object (DTO) representing the response after confirming a student's fee selection.
    /// Includes course details, applied discounts, payment type, installments, and overall status.
    /// </summary>
    public sealed class ConfirmFeeResponseDto
    {
        /// <summary>Gets or sets the unique identifier of the fee selection.</summary>
        public Guid SelectionId { get; set; }

        /// <summary>Gets or sets the name of the course.</summary>
        public string CourseName { get; set; } = string.Empty;

        /// <summary>Gets or sets the name of the fee plan.</summary>
        public string PlanName { get; set; } = string.Empty;

        /// <summary>Gets or sets the yearly fee amount.</summary>
        public decimal YearlyFee { get; set; }

        /// <summary>Gets or sets the duration of the plan in years.</summary>
        public int DurationYears { get; set; }

        /// <summary>Gets or sets the total fee before discounts.</summary>
        public decimal TotalBeforeDiscount { get; set; }

        /// <summary>Gets or sets the collection of applied discounts.</summary>
        public List<AppliedDiscountSnapshotDto> AppliedDiscounts { get; set; } = new();

        /// <summary>Gets or sets the total discount percentage applied.</summary>
        public decimal TotalDiscountPercent { get; set; }

        /// <summary>Gets or sets the total discount amount applied.</summary>
        public decimal TotalDiscountAmount { get; set; }

        /// <summary>Gets or sets the final payable amount after discounts.</summary>
        public decimal FinalAmount { get; set; }

        /// <summary>Gets or sets the payment type (e.g., full payment, installment).</summary>
        public PaymentType PaymentType { get; set; }

        /// <summary>Gets or sets the total number of installments.</summary>
        public int TotalInstallments { get; set; }

        /// <summary>Gets or sets the current status of the fee selection.</summary>
        public FeeSelectionStatus Status { get; set; }

        /// <summary>Gets or sets the collection of installment schedules.</summary>
        public List<InstallmentScheduleDto> Installments { get; set; } = new();
    }

    /// <summary>
    /// Represents a snapshot of an applied discount,
    /// including its name, percentage, amount, and whether it was applied manually.
    /// </summary>
    public sealed class AppliedDiscountSnapshotDto
    {
        /// <summary>Gets or sets the name of the discount.</summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>Gets or sets the discount percentage.</summary>
        public decimal Percent { get; set; }

        /// <summary>Gets or sets the discount amount.</summary>
        public decimal Amount { get; set; }

        /// <summary>Gets or sets a value indicating whether the discount was applied manually.</summary>
        public bool IsManual { get; set; }
    }

    /// <summary>
    /// Represents the schedule of an installment,
    /// including due date, amounts, payment status, and remarks.
    /// </summary>
    public sealed class InstallmentScheduleDto
    {
        /// <summary>Gets or sets the unique identifier of the installment.</summary>
        public Guid InstallmentId { get; set; }

        /// <summary>Gets or sets the installment number.</summary>
        public int No { get; set; }

        /// <summary>Gets or sets the due date of the installment.</summary>
        public DateTime DueDate { get; set; }

        /// <summary>Gets or sets the due amount for the installment.</summary>
        public decimal DueAmount { get; set; }

        /// <summary>Gets or sets the amount paid towards the installment.</summary>
        public decimal PaidAmount { get; set; }

        /// <summary>Gets or sets the remaining balance amount for the installment.</summary>
        public decimal BalanceAmount { get; set; }

        /// <summary>Gets or sets the current status of the installment (e.g., Pending, Paid, Overdue).</summary>
        public InstallmentStatus Status { get; set; }

        /// <summary>Gets or sets any remarks associated with the installment.</summary>
        public string? Remarks { get; set; }

        /// <summary>Gets or sets the date when payment was made, if any.</summary>
        public DateTime? PaidDate { get; set; }
    }
}
