namespace Winfocus.LMS.Domain.Entities
{
    using Winfocus.LMS.Domain.Enums;

    /// <summary>
    /// QuestionReview.
    /// </summary>
    public class QuestionReview
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
        /// Gets or sets the reviewer identifier.
        /// </summary>
        /// <value>
        /// The reviewer identifier.
        /// </value>
        public string ReviewerId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the reviewer role.
        /// </summary>
        /// <value>
        /// The reviewer role.
        /// </value>
        public string ReviewerRole { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the action.
        /// </summary>
        /// <value>
        /// The action.
        /// </value>
        public ReviewAction Action { get; set; }

        /// <summary>
        /// Gets or sets the feedback.
        /// </summary>
        /// <value>
        /// The feedback.
        /// </value>
        public string? Feedback { get; set; }

        /// <summary>
        /// Gets or sets the reviewed at.
        /// </summary>
        /// <value>
        /// The reviewed at.
        /// </value>
        public DateTime ReviewedAt { get; set; }

        /// <summary>
        /// Gets or sets the question.
        /// </summary>
        /// <value>
        /// The question.
        /// </value>
        public Question Question { get; set; } = null!;
    }
}
