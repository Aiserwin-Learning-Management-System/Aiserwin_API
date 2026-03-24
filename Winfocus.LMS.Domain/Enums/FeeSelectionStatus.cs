namespace Winfocus.LMS.Domain.Enums
{
    /// <summary>
    /// Represents the lifecycle status of a student's fee selection.
    /// </summary>
    public enum FeeSelectionStatus
    {
        /// <summary>Fee selection created but not yet confirmed by student.</summary>
        Pending = 1,

        /// <summary>Student has confirmed the selection; installments generated.</summary>
        Confirmed = 2,

        /// <summary>At least one installment has been paid.</summary>
        PartiallyPaid = 3,

        /// <summary>All installments have been fully paid.</summary>
        Paid = 4,

        /// <summary>Fee selection has been cancelled by admin.</summary>
        Cancelled = 5,
    }
}
