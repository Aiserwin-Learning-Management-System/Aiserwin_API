namespace Winfocus.LMS.Application.DTOs.Masters
{
    using Winfocus.LMS.Domain.Entities;

    /// <summary>
    /// Staff Category master data transfer object containing master-level fields.
    /// </summary>
    public class StaffCategoryDto : BaseClassDTO
    {
        /// <summary>
        /// Gets or sets the display name of the staff category.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Gets or sets the display Description of the staff category.
        /// </summary>
        public string Description { get; set; } = null!;

    }
}
