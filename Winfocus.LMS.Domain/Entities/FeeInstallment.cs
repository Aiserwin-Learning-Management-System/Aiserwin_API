namespace Winfocus.LMS.Domain.Entities
{
    using Winfocus.LMS.Domain.Common;

    /// <summary>
    /// FeeInstallment.
    /// </summary>
    /// <seealso cref="Winfocus.LMS.Domain.Common.BaseEntity" />
    public sealed class FeeInstallment : BaseEntity
    {
        /// <summary>
        /// Gets the fee plan identifier.
        /// </summary>
        /// <value>
        /// The fee plan identifier.
        /// </value>
        public Guid FeePlanId { get; private set; }

        /// <summary>
        /// Gets the installment no.
        /// </summary>
        /// <value>
        /// The installment no.
        /// </value>
        public int InstallmentNo { get; private set; }

        /// <summary>
        /// Gets the amount.
        /// </summary>
        /// <value>
        /// The amount.
        /// </value>
        public decimal Amount { get; private set; }

        /// <summary>
        /// Gets the due after days.
        /// </summary>
        /// <value>
        /// The due after days.
        /// </value>
        public int DueAfterDays { get; private set; }
    }
}
