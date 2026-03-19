using System;
using System.Collections.Generic;
using System.Text;

namespace Winfocus.LMS.Application.DTOs.Question
{
    /// <summary>
    /// DTO representing a review action performed on a question.
    /// Captures reviewer details, action taken, and feedback.
    /// </summary>
    public class ReviewResponseDto
    {
        /// <summary>
        /// Gets or sets the unique identifier of the review.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the reviewer (Admin/Reviewer).
        /// </summary>
        public Guid ReviewerId { get; set; }

        /// <summary>
        /// Gets or sets the role of the reviewer.
        /// Example: Admin, QC, Reviewer.
        /// </summary>
        public string ReviewerRole { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the action performed on the question.
        /// Example: Approved, Rejected, SentBack.
        /// </summary>
        public string Action { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the feedback or comments provided by the reviewer.
        /// </summary>
        public string? Feedback { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the review was performed.
        /// </summary>
        public DateTime ReviewedAt { get; set; }
    }
}
