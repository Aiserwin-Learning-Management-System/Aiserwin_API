namespace Winfocus.LMS.API.Controllers
{
    using Asp.Versioning;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Winfocus.LMS.Application.DTOs;
    using Winfocus.LMS.Application.DTOs.Common;
    using Winfocus.LMS.Application.DTOs.Review;
    using Winfocus.LMS.Application.Interfaces;

    /// <summary>
    /// Question Review — Admin/Academic Head reviews submitted questions.
    /// Approves or rejects with mandatory feedback.
    /// Reviewer identified via JWT token.
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/review")]
    [Authorize(Roles = "Staff,SuperAdmin,CountryAdmin,CenterAdmin,Admin")]
    public sealed class ReviewController : BaseController
    {
        private readonly IQuestionReviewService _reviewService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReviewController"/> class.
        /// </summary>
        /// <param name="reviewService">The question review service.</param>
        public ReviewController(IQuestionReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        /// <summary>
        /// Lists submitted questions pending review.
        /// Supports full hierarchy filters: Year, Syllabus, Grade, Subject, Unit, Chapter, ResourceType.
        /// </summary>
        /// <param name="request">Filter and pagination parameters.</param>
        /// <returns>Paginated list of questions pending review.</returns>
        [HttpGet("questions")]
        public async Task<ActionResult<CommonResponse<PagedResult<ReviewQuestionListDto>>>>
            GetPendingReviews([FromQuery] ReviewFilterRequest request)
        {
            var result = await _reviewService.GetPendingReviewsAsync(request);
            return Ok(result);
        }

        /// <summary>
        /// Gets full question detail for review.
        /// Includes options (MCQ), full hierarchy, and complete review history.
        /// </summary>
        /// <param name="id">Question ID.</param>
        /// <returns>Question detail with options and review history.</returns>
        [HttpGet("questions/{id:guid}")]
        public async Task<ActionResult<CommonResponse<ReviewQuestionDetailDto>>>
            GetQuestionForReview(Guid id)
        {
            var result = await _reviewService.GetQuestionForReviewAsync(id);
            return result.Success ? Ok(result) : NotFound(result);
        }

        /// <summary>
        /// Approves a submitted question.
        /// Creates review record with action=Approved.
        /// </summary>
        /// <param name="id">Question ID.</param>
        /// <param name="dto">Optional approval remarks.</param>
        /// <returns>Success/failure result.</returns>
        [HttpPost("questions/{id:guid}/approve")]
        public async Task<ActionResult<CommonResponse<bool>>>
            ApproveQuestion(Guid id, [FromBody] ApproveQuestionDto? dto)
        {
            var reviewerRole = GetReviewerRole();
            var result = await _reviewService.ApproveAsync(id, UserId, reviewerRole, dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// Rejects a submitted question with mandatory feedback.
        /// Creates review record, decrements TaskAssignment.CompletedCount.
        /// </summary>
        /// <param name="id">Question ID.</param>
        /// <param name="dto">Rejection feedback (required, min 10 chars).</param>
        /// <returns>Success/failure result.</returns>
        [HttpPost("questions/{id:guid}/reject")]
        public async Task<ActionResult<CommonResponse<bool>>>
            RejectQuestion(Guid id, [FromBody] RejectQuestionDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(CommonResponse<bool>.FailureResponse(
                    "Feedback is required when rejecting a question."));
            }

            var reviewerRole = GetReviewerRole();
            var result = await _reviewService.RejectAsync(id, UserId, reviewerRole, dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// Gets review dashboard stats — pending count, approved/rejected today, all-time.
        /// </summary>
        /// <returns>Review statistics.</returns>
        [HttpGet("stats")]
        public async Task<ActionResult<CommonResponse<ReviewStatsDto>>>
            GetStats()
        {
            var result = await _reviewService.GetReviewStatsAsync();
            return Ok(result);
        }

        /// <summary>
        /// Determines reviewer role from JWT claims.
        /// </summary>
        private string GetReviewerRole()
        {
            if (User.IsInRole("SuperAdmin"))
            {
                return "SuperAdmin";
            }

            if (User.IsInRole("CountryAdmin"))
            {
                return "CountryAdmin";
            }

            if (User.IsInRole("CenterAdmin"))
            {
                return "CenterAdmin";
            }

            return "Staff";
        }
    }
}
