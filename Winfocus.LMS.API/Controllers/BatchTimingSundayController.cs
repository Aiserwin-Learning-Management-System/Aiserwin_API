using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Winfocus.LMS.Application.DTOs;
using Winfocus.LMS.Application.DTOs.Common;
using Winfocus.LMS.Application.DTOs.Masters;
using Winfocus.LMS.Application.Interfaces;
using Winfocus.LMS.Application.Services;
using Winfocus.LMS.Domain.Entities;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Winfocus.LMS.API.Controllers
{
    /// <summary>
    /// Handles authentication endpoints.
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class BatchTimingSundayController : BaseController
    {
        private readonly IBatchTimingSundayService _batchtimingsundayService;

        /// <summary>
        /// Initializes a new instance of the <see cref="BatchTimingSundayController"/> class.
        /// </summary>
        /// <param name="batchtimingsundayService">The BatchTimingSunday service.</param>
        public BatchTimingSundayController(IBatchTimingSundayService batchtimingsundayService)
        {
            _batchtimingsundayService = batchtimingsundayService;
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns>BatchTimingSundayDto list.</returns>
        [HttpGet]
        public async Task<ActionResult<CommonResponse<BatchTimingSundayDto>>> GetAll()
            => Ok(await _batchtimingsundayService.GetAllAsync());

        /// <summary>
        /// Creates the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>BatchTimingSundayDto.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost]
        public async Task<ActionResult<CommonResponse<BatchTimingSundayDto>>> Create(
            BatchTimingRequest request)
        {
            var updatedRequest = request with
            {
                userId = UserId
            };
            var created = await _batchtimingsundayService.CreateAsync(updatedRequest);
            return Ok(created);
        }

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>BatchTimingSundayDto by id.</returns>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<CommonResponse<BatchTimingSundayDto>>> Get(Guid id)
        {
            var result = await _batchtimingsundayService.GetByIdAsync(id);
            return result;
        }

        /// <summary>
        /// Updates the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="request">The request.</param>
        /// <returns>result.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<CommonResponse<BatchTimingSundayDto>>> Update(
            Guid id,
            BatchTimingRequest request)
        {
            var updatedRequest = request with
            {
                userId = UserId
            };
            var updated = await _batchtimingsundayService.UpdateAsync(id, updatedRequest);
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
        => Ok(await _batchtimingsundayService.DeleteAsync(id));

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="subjectid">The identifier.</param>
        /// <returns>BatchTimingSundayDto by id.</returns>
        [HttpGet("by-subject/{subjectid:guid}")]
        public async Task<ActionResult<CommonResponse<List<BatchTimingSundayDto>>>> GetBySubjectId(Guid subjectid)
        {
            var result = await _batchtimingsundayService.GetBySubjectIdAsync(subjectid);
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
            await _batchtimingsundayService.BatchTimingSubjectCreate(updatedRequest);
            return NoContent();
        }

        /// <summary>
        /// Retrieves filtered batches for sunday with pagination.
        /// </summary>
        /// <param name="request">The paged request.</param>
        /// <returns>Paginated list of batches for sunday.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet("filter")]
        public async Task<ActionResult<CommonResponse<PagedResult<BatchTimingSundayDto>>>> GetFiltered(
            [FromQuery] PagedRequest request)
        {
            var result = await _batchtimingsundayService.GetFilteredAsync(request);
            return Ok(result);
        }
    }
}
