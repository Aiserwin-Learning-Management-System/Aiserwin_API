using System;

namespace Winfocus.LMS.Application.DTOs.QuestionConfig
{
    /// <summary>
    /// Request DTO for saving a Question Configuration.
    /// Admin can use the auto-suggested code or type a custom code.
    /// </summary>
    public class CreateQuestionConfigurationDto
    {
        /// <summary>
        /// Gets or sets the syllabus identifier.
        /// </summary>
        public Guid SyllabusId { get; set; }

        /// <summary>
        /// Gets or sets the academic year identifier.
        /// </summary>
        public Guid AcademicYearId { get; set; }

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
        /// Gets or sets the question type identifier.
        /// </summary>
        public Guid QuestionTypeId { get; set; }

        /// <summary>
        /// Gets or sets the Question Code.
        /// Can be the auto-suggested code or a custom admin-typed code.
        /// Must be unique across the system.
        /// </summary>
        public string QuestionCode { get; set; } = default!;
    }
}
