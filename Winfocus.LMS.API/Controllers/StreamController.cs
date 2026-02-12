using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Winfocus.LMS.Application.DTOs;
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
    public class StreamController : ControllerBase
    {
        private readonly IStreamService _streamService;

        /// <summary>
        /// Initializes a new instance of the <see cref="StreamController"/> class.
        /// </summary>
        /// <param name="streamService">The state service.</param>
        public StreamController(IStreamService streamService)
        {
            _streamService = streamService;
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns>StreamDto list.</returns>
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<StreamDto>>> GetAll()
            => Ok(await _streamService.GetAllAsync());

        /// <summary>
        /// Creates the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>StreamDto.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost]
        public async Task<ActionResult<StreamDto>> Create(
            StreamRequest request)
        {
            var created = await _streamService.CreateAsync(request);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>StreamDto by id.</returns>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<StreamDto>> Get(Guid id)
        {
            var result = await _streamService.GetByIdAsync(id);
            return result == null ? NotFound() : Ok(result);
        }

        /// <summary>
        /// Updates the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="request">The request.</param>
        /// <returns>result.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(
            Guid id,
            StreamRequest request)
        {
            await _streamService.UpdateAsync(id, request);
            return NoContent();
        }

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="gradeid">The identifier.</param>
        /// <returns>StreamDto by id.</returns>
        [HttpGet("by-grade/{gradeid:guid}")]
        public async Task<ActionResult<StreamDto>> GetByCountryId(Guid gradeid)
        {
            var result = await _streamService.GetByGradeIdAsync(gradeid);
            return result == null ? NotFound() : Ok(result);
        }

        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>result.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _streamService.DeleteAsync(id);
            return NoContent();
        }
    }
}
