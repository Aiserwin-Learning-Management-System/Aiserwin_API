using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Winfocus.LMS.Application.DTOs;
using Winfocus.LMS.Application.DTOs.Masters;
using Winfocus.LMS.Application.Interfaces;

namespace Winfocus.LMS.API.Controllers
{
    /// <summary>
    /// Handles authentication endpoints.
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class SyllabusController : BaseController
    {
        private readonly ISyllabusService _syllabusService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SyllabusController"/> class.
        /// </summary>
        /// <param name="syllabusService">The state service.</param>
        public SyllabusController(ISyllabusService syllabusService)
        {
            _syllabusService = syllabusService;
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns>SyllabusDto list.</returns>
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<SyllabusDto>>> GetAll()
            => Ok(await _syllabusService.GetAllAsync());

        /// <summary>
        /// Creates the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>SyllabusDto.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost]
        public async Task<ActionResult<SyllabusDto>> Create(
            SyllabusRequest request)
        {
            var created = await _syllabusService.CreateAsync(request);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>SyllabusDto by id.</returns>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<SyllabusDto>> Get(Guid id)
        {
            var result = await _syllabusService.GetByIdAsync(id);
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
            SyllabusRequest request)
        {
            await _syllabusService.UpdateAsync(id, request);
            return NoContent();
        }

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="centerid">The identifier.</param>
        /// <returns>SyllabusDto by id.</returns>
        [HttpGet("by-center/{centerid:guid}")]
        public async Task<ActionResult<SyllabusDto>> GetByCountryId(Guid centerid)
        {
            var result = await _syllabusService.GetByCenterIdAsync(centerid);
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
            await _syllabusService.DeleteAsync(id);
            return NoContent();
        }

    }
}
