using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Winfocus.LMS.Application.DTOs.MenuItem;
using Winfocus.LMS.Application.Interfaces;

namespace Winfocus.LMS.API.Controllers
{
    /// <summary>
    /// Handles authentication endpoints.
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class NavigationController : BaseController
    {
        private readonly INavigationService _navigationService;

        /// <summary>
        /// NavigationController.
        /// </summary>
        /// <param name="navigationService"></param>
        public NavigationController(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        /// <summary>
        /// GetMenu.
        /// </summary>
        /// <returns></returns>
        [HttpGet("menu")]
        public async Task<ActionResult<NavigationMenuDto>> GetMenu()
        {
            var menu = await _navigationService.GetMenuAsync();
            return Ok(menu);
        }
    }
}
