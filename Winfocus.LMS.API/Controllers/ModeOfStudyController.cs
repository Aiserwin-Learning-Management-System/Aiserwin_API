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
    public class ModeOfStudyController : BaseController
    {
        private readonly IModeOfStudyService _modeofstudyService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModeOfStudyController"/> class.
        /// </summary>
        /// <param name="modeofstudyService">The modeofstudy service.</param>
        public ModeOfStudyController(IModeOfStudyService modeofstudyService)
        {
            _modeofstudyService = modeofstudyService;
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns>ModeOfStudyDto list.</returns>
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ModeOfStudyDto>>> GetAll()
            => Ok(await _modeofstudyService.GetAllAsync());

        /// <summary>
        /// Creates the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>ModeOfStudyDto.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost]
        public async Task<ActionResult<ModeOfStudyDto>> Create(
            ModeOfStudyRequest request)
        {
            var created = await _modeofstudyService.CreateAsync(request);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ModeOfStudyDto by id.</returns>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ModeOfStudyDto>> Get(Guid id)
        {
            var result = await _modeofstudyService.GetByIdAsync(id);
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
            ModeOfStudyRequest request)
        {
            await _modeofstudyService.UpdateAsync(id, request);
            return NoContent();
        }

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="stateid">The identifier.</param>
        /// <returns>ModeOfStudyDto by id.</returns>
        [HttpGet("by-state/{stateid:guid}")]
        public async Task<ActionResult<ModeOfStudyDto>> GetByStateId(Guid stateid)
        {
            var result = await _modeofstudyService.GetByStateIdAsync(stateid);
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
            await _modeofstudyService.DeleteAsync(id);
            return NoContent();
        }
    }
}
