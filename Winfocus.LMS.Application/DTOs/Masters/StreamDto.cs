namespace Winfocus.LMS.Application.DTOs.Masters
{
    /// <summary>
    /// Represents an academic stream within a grade.
    /// </summary>
    public class StreamDto : BaseClassDTO
    {
        /// <summary>
        /// Gets or sets the display name of the stream.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Gets or sets the identifier of the associated grade.
        /// </summary>
        public Guid GradeId { get; set; }

        /// <summary>
        /// Gets or sets the associated grade.
        /// </summary>
        public GradeDto? Grade { get; set; }

        /// <summary>
        /// Gets or sets the courses.
        /// </summary>
        public IReadOnlyList<CourseDto> Courses { get; set; } = new List<CourseDto>();

        /// <summary>
        /// Gets or sets the display code of the stream.
        /// </summary>
        public string StreamCode { get; set; } = null!;

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
