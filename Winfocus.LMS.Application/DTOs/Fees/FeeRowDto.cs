namespace Winfocus.LMS.Application.DTOs.Fees
{
    /// <summary>
    /// FeeRowDto.
    /// </summary>
    public sealed class FeeRowDto
    {
        /// <summary>
        /// Gets or sets the fee plan identifier.
        /// </summary>
        /// <value>
        /// The fee plan identifier.
        /// </value>
        public Guid FeePlanId { get; set; }

        /// <summary>
        /// Gets or sets the course identifier.
        /// </summary>
        /// <value>
        /// The course identifier.
        /// </value>
        public Guid CourseId { get; set; }

        /// <summary>
        /// Gets or sets the name of the course.
        /// </summary>
        /// <value>
        /// The name of the course.
        /// </value>
        public string CourseName { get; set; } = null!;

        /// <summary>
        /// Gets or sets the yearly fee.
        /// </summary>
        /// <value>
        /// The yearly fee.
        /// </value>
        public decimal YearlyFee { get; set; }

        /// <summary>
        /// Gets or sets the type of the payment.
        /// </summary>
        /// <value>
        /// The type of the payment.
        /// </value>
        public string PaymentType { get; set; } = null!;

        /// <summary>
        /// Gets or sets the scholarship percent.
        /// </summary>
        /// <value>
        /// The scholarship percent.
        /// </value>
        public decimal ScholarshipPercent { get; set; }

        /// <summary>
        /// Gets or sets the seasonal percent.
        /// </summary>
        /// <value>
        /// The seasonal percent.
        /// </value>
        public decimal SeasonalPercent { get; set; }

        /// <summary>
        /// Gets or sets the fee after discount.
        /// </summary>
        /// <value>
        /// The fee after discount.
        /// </value>
        public decimal FeeAfterDiscount { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is selected.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is selected; otherwise, <c>false</c>.
        /// </value>
        public bool IsSelected { get; set; }
    }
}
