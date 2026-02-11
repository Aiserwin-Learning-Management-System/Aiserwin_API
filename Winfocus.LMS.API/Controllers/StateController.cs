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
    [Route("api/[controller]")]
    public sealed class StateController : BaseController
    {
        private readonly IStateService _stateService;

        /// <summary>
        /// Initializes a new instance of the <see cref="StateController"/> class.
        /// </summary>
        /// <param name="stateService">The state service.</param>
        public StateController(IStateService stateService)
        {
            _stateService = stateService;
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns>StateDto list.</returns>
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<StateDto>>> GetAll()
            => Ok(await _stateService.GetAllAsync());

        /// <summary>
        /// Creates the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>StateDto.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost]
        public async Task<ActionResult<StateDto>> Create(
            CreateMasterStateRequest request)
        {
            var updatedRequest = request with
            {
                userId = UserId
            };
            var created = await _stateService.CreateAsync(updatedRequest);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>StateDto by id.</returns>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<StateDto>> Get(Guid id)
        {
            var result = await _stateService.GetByIdAsync(id);
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
            CreateMasterStateRequest request)
        {
            var updatedRequest = request with
            {
                userId = UserId
            };
            await _stateService.UpdateAsync(id, updatedRequest);
            return NoContent();
        }

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="countryid">The identifier.</param>
        /// <returns>StateDto by id.</returns>
        [HttpGet("by-country/{countryid:guid}")]
        public async Task<ActionResult<StateDto>> GetByCountryId(Guid countryid)
        {
            var result = await _stateService.GetByCountryIdAsync(countryid);
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
            await _stateService.DeleteAsync(id);
            return NoContent();
        }
    }
}
