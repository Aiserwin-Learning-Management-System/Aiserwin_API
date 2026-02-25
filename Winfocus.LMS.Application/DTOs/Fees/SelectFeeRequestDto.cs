namespace Winfocus.LMS.Application.DTOs.Fees
{
    /// <summary>
    /// SelectFeeRequestDto – request body for selecting a fee plan for a student.
    /// </summary>
    public sealed class SelectFeeRequestDto
    {
        /// <summary>
        /// Gets or sets the student identifier.
        /// </summary>
        /// <value>The student identifier.</value>
        public Guid StudentId { get; set; }

        /// <summary>
        /// Gets or sets the fee plan identifier.
        /// </summary>
        /// <value>The fee plan identifier.</value>
        public Guid FeePlanId { get; set; }

        /// <summary>
        /// Gets or sets the scholarship discount percent override (optional).
        /// </summary>
        /// <value>The scholarship percent.</value>
        public decimal? ScholarshipPercent { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether scholarship discount is active.
        /// Defaults to <c>true</c>.
        /// </summary>
        /// <value><c>true</c> if scholarship is active; otherwise, <c>false</c>.</value>
        public bool IsScholarshipActive { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether seasonal discount is active.
        /// Defaults to <c>true</c>.
        /// </summary>
        /// <value><c>true</c> if seasonal discount is active; otherwise, <c>false</c>.</value>
        public bool IsSeasonalActive { get; set; } = true;

        /// <summary>
        /// Gets or sets the manual discount percent set by the admin.
        /// </summary>
        /// <value>The manual discount percent.</value>
        public decimal ManualDiscountPercent { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the manual discount is active.
        /// </summary>
        /// <value><c>true</c> if manual discount is active; otherwise, <c>false</c>.</value>
        public bool IsManualDiscountActive { get; set; }
    }
}
