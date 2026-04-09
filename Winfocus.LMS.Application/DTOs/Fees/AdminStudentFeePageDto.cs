namespace Winfocus.LMS.Application.DTOs.Fees
{
    /// <summary>
    /// Response DTO for the admin student fee management page.
    /// Provides student details, course discounts, and total payable amount.
    /// </summary>
    public sealed class AdminStudentFeePageDto
    {
        /// <summary>Gets or sets the unique identifier of the student.</summary>
        public Guid StudentId { get; set; }

        /// <summary>Gets or sets the name of the student.</summary>
        public string StudentName { get; set; } = string.Empty;

        /// <summary>Gets or sets the registration number of the student.</summary>
        public string RegistrationNumber { get; set; } = string.Empty;

        /// <summary>Gets or sets the name of the grade.</summary>
        public string GradeName { get; set; } = string.Empty;

        /// <summary>Gets or sets the name of the syllabus.</summary>
        public string SyllabusName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the collection of course discount blocks
        /// available for the student on the admin page.
        /// </summary>
        public List<CourseDiscountBlockDto> CourseDiscounts { get; set; } = new();

        /// <summary>Gets or sets the total payable amount after discounts.</summary>
        public decimal TotalPayable { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the student has requested
        /// a manual discount from the admin.
        /// </summary>
        public bool HasManualDiscountRequest { get; set; }
    }

    /// <summary>
    /// Represents a single course block on the admin fee management page,
    /// including base fee, available discounts, assigned discount, and calculated fee.
    /// </summary>
    public sealed class CourseDiscountBlockDto
    {
        /// <summary>Gets or sets the unique identifier of the course.</summary>
        public Guid CourseId { get; set; }

        /// <summary>Gets or sets the name of the course.</summary>
        public string CourseName { get; set; } = string.Empty;

        /// <summary>Gets or sets the base yearly fee for the course.</summary>
        public decimal BaseYearlyFee { get; set; }

        /// <summary>
        /// Gets or sets the available plan discounts (radio buttons — pick one).
        /// </summary>
        public List<AvailableDiscountDto> AvailableDiscounts { get; set; } = new();

        /// <summary>
        /// Gets or sets the currently assigned discount (only one).
        /// </summary>
        public AssignedDiscountDto? AssignedDiscount { get; set; }

        /// <summary>
        /// Gets or sets the calculated fee after applying discounts.
        /// </summary>
        public decimal CalculatedFeeAfterDiscount { get; set; }
    }

    /// <summary>
    /// Represents a single available discount option (radio button)
    /// for a course on the admin fee management page.
    /// </summary>
    public sealed class AvailableDiscountDto
    {
        /// <summary>Gets or sets the unique identifier of the fee plan discount.</summary>
        public Guid FeePlanDiscountId { get; set; }

        /// <summary>Gets or sets the name of the discount.</summary>
        public string DiscountName { get; set; } = string.Empty;

        /// <summary>Gets or sets the discount percentage.</summary>
        public decimal DiscountPercent { get; set; }

        /// <summary>Gets or sets the discount amount.</summary>
        public decimal DiscountAmount { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the discount
        /// is currently assigned to the student.
        /// </summary>
        public bool IsAssigned { get; set; }
    }

    /// <summary>
    /// Represents the one discount currently assigned to a student for a specific course.
    /// Can be either a plan-based discount or a manual discount.
    /// </summary>
    public sealed class AssignedDiscountDto
    {
        /// <summary>Gets or sets the name of the assigned discount.</summary>
        public string DiscountName { get; set; } = string.Empty;

        /// <summary>Gets or sets the discount percentage applied.</summary>
        public decimal DiscountPercent { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the discount
        /// was manually created (true) or derived from a predefined plan (false).
        /// </summary>
        public bool IsManual { get; set; }
    }
}
