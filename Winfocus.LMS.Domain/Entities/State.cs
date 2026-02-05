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
        public string StateName { get; set; } = null!;

        /// <summary>
        /// Gets or sets the optional code for the state.
        /// </summary>
        public string StateCode { get; set; } = null!;

        /// <summary>
        /// Gets or sets the identifier of the associated country.
        /// </summary>
        public Guid CountryId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the associated country.
        /// </summary>
        public Country Country { get; set; } = null!;
    }
}
