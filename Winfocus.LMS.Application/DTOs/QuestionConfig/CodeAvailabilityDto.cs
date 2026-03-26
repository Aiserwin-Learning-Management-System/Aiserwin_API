namespace Winfocus.LMS.Application.DTOs.QuestionConfig
{
    /// <summary>
    /// Response DTO for Question Code uniqueness check.
    /// </summary>
    public class CodeAvailabilityDto
    {
        /// <summary>
        /// Gets or sets the Question Code that was checked.
        /// </summary>
        public string Code { get; set; } = default!;

        /// <summary>
        /// Gets or sets a value indicating whether the code is available (not already in use).
        /// </summary>
        public bool IsAvailable { get; set; }

        /// <summary>
        /// Gets or sets an optional message when the code is not available.
        /// </summary>
        public string? Message { get; set; }
    }
}
