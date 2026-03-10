namespace Winfocus.LMS.API.Controllers
{
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

    /// <summary>
    /// SubjectController.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class SubjectController : BaseController
    {
        private readonly ISubjectService _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="SubjectController"/> class.
        /// </summary>
        /// <param name="service">The service.</param>
        public SubjectController(ISubjectService service)
        {
            _service = service;
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <param name="centerId">The centerId.</param>
        /// <returns>SubjectDto list.</returns>
        [HttpGet]
        public async Task<ActionResult<CommonResponse<SubjectDto>>> GetAll(Guid centerId)
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

        //  => Ok(await _service.GetAllAsync());

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="centerId">The centerId.</param>
        /// <returns>SubjectDto.</returns>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<CommonResponse<SubjectDto>>> Get(Guid id, Guid centerId)
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
        /// Gets the subjects by courses.
        /// </summary>
        /// <param name="courseIds">The course ids.</param>
        /// <returns>SubjectDto list.</returns>
        [HttpPost("courses/subjects")]
        public async Task<ActionResult<CommonResponse<SubjectDto>>> GetSubjectsByCourses([FromBody] List<Guid> courseIds)
          => Ok(await _service.GetByCourseIdsAsync(courseIds));

        /// <summary>
        /// Gets the by stream.
        /// </summary>
        /// <param name="streamId">The stream identifier.</param>
        /// <returns>SubjectDto list.</returns>
        [HttpGet("stream/{streamId:guid}")]
        public async Task<ActionResult<CommonResponse<SubjectDto>>> GetByStream(Guid streamId)
           => Ok(await _service.GetByStreamAsync(streamId));

        /// <summary>
        /// Creates the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>SubjectDto.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost]
        public async Task<ActionResult<CommonResponse<SubjectDto>>> Create(SubjectRequest request)
        {
            var updatedRequest = request with { userid = UserId };
            var created = await _service.CreateAsync(request);
            return Ok(created);
        }

        /// <summary>
        /// Updates the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="request">The request.</param>
        /// <returns>result.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<CommonResponse<SubjectDto>>> Update(Guid id, SubjectRequest request)
        {
            var updatedRequest = request with { userid = UserId };
            var result = await _service.UpdateAsync(id, updatedRequest);
            return Ok(result);
        }

        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>result.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<CommonResponse<bool>>> Delete(Guid id)
              => Ok(await _service.DeleteAsync(id, CenterId));

        /// <summary>
        /// Retrieves filtered subjects with pagination.
        /// </summary>
        /// <param name="request">The paged request.</param>
        /// <returns>Paginated list of subjects.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet("filter")]
        public async Task<ActionResult<CommonResponse<PagedResult<SubjectDto>>>> GetFiltered(
            [FromQuery] PagedRequest request)
        {
            var result = await _service.GetFilteredAsync(request);
            return Ok(result);
        }
    }
}
