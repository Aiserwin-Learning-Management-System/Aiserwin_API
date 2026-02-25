namespace Winfocus.LMS.API.Controllers
{
    using Asp.Versioning;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Winfocus.LMS.Application.DTOs;
    using Winfocus.LMS.Application.DTOs.Common;
    using Winfocus.LMS.Application.DTOs.Masters;
    using Winfocus.LMS.Application.DTOs.Students;
    using Winfocus.LMS.Application.Interfaces;
    using Winfocus.LMS.Application.Services;
    using Winfocus.LMS.Domain.Enums;

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
        public async Task<CommonResponse<List<CourseDto>>> GetAll()
        {
            return await _service.GetAllAsync();
        }

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>CourseDto list.</returns>
        [HttpGet("{id:guid}")]
        public async Task<CommonResponse<CourseDto>> Get(Guid id)
        {
            return await _service.GetByIdAsync(id);
        }

           // => Ok(await _service.GetByIdAsync(id));

        /// <summary>
        /// Gets the by stream.
        /// </summary>
        /// <param name="streamId">The stream identifier.</param>
        /// <returns>CourseDto list.</returns>
        [HttpGet("stream/{streamId:guid}")]
        public async Task<CommonResponse<List<CourseDto>>> GetByStream(Guid streamId)
        {
            return await _service.GetByStreamAsync(streamId);
        }

        //  => Ok(await _service.GetByStreamAsync(streamId));

        /// <summary>
        /// Gets the by subject.
        /// </summary>
        /// <param name="subjectId">The subject identifier.</param>
        /// <returns>CourseDto list.</returns>
        [HttpGet("subject/{subjectId:guid}")]
        public async Task<CommonResponse<List<CourseDto>>> GetBySubject(Guid subjectId)
        {
            return await _service.GetBySubjectAsync(subjectId);
        }

            //=> Ok(await _service.GetBySubjectAsync(subjectId));

        /// <summary>
        /// Creates the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>CourseDto.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost]
        public async Task<CommonResponse<CourseDto>> Create(CourseRequest request)
        {
           return await _service.CreateAsync(request);
        }

           // => Ok(await _service.CreateAsync(request));

        /// <summary>
        /// Updates the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="request">The request.</param>
        /// <returns>result.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPut("{id:guid}")]
        public async Task<CommonResponse<CourseDto>> Update(Guid id, CourseRequest request)
        {
           return await _service.UpdateAsync(id, request);
        }

        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>result.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpDelete("{id:guid}")]
        public async Task<CommonResponse<bool>> Delete(Guid id)
        {
           return await _service.DeleteAsync(id);
        }

        /// <summary>
        /// Retrieves filtered courses with pagination.
        /// </summary>
        /// <param name="centreId">Filter by centre identifier.</param>
        /// <param name="syllabusid">Filter by syllabus identifier.</param>
        /// <param name="gradeId">Filter by grade identifier.</param>
        /// <param name="streamId">Filter by stream identifier.</param>
        /// <param name="subjectsId">Filter by subject identifier.</param>
        /// <param name="startDate">Filter courses created after this date.</param>
        /// <param name="endDate">Filter courses created before this date.</param>
        /// <param name="active">Filter by active status.</param>
        /// <param name="searchText">Search keyword.</param>
        /// <param name="limit">Number of records to return.</param>
        /// <param name="offset">Number of records to skip.</param>
        /// <param name="sortOrder">Sorting order (asc/desc).</param>
        /// <returns>Paginated list of courses.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet("filter")]
        public async Task<ActionResult<CommonResponse<PagedResult<CourseDto>>>> GetFiltered(
    Guid? centreId,
    Guid? syllabusid,
    Guid? gradeId,
    Guid? streamId,
    Guid? subjectsId,
    DateTime? startDate,
    DateTime? endDate,
    bool active,
    string? searchText,
    int limit = 20,
    int offset = 0,
    string sortOrder = "asc")
        {
            var result = await _service.GetFilteredAsync(
                centreId,
                syllabusid,
                gradeId,
                streamId,
                subjectsId,
                startDate,
                endDate,
                active,
                searchText,
                limit,
                offset,
                sortOrder);

            return Ok(result);
        }
    }
}
