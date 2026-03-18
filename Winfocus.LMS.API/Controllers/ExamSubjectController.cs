using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Winfocus.LMS.Application.DTOs;
using Winfocus.LMS.Application.DTOs.Common;
using Winfocus.LMS.Application.DTOs.Masters;
using Winfocus.LMS.Application.Interfaces;

namespace Winfocus.LMS.API.Controllers
{
    /// <summary>
    /// Handles authentication endpoints.
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ExamSubjectController : BaseController
    {
        private readonly IExamSubjectService _examsubjectService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExamSubjectController"/> class.
        /// </summary>
        /// <param name="examsubjectService">The examgradeService service.</param>
        public ExamSubjectController(IExamSubjectService examsubjectService)
        {
            _examsubjectService = examsubjectService;
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns>ExamSubjectDto list.</returns>
        [HttpGet]
        public async Task<ActionResult<CommonResponse<ExamSubjectDto>>> GetAll()
            => Ok(await _examsubjectService.GetAllAsync());

        /// <summary>
        /// Creates the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>ExamSubjectDto.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost]
        public async Task<ActionResult<CommonResponse<ExamSubjectDto>>> Create(
            ExamSubjectRequestDto request)
        {
            var updatedRequest = request with
            {
                userid = UserId
            };
            var created = await _examsubjectService.CreateAsync(updatedRequest);
            return Ok(created);
        }

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="gradeID">The syllabusID.</param>
        /// <returns>ExamSubjectDto by id.</returns>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<CommonResponse<ExamSubjectDto>>> Get(Guid id, Guid gradeID)
        {
            var result = await _examsubjectService.GetByIdAsync(id, gradeID);
            return Ok(result);
        }

        /// <summary>
        /// Updates the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="request">The request.</param>
        /// <returns>result.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<CommonResponse<ExamSubjectDto>>> Update(
            Guid id,
            ExamSubjectRequestDto request)
        {
            var updatedRequest = request with
            {
                userid = UserId
            };
            var updated = await _examsubjectService.UpdateAsync(id, updatedRequest);
            return Ok(updated);
        }

        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>result.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<CommonResponse<bool>>> Delete(Guid id)
        => Ok(await _examsubjectService.DeleteAsync(id));

        /// <summary>
        /// Retrieves filtered batches for monday to friday with pagination.
        /// </summary>
        /// <param name="request">The paged request.</param>
        /// <returns>Paginated list of .</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet("filter")]
        public async Task<ActionResult<CommonResponse<PagedResult<ExamSubjectDto>>>> GetFiltered(
            [FromQuery] PagedRequest request)
        {
            var result = await _examsubjectService.GetFilteredAsync(request);
            return Ok(result);
        }
    }
}
