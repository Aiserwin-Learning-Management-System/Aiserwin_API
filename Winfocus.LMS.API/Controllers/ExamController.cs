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

        /// <summary>
        /// Gets questions for the specified exam.
        /// </summary>
        /// <param name="examId">The exam identifier.</param>
        /// <returns>List of exam-question mappings wrapped in CommonResponse.</returns>
        [HttpGet("{examId:guid}/questions")]
        public async Task<ActionResult<CommonResponse<List<ExamQuestionDto>>>> GetQuestionsForExam(Guid examId)
        {
            var result = await _examService.GetQuestionsForExamAsync(examId);
            return Ok(result);
        }

        /// <summary>
        /// Creates an exam question mapping.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>.</returns>
        [HttpPost("questions")]
        public async Task<ActionResult<CommonResponse<ExamQuestionDto>>> CreateExamQuestion([FromBody] ExamQuestionRequest request)
        {
            var result = await _examService.CreateExamQuestionAsync(request);
            return Ok(result);
        }

        /// <summary>
        /// Updates an exam question mapping.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="request">The exam request.</param>
        /// <returns>.</returns>
        [HttpPut("questions/{id:guid}")]
        public async Task<ActionResult<CommonResponse<ExamQuestionDto>>> UpdateExamQuestion(Guid id, [FromBody] ExamQuestionRequest request)
        {
            var result = await _examService.UpdateExamQuestionAsync(id, request);
            return Ok(result);
        }
    }
}
