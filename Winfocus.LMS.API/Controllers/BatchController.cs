using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Winfocus.LMS.Application.DTOs;
using Winfocus.LMS.Application.DTOs.Common;
using Winfocus.LMS.Application.DTOs.Masters;
using Winfocus.LMS.Application.Interfaces;
using Winfocus.LMS.Application.Services;
using Winfocus.LMS.Domain.Entities;

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
        private readonly ISubjectService _subjectService;

        /// <summary>
        /// Initializes a new instance of the <see cref="BatchController"/> class.
        /// </summary>
        /// <param name="batchService">The batch service.</param>
        /// <param name="subjectService">The subject service.</param>
        public BatchController(IBatchService batchService, ISubjectService subjectService)
        {
            _batchService = batchService;
            _subjectService = subjectService;
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <param name="centerId">The centerId.</param>
        /// <returns>BatchDto list.</returns>
        [HttpGet("{centerId:guid?}")]
        public async Task<ActionResult<CommonResponse<BatchDto>>> GetAll(Guid centerId)
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                if (UserId != Guid.Empty)
                {
                    var centerIdFromToken = CenterId;
                    return Ok(await _batchService.GetAllAsync(centerIdFromToken));
                }
            }

            return Ok(await _batchService.GetAllAsync(centerId));
        }

           // => Ok(await _batchService.GetAllAsync());

        /// <summary>
        /// Creates the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>BatchDto.</returns>
        [Authorize(Roles = "Admin,SuperAdmin,CenterAdmin")]
        [HttpPost]
        public async Task<ActionResult<CommonResponse<BatchDto>>> Create(
            BatchRequest request)
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
            var created = await _batchService.CreateAsync(updatedRequest);
            return Ok(created);
        }

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="centerid">The centerId.</param>
        /// <returns>BatchDto by id.</returns>
        [HttpGet("{id:guid}/center/{centerid:guid?}")]
        public async Task<ActionResult<CommonResponse<BatchDto>>> Get(Guid id, Guid centerid)
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                if (UserId != Guid.Empty)
                {
                    return Ok(await _batchService.GetByIdCenterIdAsync(id, CenterId));
                }
            }

            var result = await _batchService.GetByIdCenterIdAsync(id, centerid);
            return Ok(result);
        }

           //=> Ok(await _batchService.GetByIdAsync(id));

        /// <summary>
        /// Updates the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="request">The request.</param>
        /// <returns>result.</returns>
        [Authorize(Roles = "Admin,SuperAdmin,CenterAdmin")]
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<CommonResponse<BatchDto>>> Update(
            Guid id,
            BatchRequest request)
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
            var updated = await _batchService.UpdateAsync(id, updatedRequest);
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
           var result = await _batchService.DeleteAsync(id, CenterId);
           return Ok(result);
        }

        /// <summary>
        /// Retrieves filtered batches with pagination.
        /// </summary>
        /// <param name="request">The paged request.</param>
        /// <returns>Paginated list of batches.</returns>
        [Authorize(Roles = "Admin,SuperAdmin,CenterAdmin")]
        [HttpGet("filter")]
        public async Task<ActionResult<CommonResponse<PagedResult<BatchDto>>>> GetFiltered(
            [FromQuery] PagedRequest request)
        {
            var result = await _batchService.GetFilteredAsync(request, CenterId);
            return Ok(result);
        }
    }
}
