namespace Winfocus.LMS.Application.DTOs.Dashboard
{
    using System;

    /// <summary>
    /// RejectedQuestionDto.
    /// </summary>
    public class RejectedQuestionDto
    {
        /// <summary>
        /// Gets or sets the question identifier.
        /// </summary>
        /// <value>
        /// The question identifier.
        /// </value>
        public Guid QuestionId { get; set; }

        /// <summary>
        /// Gets or sets the question preview.
        /// </summary>
        /// <value>
        /// The question preview.
        /// </value>
        public string QuestionPreview { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the feedback.
        /// </summary>
        /// <value>
        /// The feedback.
        /// </value>
        public string? Feedback { get; set; }

        /// <summary>
        /// Gets or sets the rejected at.
        /// </summary>
        /// <value>
        /// The rejected at.
        /// </value>
        public DateTime RejectedAt { get; set; }

        /// <summary>
        /// Gets or sets the reviewer role.
        /// </summary>
        /// <value>
        /// The reviewer role.
        /// </value>
        public string ReviewerRole { get; set; } = string.Empty;
    }
}
