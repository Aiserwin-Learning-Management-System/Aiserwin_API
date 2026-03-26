namespace Winfocus.LMS.Domain.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Winfocus.LMS.Domain.Common;

    /// <summary>
    /// Represents an educational course.
    /// </summary>
    public class Course : BaseEntity
    {
        /// <summary>
        /// Gets or sets the display name of the course.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Gets or sets the identifier of the associated stream.
        /// </summary>
        public Guid StreamId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the associated stream.
        /// </summary>
        public Streams Stream { get; set; } = null!;

        /// <summary>
        /// Gets or sets the fee plans.
        /// </summary>
        /// <value>
        /// The fee plans.
        /// </value>
        public ICollection<FeePlan> FeePlans { get; set; }
    = new List<FeePlan>();

        /// <summary>
        /// Gets or sets the grade identifier.
        /// </summary>
        /// <value>
        /// The grade identifier.
        /// </value>
        public Guid GradeId { get; set; }

        /// <summary>
        /// Gets or sets the grade.
        /// </summary>
        /// <value>
        /// The grade.
        /// </value>
        public Grade Grade { get; set; } = null!;

        /// <summary>
        /// Gets or sets the CourseCode.
        /// </summary>
        /// <value>
        /// The grade.
        /// </value>
        [MaxLength(50)]
        public string CourseCode { get; set; } = null!;
    }
}
