namespace Winfocus.LMS.Domain.Entities
{
    using System;
    using Winfocus.LMS.Domain.Common;

    /// <summary>
    /// ExamSubject.
    /// </summary>
    /// <seealso cref="Winfocus.LMS.Domain.Common.BaseEntity" />
    public class ExamSubject : BaseEntity
    {
        /// <summary>
        /// Gets or sets the grade identifier.
        /// </summary>
        /// <value>
        /// The grade identifier.
        /// </value>
        public Guid GradeId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; } = default!;

        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        public string? Code { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the grade.
        /// </summary>
        /// <value>
        /// The grade.
        /// </value>
        public ExamGrade Grade { get; set; } = default!;
    }
}
