using Winfocus.LMS.Application.DTOs.Masters;

namespace Winfocus.LMS.Application.DTOs
{
    /// <summary>
    /// Country response DTO.
    /// </summary>
   public class CountryDto : BaseClassDTO
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
    }
}
