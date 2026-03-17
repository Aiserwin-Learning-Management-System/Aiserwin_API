namespace Winfocus.LMS.Application.DTOs.Dashboard
{
    /// <summary>
    /// ActiveTaskDto.
    /// </summary>
    public class ActiveTaskDto
    {
        /// <summary>
        /// Gets or sets the task identifier.
        /// </summary>
        /// <value>
        /// The task identifier.
        /// </value>
        public Guid TaskId { get; set; }

        /// <summary>
        /// Gets or sets the task code.
        /// </summary>
        /// <value>
        /// The task code.
        /// </value>
        public string TaskCode { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the type of the question.
        /// </summary>
        /// <value>
        /// The type of the question.
        /// </value>
        public string QuestionType { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the type of the resource.
        /// </summary>
        /// <value>
        /// The type of the resource.
        /// </value>
        public string ResourceType { get; set; } = string.Empty;

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
        /// Gets or sets the total questions.
        /// </summary>
        /// <value>
        /// The total questions.
        /// </value>
        public int TotalQuestions { get; set; }

        /// <summary>
        /// Gets or sets the completed questions.
        /// </summary>
        /// <value>
        /// The completed questions.
        /// </value>
        public int CompletedQuestions { get; set; }

        /// <summary>
        /// Gets or sets the progress percentage.
        /// </summary>
        /// <value>
        /// The progress percentage.
        /// </value>
        public int ProgressPercentage { get; set; }

        /// <summary>
        /// Gets or sets the deadline.
        /// </summary>
        /// <value>
        /// The deadline.
        /// </value>
        public DateTime Deadline { get; set; }

        /// <summary>
        /// Gets or sets the days remaining.
        /// </summary>
        /// <value>
        /// The days remaining.
        /// </value>
        public int DaysRemaining { get; set; }

        /// <summary>
        /// Gets or sets the priority.
        /// </summary>
        /// <value>
        /// The priority.
        /// </value>
        public string Priority { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the instructions.
        /// </summary>
        /// <value>
        /// The instructions.
        /// </value>
        public string? Instructions { get; set; }
    }
}
