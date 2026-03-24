namespace Winfocus.LMS.Application.DTOs.Fees
{
    /// <summary>
    /// Request DTO used when an admin assigns discounts to a student for a specific course.
    /// This replaces all previous discount assignments for the given student and course.
    /// </summary>
    public sealed class AssignDiscountsRequestDto
    {
        /// <summary>Gets or sets the unique identifier of the student.</summary>
        public Guid StudentId { get; set; }

        /// <summary>Gets or sets the unique identifier of the course.</summary>
        public Guid CourseId { get; set; }

        /// <summary>
        /// Gets or sets the collection of <see cref="FeePlanDiscount"/> IDs
        /// that the admin selected (checked) for the student.
        /// </summary>
        public List<Guid> SelectedDiscountIds { get; set; } = new();

        /// <summary>
        /// Gets or sets an optional manual discount entered by the admin.
        /// If provided, this represents an individual discount not tied to a predefined plan.
        /// </summary>
        public ManualDiscountRequestDto? ManualDiscount { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the admin user assigning the discount.
        /// This is typically set by the controller from the JWT token.
        /// </summary>
        public Guid UserId { get; set; }
    }

    /// <summary>
    /// Represents a manual discount request entered by an admin,
    /// including the discount name and percentage.
    /// </summary>
    public sealed class ManualDiscountRequestDto
    {
        /// <summary>Gets or sets the name of the manual discount.</summary>
        public string DiscountName { get; set; } = null!;

        /// <summary>
        /// Gets or sets the discount percentage applied.
        /// Must be greater than 0 and less than or equal to 100.
        /// </summary>
        public decimal DiscountPercent { get; set; }
    }
}
