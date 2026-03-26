using System;

namespace Winfocus.LMS.Application.DTOs.Question
{
    /// <summary>
    /// Represents a question item in the question bank.
    /// </summary>
    public class QuestionBankItemDto
    {
        /// <summary>
        /// Gets or sets the unique identifier of the question.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the sequential number assigned to the question.
        /// </summary>
        public int QuestionNumber { get; set; }

        /// <summary>
        /// Gets or sets the text/content of the question.
        /// </summary>
        public string QuestionText { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the type of the question 
        /// (e.g., Multiple Choice, True/False, Descriptive).
        /// </summary>
        public string QuestionType { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the subject associated with the question.
        /// </summary>
        public string? Subject { get; set; }

        /// <summary>
        /// Gets or sets the chapter or topic under the subject.
        /// </summary>
        public string? Chapter { get; set; }

        /// <summary>
        /// Gets or sets the marks assigned to the question.
        /// </summary>
        public decimal? Marks { get; set; }

        /// <summary>
        /// Gets or sets the current status of the question 
        /// (e.g., Draft, Submitted, Approved, Rejected).
        /// </summary>
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the task code associated with the question.
        /// </summary>
        public string? TaskCode { get; set; }

        /// <summary>
        /// Gets or sets the creation date of the question in string format.
        /// Consider using DateTime for better type safety.
        /// </summary>
        public string CreatedAt { get; set; } = string.Empty;
    }
}
