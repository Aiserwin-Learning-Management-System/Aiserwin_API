namespace Winfocus.LMS.Domain.Entities
{
    using Winfocus.LMS.Domain.Common;
    using Winfocus.LMS.Domain.Enums;

    /// <summary>
    /// Represents a question-typing task assigned to a DTP operator.
    /// Tracks assignment details, progress, and deadline.
    /// </summary>
    public class TaskAssignment : BaseEntity
    {
        /// <summary>
        /// Gets or sets the operator identifier.
        /// </summary>
        /// <value>
        /// The operator identifier.
        /// </value>
        public Guid OperatorId { get; set; }

        /// <summary>
        /// Gets or sets the assigned by.
        /// </summary>
        /// <value>
        /// The assigned by.
        /// </value>
        public string AssignedBy { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the type of the question.
        /// </summary>
        /// <value>
        /// The type of the question.
        /// </value>
        public QuestionType QuestionType { get; set; }

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
        public string? Syllabus { get; set; }

        /// <summary>
        /// Gets or sets the grade.
        /// </summary>
        /// <value>
        /// The grade.
        /// </value>
        public string? Grade { get; set; }

        /// <summary>
        /// Gets or sets the stream.
        /// </summary>
        /// <value>
        /// The stream.
        /// </value>
        public string? Stream { get; set; }

        /// <summary>
        /// Gets or sets the course.
        /// </summary>
        /// <value>
        /// The course.
        /// </value>
        public string? Course { get; set; }

        /// <summary>
        /// Gets or sets the subject.
        /// </summary>
        /// <value>
        /// The subject.
        /// </value>
        public string? Subject { get; set; }

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
        /// Gets or sets the completed count.
        /// </summary>
        /// <value>
        /// The completed count.
        /// </value>
        public int CompletedCount { get; set; } = 0;

        /// <summary>
        /// Gets or sets the deadline.
        /// </summary>
        /// <value>
        /// The deadline.
        /// </value>
        public DateTime Deadline { get; set; }

        /// <summary>
        /// Gets or sets the priority.
        /// </summary>
        /// <value>
        /// The priority.
        /// </value>
        public TaskPriority Priority { get; set; } = TaskPriority.Medium;

        /// <summary>
        /// Gets or sets the instructions.
        /// </summary>
        /// <value>
        /// The instructions.
        /// </value>
        public string? Instructions { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public Enums.TaskStatus Status { get; set; } = Enums.TaskStatus.Pending;

        /// <summary>
        /// Gets or sets the operator.
        /// </summary>
        /// <value>
        /// The operator.
        /// </value>
        public StaffRegistration Operator { get; set; } = null!;

        /// <summary>
        /// Gets or sets the questions.
        /// </summary>
        /// <value>
        /// The questions.
        /// </value>
        public ICollection<Question> Questions { get; set; } = new List<Question>();

        /// <summary>
        /// Gets or sets the daily reports.
        /// </summary>
        /// <value>
        /// The daily reports.
        /// </value>
        public ICollection<DailyActivityReport> DailyReports { get; set; } = new List<DailyActivityReport>();
    }
}
