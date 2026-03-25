namespace Winfocus.LMS.Domain.Entities
{
    using Winfocus.LMS.Domain.Common;

    /// <summary>
    /// Represents an immutable snapshot of a discount applied to a student's fee selection
    /// at the moment the student confirmed their enrollment.
    /// This ensures that discounts are preserved historically, even if plan discounts change later.
    /// </summary>
    public sealed class StudentFeeDiscount : BaseEntity
    {
        /// <summary>Gets the unique identifier of the associated fee selection.</summary>
        public Guid StudentFeeSelectionId { get; private set; }

        /// <summary>Gets the name of the discount applied.</summary>
        public string DiscountName { get; private set; } = null!;

        /// <summary>Gets the discount percentage applied.</summary>
        public decimal DiscountPercent { get; private set; }

        /// <summary>Gets the discount amount applied.</summary>
        public decimal DiscountAmount { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the discount was manually created
        /// (true) or derived from a predefined fee plan discount (false).
        /// </summary>
        public bool IsManual { get; private set; }

        /// <summary>Gets the fee selection associated with this discount.</summary>
        public StudentFeeSelection StudentFeeSelection { get; private set; } = null!;

        /// <summary>
        /// Prevents a default instance of the <see cref="StudentFeeDiscount"/> class from being created.
        /// </summary>
        private StudentFeeDiscount() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="StudentFeeDiscount"/> class.
        /// </summary>
        /// <param name="selectionId">The unique identifier of the fee selection.</param>
        /// <param name="name">The name of the discount.</param>
        /// <param name="percent">The discount percentage applied.</param>
        /// <param name="amount">The discount amount applied.</param>
        /// <param name="isManual">Indicates whether the discount was manually created.</param>
        public StudentFeeDiscount(
            Guid selectionId, string name, decimal percent,
            decimal amount, bool isManual)
        {
            StudentFeeSelectionId = selectionId;
            DiscountName = name;
            DiscountPercent = percent;
            DiscountAmount = amount;
            IsManual = isManual;
            CreatedAt = DateTime.UtcNow;
        }
    }
}
