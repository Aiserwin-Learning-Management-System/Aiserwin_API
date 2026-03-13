using System;
using System.Collections.Generic;
using System.Text;

namespace Winfocus.LMS.Application.DTOs.MenuItem
{
    /// <summary>
    /// MenuItemDto.
    /// </summary>
    public class MenuItemDto
    {
        /// <summary>
        /// Gets or sets get set Type.
        /// </summary>
        public string Type { get; set; } = string.Empty;

        /// <summary>
        ///  Gets or sets get set Type.
        /// </summary>
        public string Label { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets get set Type.
        /// </summary>
        public string? Icon { get; set; }

        /// <summary>
        /// Gets or sets get set Type.
        /// </summary>
        public string Route { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets get set Type.
        /// </summary>
        public Guid? StaffCategoryId { get; set; }

        /// <summary>
        /// Gets or sets get set Type.
        /// </summary>
        public List<MenuItemDto> Children { get; set; } = new();
    }
}
