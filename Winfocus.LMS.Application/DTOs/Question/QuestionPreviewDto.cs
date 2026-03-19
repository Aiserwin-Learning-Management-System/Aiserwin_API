using System;
using System.Collections.Generic;
using System.Text;
using Winfocus.LMS.Application.DTOs.Review;

namespace Winfocus.LMS.Application.DTOs.Question
{
    /// <summary>
    /// DTO used to preview a formatted question before submission.
    /// This is a read-only representation.
    /// </summary>
    public class QuestionPreviewDto
    {
        /// <summary>
        /// Gets or sets the question text.
        /// </summary>
        public string QuestionText { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the correct answer label (if applicable).
        /// </summary>
        public string? CorrectAnswer { get; set; }

        /// <summary>
        /// Gets or sets the list of options (for MCQ preview).
        /// </summary>
        public List<QuestionOptionDto> Options { get; set; } = new();
    }
}
