using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Winfocus.LMS.Application.DTOs;
using Winfocus.LMS.Application.DTOs.Common;
using Winfocus.LMS.Application.DTOs.Masters;
using Winfocus.LMS.Application.Interfaces;
using Winfocus.LMS.Application.Services;

namespace Winfocus.LMS.API.Controllers
{
    /// <summary>
    /// Handles authentication endpoints.
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ExamChapterController : BaseController
    {
        private readonly IExamChapterService _examchapterService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExamChapterControllerExamChapterController"/> class.
        /// </summary>
        /// <param name="examchapterService">The examchapter service.</param>
        public ExamChapterController(IExamChapterService examchapterService)
        {
            _examchapterService = examchapterService;
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns>ExamChapterDto list.</returns>
        [HttpGet]
        public async Task<ActionResult<CommonResponse<ExamChapterDto>>> GetAll()
            => Ok(await _examchapterService.GetAllAsync());

        /// <summary>
        /// Creates the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>ExamChapterDto.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost]
        public async Task<ActionResult<CommonResponse<ExamChapterDto>>> Create(
            ExamChapterRequestDto request)
        {
            var updatedRequest = request with
            {
                userid = UserId
            };
            var created = await _examchapterService.CreateAsync(updatedRequest);
            return Ok(created);
        }

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="unitid">The unitid.</param>
        /// <returns>ExamChapterDto by id.</returns>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<CommonResponse<ExamChapterDto>>> Get(Guid id, Guid unitid)
        {
            var result = await _examchapterService.GetByIdAsync(id, unitid);
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
        public async Task<ActionResult<CommonResponse<ExamChapterDto>>> Update(
            Guid id,
            ExamChapterRequestDto request)
        {
            var updatedRequest = request with
            {
                userid = UserId
            };
            var updated = await _examchapterService.UpdateAsync(id, updatedRequest);
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
        => Ok(await _examchapterService.DeleteAsync(id));

        /// <summary>
        /// Retrieves filtered batches for monday to friday with pagination.
        /// </summary>
        /// <param name="request">The paged request.</param>
        /// <returns>Paginated list of .</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet("filter")]
        public async Task<ActionResult<CommonResponse<PagedResult<ExamChapterDto>>>> GetFiltered(
            [FromQuery] PagedRequest request)
        {
            var result = await _examchapterService.GetFilteredAsync(request);
            return Ok(result);
        }

        /// <summary>
        /// Gets examchapter by unit identifier.
        /// </summary>
        /// <param name="unitId">The unit identifier.</param>
        /// <returns>ExamChapterDto list.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet("by-unit/{unitId:guid}")]
        public async Task<ActionResult<CommonResponse<List<ExamChapterDto>>>> GetByUnit(
            Guid unitId)
            => Ok(await _examchapterService.GetByUnitIdAsync(unitId));
    }
}
