namespace Winfocus.LMS.Domain.Entities
{
    using Winfocus.LMS.Domain.Common;

    /// <summary>
    /// StudentFeeSelection.
    /// </summary>
    /// <seealso cref="Winfocus.LMS.Domain.Common.BaseEntity" />
    public sealed class StudentFeeSelection : BaseEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StudentFeeSelection"/> class.
        /// </summary>
        /// <param name="studentId">The student identifier.</param>
        /// <param name="courseId">The course identifier.</param>
        /// <param name="feePlanId">The fee plan identifier.</param>
        /// <param name="manualDiscountPercent">The manual discount percent.</param>
        /// <param name="finalAmount">The final amount.</param>
        public StudentFeeSelection(
            Guid studentId,
            Guid courseId,
            Guid feePlanId,
            decimal manualDiscountPercent,
            decimal finalAmount)
        {
            StudentId = studentId;
            CourseId = courseId;
            FeePlanId = feePlanId;
            ManualDiscountPercent = manualDiscountPercent;
            FinalAmount = finalAmount;
            PaymentMode = "Offline";
        }

        private StudentFeeSelection()
        {
        }

        /// <summary>
        /// Gets the student identifier.
        /// </summary>
        /// <value>
        /// The student identifier.
        /// </value>
        public Guid StudentId { get; private set; }

        /// <summary>
        /// Gets the course identifier.
        /// </summary>
        /// <value>
        /// The course identifier.
        /// </value>
        public Guid CourseId { get; private set; }

        /// <summary>
        /// Gets the fee plan identifier.
        /// </summary>
        /// <value>
        /// The fee plan identifier.
        /// </value>
        public Guid FeePlanId { get; private set; }

        /// <summary>
        /// Gets the manual discount percent.
        /// </summary>
        /// <value>
        /// The manual discount percent.
        /// </value>
        public decimal ManualDiscountPercent { get; private set; }

        /// <summary>
        /// Gets the final amount.
        /// </summary>
        /// <value>
        /// The final amount.
        /// </value>
        public decimal FinalAmount { get; private set; }

        /// <summary>
        /// Gets the payment mode.
        /// </summary>
        /// <value>
        /// The payment mode.
        /// </value>
        public string PaymentMode { get; private set; } = "Offline";

        /// <summary>
        /// Gets the installments.
        /// </summary>
        /// <value>
        /// The installments.
        /// </value>
        public ICollection<StudentInstallment> Installments { get; private set; }
            = new List<StudentInstallment>();
    }
}
