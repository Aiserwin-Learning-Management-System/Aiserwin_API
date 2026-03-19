namespace Winfocus.LMS.Application.DTOs.QuestionTypeConfig
{
    /// <summary>
    /// Query DTO for fetching question types available for a specific hierarchy.
    /// Used when assigning tasks or configuring Question IDs.
    /// </summary>
    public class HierarchyQueryDto
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
    }
}
