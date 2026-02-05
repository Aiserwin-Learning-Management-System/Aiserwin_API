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
        public string SyllabusName { get; set; } = null!;

        /// <summary>
        /// Gets or sets the optional code for the syllabus.
        /// </summary>
        public string SyllabusCode { get; set; } = null!;

        /// <summary>
        /// Gets or sets the identifier of the associated centre.
        /// </summary>
        public Guid CenterId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the associated centre.
        /// </summary>
        public virtual CenterDto Center { get; set; } = null!;
    }
}
