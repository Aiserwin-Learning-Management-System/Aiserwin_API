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
    public class BatchTimingSaturdayController : BaseController
    {
        private readonly IBatchTimingSaturdayService _batchtimingService;
        private readonly ISubjectService _subjectService;

        /// <summary>
        /// Initializes a new instance of the <see cref="BatchTimingSaturdayController"/> class.
        /// </summary>
        /// <param name="batchtimingService">The BatchTimingSaturday service.</param>
        /// <param name="subjectService">The subject Service.</param>
        public BatchTimingSaturdayController(IBatchTimingSaturdayService batchtimingService, ISubjectService subjectService)
        {
            _batchtimingService = batchtimingService;
            _subjectService = subjectService;
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <param name="centerId">The centerId.</param>
        /// <returns>BatchTimingSaturdayDto list.</returns>
        [HttpGet("{centerId:guid?}")]
        public async Task<ActionResult<CommonResponse<BatchTimingSaturdayDto>>> GetAll(Guid centerId)
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                if (UserId != Guid.Empty)
                {
                    return Ok(await _batchtimingService.GetAllAsync(CenterId));
                }
            }

            return Ok(await _batchtimingService.GetAllAsync(centerId));
        }
            //=> Ok(await _batchtimingService.GetAllAsync(centerId));

        /// <summary>
        /// Creates the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>BatchTimingSaturdayDto.</returns>
        [Authorize(Roles = "Admin,SuperAdmin,CenterAdmin")]
        [HttpPost]
        public async Task<ActionResult<CommonResponse<BatchTimingSaturdayDto>>> Create(
            BatchTimingRequest request)
        {
            var subject = await _subjectService.GetByIdAsync(request.subjectId);
            if (subject?.Data == null)
            {
                return NotFound("Subject not found.");
            }

            if (CenterId != subject.Data.CenterId)
            {
                return StatusCode(403, "You are not allowed to create data for this center.");
            }

            var updatedRequest = request with
            {
                userId = UserId
            };
            var created = await _batchtimingService.CreateAsync(updatedRequest);
            return Ok(created);
        }

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="centerid">The centerid.</param>
        /// <returns>BatchTimingSaturdayDto by id.</returns>
        [HttpGet("{id:guid}/center/{centerid:guid?}")]
        public async Task<ActionResult<CommonResponse<BatchTimingSaturdayDto>>> Get(Guid id, Guid centerid)
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                if (UserId != Guid.Empty)
                {
                    return Ok(await _batchtimingService.GetByIdCenterIdAsync(id, CenterId));
                }
            }

            return Ok(await _batchtimingService.GetByIdCenterIdAsync(id, centerid));
        }
         //=> Ok(await _batchtimingService.GetByIdAsync(id));

        /// <summary>
        /// Updates the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="request">The request.</param>
        /// <returns>result.</returns>
        [Authorize(Roles = "Admin,SuperAdmin,CenterAdmin")]
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<CommonResponse<BatchTimingSaturdayDto>>> Update(
            Guid id,
            BatchTimingRequest request)
        {
            var subject = await _subjectService.GetByIdAsync(request.subjectId);
            if (subject?.Data == null)
            {
                return NotFound("Subject not found.");
            }

            if (CenterId != subject.Data.CenterId)
            {
                return StatusCode(403, "You are not allowed to create data for this center.");
            }

            var updatedRequest = request with
            {
                userId = UserId
            };
            var updated = await _batchtimingService.UpdateAsync(id, updatedRequest);
            return Ok(updated);
        }

        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>result.</returns>
        [Authorize(Roles = "Admin,SuperAdmin,CenterAdmin")]
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<CommonResponse<bool>>> Delete(Guid id)
        {
            var result = await _batchtimingService.DeleteAsync(id, CenterId);
            return Ok(result);
        }

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="subjectid">The identifier.</param>
        /// <returns>BatchTimingSaturdayDto by id.</returns>
        [HttpGet("by-subject/{subjectid:guid}")]
        public async Task<ActionResult<CommonResponse<List<BatchTimingSaturdayDto>>>> GetBySubjectId(Guid subjectid)
        {
            var result = await _batchtimingService.GetBySubjectIdAsync(subjectid);
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
            await _batchtimingService.BatchTimingSubjectCreate(updatedRequest);
            return NoContent();
        }

        /// <summary>
        /// Retrieves filtered batches for saturday with pagination.
        /// </summary>
        /// <param name="request">The paged request.</param>
        /// <returns>Paginated list of batches for saturday.</returns>
        [Authorize(Roles = "Admin,SuperAdmin,CenterAdmin")]
        [HttpGet("filter")]
        public async Task<ActionResult<CommonResponse<PagedResult<BatchTimingSaturdayDto>>>> GetFiltered(
            [FromQuery] PagedRequest request)
        {
            var result = await _batchtimingService.GetFilteredAsync(request, CenterId);
            return Ok(result);
        }
    }
}
