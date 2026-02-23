namespace Winfocus.LMS.Application.DTOs.Masters
{
    /// <summary>
    /// Represents a syllabus offered by a centre.
    /// </summary>
    public class SyllabusDto : BaseClassDTO
    {
        /// <summary>
        /// Gets or sets the display name of the syllabus.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Gets or sets the identifier of the associated centre.
        /// </summary>
        public Guid CenterId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the associated centre.
        /// </summary>
        public virtual CenterDto1 Center { get; set; } = null!;
    }
}
