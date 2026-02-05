namespace Winfocus.LMS.Application.DTOs.Masters
{
    /// <summary>
    /// Represents a mode of study (for example, full-time or part-time) and its country association.
    /// </summary>
    public class ModeOfStudyDto : BaseClassDTO
    {
        /// <summary>
        /// Gets or sets the display name of the mode of study.
        /// </summary>
        public string ModeName { get; set; } = null!;

        /// <summary>
        /// Gets or sets the identifier of the associated country.
        /// </summary>
        public Guid CountryId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the associated country.
        /// </summary>
        public CountryDto1 Country { get; set; } = null!;

    }
}
