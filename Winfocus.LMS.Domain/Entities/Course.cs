namespace Winfocus.LMS.Domain.Entities
{
    using System;
    using Winfocus.LMS.Domain.Common;

    /// <summary>
    /// Represents an educational course.
    /// </summary>
    public class Course : BaseEntity
    {
        /// <summary>
        /// Gets or sets the display name of the course.
        /// </summary>
        public string CourseName { get; set; } = null!;

        /// <summary>
        /// Gets or sets the optional code for the course.
        /// </summary>
        public string CourseCode { get; set; } = null!;

        /// <summary>
        /// Gets or sets the identifier of the associated stream.
        /// </summary>
        public Guid StreamId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the associated stream.
        /// </summary>
        public Streams Stream { get; set; } = null!;

        // Many courses → one subject

        /// <summary>
        /// Gets or sets the subject identifier.
        /// </summary>
        /// <value>
        /// The subject identifier.
        /// </value>
        public Guid SubjectId { get; set; }

        /// <summary>
        /// Gets or sets the subject.
        /// </summary>
        /// <value>
        /// The subject.
        /// </value>
        public Subject Subject { get; set; } = null!;

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
        /// Gets or sets the course description.
        /// </summary>
        /// <value>
        /// The course description.
        /// </value>
        public string CourseDescription { get; set; } = null!;

        /// <summary>
        /// Gets or sets the course page url.
        /// </summary>
        /// <value>
        /// The course page url.
        /// </value>
        public string CourseUrl { get; set; } = null!;

        /// <summary>
        /// Gets or sets the count of the max student.
        /// </summary>
        /// <value>
        /// The count of the max student.
        /// </value>
        public int MaxStudent { get; set; }

        /// <summary>
        /// Gets or sets the academic year.
        /// </summary>
        /// <value>
        /// The academic year.
        /// </value>
        public Guid AcademicYear { get; set; }
    }
}
