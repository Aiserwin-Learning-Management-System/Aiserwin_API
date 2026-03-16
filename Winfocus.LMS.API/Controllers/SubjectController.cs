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
        private readonly ICourseService _courseService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SubjectController"/> class.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="courseService">The course Service.</param>
        public SubjectController(ISubjectService service, ICourseService courseService)
        {
            _service = service;
            _courseService = courseService;
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <param name="centerId">The centerId.</param>
        /// <returns>SubjectDto list.</returns>
        [HttpGet("{centerId:guid?}")]
        public async Task<ActionResult<CommonResponse<SubjectDto>>> GetAll(Guid centerId)
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                if (User.IsInRole("SuperAdmin"))
                {
                    return Ok(await _service.GetAllAsync(Guid.Empty));
                }

                if (UserId != Guid.Empty)
                {
                    return Ok(await _service.GetAllAsync(CenterId));
                }
            }

            return Ok(await _service.GetAllAsync(centerId));
        }

        //  => Ok(await _service.GetAllAsync());

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="centerid">The centerId.</param>
        /// <returns>SubjectDto.</returns>
        [HttpGet("{id:guid}/center/{centerid:guid?}")]
        public async Task<ActionResult<CommonResponse<SubjectDto>>> Get(Guid id, Guid centerid)
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                if (User.IsInRole("SuperAdmin"))
                {
                    return Ok(await _service.GetByIdAsync(id));
                }

                if (UserId != Guid.Empty)
                {
                    return Ok(await _service.GetByIdCenterIdAsync(id, CenterId));
                }
            }

            return Ok(await _service.GetByIdCenterIdAsync(id, centerid));
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
        [Authorize(Roles = "Admin,SuperAdmin,CenterAdmin")]
        [HttpPost]
        public async Task<ActionResult<CommonResponse<SubjectDto>>> Create(SubjectRequest request)
        {
            var course = await _courseService.GetByIdAsync(request.courseid);
            if (course?.Data == null)
            {
                return NotFound("Course not found.");
            }

            var isSuperAdmin = User.IsInRole("SuperAdmin");
            if (!isSuperAdmin && CenterId != course.Data.CenterId)
            {
                return StatusCode(403, "You are not allowed to create data for this center.");
            }

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
        [Authorize(Roles = "Admin,SuperAdmin,CenterAdmin")]
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<CommonResponse<SubjectDto>>> Update(Guid id, SubjectRequest request)
        {
            var course = await _courseService.GetByIdAsync(request.courseid);
            if (course?.Data == null)
            {
                return NotFound("Course not found.");
            }
            var isSuperAdmin = User.IsInRole("SuperAdmin");
            if (!isSuperAdmin && CenterId != course.Data.CenterId)
            {
                return StatusCode(403, "You are not allowed to create data for this center.");
            }

            var updatedRequest = request with { userid = UserId };
            var result = await _service.UpdateAsync(id, updatedRequest);
            return Ok(result);
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
                return await _service.DeleteAsync(id, Guid.Empty);
            }

            var result = await _service.DeleteAsync(id, CenterId);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves filtered subjects with pagination.
        /// </summary>
        /// <param name="request">The paged request.</param>
        /// <returns>Paginated list of subjects.</returns>
        [Authorize(Roles = "Admin,SuperAdmin,CenterAdmin")]
        [HttpGet("filter")]
        public async Task<ActionResult<CommonResponse<PagedResult<SubjectDto>>>> GetFiltered(
            [FromQuery] PagedRequest request)
        {
            var isSuperAdmin = User.IsInRole("SuperAdmin");
            if (isSuperAdmin)
            {
                return Ok(await _service.GetFilteredAsync(request, Guid.Empty));
            }

            var result = await _service.GetFilteredAsync(request, CenterId);
            return Ok(result);
        }
    }
}
