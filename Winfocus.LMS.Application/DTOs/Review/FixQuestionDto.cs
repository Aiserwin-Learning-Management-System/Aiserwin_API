namespace Winfocus.LMS.Application.DTOs.Review
{
    /// <summary>
    /// FixQuestionDto.
    /// </summary>
    public class FixQuestionDto
    {
        /// <summary>
        /// Gets or sets the question text.
        /// </summary>
        /// <value>
        /// The question text.
        /// </value>
        public string QuestionText { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the marks.
        /// </summary>
        /// <value>
        /// The marks.
        /// </value>
        public decimal? Marks { get; set; }

        /// <summary>
        /// Gets or sets the correct answer.
        /// </summary>
        /// <value>
        /// The correct answer.
        /// </value>
        public string? CorrectAnswer { get; set; }

        /// <summary>
        /// Gets or sets the correct answer text.
        /// </summary>
        /// <value>
        /// The correct answer text.
        /// </value>
        public string? CorrectAnswerText { get; set; }

        /// <summary>
        /// Gets or sets the reference.
        /// </summary>
        /// <value>
        /// The reference.
        /// </value>
        public string? Reference { get; set; }

        /// <summary>
        /// Gets or sets the options.
        /// </summary>
        /// <value>
        /// The options.
        /// </value>
        public List<FixOptionDto>? Options { get; set; }
    }

    /// <summary>
    /// FixOptionDto.
    /// </summary>
    public class FixOptionDto
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public Guid? Id { get; set; }

        /// <summary>
        /// Gets or sets the option label.
        /// </summary>
        /// <value>
        /// The option label.
        /// </value>
        public string OptionLabel { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the option text.
        /// </summary>
        /// <value>
        /// The option text.
        /// </value>
        public string OptionText { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is correct.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is correct; otherwise, <c>false</c>.
        /// </value>
        public bool IsCorrect { get; set; }
    }
}
