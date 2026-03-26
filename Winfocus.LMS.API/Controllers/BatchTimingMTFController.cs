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
        private readonly ISubjectService _subjectService;

        /// <summary>
        /// Initializes a new instance of the <see cref="BatchTimingMTFController"/> class.
        /// </summary>
        /// <param name="batchtimingmtfService">The batchtimingmtf service.</param>
        /// <param name="subjectService">The subject service.</param>
        public BatchTimingMTFController(IBatchTimingMTFService batchtimingmtfService, ISubjectService subjectService)
        {
            _batchtimingmtfService = batchtimingmtfService;
            _subjectService = subjectService;
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <param name="centerId">The centerId.</param>
        /// <returns>BatchTimingMTFDto list.</returns>
        [HttpGet("{centerId:guid?}")]
        public async Task<ActionResult<CommonResponse<BatchTimingMTFDto>>> GetAll(Guid centerId)
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                if (User.IsInRole("SuperAdmin"))
                {
                    return Ok(await _batchtimingmtfService.GetAllAsync(Guid.Empty));
                }

                if (UserId != Guid.Empty)
                {
                    return Ok(await _batchtimingmtfService.GetAllAsync(CenterId));
                }
            }

            return Ok(await _batchtimingmtfService.GetAllAsync(centerId));
        }

           // => Ok(await _batchtimingmtfService.GetAllAsync());

        /// <summary>
        /// Creates the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>BatchTimingMTFDto.</returns>
        [Authorize(Roles = "Admin,SuperAdmin,CenterAdmin")]
        [HttpPost]
        public async Task<ActionResult<CommonResponse<BatchTimingMTFDto>>> Create(
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
            var created = await _batchtimingmtfService.CreateAsync(updatedRequest);
            return Ok(created);
        }

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="centerid">The centerId.</param>
        /// <returns>BatchTimingMTFDto by id.</returns>
        [HttpGet("{id:guid}/center/{centerid:guid?}")]
        public async Task<ActionResult<CommonResponse<BatchTimingMTFDto>>> Get(Guid id, Guid centerid)
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                if (UserId != Guid.Empty)
                {
                    return Ok(await _batchtimingmtfService.GetByIdCenterIdAsync(id, CenterId));
                }

                if (User.IsInRole("SuperAdmin"))
                {
                    return Ok(await _batchtimingmtfService.GetByIdAsync(id));
                }
            }

            return Ok(await _batchtimingmtfService.GetByIdCenterIdAsync(id, centerid));
        }

        /// <summary>
        /// Updates the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="request">The request.</param>
        /// <returns>result.</returns>
        [Authorize(Roles = "Admin,SuperAdmin,CenterAdmin")]
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<CommonResponse<BatchTimingMTFDto>>> Update(
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
            var updated = await _batchtimingmtfService.UpdateAsync(id, updatedRequest);
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
                return Ok(await _batchtimingmtfService.DeleteAsync(id, Guid.Empty));
            }

            var result = await _batchtimingmtfService.DeleteAsync(id, CenterId);
            return Ok(result);
        }

       // => Ok(await _batchtimingmtfService.DeleteAsync(id, CenterId));

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
        [Authorize(Roles = "Admin,SuperAdmin,CenterAdmin")]
        [HttpGet("filter")]
        public async Task<ActionResult<CommonResponse<PagedResult<BatchTimingMTFDto>>>> GetFiltered(
            [FromQuery] PagedRequest request)
        {
            var isSuperAdmin = User.IsInRole("SuperAdmin");
            if (isSuperAdmin)
            {
                return Ok(await _batchtimingmtfService.GetFilteredAsync(request, Guid.Empty));
            }

            var result = await _batchtimingmtfService.GetFilteredAsync(request, CenterId);
            return Ok(result);
        }
    }
}
