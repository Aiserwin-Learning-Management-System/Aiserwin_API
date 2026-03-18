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
    public class ExamGradeController : BaseController
    {
        private readonly IExamGradeService _examgradeService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExamGradeController"/> class.
        /// </summary>
        /// <param name="examgradeService">The examgradeService service.</param>
        public ExamGradeController(IExamGradeService examgradeService)
        {
            _examgradeService = examgradeService;
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns>ExamGradeDto list.</returns>
        [HttpGet]
        public async Task<ActionResult<CommonResponse<ExamGradeDto>>> GetAll()
            => Ok(await _examgradeService.GetAllAsync());

        /// <summary>
        /// Creates the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>ExamGradeDto.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost]
        public async Task<ActionResult<CommonResponse<ExamGradeDto>>> Create(
            ExamGradeRequestDto request)
        {
            var updatedRequest = request with
            {
                userid = UserId
            };
            var created = await _examgradeService.CreateAsync(updatedRequest);
            return Ok(created);
        }

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="syllabusID">The syllabusID.</param>
        /// <returns>ExamGradeDto by id.</returns>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<CommonResponse<ExamGradeDto>>> Get(Guid id, Guid syllabusID)
        {
            var result = await _examgradeService.GetByIdAsync(id, syllabusID);
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
        public async Task<ActionResult<CommonResponse<ExamGradeDto>>> Update(
            Guid id,
            ExamGradeRequestDto request)
        {
            var updatedRequest = request with
            {
                userid = UserId
            };
            var updated = await _examgradeService.UpdateAsync(id, updatedRequest);
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
        => Ok(await _examgradeService.DeleteAsync(id));

        /// <summary>
        /// Retrieves filtered batches for monday to friday with pagination.
        /// </summary>
        /// <param name="request">The paged request.</param>
        /// <returns>Paginated list of doubt clearing.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet("filter")]
        public async Task<ActionResult<CommonResponse<PagedResult<ExamGradeDto>>>> GetFiltered(
            [FromQuery] PagedRequest request)
        {
            var result = await _examgradeService.GetFilteredAsync(request);
            return Ok(result);
        }
    }
}
