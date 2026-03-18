namespace Winfocus.LMS.Application.DTOs.Review
{
    /// <summary>
    /// CorrectionDetailDto.
    /// </summary>
    public class CorrectionDetailDto
    {
        /// <summary>
        /// Gets or sets the question identifier.
        /// </summary>
        /// <value>
        /// The question identifier.
        /// </value>
        public Guid QuestionId { get; set; }

        /// <summary>
        /// Gets or sets the task identifier.
        /// </summary>
        /// <value>
        /// The task identifier.
        /// </value>
        public Guid TaskId { get; set; }

        /// <summary>
        /// Gets or sets the question text.
        /// </summary>
        /// <value>
        /// The question text.
        /// </value>
        public string QuestionText { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the type of the question.
        /// </summary>
        /// <value>
        /// The type of the question.
        /// </value>
        public string QuestionType { get; set; } = string.Empty;

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
        /// Gets or sets the task code.
        /// </summary>
        /// <value>
        /// The task code.
        /// </value>
        public string TaskCode { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the latest feedback.
        /// </summary>
        /// <value>
        /// The latest feedback.
        /// </value>
        public string LatestFeedback { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the review cycle.
        /// </summary>
        /// <value>
        /// The review cycle.
        /// </value>
        public int ReviewCycle { get; set; }

        /// <summary>
        /// Gets or sets the year.
        /// </summary>
        /// <value>
        /// The year.
        /// </value>
        public int? Year { get; set; }

        /// <summary>
        /// Gets or sets the syllabus.
        /// </summary>
        /// <value>
        /// The syllabus.
        /// </value>
        public string Syllabus { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the grade.
        /// </summary>
        /// <value>
        /// The grade.
        /// </value>
        public string Grade { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the subject.
        /// </summary>
        /// <value>
        /// The subject.
        /// </value>
        public string Subject { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the unit.
        /// </summary>
        /// <value>
        /// The unit.
        /// </value>
        public string? Unit { get; set; }

        /// <summary>
        /// Gets or sets the chapter.
        /// </summary>
        /// <value>
        /// The chapter.
        /// </value>
        public string? Chapter { get; set; }

        /// <summary>
        /// Gets or sets the type of the resource.
        /// </summary>
        /// <value>
        /// The type of the resource.
        /// </value>
        public string ResourceType { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the options.
        /// </summary>
        /// <value>
        /// The options.
        /// </value>
        public List<QuestionOptionDto> Options { get; set; } = new ();

        /// <summary>
        /// Gets or sets the review history.
        /// </summary>
        /// <value>
        /// The review history.
        /// </value>
        public List<ReviewHistoryDto> ReviewHistory { get; set; } = new ();
    }
}
