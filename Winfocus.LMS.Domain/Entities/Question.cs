namespace Winfocus.LMS.Domain.Entities
{
    using Winfocus.LMS.Domain.Common;
    using Winfocus.LMS.Domain.Enums;

    /// <summary>
    /// Represents a single question entered by a DTP operator
    /// as part of a task assignment.
    /// </summary>
    public class Question : BaseEntity
    {
        /// <summary>
        /// Gets or sets the task identifier.
        /// </summary>
        /// <value>
        /// The task identifier.
        /// </value>
        public Guid TaskId { get; set; }

        /// <summary>
        /// Gets or sets the operator identifier.
        /// </summary>
        /// <value>
        /// The operator identifier.
        /// </value>
        public Guid OperatorId { get; set; }

        /// <summary>
        /// Gets or sets the type of the question.
        /// </summary>
        /// <value>
        /// The type of the question.
        /// </value>
        public int QuestionType { get; set; }

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
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public int Status { get; set; }

        /// <summary>
        /// Gets or sets the task assignment.
        /// </summary>
        /// <value>
        /// The task assignment.
        /// </value>
        public TaskAssignment TaskAssignment { get; set; } = null!;

        /// <summary>
        /// Gets or sets the operator.
        /// </summary>
        /// <value>
        /// The operator.
        /// </value>
        public StaffRegistration Operator { get; set; } = null!;

        /// <summary>
        /// Gets or sets the options.
        /// </summary>
        /// <value>
        /// The options.
        /// </value>
        public ICollection<QuestionOption> Options { get; set; } = new List<QuestionOption>();

        /// <summary>
        /// Gets or sets the reviews.
        /// </summary>
        /// <value>
        /// The reviews.
        /// </value>
        public ICollection<QuestionReview> Reviews { get; set; } = new List<QuestionReview>();
    }
}
