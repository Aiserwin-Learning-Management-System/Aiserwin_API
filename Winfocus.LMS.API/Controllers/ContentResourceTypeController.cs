using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Winfocus.LMS.Application.DTOs;
using Winfocus.LMS.Application.DTOs.Common;
using Winfocus.LMS.Application.DTOs.Masters;
using Winfocus.LMS.Application.Interfaces;
using Winfocus.LMS.Application.Services;

namespace Winfocus.LMS.API.Controllers
{
    /// <summary>
    /// Handles authentication endpoints.
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ContentResourceTypeController : BaseController
    {
        private readonly IContentResourceTypeService _contentService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentResourceTypeController"/> class.
        /// </summary>
        /// <param name="contentService">The contentService service.</param>
        public ContentResourceTypeController(IContentResourceTypeService contentService)
        {
            _contentService = contentService;
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns>ContentResourceTypeDto list.</returns>
        [HttpGet]
        public async Task<ActionResult<CommonResponse<ContentResourceTypeDto>>> GetAll()
            => Ok(await _contentService.GetAllAsync());

        /// <summary>
        /// Creates the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>ContentResourceTypeDto.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost]
        public async Task<ActionResult<CommonResponse<ContentResourceTypeDto>>> Create(
            ContentResourceTypeRequest request)
        {
            var created = await _contentService.CreateAsync(request);
            return Ok(created);
        }

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ContentResourceTypeDto by id.</returns>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<CommonResponse<ContentResourceTypeDto>>> Get(Guid id)
        {
            var result = await _contentService.GetByIdAsync(id);
            return Ok(result);
        }

        /// <summary>
        /// Updates the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="request">The request.</param>
        /// <returns>result.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<CommonResponse<ContentResourceTypeDto>>> Update(
            Guid id,
            ContentResourceTypeRequest request)
        {
            var updated = await _contentService.UpdateAsync(id, request);
            return Ok(updated);
        }

        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>result.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<CommonResponse<bool>>> Delete(Guid id)
        => Ok(await _contentService.DeleteAsync(id));

        /// <summary>
        /// Retrieves filtered batches for monday to friday with pagination.
        /// </summary>
        /// <param name="request">The paged request.</param>
        /// <returns>Paginated list of doubt clearing.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet("filter")]
        public async Task<ActionResult<CommonResponse<PagedResult<ContentResourceTypeDto>>>> GetFiltered(
            [FromQuery] PagedRequest request)
        {
            var result = await _contentService.GetFilteredAsync(request);
            return Ok(result);
        }

        /// <summary>
        /// Gets content resource type by chapter identifier.
        /// </summary>
        /// <param name="chapterid">The chapter identifier.</param>
        /// <returns>ContentResourceTypeDto list.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet("by-chapter/{chapterid:guid}")]
        public async Task<ActionResult<CommonResponse<List<ContentResourceTypeDto>>>> GetByChapter(
            Guid chapterid)
            => Ok(await _contentService.GetByChapterIdAsync(chapterid));
    }
}
