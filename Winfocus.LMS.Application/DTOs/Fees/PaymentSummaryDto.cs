namespace Winfocus.LMS.Application.DTOs.Fees
{
    using Winfocus.LMS.Domain.Enums;

    /// <summary>
    /// Data Transfer Object (DTO) representing a summary of payments
    /// for a student across multiple fee selections.
    /// </summary>
    public sealed class PaymentSummaryDto
    {
        /// <summary>Gets or sets the unique identifier of the student.</summary>
        public Guid StudentId { get; set; }

        /// <summary>Gets or sets the name of the student.</summary>
        public string StudentName { get; set; } = string.Empty;

        /// <summary>Gets or sets the collection of fee selections with payment details.</summary>
        public List<SelectionPaymentDto> Selections { get; set; } = new();

        /// <summary>Gets or sets the grand total fee across all selections.</summary>
        public decimal GrandTotal { get; set; }

        /// <summary>Gets or sets the total amount paid across all selections.</summary>
        public decimal GrandPaid { get; set; }

        /// <summary>Gets or sets the total remaining balance across all selections.</summary>
        public decimal GrandRemaining { get; set; }
    }

    /// <summary>
    /// Represents payment details for a specific fee selection,
    /// including course, plan, amounts, status, and installment schedule.
    /// </summary>
    public sealed class SelectionPaymentDto
    {
        /// <summary>Gets or sets the unique identifier of the fee selection.</summary>
        public Guid SelectionId { get; set; }

        /// <summary>Gets or sets the name of the course.</summary>
        public string CourseName { get; set; } = string.Empty;

        /// <summary>Gets or sets the name of the fee plan.</summary>
        public string PlanName { get; set; } = string.Empty;

        /// <summary>Gets or sets the payment type (e.g., full payment, installment).</summary>
        public PaymentType PaymentType { get; set; }

        /// <summary>Gets or sets the total fee amount for the selection.</summary>
        public decimal TotalFee { get; set; }

        /// <summary>Gets or sets the total amount paid for the selection.</summary>
        public decimal TotalPaid { get; set; }

        /// <summary>Gets or sets the total remaining balance for the selection.</summary>
        public decimal TotalRemaining { get; set; }

        /// <summary>Gets or sets the current status of the fee selection.</summary>
        public FeeSelectionStatus Status { get; set; }

        /// <summary>Gets or sets the next due date for payment, if applicable.</summary>
        public DateTime? NextDueDate { get; set; }

        /// <summary>Gets or sets the next due amount for payment.</summary>
        public decimal NextDueAmount { get; set; }

        /// <summary>Gets or sets the collection of installment schedules for the selection.</summary>
        public List<InstallmentScheduleDto> Installments { get; set; } = new();
    }
}
