using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Winfocus.LMS.Application.DTOs;
using Winfocus.LMS.Application.DTOs.Common;
using Winfocus.LMS.Application.DTOs.Exam;
using Winfocus.LMS.Application.DTOs.Masters;
using Winfocus.LMS.Application.Interfaces;
using Winfocus.LMS.Application.Services;

namespace Winfocus.LMS.API.Controllers
{
    /// <summary>
    /// Handles authentication endpoints.
    /// </summary>
    [Authorize(Roles = "Admin,SuperAdmin")]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ExamController : BaseController
    {
        private readonly IExamService _examService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExamController"/> class.
        /// </summary>
        /// <param name="examService">The exam service.</param>
        public ExamController(IExamService examService)
        {
            _examService = examService;
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns>ExamDto list.</returns>
        [HttpGet("{centerId:guid?}")]
        public async Task<ActionResult<CommonResponse<ExamDto>>> GetAll()
        {
            return Ok(await _examService.GetAllAsync());
        }

        /// <summary>
        /// Creates the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>ExamDto.</returns>
        [HttpPost]
        public async Task<ActionResult<CommonResponse<ExamDto>>> Create(
            ExamRequest request)
        {
            var created = await _examService.CreateAsync(request);
            return Ok(created);
        }

        /// <summary>
        /// Updates the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="request">The request.</param>
        /// <returns>result.</returns>
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<CommonResponse<ExamDto>>> Update(
            Guid id,
            ExamRequest request)
        {
            var updated = await _examService.UpdateAsync(id, request);
            return Ok(updated);
        }

        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>result.</returns>
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<CommonResponse<bool>>> Delete(Guid id)
        {
            var result = await _examService.DeleteAsync(id);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves filtered batches with pagination.
        /// </summary>
        /// <param name="request">The paged request.</param>
        /// <returns>Paginated list of batches.</returns>
        [HttpGet("filter")]
        public async Task<ActionResult<CommonResponse<PagedResult<ExamDto>>>> GetFiltered(
            [FromQuery] PagedRequest request)
        {
            var result = await _examService.GetFilteredAsync(request);
            return Ok(result);
        }
    }
}
