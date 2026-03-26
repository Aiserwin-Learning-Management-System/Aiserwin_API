namespace Winfocus.LMS.Application.DTOs
{
    using Winfocus.LMS.Application.DTOs.Masters;

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
        /// Gets or sets the phone code.
        /// </summary>
        /// <value>
        /// The phone code.
        /// </value>
        public string PhoneCode { get; set; } = null!;
    }
}
