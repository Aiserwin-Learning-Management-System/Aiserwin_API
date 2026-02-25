namespace Winfocus.LMS.API.Controllers
{
    using Asp.Versioning;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http.HttpResults;
    using Microsoft.AspNetCore.Mvc;
    using Winfocus.LMS.Application.DTOs;
    using Winfocus.LMS.Application.DTOs.Masters;
    using Winfocus.LMS.Application.Interfaces;
    using Winfocus.LMS.Application.Services;
    using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

    /// <summary>
    /// SubjectController.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class SubjectController : ControllerBase
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
        /// <returns>SubjectDto list.</returns>
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<SubjectDto>>> GetAll()
            => Ok(await _service.GetAllAsync());

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>SubjectDto.</returns>
        [HttpGet("{id:guid}")]
        public async Task<CommonResponse<SubjectDto>> Get(Guid id)
        {
            var result = await _service.GetByIdAsync(id);
            return result;
        }

                //        => Ok(await _service.GetByIdAsync(id));

        /// <summary>
        /// Gets the subjects by courses.
        /// </summary>
        /// <param name="courseIds">The course ids.</param>
        /// <returns>SubjectDto list.</returns>
        [HttpPost("courses/subjects")]
        public async Task<ActionResult<IReadOnlyList<SubjectDto>>> GetSubjectsByCourses([FromBody] List<Guid> courseIds)
        {
            var subjects = await _service.GetByCourseIdsAsync(courseIds);
            return Ok(subjects);
        }

        /// <summary>
        /// Gets the by stream.
        /// </summary>
        /// <param name="streamId">The stream identifier.</param>
        /// <returns>SubjectDto list.</returns>
        [HttpGet("stream/{streamId:guid}")]
        public async Task<IReadOnlyList<SubjectDto>> GetByStream(Guid streamId)
        {
            return await _service.GetByStreamAsync(streamId);
        }

           // => Ok(await _service.GetByStreamAsync(streamId));

        /// <summary>
        /// Creates the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>SubjectDto.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost]
        public async Task<CommonResponse<SubjectDto>> Create(SubjectRequest request)
        {
            var created = await _service.CreateAsync(request);
            if (created == null)
            {
                return CommonResponse<SubjectDto>.FailureResponse("Failed to create Subject.");
            }
            else
            {
                return CommonResponse<SubjectDto>.SuccessResponse("Subject created successfully.", created);
            }
        }

         //   => Ok(await _service.CreateAsync(request));

        /// <summary>
        /// Updates the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="request">The request.</param>
        /// <returns>result.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPut("{id:guid}")]
        public async Task<CommonResponse<SubjectDto>> Update(Guid id, SubjectRequest request)
        {
            var updated = await _service.UpdateAsync(id, request);
            if (updated == null)
            {
                return CommonResponse<SubjectDto>.FailureResponse("Failed to update Subject.");
            }
            else
            {
                return CommonResponse<SubjectDto>.SuccessResponse("Batchtiming for subject updated successfully.", updated);
            }
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
            bool result = await _service.DeleteAsync(id);
            if (result)
            {
                return CommonResponse<bool>.SuccessResponse("Subject deleted successfully.", true);
            }
            else
            {
                return CommonResponse<bool>.FailureResponse("Failed to delete Subject.");
            }
        }
    }
}
