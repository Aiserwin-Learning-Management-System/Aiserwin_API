namespace Winfocus.LMS.Application.DTOs.Review
{
    /// <summary>
    /// ReviewQuestionDetailDto.
    /// </summary>
    public class ReviewQuestionDetailDto
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
        /// Gets or sets the name of the operator.
        /// </summary>
        /// <value>
        /// The name of the operator.
        /// </value>
        public string OperatorName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the submitted at.
        /// </summary>
        /// <value>
        /// The submitted at.
        /// </value>
        public DateTime SubmittedAt { get; set; }

        /// <summary>
        /// Gets or sets the task code.
        /// </summary>
        /// <value>
        /// The task code.
        /// </value>
        public string TaskCode { get; set; } = string.Empty;

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
        /// Gets or sets the syllabus identifier.
        /// </summary>
        /// <value>
        /// The syllabus identifier.
        /// </value>
        public Guid? SyllabusId { get; set; }

        /// <summary>
        /// Gets or sets the grade.
        /// </summary>
        /// <value>
        /// The grade.
        /// </value>
        public string Grade { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the grade identifier.
        /// </summary>
        /// <value>
        /// The grade identifier.
        /// </value>
        public Guid? GradeId { get; set; }

        /// <summary>
        /// Gets or sets the subject.
        /// </summary>
        /// <value>
        /// The subject.
        /// </value>
        public string Subject { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the subject identifier.
        /// </summary>
        /// <value>
        /// The subject identifier.
        /// </value>
        public Guid? SubjectId { get; set; }

        /// <summary>
        /// Gets or sets the unit.
        /// </summary>
        /// <value>
        /// The unit.
        /// </value>
        public string? Unit { get; set; }

        /// <summary>
        /// Gets or sets the unit identifier.
        /// </summary>
        /// <value>
        /// The unit identifier.
        /// </value>
        public Guid? UnitId { get; set; }

        /// <summary>
        /// Gets or sets the chapter.
        /// </summary>
        /// <value>
        /// The chapter.
        /// </value>
        public string? Chapter { get; set; }

        /// <summary>
        /// Gets or sets the chapter identifier.
        /// </summary>
        /// <value>
        /// The chapter identifier.
        /// </value>
        public Guid? ChapterId { get; set; }

        /// <summary>
        /// Gets or sets the type of the resource.
        /// </summary>
        /// <value>
        /// The type of the resource.
        /// </value>
        public string ResourceType { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the resource type identifier.
        /// </summary>
        /// <value>
        /// The resource type identifier.
        /// </value>
        public Guid? ResourceTypeId { get; set; }

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

    /// <summary>
    /// QuestionOptionDto.
    /// </summary>
    public class QuestionOptionDto
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public Guid Id { get; set; }

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

    /// <summary>
    /// ReviewHistoryDto.
    /// </summary>
    public class ReviewHistoryDto
    {
        /// <summary>
        /// Gets or sets the cycle.
        /// </summary>
        /// <value>
        /// The cycle.
        /// </value>
        public int Cycle { get; set; }

        /// <summary>
        /// Gets or sets the action.
        /// </summary>
        /// <value>
        /// The action.
        /// </value>
        public string Action { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the feedback.
        /// </summary>
        /// <value>
        /// The feedback.
        /// </value>
        public string? Feedback { get; set; }

        /// <summary>
        /// Gets or sets the reviewer role.
        /// </summary>
        /// <value>
        /// The reviewer role.
        /// </value>
        public string ReviewerRole { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the reviewed at.
        /// </summary>
        /// <value>
        /// The reviewed at.
        /// </value>
        public DateTime ReviewedAt { get; set; }
    }
}
