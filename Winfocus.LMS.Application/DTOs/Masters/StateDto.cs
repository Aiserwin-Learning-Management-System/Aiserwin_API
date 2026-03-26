namespace Winfocus.LMS.Application.DTOs.Masters
{
    /// <summary>
    /// Represents a political or administrative state/province within a country.
    /// </summary>
    public class StateDto : BaseClassDTO
    {
        /// <summary>
        /// Gets or sets the display name of the state.
        /// </summary>
        public string Name { get; set; } = null!;

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
    }
}
