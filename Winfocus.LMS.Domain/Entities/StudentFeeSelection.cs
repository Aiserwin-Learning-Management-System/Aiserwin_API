namespace Winfocus.LMS.Domain.Entities
{
    using Winfocus.LMS.Domain.Common;

    /// <summary>
    /// StudentFeeSelection – stores the fee selection made for a student,
    /// including all three discount types (scholarship, seasonal, manual).
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
        /// <param name="scholarshipPercent">The scholarship discount percent.</param>
        /// <param name="isScholarshipActive">Whether scholarship discount is active.</param>
        /// <param name="seasonalPercent">The seasonal discount percent.</param>
        /// <param name="isSeasonalActive">Whether seasonal discount is active.</param>
        /// <param name="manualDiscountPercent">The manual discount percent.</param>
        /// <param name="isManualDiscountActive">Whether manual discount is active.</param>
        /// <param name="baseFee">The base fee before any discounts.</param>
        /// <param name="finalAmount">The final amount after all active discounts.</param>
        public StudentFeeSelection(
            Guid studentId,
            Guid courseId,
            Guid feePlanId,
            decimal scholarshipPercent,
            bool isScholarshipActive,
            decimal seasonalPercent,
            bool isSeasonalActive,
            decimal manualDiscountPercent,
            bool isManualDiscountActive,
            decimal baseFee,
            decimal finalAmount)
        {
            StudentId = studentId;
            CourseId = courseId;
            FeePlanId = feePlanId;
            ScholarshipPercent = scholarshipPercent;
            IsScholarshipActive = isScholarshipActive;
            SeasonalPercent = seasonalPercent;
            IsSeasonalActive = isSeasonalActive;
            ManualDiscountPercent = manualDiscountPercent;
            IsManualDiscountActive = isManualDiscountActive;
            BaseFee = baseFee;
            FinalAmount = finalAmount;
            PaymentMode = "Offline";
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="StudentFeeSelection"/> class from being created.
        /// </summary>
        private StudentFeeSelection()
        {
        }

        /// <summary>
        /// Gets the student identifier.
        /// </summary>
        /// <value>The student identifier.</value>
        public Guid StudentId { get; private set; }

        /// <summary>
        /// Gets the course identifier.
        /// </summary>
        /// <value>The course identifier.</value>
        public Guid CourseId { get; private set; }

        /// <summary>
        /// Gets the fee plan identifier.
        /// </summary>
        /// <value>The fee plan identifier.</value>
        public Guid FeePlanId { get; private set; }

        /// <summary>
        /// Gets the scholarship discount percent applied to this selection.
        /// </summary>
        /// <value>The scholarship percent.</value>
        public decimal ScholarshipPercent { get; private set; }

        /// <summary>
        /// Gets a value indicating whether scholarship discount was active at time of selection.
        /// </summary>
        /// <value><c>true</c> if scholarship is active; otherwise, <c>false</c>.</value>
        public bool IsScholarshipActive { get; private set; }

        /// <summary>
        /// Gets the seasonal discount percent applied to this selection.
        /// </summary>
        /// <value>The seasonal percent.</value>
        public decimal SeasonalPercent { get; private set; }

        /// <summary>
        /// Gets a value indicating whether seasonal discount was active at time of selection.
        /// </summary>
        /// <value><c>true</c> if seasonal discount is active; otherwise, <c>false</c>.</value>
        public bool IsSeasonalActive { get; private set; }

        /// <summary>
        /// Gets the manual discount percent set by the admin.
        /// </summary>
        /// <value>The manual discount percent.</value>
        public decimal ManualDiscountPercent { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the manual discount is active.
        /// </summary>
        /// <value><c>true</c> if manual discount is active; otherwise, <c>false</c>.</value>
        public bool IsManualDiscountActive { get; private set; }

        /// <summary>
        /// Gets the base fee (tuition fee only, no registration fee).
        /// </summary>
        /// <value>The base fee.</value>
        public decimal BaseFee { get; private set; }

        /// <summary>
        /// Gets the final amount after all active discounts.
        /// </summary>
        /// <value>The final amount.</value>
        public decimal FinalAmount { get; private set; }

        /// <summary>
        /// Gets the payment mode.
        /// </summary>
        /// <value>The payment mode.</value>
        public string PaymentMode { get; private set; } = "Offline";

        /// <summary>
        /// Gets the student navigation property.
        /// </summary>
        /// <value>The student.</value>
        public Student Student { get; private set; } = null!;

        /// <summary>
        /// Gets the fee plan navigation property.
        /// </summary>
        /// <value>The fee plan.</value>
        public FeePlan FeePlan { get; private set; } = null!;

        /// <summary>
        /// Gets the installments collection.
        /// </summary>
        /// <value>The installments.</value>
        public ICollection<StudentInstallment> Installments { get; private set; }
            = new List<StudentInstallment>();

        /// <summary>
        /// Updates the scholarship discount for this selection.
        /// </summary>
        /// <param name="percent">The new scholarship percent.</param>
        /// <param name="isActive">Whether the scholarship is active.</param>
        public void UpdateScholarshipDiscount(decimal percent, bool isActive)
        {
            ScholarshipPercent = percent;
            IsScholarshipActive = isActive;
            UpdatedAt = DateTime.UtcNow;
            RecalculateFinalAmount();
        }

        /// <summary>
        /// Updates the seasonal discount for this selection.
        /// </summary>
        /// <param name="percent">The new seasonal percent.</param>
        /// <param name="isActive">Whether the seasonal discount is active.</param>
        public void UpdateSeasonalDiscount(decimal percent, bool isActive)
        {
            SeasonalPercent = percent;
            IsSeasonalActive = isActive;
            UpdatedAt = DateTime.UtcNow;
            RecalculateFinalAmount();
        }

        /// <summary>
        /// Updates the manual discount for this selection.
        /// </summary>
        /// <param name="percent">The new manual discount percent.</param>
        /// <param name="isActive">Whether the manual discount is active.</param>
        public void UpdateManualDiscount(decimal percent, bool isActive)
        {
            ManualDiscountPercent = percent;
            IsManualDiscountActive = isActive;
            UpdatedAt = DateTime.UtcNow;
            RecalculateFinalAmount();
        }

        /// <summary>
        /// Recalculates the final amount based on currently active discounts.
        /// Order: Scholarship → Seasonal → Manual (each applied sequentially).
        /// </summary>
        public void RecalculateFinalAmount()
        {
            var amount = BaseFee;

            if (IsScholarshipActive && ScholarshipPercent > 0)
            {
                amount -= amount * ScholarshipPercent / 100m;
            }

            if (IsSeasonalActive && SeasonalPercent > 0)
            {
                amount -= amount * SeasonalPercent / 100m;
            }

            if (IsManualDiscountActive && ManualDiscountPercent > 0)
            {
                amount -= amount * ManualDiscountPercent / 100m;
            }

            FinalAmount = Math.Max(amount, 0);
        }
    }
}
