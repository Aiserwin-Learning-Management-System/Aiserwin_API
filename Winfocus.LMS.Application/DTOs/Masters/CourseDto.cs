namespace Winfocus.LMS.Application.DTOs.Masters
{
    using Winfocus.LMS.Domain.Entities;

    /// <summary>
    /// Represents an educational course.
    /// </summary>
    public class CourseDto : BaseClassDTO
    {
        /// <summary>
        /// Gets or sets the display name of the course.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Gets or sets the stream identifier.
        /// </summary>
        /// <value>
        /// The stream identifier.
        /// </value>
        public Guid StreamId { get; set; }

        /// <summary>
        /// Gets or sets the stream.
        /// </summary>
        /// <value>
        /// The stream.
        /// </value>
        public StreamDto Stream { get; set; } = null!;

        /// <summary>
        /// Gets or sets the subject.
        /// </summary>
        /// <value>
        /// The subject.
        /// </value>
        public SubjectDto? Subject { get; set; } = null!;

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
        /// Gets or sets the display course code code of the course.
        /// </summary>
        public string CourseCode { get; set; } = null!;

        /// <summary>
        /// Gets or sets the identifier of the associated syllabus.
        /// </summary>
        public Guid SyllabusId { get; set; }

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
        public Guid? CenterId { get; set; }
    }
}
