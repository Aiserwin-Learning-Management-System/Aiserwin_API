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

        /// <summary>
        /// Gets or sets the associated courses.
        /// </summary>
        public string SubjectCode { get; set; } = null!;

        /// <summary>
        /// Gets or sets the stream identifier.
        /// </summary>
        /// <value>
        /// The stream identifier.
        /// </value>
        public Guid StreamId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the associated syllabus.
        /// </summary>
        public Guid SyllabusId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the associated Grade.
        /// </summary>
        public Guid GradeId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the associated country.
        /// </summary>
        public Guid CountryId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the modeOfStudy where the centre is located.
        /// </summary>
        public Guid ModeOfStudyId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the state where the centre is located.
        /// </summary>
        public Guid? StateId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the center.
        /// </summary>
        public Guid CenterId { get; set; }
    }

}
