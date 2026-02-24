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
    public class BatchController : BaseController
    {
        private readonly IBatchService _batchService;

        /// <summary>
        /// Initializes a new instance of the <see cref="BatchController"/> class.
        /// </summary>
        /// <param name="batchService">The batch service.</param>
        public BatchController(IBatchService batchService)
        {
            _batchService = batchService;
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns>BatchDto list.</returns>
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<BatchDto>>> GetAll()
            => Ok(await _batchService.GetAllAsync());

        /// <summary>
        /// Creates the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>BatchDto.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost]
        public async Task<ActionResult<BatchDto>> Create(
            BatchRequest request)
        {
            var updatedRequest = request with
            {
                userId = UserId
            };
            var created = await _batchService.CreateAsync(updatedRequest);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>BatchDto by id.</returns>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<BatchDto>> Get(Guid id)
        {
            var result = await _batchService.GetByIdAsync(id);
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
            BatchRequest request)
        {
            var updatedRequest = request with
            {
                userId = UserId
            };
            await _batchService.UpdateAsync(id, updatedRequest);
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
            await _batchService.DeleteAsync(id);
            return NoContent();
        }
    }
}
