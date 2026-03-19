namespace Winfocus.LMS.Application.DTOs.QuestionTypeConfig
{
    using Winfocus.LMS.Application.DTOs.Common;

    /// <summary>
    /// Filter request for Question Type Configuration list.
    /// Extends <see cref="PagedRequest"/> with hierarchy-specific filters.
    /// </summary>
    public class QuestionTypeConfigFilterRequest : PagedRequest
    {
        /// <summary>
        /// Gets or sets the optional syllabus filter.
        /// </summary>
        public Guid? SyllabusId { get; set; }

        /// <summary>
        /// Gets or sets the optional grade filter.
        /// </summary>
        public Guid? GradeId { get; set; }

        /// <summary>
        /// Gets or sets the optional subject filter.
        /// </summary>
        public Guid? SubjectId { get; set; }

        /// <summary>
        /// Gets or sets the optional unit filter.
        /// </summary>
        public Guid? UnitId { get; set; }

        /// <summary>
        /// Gets or sets the optional chapter filter.
        /// </summary>
        public Guid? ChapterId { get; set; }

        /// <summary>
        /// Gets or sets the optional resource type filter.
        /// </summary>
        public Guid? ResourceTypeId { get; set; }
    }
}
