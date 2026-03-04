namespace Winfocus.LMS.Domain.Entities
{
    using Winfocus.LMS.Domain.Common;

    /// <summary>
    /// Represents a political or administrative state/province within a country.
    /// </summary>
    public class State : BaseEntity
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
        public Country Country { get; set; } = null!;

        /// <summary>
        /// Gets or sets the identifier of the modeOfStudy where the centre is located.
        /// </summary>
        public Guid ModeOfStudyId { get; set; }

        /// <summary>
        /// Gets or sets the modeOfStudy entity associated with the centre.
        /// </summary>
        public ModeOfStudy ModeOfStudy { get; set; } = null!;

        /// <summary>
        /// Gets or sets the collection of centres associated with the state.
        /// </summary>
        public ICollection<Center> Centers { get; set; } = new List<Center>();
    }
}
