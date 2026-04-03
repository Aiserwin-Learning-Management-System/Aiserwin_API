namespace Winfocus.LMS.Application.DTOs.Masters
{
    /// <summary>
    /// Represents a syllabus offered by a centre.
    /// </summary>
    public class SyllabusDto : BaseClassDTO
    {
        /// <summary>
        /// Gets or sets the display name of the syllabus.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Gets or sets the identifier of the center.
        /// </summary>
        public Guid? CenterId { get; set; }

        /// <summary>
        /// Gets or sets the center entity.
        /// </summary>
        public CenterDto? Center { get; set; } = null!;

        /// <summary>
        /// Gets or sets the identifier of the associated country.
        /// </summary>
        public Guid CountryId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the associated country.
        /// </summary>
        public CountryDto Country { get; set; } = null!;

        /// <summary>
        /// Gets or sets the identifier of the modeOfStudy where the centre is located.
        /// </summary>
        public Guid ModeOfStudyId { get; set; }

        /// <summary>
        /// Gets or sets the modeOfStudy entity associated with the centre.
        /// </summary>
        public ModeOfStudyDto ModeOfStudy { get; set; } = null!;

        /// <summary>
        /// Gets or sets the identifier of the state where the centre is located.
        /// </summary>
        public Guid State_Id { get; set; }

        /// <summary>
        /// Gets or sets the state associated with the centre.
        /// </summary>
        public StateDto State { get; set; } = null!;

        /// <summary>
        /// Gets or sets the identifier of the academicyear.
        /// </summary>
        public Guid AcademicYearId { get; set; }

        /// <summary>
        /// Gets or sets the AcademicYear.
        /// </summary>
        public AcademicYearDto AcademicYear { get; set; } = null!;
    }
}
