// Domain/Entities/StudentInstallment.cs
namespace Winfocus.LMS.Domain.Entities
{
    using Winfocus.LMS.Domain.Common;
    using Winfocus.LMS.Domain.Enums;

    /// <summary>
    /// Represents an installment payment for a student's fee selection,
    /// including due amount, payment details, balance, and status.
    /// </summary>
    public sealed class StudentInstallment : BaseEntity
    {
        /// <summary>Gets the unique identifier of the associated fee selection.</summary>
        public Guid StudentFeeSelectionId { get; private set; }

        /// <summary>Gets the installment number (sequence).</summary>
        public int InstallmentNo { get; private set; }

        /// <summary>Gets the due amount for this installment.</summary>
        public decimal DueAmount { get; private set; }

        /// <summary>Gets the due date for this installment.</summary>
        public DateTime DueDate { get; private set; }

        /// <summary>Gets the total amount paid towards this installment.</summary>
        public decimal PaidAmount { get; private set; }

        /// <summary>Gets the date when payment was made, if any.</summary>
        public DateTime? PaidDate { get; private set; }

        /// <summary>Gets the remaining balance amount for this installment.</summary>
        public decimal BalanceAmount { get; private set; }

        /// <summary>Gets the current status of the installment (e.g., Pending, Paid, Overdue).</summary>
        public InstallmentStatus Status { get; private set; }

        /// <summary>Gets any remarks or notes associated with this installment.</summary>
        public string? Remarks { get; private set; }

        /// <summary>Gets the fee selection associated with this installment.</summary>
        public StudentFeeSelection StudentFeeSelection { get; private set; } = null!;

        /// <summary>
        /// Prevents a default instance of the <see cref="StudentInstallment"/> class from being created.
        /// </summary>
        private StudentInstallment() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="StudentInstallment"/> class.
        /// </summary>
        /// <param name="selectionId">The unique identifier of the fee selection.</param>
        /// <param name="installmentNo">The installment number.</param>
        /// <param name="dueAmount">The due amount for this installment.</param>
        /// <param name="dueDate">The due date for this installment.</param>
        public StudentInstallment(
            Guid selectionId, int installmentNo,
            decimal dueAmount, DateTime dueDate)
        {
            StudentFeeSelectionId = selectionId;
            InstallmentNo = installmentNo;
            DueAmount = dueAmount;
            DueDate = dueDate;
            PaidAmount = 0;
            BalanceAmount = dueAmount;
            Status = InstallmentStatus.Pending;
            CreatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Records a payment against this installment.
        /// </summary>
        /// <param name="amount">The payment amount.</param>
        /// <param name="paidDate">The date of payment.</param>
        /// <param name="remarks">Optional remarks for the payment.</param>
        /// <exception cref="ArgumentException">Thrown when the payment amount is not positive.</exception>
        public void RecordPayment(decimal amount, DateTime paidDate, string? remarks)
        {
            if (amount <= 0)
                throw new ArgumentException("Payment amount must be positive.");

            PaidAmount += amount;
            PaidDate = paidDate;
            BalanceAmount = Math.Max(DueAmount - PaidAmount, 0);
            Remarks = remarks;

            Status = PaidAmount >= DueAmount
                ? InstallmentStatus.Paid
                : InstallmentStatus.Pending;

            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Marks the installment as overdue if it has not been fully paid.
        /// </summary>
        public void MarkOverdue()
        {
            if (Status != InstallmentStatus.Paid)
            {
                Status = InstallmentStatus.Overdue;
                UpdatedAt = DateTime.UtcNow;
            }
        }
    }
}
