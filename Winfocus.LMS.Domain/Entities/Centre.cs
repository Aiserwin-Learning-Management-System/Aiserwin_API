namespace Winfocus.LMS.Domain.Entities
{
    using Winfocus.LMS.Domain.Common;
    using Winfocus.LMS.Domain.Enums;

    /// <summary>
    /// Represents a training or learning centre.
    /// </summary>
    public class Centre : BaseEntity
    {
        /// <summary>
        /// Gets or sets the name of the centre.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Gets or sets the type of the centre (Offline, Online, or Hybrid).
        /// </summary>
        public CentreType Type { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the country where the centre is located.
        /// </summary>
        public Guid CountryId { get; set; }

        /// <summary>
        /// Gets or sets the country entity associated with the centre.
        /// </summary>
        public Country Country { get; set; } = null!;
    }
}
