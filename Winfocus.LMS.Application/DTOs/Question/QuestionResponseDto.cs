using System;
using System.Collections.Generic;
using System.Text;
using Winfocus.LMS.Application.DTOs.Review;

namespace Winfocus.LMS.Application.DTOs.Question
{
    /// <summary>
    /// DTO representing detailed information about a question.
    /// Includes options and review history.
    /// </summary>
    public class QuestionResponseDto
    {
        /// <summary>
        /// Gets or sets the question identifier.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the associated task identifier.
        /// </summary>
        public Guid TaskId { get; set; }

        /// <summary>
        /// Gets or sets the task code.
        /// </summary>
        public string TaskCode { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the sequential question number within the task.
        /// </summary>
        public int QuestionNumber { get; set; }

        /// <summary>
        /// Gets or sets the question type as a readable string.
        /// </summary>
        public string QuestionType { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the question text/content.
        /// </summary>
        public string QuestionText { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the marks assigned to the question.
        /// </summary>
        public decimal? Marks { get; set; }

        /// <summary>
        /// Gets or sets the correct answer label (for MCQ).
        /// </summary>
        public string? CorrectAnswer { get; set; }

        /// <summary>
        /// Gets or sets the reference (Subject-Unit-Chapter).
        /// </summary>
        public string? Reference { get; set; }

        /// <summary>
        /// Gets or sets the current status of the question.
        /// Example: Draft, Submitted, Approved, Rejected.
        /// </summary>
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the list of options (for MCQ).
        /// </summary>
        public List<QuestionOptionDto> Options { get; set; } = new();

        /// <summary>
        /// Gets or sets the review history of the question.
        /// </summary>
        public List<ReviewResponseDto> ReviewHistory { get; set; } = new();

        /// <summary>
        /// Gets or sets the date and time when the question was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }
}
