namespace Winfocus.LMS.Application.DTOs.Fees
{
    /// <summary>
    /// Complete fee page response for student portal.
    /// </summary>
    public class StudentFeePageDto
    {
        /// <summary>
        /// Gets or sets the student identifier.
        /// </summary>
        /// <value>
        /// The student identifier.
        /// </value>
        public Guid StudentId { get; set; }

        /// <summary>
        /// Gets or sets the name of the student.
        /// </summary>
        /// <value>
        /// The name of the student.
        /// </value>
        public string StudentName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the registration number.
        /// </summary>
        /// <value>
        /// The registration number.
        /// </value>
        public string RegistrationNumber { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the fee listings.
        /// </summary>
        /// <value>
        /// The fee listings.
        /// </value>
        public List<FeeListingRowDto> FeeListings { get; set; } = new();

        /// <summary>
        /// Gets or sets the selected fee plan identifier.
        /// </summary>
        /// <value>
        /// The selected fee plan identifier.
        /// </value>
        public Guid? SelectedFeePlanId { get; set; }
    }

    /// <summary>
    /// Single row in the fee listing table.
    /// Each row = Course + PaymentType + Duration combination.
    /// </summary>
    public class FeeListingRowDto
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
        public string CourseName { get; set; } = string.Empty;

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
        public string PaymentType { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the duration in years.
        /// </summary>
        /// <value>
        /// The duration in years.
        /// </value>
        public int DurationInYears { get; set; }

        /// <summary>
        /// Gets or sets the discount percent.
        /// </summary>
        /// <value>
        /// The discount percent.
        /// </value>
        public decimal DiscountPercent { get; set; }

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

        /// <summary>
        /// Gets or sets the discounts.
        /// </summary>
        /// <value>
        /// The discounts.
        /// </value>
        public List<DiscountBadgeDto> Discounts { get; set; } = new();
    }

    /// <summary>
    /// DiscountBadgeDto.
    /// </summary>
    public class DiscountBadgeDto
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the percent.
        /// </summary>
        /// <value>
        /// The percent.
        /// </value>
        public decimal Percent { get; set; }
    }
}
