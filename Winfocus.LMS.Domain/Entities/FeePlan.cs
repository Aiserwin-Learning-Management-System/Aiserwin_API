namespace Winfocus.LMS.Domain.Entities
{
    using Winfocus.LMS.Domain.Common;
    using Winfocus.LMS.Domain.Enums;

    /// <summary>
    /// Represents a fee plan for a course, including tuition fees, duration,
    /// payment type, installment options, and applicable discounts.
    /// </summary>
    public sealed class FeePlan : BaseEntity
    {
        /// <summary>Gets or sets the unique identifier of the associated course.</summary>
        public Guid CourseId { get; set; }

        /// <summary>Gets or sets the name of the fee plan.</summary>
        public string PlanName { get; set; } = null!;

        /// <summary>Gets or sets the tuition fee amount for the plan.</summary>
        public decimal TuitionFee { get; set; }

        /// <summary>Gets or sets the duration of the plan in years.</summary>
        public int DurationinYears { get; set; }

        /// <summary>Gets or sets the unique identifier of the associated subject.</summary>
        public Guid SubjectId { get; set; }

        /// <summary>Gets or sets a value indicating whether installment payments are allowed.</summary>
        public bool IsInstallmentAllowed { get; set; }

        /// <summary>Gets or sets the payment type (e.g., full payment, installment).</summary>
        public PaymentType PaymentType { get; set; }

        /// <summary>Gets or sets the course associated with this fee plan.</summary>
        public Course Course { get; set; } = null!;

        /// <summary>Gets or sets the subject associated with this fee plan.</summary>
        public Subject Subject { get; set; } = null!;

        /// <summary>Gets or sets the collection of installments defined in this fee plan.</summary>
        public ICollection<FeeInstallment> Installments { get; set; }
            = new List<FeeInstallment>();

        /// <summary>Gets or sets the collection of discounts applicable to this fee plan.</summary>
        public ICollection<FeePlanDiscount> Discounts { get; set; }
            = new List<FeePlanDiscount>();

        /// <summary>
        /// Prevents a default instance of the <see cref="FeePlan"/> class from being created.
        /// </summary>
        private FeePlan()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FeePlan"/> class.
        /// </summary>
        /// <param name="courseId">The unique identifier of the course.</param>
        /// <param name="planName">The name of the fee plan.</param>
        /// <param name="tuitionFee">The tuition fee amount.</param>
        /// <param name="isInstallmentAllowed">Indicates whether installment payments are allowed.</param>
        /// <param name="paymentType">The payment type.</param>
        /// <param name="durationinYears">The duration of the plan in years.</param>
        /// <param name="subjectId">The unique identifier of the subject.</param>
        /// <exception cref="ArgumentException">Thrown when plan name is empty or tuition fee is invalid.</exception>
        public FeePlan(
            Guid courseId,
            string planName,
            decimal tuitionFee,
            bool isInstallmentAllowed,
            PaymentType paymentType,
            int durationinYears,
            Guid subjectId)
        {
            if (string.IsNullOrWhiteSpace(planName))
            {
                throw new ArgumentException("Plan name is required.");
            }

            if (tuitionFee <= 0)
            {
                throw new ArgumentException("Tuition fee must be greater than zero.");
            }

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
        /// Updates the details of the fee plan.
        /// </summary>
        /// <param name="planName">The updated plan name.</param>
        /// <param name="tuitionFee">The updated tuition fee amount.</param>
        /// <param name="isInstallmentAllowed">Indicates whether installment payments are allowed.</param>
        /// <param name="paymentType">The updated payment type.</param>
        /// <param name="durationinYears">The updated duration in years.</param>
        /// <param name="subjectId">The updated subject identifier.</param>
        /// <exception cref="ArgumentException">Thrown when plan name is empty or tuition fee is invalid.</exception>
        public void Update(
            string planName,
            decimal tuitionFee,
            bool isInstallmentAllowed,
            PaymentType paymentType,
            int durationinYears,
            Guid subjectId)
        {
            if (string.IsNullOrWhiteSpace(planName))
            {
                throw new ArgumentException("Plan name is required.");
            }

            if (tuitionFee <= 0)
            {
                throw new ArgumentException("Tuition fee must be greater than zero.");
            }

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
