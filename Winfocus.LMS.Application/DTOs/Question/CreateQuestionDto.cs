using System;
using System.Collections.Generic;
using System.Text;
using Winfocus.LMS.Application.DTOs.Review;

namespace Winfocus.LMS.Application.DTOs.Question
{
    /// <summary>
    /// DTO used to create a new question (MCQ or Long Answer).
    /// Supports both draft and submit modes.
    /// </summary>
    public class CreateQuestionDto
    {
        /// <summary>
        /// Gets or sets the task identifier to which this question belongs.
        /// </summary>
        public Guid TaskId { get; set; }

        /// <summary>
        /// Gets or sets the type of the question.
        /// 0 = MCQ, 1 = Short Answer, 2 = Long Answer.
        /// </summary>
        public int QuestionTypeId { get; set; }

        /// <summary>
        /// Gets or sets the question text/content.
        /// </summary>
        public string QuestionText { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the marks assigned to the question.
        /// Must be greater than zero.
        /// </summary>
        public decimal Marks { get; set; }

        /// <summary>
        /// Gets or sets the correct answer label (A, B, C, D).
        /// Required for MCQ type questions.
        /// </summary>
        public string? CorrectAnswer { get; set; }

        /// <summary>
        /// Gets or sets the descriptive correct answer text.
        /// Required for Long Answer questions.
        /// </summary>
        public string? CorrectAnswerText { get; set; }

        /// <summary>
        /// Gets or sets the reference in the format:
        /// Subject-Unit-Chapter.
        /// Example: Physics-Unit1-Chapter3.
        /// </summary>
        public string? Reference { get; set; }

        /// <summary>
        /// Gets or sets the list of options.
        /// Required only for MCQ questions (exactly 4 options).
        /// </summary>
        public List<QuestionOptionDto> Options { get; set; } = new();

        /// <summary>
        /// Gets or sets a value indicating whether the question
        /// should be saved as draft instead of submitting.
        /// </summary>
        public bool SaveAsDraft { get; set; }
    }
}
