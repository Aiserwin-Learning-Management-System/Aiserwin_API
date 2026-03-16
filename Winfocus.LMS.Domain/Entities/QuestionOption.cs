namespace Winfocus.LMS.Domain.Entities
{
    /// <summary>
    /// QuestionOption.
    /// </summary>
    public class QuestionOption
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the question identifier.
        /// </summary>
        /// <value>
        /// The question identifier.
        /// </value>
        public Guid QuestionId { get; set; }

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
        public bool IsCorrect { get; set; } = false;

        /// <summary>
        /// Gets or sets the question.
        /// </summary>
        /// <value>
        /// The question.
        /// </value>
        public Question Question { get; set; } = null!;
    }
}
