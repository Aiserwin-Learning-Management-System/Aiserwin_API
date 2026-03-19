namespace Winfocus.LMS.Application.DTOs.Review
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// RejectQuestionDto.
    /// </summary>
    public class RejectQuestionDto
    {
        /// <summary>
        /// Gets or sets the feedback.
        /// </summary>
        /// <value>
        /// The feedback.
        /// </value>
        [Required(ErrorMessage = "Feedback is required when rejecting a question.")]
        [MinLength(10, ErrorMessage = "Feedback must be at least 10 characters.")]
        public string Feedback { get; set; } = string.Empty;
    }
}
