namespace Winfocus.LMS.Application.DTOs.Fees
{
    /// <summary>
    /// Request DTO used when an admin records a payment
    /// against a specific student installment.
    /// </summary>
    public sealed class RecordPaymentRequestDto
    {
        /// <summary>
        /// Gets or sets the amount paid by the student.
        /// Must be greater than zero.
        /// </summary>
        public decimal AmountPaid { get; set; }

        /// <summary>
        /// Gets or sets the date when the payment was made.
        /// </summary>
        public DateTime PaymentDate { get; set; }

        /// <summary>
        /// Gets or sets the payment method used (e.g., Online, Cash, Card).
        /// </summary>
        public string? PaymentMethod { get; set; }

        /// <summary>
        /// Gets or sets optional remarks or notes associated with the payment.
        /// </summary>
        public string? Remarks { get; set; }
    }
}
