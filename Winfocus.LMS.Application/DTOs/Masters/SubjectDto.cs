using Winfocus.LMS.Domain.Entities;

namespace Winfocus.LMS.Application.DTOs.Masters
{
    /// <summary>
    /// Represents an academic subject taught within a course.
    /// </summary>
    public class SubjectDto : BaseClassDTO
    {
        /// <summary>
        /// Gets or sets the display name of the subject.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Gets or sets the identifier of the associated courses.
        /// </summary>
        public Guid CourseId { get; set; }

        /// <summary>
        /// Gets or sets the associated courses.
        /// </summary>
        public CourseDto Course { get; set; } = null!;
    }
}
