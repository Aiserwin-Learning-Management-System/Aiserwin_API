using System;
using System.Collections.Generic;
using System.Text;
using Winfocus.LMS.Application.DTOs.MenuItem;

namespace Winfocus.LMS.Application.Constants
{
    /// <summary>
    /// NavigationConstants.
    /// </summary>
    public static class NavigationConstants
    {
        /// <summary>
        /// Gets fixedMenus.
        /// </summary>
        public static List<MenuItemDto> FixedMenus => new()
    {
        new MenuItemDto
        {
            Type = "fixed",
            Label = "Students Registration",
            Icon = "graduation-cap",
            Route = "/students/registration",
            Children = new()
        },
        new MenuItemDto
        {
            Type = "module",
            Label = "User Management",
            Icon = "users",
            Route = "/user-management",
            Children = new()
            {
                new MenuItemDto
                {
                    Type = "fixed",
                    Label = "Create User",
                    Route = "/user-management/create-user"
                },
                new MenuItemDto
                {
                    Type = "fixed",
                    Label = "Content Management",
                    Route = "/user-management/content-management"
                },
                new MenuItemDto
                {
                    Type = "fixed",
                    Label = "Staff Registration Form",
                    Route = "/user-management/staff-registration-form"
                }
            }
        }
    };
    }

}
