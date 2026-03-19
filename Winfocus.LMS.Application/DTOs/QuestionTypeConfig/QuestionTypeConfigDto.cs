namespace Winfocus.LMS.Application.DTOs.QuestionTypeConfig
{
    /// <summary>
    /// Response DTO for Question Type Configuration with resolved master names.
    /// </summary>
    public class QuestionTypeConfigDto
    {
        /// <summary>
        /// Gets or sets the unique identifier.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the question type name.
        /// </summary>
        public string Name { get; set; } = default!;

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the syllabus name.
        /// </summary>
        public string SyllabusName { get; set; } = default!;

        /// <summary>
        /// Gets or sets the grade name.
        /// </summary>
        public string GradeName { get; set; } = default!;

        /// <summary>
        /// Gets or sets the subject name.
        /// </summary>
        public string SubjectName { get; set; } = default!;

        /// <summary>
        /// Gets or sets the subject code.
        /// </summary>
        public string? SubjectCode { get; set; }

        /// <summary>
        /// Gets or sets the unit name.
        /// </summary>
        public string UnitName { get; set; } = default!;

        /// <summary>
        /// Gets or sets the chapter name.
        /// </summary>
        public string ChapterName { get; set; } = default!;

        /// <summary>
        /// Gets or sets the resource type name.
        /// </summary>
        public string ResourceTypeName { get; set; } = default!;

        /// <summary>
        /// Gets or sets a value indicating whether the record is active.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the creation timestamp.
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }
}
