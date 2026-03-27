using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Winfocus.LMS.Application.DTOs;
using Winfocus.LMS.Application.DTOs.Common;
using Winfocus.LMS.Application.DTOs.Masters;
using Winfocus.LMS.Application.DTOs.Teacher;
using Winfocus.LMS.Application.Interfaces;
using Winfocus.LMS.Application.Services;

namespace Winfocus.LMS.API.Controllers
{
    /// <summary>
    /// Handles authentication endpoints.
    /// </summary>
    [Authorize(Roles = "Admin,SuperAdmin")]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class TeachingToolsController : BaseController
    {
        private readonly ITeachingToolsService _toolsService;

        /// <summary>
        /// Initializes a new instance of the <see cref="TeachingToolsController"/> class.
        /// </summary>
        /// <param name="toolsService">The teaching tools service.</param>
        public TeachingToolsController(ITeachingToolsService toolsService)
        {
            _toolsService = toolsService;
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns>teaching tools list.</returns>
        [HttpGet]
        public async Task<ActionResult<CommonResponse<TeachingToolsResponseDto>>> GetAll()
            => Ok(await _toolsService.GetAllAsync());

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>TeachingToolsResponseDto by id.</returns>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<CommonResponse<TeachingToolsResponseDto>>> Get(Guid id)
           => Ok(await _toolsService.GetByIdAsync(id));

        /// <summary>
        /// Creates the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>TeachingToolsResponseDto.</returns>
        [HttpPost]
        public async Task<ActionResult<CommonResponse<TeachingToolsResponseDto>>> Create(
            TeachingToolsDto request)
        {
            request.Userid = UserId;
            var created = await _toolsService.CreateAsync(request);
            return Ok(created);
        }

        /// <summary>
        /// Updates the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="request">The request.</param>
        /// <returns>result.</returns>
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<CommonResponse<TeachingToolsResponseDto>>> Update(
            Guid id,
            TeachingToolsDto request)
        {
            request.Userid = UserId;
            var updated = await _toolsService.UpdateAsync(id, request);
            return Ok(updated);
        }

        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>result.</returns>
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<CommonResponse<bool>>> Delete(Guid id)
        {
            var result = await _toolsService.DeleteAsync(id);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves filtered teaching tools with pagination.
        /// </summary>
        /// <param name="request">The paged request.</param>
        /// <returns>Paginated list of teaching tools.</returns>
        [HttpGet("filter")]
        public async Task<ActionResult<CommonResponse<PagedResult<TeachingToolsResponseDto>>>> GetFiltered(
            [FromQuery] PagedRequest request)
        {
            var result = await _toolsService.GetFilteredAsync(request);
            return Ok(result);
        }
    }
}
