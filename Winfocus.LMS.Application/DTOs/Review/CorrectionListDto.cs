namespace Winfocus.LMS.Application.DTOs.Review
{
    /// <summary>
    /// CorrectionListDto.
    /// </summary>
    public class CorrectionListDto
    {

        /// <summary>
        /// Gets or sets the question identifier.
        /// </summary>
        /// <value>
        /// The question identifier.
        /// </value>
        public Guid QuestionId { get; set; }

        /// <summary>
        /// Gets or sets the question number.
        /// </summary>
        /// <value>
        /// The question number.
        /// </value>
        public int QuestionNumber { get; set; }

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
        /// Gets or sets the task code.
        /// </summary>
        /// <value>
        /// The task code.
        /// </value>
        public string TaskCode { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the rejected at.
        /// </summary>
        /// <value>
        /// The rejected at.
        /// </value>
        public DateTime RejectedAt { get; set; }

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
    }
}
