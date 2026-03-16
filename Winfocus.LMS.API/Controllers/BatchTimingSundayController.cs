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
        private readonly ISubjectService _subjectService;

        /// <summary>
        /// Initializes a new instance of the <see cref="BatchTimingSundayController"/> class.
        /// </summary>
        /// <param name="batchtimingsundayService">The BatchTimingSunday service.</param>
        /// <param name="subjectService">The subject Service.</param>
        public BatchTimingSundayController(IBatchTimingSundayService batchtimingsundayService, ISubjectService subjectService)
        {
            _batchtimingsundayService = batchtimingsundayService;
            _subjectService = subjectService;
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <param name="centerId">The centerId.</param>
        /// <returns>BatchTimingSundayDto list.</returns>
        [HttpGet("{centerId:guid?}")]
        public async Task<ActionResult<CommonResponse<BatchTimingSundayDto>>> GetAll(Guid centerId)
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                if (User.IsInRole("SuperAdmin"))
                {
                    return Ok(await _batchtimingsundayService.GetAllAsync(Guid.Empty));
                }

                if (UserId != Guid.Empty)
                {
                    return Ok(await _batchtimingsundayService.GetAllAsync(CenterId));
                }
            }

            return Ok(await _batchtimingsundayService.GetAllAsync(centerId));
        }

            //=> Ok(await _batchtimingsundayService.GetAllAsync(centerId));

        /// <summary>
        /// Creates the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>BatchTimingSundayDto.</returns>
        [Authorize(Roles = "Admin,SuperAdmin,CenterAdmin")]
        [HttpPost]
        public async Task<ActionResult<CommonResponse<BatchTimingSundayDto>>> Create(
            BatchTimingRequest request)
        {
            var subject = await _subjectService.GetByIdAsync(request.subjectId);
            if (subject?.Data == null)
            {
                return NotFound("Subject not found.");
            }

            var isSuperAdmin = User.IsInRole("SuperAdmin");
            if (!isSuperAdmin && CenterId != subject.Data.CenterId)
            {
                return StatusCode(403, "You are not allowed to create data for this center.");
            }

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
        /// <param name="centerid">The centerid.</param>
        /// <returns>BatchTimingSundayDto by id.</returns>
        [HttpGet("{id:guid}/center/{centerid:guid?}")]
        public async Task<ActionResult<CommonResponse<BatchTimingSundayDto>>> Get(Guid id, Guid centerid)
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                if (UserId != Guid.Empty)
                {
                    return Ok(await _batchtimingsundayService.GetByIdCenterIdAsync(id, CenterId));
                }
                if (User.IsInRole("SuperAdmin"))
                {
                    return Ok(await _batchtimingsundayService.GetByIdAsync(id));
                }
            }

            return Ok(await _batchtimingsundayService.GetByIdCenterIdAsync(id, centerid));
        }

        /// <summary>
        /// Updates the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="request">The request.</param>
        /// <returns>result.</returns>
        [Authorize(Roles = "Admin,SuperAdmin,CenterAdmin")]
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<CommonResponse<BatchTimingSundayDto>>> Update(
            Guid id,
            BatchTimingRequest request)
        {
            var subject = await _subjectService.GetByIdAsync(request.subjectId);
            if (subject?.Data == null)
            {
                return NotFound("Subject not found.");
            }
            var isSuperAdmin = User.IsInRole("SuperAdmin");
            if (!isSuperAdmin && CenterId != subject.Data.CenterId)
            {
                return StatusCode(403, "You are not allowed to create data for this center.");
            }

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
        [Authorize(Roles = "Admin,SuperAdmin,CenterAdmin")]
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<CommonResponse<bool>>> Delete(Guid id)
        {
            var isSuperAdmin = User.IsInRole("SuperAdmin");
            if (isSuperAdmin)
            {
                return await _batchtimingsundayService.DeleteAsync(id, Guid.Empty);
            }

            var result = await _batchtimingsundayService.DeleteAsync(id, CenterId);
            return Ok(result);
        }
       // => Ok(await _batchtimingsundayService.DeleteAsync(id, CenterId));

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
        [Authorize(Roles = "Admin,SuperAdmin,CenterAdmin")]
        [HttpGet("filter")]
        public async Task<ActionResult<CommonResponse<PagedResult<BatchTimingSundayDto>>>> GetFiltered(
            [FromQuery] PagedRequest request)
        {

            var isSuperAdmin = User.IsInRole("SuperAdmin");
            if (isSuperAdmin)
            {
                return Ok(await _batchtimingsundayService.GetFilteredAsync(request, Guid.Empty));
            }

            var result = await _batchtimingsundayService.GetFilteredAsync(request, CenterId);
            return Ok(result);
        }
    }
}
