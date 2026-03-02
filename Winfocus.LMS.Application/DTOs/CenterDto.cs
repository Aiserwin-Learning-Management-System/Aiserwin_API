using Winfocus.LMS.Application.DTOs.Masters;
using Winfocus.LMS.Domain.Entities;
using Winfocus.LMS.Domain.Enums;

namespace Winfocus.LMS.Application.DTOs
{
    /// <summary>
    /// Center master data transfer object containing master-level fields.
    /// </summary>
    public class CenterDto : BaseClassDTO
    {
        /// <summary>
        /// Gets or sets the name of the centre.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Gets or sets the type of the centre (Offline, Online, or Hybrid).
        /// </summary>
        public CentreType CenterType { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the modeOfStudy where the centre is located.
        /// </summary>
        public Guid ModeOfStudyId { get; set; }

        /// <summary>
        /// Gets or sets the modeOfStudy entity associated with the centre.
        /// </summary>
        public ModeOfStudyDto modeOfStudy { get; set; } = null!;
    }
}
