namespace Winfocus.LMS.Domain.Enums
{
    /// <summary>
    /// Represents the payment status of a single installment.
    /// </summary>
    public enum InstallmentStatus
    {
        /// <summary>Payment has not been made yet.</summary>
        Pending = 1,

        /// <summary>Full payment has been received.</summary>
        Paid = 2,

        /// <summary>Due date has passed without full payment.</summary>
        Overdue = 3,
    }
}
