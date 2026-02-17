using Winfocus.LMS.Domain.Common;

namespace Winfocus.LMS.Domain.Entities
{
    /// <summary>
    /// Represents the association between a Student and a course.
    /// </summary>
    public class StudentAcademicCouses : BaseEntity
    {
        /// <summary>
        /// Gets or sets the identifier of the student.
        /// </summary>
        public Guid StudentId { get; set; }

        /// <summary>
        /// Gets or sets the student associated with this mapping.
        /// </summary>
        public Student Student { get; set; } = null!;

        /// <summary>
        /// Gets or sets the identifier of the Course.
        /// </summary>
        public Guid CourseId { get; set; }

        /// <summary>
        /// Gets or sets the Course associated with this mapping.
        /// </summary>
        public Course Course { get; set; } = null!;
    }
}
