namespace Winfocus.LMS.Application.DTOs.QuestionConfig
{
    using System;

    /// <summary>
    /// Request DTO for generating a suggested Question Code.
    /// Admin selects all dropdowns, system returns a suggested code without saving.
    /// </summary>
    public class SuggestQuestionCodeDto
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
        /// Gets or sets the question type identifier.
        /// </summary>
        public Guid QuestionTypeId { get; set; }
    }
}
