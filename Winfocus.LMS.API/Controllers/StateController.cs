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
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public sealed class StateController : ControllerBase
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
        /// <returns>CountryDto list.</returns>
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<CountryDto>>> GetAll()
            => Ok(await _stateService.GetAllAsync());

        /// <summary>
        /// Creates the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>CountryDto.</returns>
        [HttpPost]
        public async Task<ActionResult<StateDto>> Create(
            CreateMasterStateRequest request)
        {
            var created = await _stateService.CreateAsync(request);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>CountryDto by id.</returns>
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
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(
            Guid id,
            CreateMasterStateRequest request)
        {
            await _stateService.UpdateAsync(id, request);
            return NoContent();
        }

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="countryid">The identifier.</param>
        /// <returns>CountryDto by id.</returns>
        [HttpGet("by-country/{countryid:guid}")]
        public async Task<ActionResult<StateDto>> GetByCountryId(Guid countryid)
        {
            var result = await _stateService.GetByCountryIdAsync(countryid);
            return result == null ? NotFound() : Ok(result);
        }
    }
}
