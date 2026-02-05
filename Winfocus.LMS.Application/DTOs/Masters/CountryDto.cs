namespace Winfocus.LMS.Application.DTOs.Masters
{
    /// <summary>
    /// Country master data transfer object containing master-level fields.
    /// </summary>
    public class CountryDto : BaseClassDTO
    {
        /// <summary>
        /// Gets or sets the display name of the country.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Gets or sets the short code  of the country.
        /// </summary>
        public string Code { get; set; } = null!;
    }
}
