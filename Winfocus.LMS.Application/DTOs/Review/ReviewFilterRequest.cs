namespace Winfocus.LMS.Application.DTOs.Review
{
    using Winfocus.LMS.Application.DTOs.Common;

    /// <summary>
    /// ReviewFilterRequest.
    /// </summary>
    /// <seealso cref="Winfocus.LMS.Application.DTOs.Common.PagedRequest" />
    public class ReviewFilterRequest : PagedRequest
    {
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
        public Guid? SyllabusId { get; set; }

        /// <summary>
        /// Gets or sets the grade identifier.
        /// </summary>
        /// <value>
        /// The grade identifier.
        /// </value>
        public Guid? GradeId { get; set; }

        /// <summary>
        /// Gets or sets the subject identifier.
        /// </summary>
        /// <value>
        /// The subject identifier.
        /// </value>
        public Guid? SubjectId { get; set; }

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
        /// Gets or sets the resource type identifier.
        /// </summary>
        /// <value>
        /// The resource type identifier.
        /// </value>
        public Guid? ResourceTypeId { get; set; }

        /// <summary>
        /// Gets or sets the operator identifier.
        /// </summary>
        /// <value>
        /// The operator identifier.
        /// </value>
        public Guid? OperatorId { get; set; }

        /// <summary>
        /// Gets or sets the type of the question.
        /// </summary>
        /// <value>
        /// The type of the question.
        /// </value>
        public int? QuestionType { get; set; }
    }
}
