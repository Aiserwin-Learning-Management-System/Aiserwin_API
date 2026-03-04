namespace Winfocus.LMS.Domain.Entities
{
    using Winfocus.LMS.Domain.Common;

    /// <summary>
    /// FeePlan – represents a pricing plan for a course.
    /// Seasonal discount lives here because it is plan-level, not student-level.
    /// </summary>
    /// <seealso cref="Winfocus.LMS.Domain.Common.BaseEntity" />
    public sealed class FeePlan : BaseEntity
    {
        /// <summary>
        /// Gets the course identifier.
        /// </summary>
        /// <value>The course identifier.</value>
        public Guid CourseId { get; private set; }

        /// <summary>
        /// Gets the name of the plan.
        /// </summary>
        /// <value>The name of the plan.</value>
        public string PlanName { get; private set; } = null!;

        /// <summary>
        /// Gets the tuition fee (base fee). Registration fee is no longer added.
        /// </summary>
        /// <value>The tuition fee.</value>
        public decimal TuitionFee { get; private set; }

        /// <summary>
        /// Gets a value indicating whether installment payment is allowed.
        /// </summary>
        /// <value><c>true</c> if installment is allowed; otherwise, <c>false</c>.</value>
        public bool IsInstallmentAllowed { get; private set; }

        /// <summary>
        /// Gets the course navigation property.
        /// </summary>
        /// <value>The course.</value>
        public Course Course { get; private set; } = null!;

        /// <summary>
        /// Gets the installments collection.
        /// </summary>
        /// <value>The installments.</value>
        public ICollection<FeeInstallment> Installments { get; private set; }
            = new List<FeeInstallment>();

        /// <summary>
        /// Gets the collection of discounts associated with this FeePlan.
        /// </summary>
        /// <value>
        /// A collection of FeePlanDiscount entities.
        /// </value>
        public ICollection<FeePlanDiscount> Discounts { get; private set; }
            = new List<FeePlanDiscount>();

        /// <summary>
        /// Required by EF Core.
        /// </summary>
        private FeePlan()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FeePlan"/> class.
        /// </summary>
        /// <param name="courseId">The associated course identifier.</param>
        /// <param name="planName">The name of the plan.</param>
        /// <param name="tuitionFee">The tuition fee amount.</param>
        /// <param name="isInstallmentAllowed">Indicates whether installment payment is allowed.</param>
        public FeePlan(
            Guid courseId,
            string planName,
            decimal tuitionFee,
            bool isInstallmentAllowed)
        {
            if (string.IsNullOrWhiteSpace(planName))
                throw new ArgumentException("Plan name is required.");

            if (tuitionFee <= 0)
                throw new ArgumentException("Tuition fee must be greater than zero.");

            CourseId = courseId;
            PlanName = planName;
            TuitionFee = tuitionFee;
            IsInstallmentAllowed = isInstallmentAllowed;
        }

        /// <summary>
        /// Updates the fee plan details.
        /// </summary>
        /// <param name="planName">Updated plan name.</param>
        /// <param name="tuitionFee">Updated tuition fee.</param>
        /// <param name="isInstallmentAllowed">Indicates if installment is allowed.</param>
        public void Update(
            string planName,
            decimal tuitionFee,
            bool isInstallmentAllowed)
        {
            if (string.IsNullOrWhiteSpace(planName))
                throw new ArgumentException("Plan name is required.");

            if (tuitionFee <= 0)
                throw new ArgumentException("Tuition fee must be greater than zero.");

            PlanName = planName;
            TuitionFee = tuitionFee;
            IsInstallmentAllowed = isInstallmentAllowed;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
