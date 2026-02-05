namespace Winfocus.LMS.Application.DTOs.Masters
{
    /// <summary>
    /// Represents a training or learning centre.
    /// </summary>
    public class CenterDto1 : BaseClassDTO
    {
        /// <summary>
        /// Gets or sets the name of the centre.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Gets or sets the type of the centre (Offline, Online, or Hybrid).
        /// </summary>
        public string Type { get; set; } = "Offline";

        /// <summary>
        /// Gets or sets the identifier of the country where the centre is located.
        /// </summary>
        public Guid CountryId { get; set; }

        /// <summary>
        /// Gets or sets the country entity associated with the centre.
        /// </summary>
        public CourseDto Country { get; set; } = null!;
    }
}
