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
    public class ExamUnitController : BaseController
    {
        private readonly IExamUnitService _examunitService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExamUnitController"/> class.
        /// </summary>
        /// <param name="examunitService">The examgradeService service.</param>
        public ExamUnitController(IExamUnitService examunitService)
        {
            _examunitService = examunitService;
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns>ExamUnitDto list.</returns>
        [HttpGet]
        public async Task<ActionResult<CommonResponse<ExamUnitDto>>> GetAll()
            => Ok(await _examunitService.GetAllAsync());

        /// <summary>
        /// Creates the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>ExamUnitDto.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost]
        public async Task<ActionResult<CommonResponse<ExamUnitDto>>> Create(
            ExamUnitRequestDto request)
        {
            var updatedRequest = request with
            {
                userid = UserId
            };
            var created = await _examunitService.CreateAsync(updatedRequest);
            return Ok(created);
        }

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="subjectID">The syllabusID.</param>
        /// <returns>ExamUnitDto by id.</returns>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<CommonResponse<ExamUnitDto>>> Get(Guid id, Guid subjectID)
        {
            var result = await _examunitService.GetByIdAsync(id, subjectID);
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
        public async Task<ActionResult<CommonResponse<ExamUnitDto>>> Update(
            Guid id,
            ExamUnitRequestDto request)
        {
            var updatedRequest = request with
            {
                userid = UserId
            };
            var updated = await _examunitService.UpdateAsync(id, updatedRequest);
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
        => Ok(await _examunitService.DeleteAsync(id));

        /// <summary>
        /// Retrieves filtered batches for monday to friday with pagination.
        /// </summary>
        /// <param name="request">The paged request.</param>
        /// <returns>Paginated list of .</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet("filter")]
        public async Task<ActionResult<CommonResponse<PagedResult<ExamUnitDto>>>> GetFiltered(
            [FromQuery] PagedRequest request)
        {
            var result = await _examunitService.GetFilteredAsync(request);
            return Ok(result);
        }
    }
}
