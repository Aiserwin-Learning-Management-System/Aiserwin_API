using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Winfocus.LMS.Application.DTOs;
using Winfocus.LMS.Application.DTOs.Common;
using Winfocus.LMS.Application.DTOs.Masters;
using Winfocus.LMS.Application.DTOs.Question;
using Winfocus.LMS.Application.DTOs.Stats;
using Winfocus.LMS.Application.Interfaces;
using Winfocus.LMS.Application.Services;

namespace Winfocus.LMS.API.Controllers
{
    /// <summary>
    /// Handles authentication endpoints.
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/operator/question-bank")]
    [Authorize(Roles = "Staff,Admin,SuperAdmin")]
    public class OperatorQuestionBankController : BaseController
    {
        private readonly IQuestionService _questionService;

        /// <summary>
        /// Initializes a new instance of the <see cref="OperatorQuestionBankController"/> class.
        /// </summary>
        /// <param name="questionService">The questionService.</param>
        public OperatorQuestionBankController(IQuestionService questionService)
        {
            _questionService = questionService;
        }

        /// <summary>
        /// Gets question status.
        /// </summary>
        /// <returns>QuestionStatsDto list.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet("status")]
        public async Task<ActionResult<CommonResponse<QuestionStatsDto>>> Status()
        {
            try
            {
                var stats = await _question_service_stats_wrapper();
                return Ok(CommonResponse<QuestionStatsDto>.SuccessResponse("Status fetched.", stats));
            }
            catch (Exception ex)
            {
                return BadRequest(CommonResponse<QuestionStatsDto>.FailureResponse(ex.Message));
            }
        }

        private async Task<QuestionStatsDto> _question_service_stats_wrapper()
        {
            return await _questionService.GetQuestionStatsAsync(UserId);
        }

        /// <summary>
        /// Retrieves filtered batches for monday to friday with pagination.
        /// </summary>
        /// <param name="request">The paged request.</param>
        /// <param name="subject">The paged subject.</param>
        /// <param name="chapter">The paged chapter.</param>
        /// <param name="status">The paged status.</param>
        /// <param name="questionType">The paged questionType.</param>
        /// <returns>Paginated list of .</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet("filter")]
        public async Task<ActionResult<CommonResponse<PagedResult<QuestionBankItemDto>>>> GetFiltered(
            [FromQuery] PagedRequest request,
            [FromQuery] string? subject = null,
            [FromQuery] string? chapter = null,
            [FromQuery] int? status = null,
            [FromQuery] int? questionType = null)
        {
            var result = await _questionService.GetFilteredAsync(request, subject, chapter, status, questionType);
            return Ok(result);
        }
    }
}
