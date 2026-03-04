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
        /// Gets or sets the course identifier.
        /// </summary>
        /// <value>The course identifier.</value>
        public Guid CourseId { get; set; }

        /// <summary>
        /// Gets or sets the name of the plan.
        /// </summary>
        /// <value>The name of the plan.</value>
        public string PlanName { get; set; } = null!;

        /// <summary>
        /// Gets or sets the tuition fee (base fee). Registration fee is no longer added.
        /// </summary>
        /// <value>The tuition fee.</value>
        public decimal TuitionFee { get; set; }

        /// <summary>
        /// Gets or sets the course duration in years.
        /// </summary>
        /// <value>course duration in years.</value>
        public int DurationinYears { get; set; }

        /// <summary>
        /// Gets or sets the Subject identifier.
        /// </summary>
        /// <value>The Subject identifier.</value>
        public Guid SubjectId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether installment payment is allowed.
        /// </summary>
        /// <value><c>true</c> if installment is allowed; otherwise, <c>false</c>.</value>
        public bool IsInstallmentAllowed { get; set; }

        /// <summary>
        /// Gets or sets the type of the payment.
        /// </summary>
        /// <value>The type of the payment.</value>
        public string PaymentType { get; set; } = null!;

        /// <summary>
        /// Gets or sets the course navigation property.
        /// </summary>
        /// <value>The course.</value>
        public Course Course { get; set; } = null!;

        /// <summary>
        /// Gets or sets the Subject navigation property.
        /// </summary>
        /// <value>The Subject.</value>
        public Subject Subject { get; set; } = null!;

        /// <summary>
        /// Gets or sets the installments collection.
        /// </summary>
        /// <value>The installments.</value>
        public ICollection<FeeInstallment> Installments { get; set; }
            = new List<FeeInstallment>();

        /// <summary>
        /// Gets or sets the collection of discounts associated with this FeePlan.
        /// </summary>
        /// <value>
        /// A collection of FeePlanDiscount entities.
        /// </value>
        public ICollection<FeePlanDiscount> Discounts { get; set; }
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
        /// <param name="paymentType">paymentType.</param>
        /// <param name="durationinYears">durationinYears.</param>
        /// <param name="subjectId">subjectId.</param>
        public FeePlan(
            Guid courseId,
            string planName,
            decimal tuitionFee,
            bool isInstallmentAllowed,
            string paymentType,
            int durationinYears,
            Guid subjectId)
        {
            if (string.IsNullOrWhiteSpace(planName))
                throw new ArgumentException("Plan name is required.");

            if (tuitionFee <= 0)
                throw new ArgumentException("Tuition fee must be greater than zero.");

            CourseId = courseId;
            PlanName = planName;
            TuitionFee = tuitionFee;
            IsInstallmentAllowed = isInstallmentAllowed;
            PaymentType = paymentType;
            SubjectId = subjectId;
            DurationinYears = durationinYears;
            CreatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Updates the fee plan details.
        /// </summary>
        /// <param name="planName">Updated plan name.</param>
        /// <param name="tuitionFee">Updated tuition fee.</param>
        /// <param name="isInstallmentAllowed">Indicates if installment is allowed.</param>
        /// <param name="paymentType">paymentType.</param>
        /// <param name="durationinYears">durationinYears.</param>
        /// <param name="subjectId">subjectId.</param>
        public void Update(
            string planName,
            decimal tuitionFee,
            bool isInstallmentAllowed,
            string paymentType,
            int durationinYears,
            Guid subjectId)
        {
            if (string.IsNullOrWhiteSpace(planName))
                throw new ArgumentException("Plan name is required.");

            if (tuitionFee <= 0)
                throw new ArgumentException("Tuition fee must be greater than zero.");

            PlanName = planName;
            TuitionFee = tuitionFee;
            IsInstallmentAllowed = isInstallmentAllowed;
            PaymentType = paymentType;
            DurationinYears = durationinYears;
            SubjectId = subjectId;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
