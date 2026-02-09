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
    public class ModeOfStudyController : Controller
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
        public async Task<ActionResult<ModeOfStudyDto>> GetByCountryId(Guid stateid)
        {
            var result = await _modeofstudyService.GetByStateIdAsync(stateid);
            return result == null ? NotFound() : Ok(result);
        }
    }
}
