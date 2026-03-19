namespace Winfocus.LMS.Domain.Entities
{
    using Winfocus.LMS.Domain.Common;

    /// <summary>
    /// Represents a question type mapped to a specific academic hierarchy.
    /// Admin defines types like MCQ, Descriptive, Fill in the blanks for each
    /// Syllabus + Grade + Subject + Unit + Chapter + Resource Type combination.
    /// </summary>
    public class QuestionTypeConfig : BaseEntity
    {
        /// <summary>
        /// Gets or sets the foreign key to the exam syllabus.
        /// </summary>
        public Guid SyllabusId { get; set; }

        /// <summary>
        /// Gets or sets the foreign key to the exam grade.
        /// </summary>
        public Guid GradeId { get; set; }

        /// <summary>
        /// Gets or sets the foreign key to the exam subject.
        /// </summary>
        public Guid SubjectId { get; set; }

        /// <summary>
        /// Gets or sets the foreign key to the exam unit.
        /// </summary>
        public Guid UnitId { get; set; }

        /// <summary>
        /// Gets or sets the foreign key to the exam chapter.
        /// </summary>
        public Guid ChapterId { get; set; }

        /// <summary>
        /// Gets or sets the foreign key to the content resource type.
        /// </summary>
        public Guid ResourceTypeId { get; set; }

        /// <summary>
        /// Gets or sets the question type name.
        /// User-defined text input.
        /// Examples: "MCQ", "Descriptive", "Fill in the blanks", "True/False".
        /// </summary>
        public string Name { get; set; } = default!;

        /// <summary>
        /// Gets or sets an optional description.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the exam syllabus.
        /// </summary>
        public ExamSyllabus Syllabus { get; set; } = default!;

        /// <summary>
        /// Gets or sets the navigation property to the exam grade.
        /// </summary>
        public ExamGrade Grade { get; set; } = default!;

        /// <summary>
        /// Gets or sets the navigation property to the exam subject.
        /// </summary>
        public ExamSubject Subject { get; set; } = default!;

        /// <summary>
        /// Gets or sets the navigation property to the exam unit.
        /// </summary>
        public ExamUnit Unit { get; set; } = default!;

        /// <summary>
        /// Gets or sets the navigation property to the exam chapter.
        /// </summary>
        public ExamChapter Chapter { get; set; } = default!;

        /// <summary>
        /// Gets or sets the navigation property to the content resource type.
        /// </summary>
        public ContentResourceType ResourceType { get; set; } = default!;
    }
}
