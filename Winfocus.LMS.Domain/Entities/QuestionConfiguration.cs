namespace Winfocus.LMS.Domain.Entities
{
    using Winfocus.LMS.Domain.Common;
    using Winfocus.LMS.Domain.Enums;

    /// <summary>
    /// Represents a configured Question ID mapped to an academic hierarchy.
    /// Admin selects dropdowns, system suggests a code, admin confirms or edits, then saves.
    /// </summary>
    public class QuestionConfiguration : BaseEntity
    {
        /// <summary>
        /// Gets or sets the foreign key to the exam syllabus.
        /// Example: CBSE, ICSE, JEE.
        /// </summary>
        public Guid SyllabusId { get; set; }

        /// <summary>
        /// Gets or sets the foreign key to the academic year.
        /// Example: 2025-2026.
        /// </summary>
        public Guid AcademicYearId { get; set; }

        /// <summary>
        /// Gets or sets the foreign key to the exam grade.
        /// Example: 10th, 11th, 12th.
        /// </summary>
        public Guid GradeId { get; set; }

        /// <summary>
        /// Gets or sets the foreign key to the exam subject.
        /// Example: Physics, Chemistry.
        /// </summary>
        public Guid SubjectId { get; set; }

        /// <summary>
        /// Gets or sets the foreign key to the exam unit.
        /// Example: Electrostatics.
        /// </summary>
        public Guid UnitId { get; set; }

        /// <summary>
        /// Gets or sets the foreign key to the exam chapter.
        /// Example: Electric Charges and Fields.
        /// </summary>
        public Guid ChapterId { get; set; }

        /// <summary>
        /// Gets or sets the foreign key to the content resource type.
        /// Example: Question Bank, Workbook.
        /// </summary>
        public Guid ResourceTypeId { get; set; }

        /// <summary>
        /// Gets or sets the foreign key to the question type.
        /// Example: MCQ, Short Answer, Long Answer.
        /// </summary>
        public Guid QuestionTypeId { get; set; }

        /// <summary>
        /// Gets or sets the unique Question Code.
        /// Format: [SYL]-[YYYY]-[GRD]-[SUB]-[UNIT]-[CH]-[TYPE]-[SEQ].
        /// Example: CBSE-2025-12-PHY-U01-CH01-MCQ-0001.
        /// Admin can use auto-suggested code or type a custom one.
        /// </summary>
        public string QuestionCode { get; set; } = default!;

        /// <summary>
        /// Gets or sets the sequence number within the scope of
        /// (Syllabus + AcademicYear + Grade + Subject + Unit + Chapter + QuestionType).
        /// Resets to 1 for each new chapter or question type combination.
        /// </summary>
        public int SequenceNumber { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the exam syllabus.
        /// </summary>
        public Syllabus Syllabus { get; set; } = default!;

        /// <summary>
        /// Gets or sets the navigation property to the academic year.
        /// </summary>
        public AcademicYear AcademicYear { get; set; } = default!;

        /// <summary>
        /// Gets or sets the navigation property to the exam grade.
        /// </summary>
        public Grade Grade { get; set; } = default!;

        /// <summary>
        /// Gets or sets the navigation property to the exam subject.
        /// </summary>
        public Subject Subject { get; set; } = default!;

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

        /// <summary>
        /// Gets or sets the navigation property to the question type.
        /// </summary>
        public QuestionTypeConfig QuestionType { get; set; } = default!;
    }
}
