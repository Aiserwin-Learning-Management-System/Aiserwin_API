using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Winfocus.LMS.Application.DTOs;
using Winfocus.LMS.Application.DTOs.Common;
using Winfocus.LMS.Application.DTOs.Masters;
using Winfocus.LMS.Application.DTOs.Students;
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
    public class BatchTimingMTFController : BaseController
    {
        private readonly IBatchTimingMTFService _batchtimingmtfService;

        /// <summary>
        /// Initializes a new instance of the <see cref="BatchTimingMTFController"/> class.
        /// </summary>
        /// <param name="batchtimingmtfService">The batchtimingmtf service.</param>
        public BatchTimingMTFController(IBatchTimingMTFService batchtimingmtfService)
        {
            _batchtimingmtfService = batchtimingmtfService;
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns>BatchTimingMTFDto list.</returns>
        [HttpGet]
        public async Task<ActionResult<CommonResponse<BatchTimingMTFDto>>> GetAll()
            => Ok(await _batchtimingmtfService.GetAllAsync());

        /// <summary>
        /// Creates the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>BatchTimingMTFDto.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost]
        public async Task<ActionResult<CommonResponse<BatchTimingMTFDto>>> Create(
            BatchTimingRequest request)
        {
            var updatedRequest = request with
            {
                userId = UserId
            };
            var created = await _batchtimingmtfService.CreateAsync(updatedRequest);
            return Ok(created);
        }

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>BatchTimingMTFDto by id.</returns>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<CommonResponse<BatchTimingMTFDto>>> Get(Guid id)
        {
            var result = await _batchtimingmtfService.GetByIdAsync(id);
            return Ok(result);
        }

        /// <summary>
        /// Updates the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="request">The request.</param>
        /// <returns>result.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<CommonResponse<BatchTimingMTFDto>>> Update(
            Guid id,
            BatchTimingRequest request)
        {
            var updatedRequest = request with
            {
                userId = UserId
            };
            var updated = await _batchtimingmtfService.UpdateAsync(id, updatedRequest);
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
        => Ok(await _batchtimingmtfService.DeleteAsync(id));

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="subjectid">The identifier.</param>
        /// <returns>BatchTimingMTFDto by id.</returns>
        [HttpGet("by-subject/{subjectid:guid}")]
        public async Task<ActionResult<CommonResponse<List<BatchTimingMTFDto>>>> GetBySubjectId(Guid subjectid)
        {
            var result = await _batchtimingmtfService.GetBySubjectIdAsync(subjectid);
            return Ok(result);
        }

        /// <summary>
        /// Creates the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>result.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost("subject")]
        public async Task<IActionResult> BatchTimingSubjectCreate(
            SubjectBatchTimingRequest request)
        {
            var updatedRequest = request with
            {
                userid = UserId
            };
            await _batchtimingmtfService.BatchTimingSubjectCreate(updatedRequest);
            return NoContent();
        }

        /// <summary>
        /// Retrieves filtered batches for monday to friday with pagination.
        /// </summary>
        /// <param name="request">The paged request.</param>
        /// <returns>Paginated list of batches for monday to friday.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet("filter")]
        public async Task<ActionResult<CommonResponse<PagedResult<BatchTimingMTFDto>>>> GetFiltered(
            [FromQuery] PagedRequest request)
        {
            var result = await _batchtimingmtfService.GetFilteredAsync(request);
            return Ok(result);
        }
    }
}
