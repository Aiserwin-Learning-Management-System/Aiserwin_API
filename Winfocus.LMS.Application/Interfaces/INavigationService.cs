using System;
using System.Collections.Generic;
using System.Text;
using Winfocus.LMS.Application.DTOs.MenuItem;

namespace Winfocus.LMS.Application.Interfaces
{
    /// <summary>
    /// INavigationService.
    /// </summary>
    public interface INavigationService
    {
        /// <summary>
        /// GetMenuAsync.
        /// </summary>
        /// <returns></returns>
        Task<NavigationMenuDto> GetMenuAsync();
    }
}
