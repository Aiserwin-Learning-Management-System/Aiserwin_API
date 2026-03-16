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
        public Guid AssignedBy { get; set; }

        /// <summary>
        /// Gets or sets the resource type identifier.
        /// </summary>
        /// <value>
        /// The resource type identifier.
        /// </value>
        public Guid ResourceTypeId { get; set; }

        /// <summary>
        /// Gets or sets the type of the question.
        /// </summary>
        /// <value>
        /// The type of the question.
        /// </value>
        public int QuestionType { get; set; }

        /// <summary>
        /// Gets or sets the year.
        /// </summary>
        /// <value>
        /// The year.
        /// </value>
        public int? Year { get; set; }

        /// <summary>
        /// Gets or sets the syllabus identifier.
        /// </summary>
        /// <value>
        /// The syllabus identifier.
        /// </value>
        public Guid SyllabusId { get; set; }

        /// <summary>
        /// Gets or sets the grade identifier.
        /// </summary>
        /// <value>
        /// The grade identifier.
        /// </value>
        public Guid GradeId { get; set; }

        /// <summary>
        /// Gets or sets the subject identifier.
        /// </summary>
        /// <value>
        /// The subject identifier.
        /// </value>
        public Guid SubjectId { get; set; }

        /// <summary>
        /// Gets or sets the unit identifier.
        /// </summary>
        /// <value>
        /// The unit identifier.
        /// </value>
        public Guid? UnitId { get; set; }

        /// <summary>
        /// Gets or sets the chapter identifier.
        /// </summary>
        /// <value>
        /// The chapter identifier.
        /// </value>
        public Guid? ChapterId { get; set; }

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
        public int CompletedCount { get; set; }

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
        public int Priority { get; set; }

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
        public int Status { get; set; }

        /// <summary>
        /// Gets or sets the operator.
        /// </summary>
        /// <value>
        /// The operator.
        /// </value>
        public StaffRegistration Operator { get; set; } = default!;

        /// <summary>
        /// Gets or sets the type of the resource.
        /// </summary>
        /// <value>
        /// The type of the resource.
        /// </value>
        public ContentResourceType ResourceType { get; set; } = default!;

        /// <summary>
        /// Gets or sets the syllabus.
        /// </summary>
        /// <value>
        /// The syllabus.
        /// </value>
        public ExamSyllabus Syllabus { get; set; } = default!;

        /// <summary>
        /// Gets or sets the grade.
        /// </summary>
        /// <value>
        /// The grade.
        /// </value>
        public ExamGrade Grade { get; set; } = default!;

        /// <summary>
        /// Gets or sets the subject.
        /// </summary>
        /// <value>
        /// The subject.
        /// </value>
        public ExamSubject Subject { get; set; } = default!;

        /// <summary>
        /// Gets or sets the unit.
        /// </summary>
        /// <value>
        /// The unit.
        /// </value>
        public ExamUnit? Unit { get; set; }

        /// <summary>
        /// Gets or sets the chapter.
        /// </summary>
        /// <value>
        /// The chapter.
        /// </value>
        public ExamChapter? Chapter { get; set; }

        /// <summary>
        /// Gets or sets the questions.
        /// </summary>
        /// <value>
        /// The questions.
        /// </value>
        public ICollection<Question> Questions { get; set; } = new List<Question>();

        /// <summary>
        /// Gets or sets the daily activity reports.
        /// </summary>
        /// <value>
        /// The daily activity reports.
        /// </value>
        public ICollection<DailyActivityReport> DailyActivityReports { get; set; } = new List<DailyActivityReport>();
    }
}
