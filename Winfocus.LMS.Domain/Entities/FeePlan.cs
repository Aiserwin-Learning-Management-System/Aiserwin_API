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
        /// Gets the registration fee. Kept for historical data but excluded from calculations.
        /// </summary>
        /// <value>The registration fee.</value>
        public decimal RegistrationFee { get; private set; }

        /// <summary>
        /// Gets the scholarship percent (plan-level default; overridden at student level).
        /// </summary>
        /// <value>The scholarship percent.</value>
        public decimal ScholarshipPercent { get; private set; }

        /// <summary>
        /// Gets the seasonal discount percent.
        /// </summary>
        /// <value>The seasonal percent.</value>
        public decimal SeasonalPercent { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the seasonal discount is currently active.
        /// </summary>
        /// <value><c>true</c> if the seasonal discount is active; otherwise, <c>false</c>.</value>
        public bool IsSeasonalDiscountActive { get; private set; }

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
        /// Updates the seasonal discount settings.
        /// </summary>
        /// <param name="percent">The seasonal discount percent.</param>
        /// <param name="isActive">Whether the seasonal discount is active.</param>
        public void UpdateSeasonalDiscount(decimal percent, bool isActive)
        {
            SeasonalPercent = percent;
            IsSeasonalDiscountActive = isActive;
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Updates the scholarship percent on the plan (default value).
        /// </summary>
        /// <param name="percent">The scholarship percent.</param>
        public void UpdateScholarshipPercent(decimal percent)
        {
            ScholarshipPercent = percent;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
