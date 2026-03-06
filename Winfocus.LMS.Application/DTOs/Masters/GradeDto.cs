namespace Winfocus.LMS.Application.DTOs.Masters
{
    /// <summary>
    /// Represents an academic grade (class/level) within a syllabus.
    /// </summary>
    public class GradeDto : BaseClassDTO
    {
        /// <summary>
        /// Gets or sets the display name of the grade.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Gets or sets the identifier of the associated syllabus.
        /// </summary>
        public Guid SyllabusId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the associated syllabus.
        /// </summary>
        public SyllabusDto Syllabus { get; set; } = null!;

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
