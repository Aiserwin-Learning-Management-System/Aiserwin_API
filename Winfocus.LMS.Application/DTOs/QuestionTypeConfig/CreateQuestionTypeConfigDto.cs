namespace Winfocus.LMS.Application.DTOs.QuestionTypeConfig
{
    /// <summary>
    /// Request DTO for creating a single Question Type Configuration.
    /// </summary>
    public class CreateQuestionTypeConfigDto
    {
        /// <summary>
        /// Gets or sets the syllabus identifier.
        /// </summary>
        public Guid SyllabusId { get; set; }

        /// <summary>
        /// Gets or sets the grade identifier.
        /// </summary>
        public Guid GradeId { get; set; }

        /// <summary>
        /// Gets or sets the subject identifier.
        /// </summary>
        public Guid SubjectId { get; set; }

        /// <summary>
        /// Gets or sets the unit identifier.
        /// </summary>
        public Guid UnitId { get; set; }

        /// <summary>
        /// Gets or sets the chapter identifier.
        /// </summary>
        public Guid ChapterId { get; set; }

        /// <summary>
        /// Gets or sets the resource type identifier.
        /// </summary>
        public Guid ResourceTypeId { get; set; }

        /// <summary>
        /// Gets or sets the question type name.
        /// Example: "MCQ", "Descriptive", "Fill in the blanks".
        /// </summary>
        public string Name { get; set; } = default!;

        /// <summary>
        /// Gets or sets an optional description.
        /// </summary>
        public string? Description { get; set; }
    }
}
