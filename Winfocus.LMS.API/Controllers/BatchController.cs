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
        public async Task<ActionResult<CommonResponse<BatchDto>>> GetAll()
            => Ok(await _batchService.GetAllAsync());

        /// <summary>
        /// Creates the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>BatchDto.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost]
        public async Task<ActionResult<CommonResponse<BatchDto>>> Create(
            BatchRequest request)
        {
            var updatedRequest = request with
            {
                userId = UserId
            };
            var created = await _batchService.CreateAsync(updatedRequest);
            return Ok(created);
        }

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>BatchDto by id.</returns>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<CommonResponse<BatchDto>>> Get(Guid id)
           => Ok(await _batchService.GetByIdAsync(id));

        /// <summary>
        /// Updates the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="request">The request.</param>
        /// <returns>result.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<CommonResponse<BatchDto>>> Update(
            Guid id,
            BatchRequest request)
        {
            var updatedRequest = request with
            {
                userId = UserId
            };
            var updated = await _batchService.UpdateAsync(id, updatedRequest);
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
        {
           var result = await _batchService.DeleteAsync(id);
           return Ok(result);
        }

        /// <summary>
        /// Retrieves filtered batches with pagination.
        /// </summary>
        /// <param name="request">The paged request.</param>
        /// <returns>Paginated list of batches.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet("filter")]
        public async Task<ActionResult<CommonResponse<PagedResult<BatchDto>>>> GetFiltered(
            [FromQuery] PagedRequest request)
        {
            var result = await _batchService.GetFilteredAsync(request);
            return Ok(result);
        }
    }
}
