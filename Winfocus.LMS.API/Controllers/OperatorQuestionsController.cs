using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Winfocus.LMS.Application.DTOs;
using Winfocus.LMS.Application.DTOs.Question;
using Winfocus.LMS.Application.Interfaces;

namespace Winfocus.LMS.API.Controllers
{
    /// <summary>
    /// Provides CRUD and workflow endpoints for operator-created questions.
    /// All operations act on behalf of the authenticated operator (extracted from the JWT
    /// and available via <see cref="BaseController.UserId"/>).
    /// Exposed functionality includes creating (draft or submit), retrieving details,
    /// updating drafts/rejected items, deleting drafts, submitting for review, listing by task,
    /// and previewing formatted questions.
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/operator/questions")]
    [Authorize(Roles = "Staff,Admin,SuperAdmin")]
    public class OperatorQuestionsController : BaseController
    {
        private readonly IQuestionService _questionService;

        /// <summary>
        /// Initializes a new instance of the <see cref="OperatorQuestionsController"/> class.
        /// </summary>
        /// <param name="questionService">Service responsible for question business logic.</param>
        public OperatorQuestionsController(IQuestionService questionService)
        {
            _questionService = questionService;
        }

        /// <summary>
        /// Creates a new question for the current operator. Supports both MCQ and descriptive
        /// question types. If <c>dto.SaveAsDraft</c> is true the question is saved as Draft,
        /// otherwise it is submitted for review immediately.
        /// </summary>
        /// <param name="dto">Payload containing question details and options.</param>
        /// <returns>
        /// A <see cref="CommonResponse{Guid}"/> containing the created question Id on success.
        /// </returns>
        [HttpPost]
        public async Task<ActionResult<CommonResponse<Guid>>> Create([FromBody] CreateQuestionDto dto)
        {
            try
            {
                var id = await _questionService.CreateAsync(dto, UserId);
                return Ok(CommonResponse<Guid>.SuccessResponse("Question created successfully.", id));
            }
            catch (Exception ex)
            {
                return BadRequest(CommonResponse<Guid>.FailureResponse(ex.Message));
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="id">id.</param>
        /// <returns>
        /// A <see cref="CommonResponse{Guid}"/> containing the created question Id on success.
        /// </returns>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<CommonResponse<QuestionResponseDto>>> Get(Guid id)
        {
            try
            {
                var q = await _questionService.GetByIdAsync(id);
                return Ok(CommonResponse<QuestionResponseDto>.SuccessResponse("Fetched question.", q));
            }
            catch (Exception ex)
            {
                return NotFound(CommonResponse<QuestionResponseDto>.FailureResponse(ex.Message));
            }
        }

        /// <summary>
        /// Updates an existing question. Only questions in Draft or Rejected status are editable.
        /// For MCQ questions the request must include exactly 4 options and a valid correct label.
        /// </summary>
        /// <param name="id">Identifier of the question to update.</param>
        /// <param name="dto">Updated question payload.</param>
        /// <returns>A <see cref="CommonResponse{bool}"/> indicating success or failure.</returns>
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<CommonResponse<bool>>> Update(Guid id, [FromBody] CreateQuestionDto dto)
        {
            try
            {
                await _questionService.UpdateAsync(id, dto);
                return Ok(CommonResponse<bool>.SuccessResponse("Question updated.", true));
            }
            catch (Exception ex)
            {
                return BadRequest(CommonResponse<bool>.FailureResponse(ex.Message));
            }
        }

        /// <summary>
        /// Deletes a question. Only questions in Draft status may be deleted.
        /// </summary>
        /// <param name="id">Identifier of the draft question to delete.</param>
        /// <returns>A <see cref="CommonResponse{bool}"/> indicating operation result.</returns>
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<CommonResponse<bool>>> Delete(Guid id)
        {
            try
            {
                await _questionService.DeleteAsync(id);
                return Ok(CommonResponse<bool>.SuccessResponse("Question deleted.", true));
            }
            catch (Exception ex)
            {
                return BadRequest(CommonResponse<bool>.FailureResponse(ex.Message));
            }
        }

        /// <summary>
        /// Submits a draft question for review. Changes status from Draft &rarr; Submitted
        /// and increments the parent task's CompletedCount and sets Task status to InProgress when applicable.
        /// </summary>
        /// <param name="id">Identifier of the draft question to submit.</param>
        /// <returns>A <see cref="CommonResponse{bool}"/> indicating success or failure.</returns>
        [HttpPost("{id:guid}/submit")]
        public async Task<ActionResult<CommonResponse<bool>>> Submit(Guid id)
        {
            try
            {
                await _questionService.SubmitAsync(id);
                return Ok(CommonResponse<bool>.SuccessResponse("Question submitted for review.", true));
            }
            catch (Exception ex)
            {
                return BadRequest(CommonResponse<bool>.FailureResponse(ex.Message));
            }
        }

        /// <summary>
        /// Returns a paginated list of questions belonging to the specified task.
        /// Options and review history are not included in the list DTO — use Get(id) for full details.
        /// </summary>
        /// <param name="taskId">Identifier of the task whose questions are requested.</param>
        /// <param name="page">Page number (1-based).</param>
        /// <param name="pageSize">Number of items per page.</param>
        /// <returns>A <see cref="CommonResponse{List{QuestionListDto}}"/> containing the page of questions.</returns>
        [HttpGet("task/{taskId:guid}")]
        public async Task<ActionResult<CommonResponse<List<QuestionListDto>>>> GetByTask(Guid taskId, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            try
            {
                var list = await _questionService.GetByTaskIdAsync(taskId, page, pageSize);
                return Ok(CommonResponse<List<QuestionListDto>>.SuccessResponse("Fetched questions.", list));
            }
            catch (Exception ex)
            {
                return BadRequest(CommonResponse<List<QuestionListDto>>.FailureResponse(ex.Message));
            }
        }

        /// <summary>
        /// Returns a formatted, read-only preview of the specified question suitable for display.
        /// </summary>
        /// <param name="id">Identifier of the question to preview.</param>
        /// <returns>A <see cref="CommonResponse{QuestionPreviewDto}"/> containing the preview data.</returns>
        [HttpGet("{id:guid}/preview")]
        public async Task<ActionResult<CommonResponse<QuestionPreviewDto>>> Preview(Guid id)
        {
            try
            {
                var p = await _questionService.PreviewAsync(id);
                return Ok(CommonResponse<QuestionPreviewDto>.SuccessResponse("Preview generated.", p));
            }
            catch (Exception ex)
            {
                return NotFound(CommonResponse<QuestionPreviewDto>.FailureResponse(ex.Message));
            }
        }
    }
}
