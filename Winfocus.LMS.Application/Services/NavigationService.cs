using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;
using Winfocus.LMS.Application.Constants;
using Winfocus.LMS.Application.DTOs.MenuItem;
using Winfocus.LMS.Application.Interfaces;
using Winfocus.LMS.Domain.Entities;

namespace Winfocus.LMS.Application.Services
{
    /// <summary>
    /// NavigationService.
    /// </summary>
    public class NavigationService : INavigationService
    {
        private readonly IStaffCategoryRepository _staffCategory;
        private readonly IMemoryCache _cache;

        /// <summary>
        /// CACHE_KEY for cache.
        /// </summary>
        private const string CACHE_KEY = "NAVIGATION_MENU";

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationService"/> class.
        /// </summary>
        /// <param name="staffcategory"></param>
        /// <param name="cache"></param>
        public NavigationService(
        IStaffCategoryRepository staffcategory,
        IMemoryCache cache)
        {
            _staffCategory = staffcategory;
            _cache = cache;
        }
        /// <summary>
        /// GetMenuAsync.
        /// </summary>
        /// <returns></returns>
        public async Task<NavigationMenuDto> GetMenuAsync()
        {
            if (_cache.TryGetValue(CACHE_KEY, out NavigationMenuDto cachedMenu))
            {
                return cachedMenu;
            }

            var menu = new NavigationMenuDto();

            // Fixed items
            var fixedMenus = NavigationConstants.FixedMenus;

            // Fetch dynamic staff categories
            var categories = await _staffCategory.GetAllAsync();

            var dynamicMenus = categories.Select(cat => new MenuItemDto
            {
                Type = "dynamic",
                Label = cat.Name,
                Icon = "folder",
                Route = $"/staff/{cat.Id}",
                StaffCategoryId = cat.Id
            }).ToList();

            menu.MenuItems = fixedMenus
                .Concat(dynamicMenus)
                .OrderBy(x => x.Label)
                .ToList();

            var cacheOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(30));

            _cache.Set(CACHE_KEY, menu, cacheOptions);

            return menu;
        }

    }
}
