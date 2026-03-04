namespace Winfocus.LMS.Domain.Entities
{
    using Winfocus.LMS.Domain.Common;
    using Winfocus.LMS.Domain.Enums;

    /// <summary>
    /// Represents a training or learning centre.
    /// </summary>
    public class Center : BaseEntity
    {
        /// <summary>
        /// Gets or sets the name of the centre.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Gets or sets the type of the centre (Offline, Online, or Hybrid).
        /// </summary>
        public CentreType CenterType { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the modeOfStudy where the centre is located.
        /// </summary>
        public Guid ModeOfStudyId { get; set; }

        /// <summary>
        /// Gets or sets the modeOfStudy entity associated with the centre.
        /// </summary>
        public ModeOfStudy modeOfStudy { get; set; } = null!;

        /// <summary>
        /// Gets or sets the identifier of the country where the centre is located.
        /// </summary>
        public Guid CountryId { get; set; }

        /// <summary>
        /// Gets or sets the modeOfStudy entity associated with the centre.
        /// </summary>
        public Country Country { get; set; } = null!;

        /// <summary>
        /// Gets or sets the identifier of the state where the centre is located.
        /// </summary>
        public Guid? StateId { get; set; }

        /// <summary>
        /// Gets or sets the state entity associated with the centre.
        /// </summary>
        public State? State { get; set; } = null!;
    }
}
