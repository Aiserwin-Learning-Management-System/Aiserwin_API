using System;
using System.Collections.Generic;
using System.Text;

namespace Winfocus.LMS.Application.DTOs.MenuItem
{
    /// <summary>
    /// NavigationMenuDto.
    /// </summary>
    public class NavigationMenuDto
    {
        /// <summary>
        /// Gets or sets get set Type.
        /// </summary>
        public List<MenuItemDto> MenuItems { get; set; } = new();
    }
}
