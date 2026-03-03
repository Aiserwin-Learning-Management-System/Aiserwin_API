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
        /// Gets or sets the code.
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        public string Code { get; set; } = null!;

        /// <summary>
        /// Gets or sets the collection of centres associated with the country.
        /// </summary>
        public ICollection<Center> Centers { get; set; } = new List<Center>();
    }
}
