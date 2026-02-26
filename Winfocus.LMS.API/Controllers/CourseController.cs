namespace Winfocus.LMS.API.Controllers
{
    using Asp.Versioning;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Winfocus.LMS.Application.DTOs;
    using Winfocus.LMS.Application.DTOs.Common;
    using Winfocus.LMS.Application.DTOs.Masters;
    using Winfocus.LMS.Application.Interfaces;

    /// <summary>
    /// Handles course endpoints.
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CourseController : BaseController
    {
        private readonly ICourseService _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="CourseController"/> class.
        /// </summary>
        /// <param name="service">The service.</param>
        public CourseController(ICourseService service)
        {
            _service = service;
        }

        /// <summary>
        /// Gets all courses.
        /// </summary>
        /// <returns>CourseDto list.</returns>
        [HttpGet]
        public async Task<ActionResult<CommonResponse<List<CourseDto>>>> GetAll()
            => Ok(await _service.GetAllAsync());

        /// <summary>
        /// Gets a course by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>CourseDto.</returns>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<CommonResponse<CourseDto>>> Get(Guid id)
            => Ok(await _service.GetByIdAsync(id));

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
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost]
        public async Task<ActionResult<CommonResponse<CourseDto>>> Create(
            CourseRequest request)
        {
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
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<CommonResponse<CourseDto>>> Update(
            Guid id,
            CourseRequest request)
        {
            var updatedRequest = request with { userId = UserId };
            var result = await _service.UpdateAsync(id, updatedRequest);
            return Ok(result);
        }

        /// <summary>
        /// Deletes a course.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>bool.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<CommonResponse<bool>>> Delete(Guid id)
            => Ok(await _service.DeleteAsync(id));

        /// <summary>
        /// Retrieves filtered courses with pagination.
        /// </summary>
        /// <param name="request">The paged request.</param>
        /// <returns>Paginated list of courses.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet("filter")]
        public async Task<ActionResult<CommonResponse<PagedResult<CourseDto>>>> GetFiltered(
            [FromQuery] PagedRequest request)
        {
            var result = await _service.GetFilteredAsync(request);
            return Ok(result);
        }
    }
}
