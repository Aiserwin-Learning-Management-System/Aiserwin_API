using Winfocus.LMS.Domain.Entities;

namespace Winfocus.LMS.Application.DTOs.Masters
{
    /// <summary>
    /// Represents an educational course.
    /// </summary>
    public class CourseDto : BaseClassDTO
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
        /// Gets or sets the stream identifier.
        /// </summary>
        /// <value>
        /// The stream identifier.
        /// </value>
        public Guid StreamId { get; set; }

        // One course → one subject

        /// <summary>
        /// Gets or sets the subject.
        /// </summary>
        /// <value>
        /// The subject.
        /// </value>
        public SubjectDto Subject { get; set; } = null!;

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
        public GradeDto Grade { get; set; } = null!;

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

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// status.
        /// </value>
        public string Status { get; set; } = null!;
    }
}
