namespace Winfocus.LMS.Domain.Entities
{
    using Winfocus.LMS.Domain.Common;

    /// <summary>
    /// Represents an admin-assigned discount for a student on a specific course.
    /// This entity captures both plan-based discounts (selected via checkbox)
    /// and manual discounts (entered individually by an admin).
    /// </summary>
    public sealed class StudentCourseDiscount : BaseEntity
    {
        /// <summary>Gets the unique identifier of the student.</summary>
        public Guid StudentId { get; private set; }

        /// <summary>Gets the unique identifier of the course.</summary>
        public Guid CourseId { get; private set; }

        /// <summary>
        /// Gets the unique identifier of the fee plan discount, if applicable.
        /// Null when the discount is manually created.
        /// </summary>
        public Guid? FeePlanDiscountId { get; private set; }

        /// <summary>Gets the name of the discount.</summary>
        public string DiscountName { get; private set; } = null!;

        /// <summary>Gets the discount percentage applied.</summary>
        public decimal DiscountPercent { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the discount was manually created
        /// (true) or derived from a predefined fee plan discount (false).
        /// </summary>
        public bool IsManual { get; private set; }

        /// <summary>Gets the student associated with this discount.</summary>
        public Student Student { get; private set; } = null!;

        /// <summary>Gets the course associated with this discount.</summary>
        public Course Course { get; private set; } = null!;

        /// <summary>
        /// Gets the fee plan discount associated with this discount, if applicable.
        /// </summary>
        public FeePlanDiscount? FeePlanDiscount { get; private set; }

        /// <summary>
        /// Prevents a default instance of the <see cref="StudentCourseDiscount"/> class from being created.
        /// </summary>
        private StudentCourseDiscount() { }

        /// <summary>
        /// Creates a discount from an existing <see cref="FeePlanDiscount"/>.
        /// Typically used when an admin selects a predefined discount via checkbox.
        /// </summary>
        /// <param name="studentId">The unique identifier of the student.</param>
        /// <param name="courseId">The unique identifier of the course.</param>
        /// <param name="planDiscount">The fee plan discount to apply.</param>
        /// <param name="createdBy">The identifier of the admin who created the discount.</param>
        /// <returns>A new <see cref="StudentCourseDiscount"/> instance.</returns>
        public static StudentCourseDiscount FromPlanDiscount(
            Guid studentId, Guid courseId, FeePlanDiscount planDiscount, Guid createdBy)
        {
            return new StudentCourseDiscount
            {
                StudentId = studentId,
                CourseId = courseId,
                FeePlanDiscountId = planDiscount.Id,
                DiscountName = planDiscount.DiscountName,
                DiscountPercent = planDiscount.DiscountPercent,
                IsManual = false,
                CreatedBy = createdBy,
                CreatedAt = DateTime.UtcNow,
            };
        }

        /// <summary>
        /// Creates a manual discount for a student on a course.
        /// Typically used when an admin enters a custom discount name and percentage.
        /// </summary>
        /// <param name="studentId">The unique identifier of the student.</param>
        /// <param name="courseId">The unique identifier of the course.</param>
        /// <param name="name">The name of the discount.</param>
        /// <param name="percent">The discount percentage (between 0 and 100).</param>
        /// <param name="createdBy">The identifier of the admin who created the discount.</param>
        /// <returns>A new <see cref="StudentCourseDiscount"/> instance.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown when the discount name is empty or the percentage is invalid.
        /// </exception>
        public static StudentCourseDiscount Manual(
            Guid studentId, Guid courseId, string name, decimal percent, Guid createdBy)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Discount name is required.");
            if (percent <= 0 || percent > 100)
                throw new ArgumentException("Percent must be between 0 and 100.");

            return new StudentCourseDiscount
            {
                StudentId = studentId,
                CourseId = courseId,
                FeePlanDiscountId = null,
                DiscountName = name,
                DiscountPercent = percent,
                IsManual = true,
                CreatedBy = createdBy,
                CreatedAt = DateTime.UtcNow,
            };
        }
    }
}
