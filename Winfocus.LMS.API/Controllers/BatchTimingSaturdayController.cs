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
    public class BatchTimingSaturdayController : BaseController
    {
        private readonly IBatchTimingSaturdayService _batchtimingService;

        /// <summary>
        /// Initializes a new instance of the <see cref="BatchTimingSaturdayController"/> class.
        /// </summary>
        /// <param name="batchtimingService">The BatchTimingSaturday service.</param>
        public BatchTimingSaturdayController(IBatchTimingSaturdayService batchtimingService)
        {
            _batchtimingService = batchtimingService;
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns>BatchTimingSaturdayDto list.</returns>
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<BatchTimingSaturdayDto>>> GetAll()
            => Ok(await _batchtimingService.GetAllAsync());

        /// <summary>
        /// Creates the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>BatchTimingSaturdayDto.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost]
        public async Task<ActionResult<BatchTimingSaturdayDto>> Create(
            BatchTimingRequest request)
        {
            var updatedRequest = request with
            {
                userId = UserId
            };
            var created = await _batchtimingService.CreateAsync(updatedRequest);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>BatchTimingSaturdayDto by id.</returns>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<BatchTimingSaturdayDto>> Get(Guid id)
        {
            var result = await _batchtimingService.GetByIdAsync(id);
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
            BatchTimingRequest request)
        {
            var updatedRequest = request with
            {
                userId = UserId
            };
            await _batchtimingService.UpdateAsync(id, updatedRequest);
            return NoContent();
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
            await _batchtimingService.DeleteAsync(id);
            return NoContent();
        }
    }
}
