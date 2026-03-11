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
    /// Handles course endpoints.
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CourseController : BaseController
    {
        private readonly ICourseService _service;
        private readonly IStreamService _streamService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CourseController"/> class.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="streamService">The stream Service.</param>
        public CourseController(ICourseService service, IStreamService streamService)
        {
            _service = service;
            _streamService = streamService;
        }

        /// <summary>
        /// Gets all courses.
        /// </summary>
        /// <param name="centerId">The centerId.</param>
        /// <returns>CourseDto list.</returns>
        [HttpGet("{centerId:guid?}")]
        public async Task<ActionResult<CommonResponse<List<CourseDto>>>> GetAll(Guid centerId)
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                var userId = UserId;

                if (userId != Guid.Empty)
                {
                    var centerIdFromToken = CenterId;
                    return Ok(await _service.GetAllAsync(centerIdFromToken));
                }
            }

            return Ok(await _service.GetAllAsync(centerId));
        }
        //=> Ok(await _service.GetAllAsync());

        /// <summary>
        /// Gets a course by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="centerId">The centerId.</param>
        /// <returns>CourseDto.</returns>
        [HttpGet("{id:guid}/center/{centerid:guid?}")]
        public async Task<ActionResult<CommonResponse<CourseDto>>> Get(Guid id, Guid centerId)
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                var userId = UserId;

                if (userId != Guid.Empty)
                {
                    var centerIdFromToken = CenterId;
                    return Ok(await _service.GetByIdCenterIdAsync(id, centerIdFromToken));
                }
            }

            return Ok(await _service.GetByIdCenterIdAsync(id, centerId));
        }

           // => Ok(await _service.GetByIdAsync(id));

        /// <summary>
        /// Gets courses by stream identifier.
        /// </summary>
        /// <param name="streamId">The stream identifier.</param>
        /// <returns>CourseDto list.</returns>
        [HttpGet("stream/{streamId:guid}")]
        public async Task<ActionResult<CommonResponse<List<CourseDto>>>> GetByStream(
            Guid streamId)
            => Ok(await _service.GetByStreamAsync(streamId));

        /// <summary>
        /// Gets courses by subject identifier.
        /// </summary>
        /// <param name="subjectId">The subject identifier.</param>
        /// <returns>CourseDto list.</returns>
        [HttpGet("subject/{subjectId:guid}")]
        public async Task<ActionResult<CommonResponse<List<CourseDto>>>> GetBySubject(
            Guid subjectId)
            => Ok(await _service.GetBySubjectAsync(subjectId));

        /// <summary>
        /// Creates a new course.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>CourseDto.</returns>
        [Authorize(Roles = "Admin,SuperAdmin,CenterAdmin")]
        [HttpPost]
        public async Task<ActionResult<CommonResponse<CourseDto>>> Create(
            CourseRequest request)
        {
            var stream = await _streamService.GetByIdAsync(request.streamid);
            if (stream?.Data == null)
            {
                return NotFound("Grade not found.");
            }

            if (CenterId != stream.Data.CenterId)
            {
                return StatusCode(403, "You are not allowed to create data for this center.");
            }

            var updatedRequest = request with { userId = UserId };
            var result = await _service.CreateAsync(updatedRequest);
            return Ok(result);
        }

        /// <summary>
        /// Updates a course.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="request">The request.</param>
        /// <returns>CourseDto.</returns>
        [Authorize(Roles = "Admin,SuperAdmin,CenterAdmin")]
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<CommonResponse<CourseDto>>> Update(
            Guid id,
            CourseRequest request)
        {
            var stream = await _streamService.GetByIdAsync(request.streamid);
            if (stream?.Data == null)
            {
                return NotFound("Grade not found.");
            }

            if (CenterId != stream.Data.CenterId)
            {
                return StatusCode(403, "You are not allowed to create data for this center.");
            }
            var updatedRequest = request with { userId = UserId };
            var result = await _service.UpdateAsync(id, updatedRequest);
            return Ok(result);
        }

        /// <summary>
        /// Deletes a course.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>bool.</returns>
        [Authorize(Roles = "Admin,SuperAdmin,CenterAdmin")]
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<CommonResponse<bool>>> Delete(Guid id)
            => Ok(await _service.DeleteAsync(id, CenterId));

        /// <summary>
        /// Retrieves filtered courses with pagination.
        /// </summary>
        /// <param name="request">The paged request.</param>
        /// <returns>Paginated list of courses.</returns>
        [Authorize(Roles = "Admin,SuperAdmin,CenterAdmin")]
        [HttpGet("filter")]
        public async Task<ActionResult<CommonResponse<PagedResult<CourseDto>>>> GetFiltered(
            [FromQuery] PagedRequest request)
        {
            var result = await _service.GetFilteredAsync(request, CenterId);
            return Ok(result);
        }
    }
}
