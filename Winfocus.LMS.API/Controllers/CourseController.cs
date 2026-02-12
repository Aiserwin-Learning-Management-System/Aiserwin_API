namespace Winfocus.LMS.API.Controllers
{
    using Asp.Versioning;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Winfocus.LMS.Application.DTOs;
    using Winfocus.LMS.Application.DTOs.Masters;
    using Winfocus.LMS.Application.Interfaces;

    /// <summary>
    /// CourseController.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CourseController : ControllerBase
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
        /// Gets all.
        /// </summary>
        /// <returns>CourseDto list.</returns>
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<CourseDto>>> GetAll()
            => Ok(await _service.GetAllAsync());

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>CourseDto list.</returns>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<CourseDto>> Get(Guid id)
            => Ok(await _service.GetByIdAsync(id));

        /// <summary>
        /// Gets the by stream.
        /// </summary>
        /// <param name="streamId">The stream identifier.</param>
        /// <returns>CourseDto list.</returns>
        [HttpGet("stream/{streamId:guid}")]
        public async Task<ActionResult<IReadOnlyList<CourseDto>>> GetByStream(Guid streamId)
            => Ok(await _service.GetByStreamAsync(streamId));

        /// <summary>
        /// Gets the by subject.
        /// </summary>
        /// <param name="subjectId">The subject identifier.</param>
        /// <returns>CourseDto list.</returns>
        [HttpGet("subject/{subjectId:guid}")]
        public async Task<ActionResult<IReadOnlyList<CourseDto>>> GetBySubject(Guid subjectId)
            => Ok(await _service.GetBySubjectAsync(subjectId));

        /// <summary>
        /// Creates the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>CourseDto.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost]
        public async Task<ActionResult<CourseDto>> Create(CourseRequest request)
            => Ok(await _service.CreateAsync(request));

        /// <summary>
        /// Updates the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="request">The request.</param>
        /// <returns>result.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, CourseRequest request)
        {
            await _service.UpdateAsync(id, request);
            return NoContent();
        }

        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>result.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
