namespace Winfocus.LMS.Domain.Entities
{
    using Winfocus.LMS.Domain.Common;

    /// <summary>
    /// Represents a country entity with its name, code, and associated centres.
    /// </summary>
    public class Country : BaseEntity
    {
        /// <summary>
        /// Gets or sets the name of the country.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Gets or sets the code of the country.
        /// </summary>
        public string Code { get; set; } = null!;

        /// <summary>
        /// Gets or sets the iso alpha3.
        /// </summary>
        /// <value>
        /// The iso alpha3.
        /// </value>
        public string IsoAlpha3 { get; set; } = null!;

        /// <summary>
        /// Gets or sets the iso numeric.
        /// </summary>
        /// <value>
        /// The iso numeric.
        /// </value>
        public int IsoNumeric { get; set; }

        /// <summary>
        /// Gets or sets the collection of centres associated with the country.
        /// </summary>
        public ICollection<Centre> Centres { get; set; } = new List<Centre>();
    }
}
