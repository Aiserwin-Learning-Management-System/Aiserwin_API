namespace Winfocus.LMS.Domain.Enums
{
    /// <summary>
    /// Represents the type of discount applied to a fee.
    /// Business Rule: Only one discount type can be active at a time.
    /// </summary>
    public enum DiscountType
    {
        /// <summary>
        /// No discount is applied.
        /// The student pays the full base amount (tuition + registration).
        /// </summary>
        None = 0,

        /// <summary>
        /// Scholarship discount is applied.
        /// This discount is earned when a student scores above 50% in scholarship exams.
        /// </summary>
        Scholarship = 1,

        /// <summary>
        /// Seasonal discount is applied.
        /// This is a common promotional discount available during specific periods.
        /// </summary>
        Seasonal = 2,

        /// <summary>
        /// Manual discount is applied.
        /// This is a discretionary discount applied by administrators for special cases.
        /// </summary>
        Manual = 3,
    }
}
