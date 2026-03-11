namespace Winfocus.LMS.API.Controllers
{
    using Asp.Versioning;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Winfocus.LMS.Application.DTOs;
    using Winfocus.LMS.Application.DTOs.Common;
    using Winfocus.LMS.Application.DTOs.Masters;
    using Winfocus.LMS.Application.Interfaces;
    using Winfocus.LMS.Application.Services;
    using Winfocus.LMS.Domain.Entities;

    /// <summary>
    /// Handles stream endpoints.
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class StreamController : BaseController
    {
        private readonly IStreamService _streamService;
        private readonly IGradeService _gradeService;

        /// <summary>
        /// Initializes a new instance of the <see cref="StreamController"/> class.
        /// </summary>
        /// <param name="streamService">The stream service.</param>
        /// <param name="gradeService">The grade service.</param>
        public StreamController(IStreamService streamService, IGradeService gradeService)
        {
            _streamService = streamService;
            _gradeService = gradeService;
        }

        /// <summary>
        /// Gets all streams.
        /// </summary>
        /// <param name="centerId">The centerId.</param>
        /// <returns>StreamDto list.</returns>
        [HttpGet("{centerId:guid?}")]
        public async Task<ActionResult<CommonResponse<List<StreamDto>>>> GetAll(Guid centerId)
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                var userId = UserId;

                if (userId != Guid.Empty)
                {
                    var centerIdFromToken = CenterId;
                    return Ok(await _streamService.GetAllAsync(centerIdFromToken));
                }
            }

            return Ok(await _streamService.GetAllAsync(centerId));
        }

            //=> Ok(await _streamService.GetAllAsync());

        /// <summary>
        /// Creates a new stream.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>StreamDto.</returns>
        [Authorize(Roles = "Admin,SuperAdmin,CenterAdmin")]
        [HttpPost]
        public async Task<ActionResult<CommonResponse<StreamDto>>> Create(
            StreamRequest request)
        {
            var grade = await _gradeService.GetByIdAsync(request.gradeid);
            if (grade?.Data == null)
            {
                return NotFound("Grade not found.");
            }

            if (CenterId != grade.Data.CenterId)
            {
                return StatusCode(403, "You are not allowed to create data for this center.");
            }

            var updatedRequest = request with { userId = UserId };
            var created = await _streamService.CreateAsync(updatedRequest);
            return Ok(created);
        }

        /// <summary>
        /// Gets a stream by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="centerid">The centerId.</param>
        /// <returns>StreamDto.</returns>
        [HttpGet("{id:guid}/center/{centerid:guid?}")]
        public async Task<ActionResult<CommonResponse<StreamDto>>> Get(Guid id, Guid centerid)
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                var userId = UserId;

                if (userId != Guid.Empty)
                {
                    return Ok(await _streamService.GetByIdCenterIdAsync(id, CenterId));
                }
            }

            return Ok(await _streamService.GetByIdCenterIdAsync(id, centerid));
        }

        /// <summary>
        /// Updates a stream.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="request">The request.</param>
        /// <returns>StreamDto.</returns>
        [Authorize(Roles = "Admin,SuperAdmin,CenterAdmin")]
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<CommonResponse<StreamDto>>> Update(
            Guid id,
            StreamRequest request)
        {
            var grade = await _gradeService.GetByIdAsync(request.gradeid);
            if (grade?.Data == null)
            {
                return NotFound("Grade not found.");
            }

            if (CenterId != grade.Data.CenterId)
            {
                return StatusCode(403, "You are not allowed to create data for this center.");
            }

            var updatedRequest = request with { userId = UserId };
            var updated = await _streamService.UpdateAsync(id, updatedRequest);
            return Ok(updated);
        }

        /// <summary>
        /// Gets streams by grade identifier.
        /// </summary>
        /// <param name="gradeid">The grade identifier.</param>
        /// <returns>StreamDto list.</returns>
        [HttpGet("by-grade/{gradeid:guid}")]
        public async Task<ActionResult<CommonResponse<List<StreamDto>>>> GetByGradeId(
            Guid gradeid)
        {
            var result = await _streamService.GetByGradeIdAsync(gradeid);
            return Ok(result);
        }

        /// <summary>
        /// Deletes a stream.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>bool.</returns>
        [Authorize(Roles = "Admin,SuperAdmin,CenterAdmin")]
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<CommonResponse<bool>>> Delete(Guid id)
        {
            var result = await _streamService.DeleteAsync(id, CenterId);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves filtered streams with pagination.
        /// </summary>
        /// <param name="request">The paged request.</param>
        /// <returns>Paginated list of streams.</returns>
        [Authorize(Roles = "Admin,SuperAdmin,CenterAdmin")]
        [HttpGet("filter")]
        public async Task<ActionResult<CommonResponse<PagedResult<StreamDto>>>> GetFiltered(
            [FromQuery] PagedRequest request)
        {
            var result = await _streamService.GetFilteredAsync(request, CenterId);
            return Ok(result);
        }
    }
}
