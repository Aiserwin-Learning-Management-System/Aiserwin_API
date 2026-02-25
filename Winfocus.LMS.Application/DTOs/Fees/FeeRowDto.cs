namespace Winfocus.LMS.Application.DTOs.Fees
{
    /// <summary>
    /// FeeRowDto – single row in the pricing table returned to the UI.
    /// </summary>
    public sealed class FeeRowDto
    {
        /// <summary>
        /// Gets or sets the fee plan identifier.
        /// </summary>
        /// <value>The fee plan identifier.</value>
        public Guid FeePlanId { get; set; }

        /// <summary>
        /// Gets or sets the course identifier.
        /// </summary>
        /// <value>The course identifier.</value>
        public Guid CourseId { get; set; }

        /// <summary>
        /// Gets or sets the name of the course.
        /// </summary>
        /// <value>The name of the course.</value>
        public string CourseName { get; set; } = null!;

        /// <summary>
        /// Gets or sets the base fee (tuition only – registration fee excluded).
        /// </summary>
        /// <value>The base fee.</value>
        public decimal BaseFee { get; set; }

        /// <summary>
        /// Gets or sets the type of the payment plan.
        /// </summary>
        /// <value>The type of the payment.</value>
        public string PaymentType { get; set; } = null!;

        /// <summary>
        /// Gets or sets the scholarship discount percent.
        /// </summary>
        /// <value>The scholarship percent.</value>
        public decimal ScholarshipPercent { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether scholarship discount is active.
        /// </summary>
        /// <value><c>true</c> if scholarship is active; otherwise, <c>false</c>.</value>
        public bool IsScholarshipActive { get; set; }

        /// <summary>
        /// Gets or sets the seasonal discount percent.
        /// </summary>
        /// <value>The seasonal percent.</value>
        public decimal SeasonalPercent { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether seasonal discount is active.
        /// </summary>
        /// <value><c>true</c> if seasonal discount is active; otherwise, <c>false</c>.</value>
        public bool IsSeasonalActive { get; set; }

        /// <summary>
        /// Gets or sets the manual discount percent (from a prior admin entry if any).
        /// </summary>
        /// <value>The manual discount percent.</value>
        public decimal ManualDiscountPercent { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether manual discount is active.
        /// </summary>
        /// <value><c>true</c> if manual discount is active; otherwise, <c>false</c>.</value>
        public bool IsManualDiscountActive { get; set; }

        /// <summary>
        /// Gets or sets the fee after all active discounts.
        /// </summary>
        /// <value>The fee after discount.</value>
        public decimal FeeAfterDiscount { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this course is selected by the student.
        /// </summary>
        /// <value><c>true</c> if this instance is selected; otherwise, <c>false</c>.</value>
        public bool IsSelected { get; set; }
    }
}
