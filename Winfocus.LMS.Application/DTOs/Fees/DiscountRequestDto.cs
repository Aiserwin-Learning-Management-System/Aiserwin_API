namespace Winfocus.LMS.Application.DTOs.Fees
{
    /// <summary>
    /// Response DTO for a student discount request entry in the admin list.
    /// </summary>
    public sealed class DiscountRequestDto
    {
        /// <summary>Gets or sets the unique identifier of the student.</summary>
        public Guid StudentId { get; set; }

        /// <summary>Gets or sets the name of the student.</summary>
        public string StudentName { get; set; } = string.Empty;

        /// <summary>Gets or sets the registration number of the student.</summary>
        public string RegistrationNumber { get; set; } = string.Empty;

        /// <summary>Gets or sets the grade name of the student.</summary>
        public string GradeName { get; set; } = string.Empty;

        /// <summary>Gets or sets the date when the discount request was submitted.</summary>
        public DateTime RequestedAt { get; set; }
    }
}
