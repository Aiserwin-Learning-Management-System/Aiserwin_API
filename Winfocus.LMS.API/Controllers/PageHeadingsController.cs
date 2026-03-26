using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Winfocus.LMS.Application.DTOs;
using Winfocus.LMS.Application.Interfaces;

namespace Winfocus.LMS.API.Controllers
{ 
     /// <summary>
     /// Handles authentication endpoints.
     /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class PageHeadingsController : BaseController
    {
        private readonly IPageHeadingService _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="PageHeadingsController"/> class.
        /// </summary>
        /// <param name="service">Page heading service.</param>
        public PageHeadingsController(IPageHeadingService service)
        {
            _service = service;
        }

        /// <summary>
        /// Retrieves all page headings.
        /// </summary>
        /// <returns>List of page headings.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet]
        public async Task<ActionResult<List<PageHeadingResponseDto>>> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        /// <summary>
        /// Retrieves a page heading using the page key.
        /// </summary>
        /// <param name="pageKey">Unique key identifying the page.</param>
        /// <returns>Page heading details.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet("{pageKey}")]
        public async Task<ActionResult<PageHeadingResponseDto>> GetByKey(string pageKey)
        {
            var result = await _service.GetByKeyAsync(pageKey);
            return Ok(result);
        }

        /// <summary>
        /// Updates the main and sub heading of a page.
        /// </summary>
        /// <param name="pageKey">Unique key identifying the page.</param>
        /// <param name="dto">Updated heading details.</param>
        /// <returns>No content if update succeeds.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPut("{pageKey}")]
        public async Task<IActionResult> Update(string pageKey, UpdatePageHeadingDto dto)
        {
           var result = await _service.UpdateAsync(pageKey, dto);
           return Ok(result);
        }
    }
}
