namespace Winfocus.LMS.API.Controllers
{
    using Asp.Versioning;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Winfocus.LMS.Application.DTOs;
    using Winfocus.LMS.Application.DTOs.Common;
    using Winfocus.LMS.Application.DTOs.Dashboard;
    using Winfocus.LMS.Application.DTOs.Review;
    using Winfocus.LMS.Application.Interfaces;

    /// <summary>
    /// DTP Operator dashboard — profile, stats, tasks, corrections.
    /// Operator identified via JWT token (no operator ID in URL).
    /// DTP staff = User with Role "Staff" + StaffCategory containing "DTP".
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/DTPoperator")]
    [Authorize(Roles = "Staff,SuperAdmin,CountryAdmin,CenterAdmin,Admin")]
    public sealed class OperatorController : BaseController
    {
        private readonly IOperatorDashboardService _dashboardService;
        private readonly IQuestionCorrectionService _correctionService;

        /// <summary>
        /// Initializes a new instance of the <see cref="OperatorController"/> class.
        /// </summary>
        /// <param name="dashboardService">The dashboard service.</param>
        /// <param name="correctionService">The correction service.</param>
        public OperatorController(
            IOperatorDashboardService dashboardService,
            IQuestionCorrectionService correctionService)
        {
            _dashboardService = dashboardService;
            _correctionService = correctionService;
        }

        /// <summary>
        /// Gets the complete operator dashboard in a single API call.
        /// </summary>
        /// <param name="period">Stats period: daily, weekly, monthly, all.</param>
        /// <returns>Complete dashboard data.</returns>
        [HttpGet("dashboard")]
        public async Task<ActionResult<CommonResponse<DashboardDto>>> GetDashboard(
            [FromQuery] string period = "monthly")
        {
            var result = await _dashboardService.GetDashboardAsync(UserId, period);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// Gets the operator's profile info extracted from registration data.
        /// </summary>
        /// <returns>Profile with name, email, phone, employee ID.</returns>
        [HttpGet("profile")]
        public async Task<ActionResult<CommonResponse<OperatorProfileDto>>> GetProfile()
        {
            var result = await _dashboardService.GetProfileAsync(UserId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// Gets the operator's assigned tasks with pagination and filters.
        /// </summary>
        /// <param name="request">Pagination and filter parameters.</param>
        /// <returns>Paginated task list with progress info.</returns>
        [HttpGet("tasks")]
        public async Task<ActionResult<CommonResponse<PagedResult<ActiveTaskDto>>>> GetMyTasks(
            [FromQuery] OperatorTaskFilterRequest request)
        {
            var result = await _dashboardService.GetMyTasksAsync(UserId, request);
            return Ok(result);
        }

        /// <summary>
        /// Gets productivity statistics for a specified period.
        /// </summary>
        /// <param name="request">Period filter (daily/weekly/monthly/custom).</param>
        /// <returns>Productivity metrics.</returns>
        [HttpGet("stats")]
        public async Task<ActionResult<CommonResponse<ProductivityStatsDto>>> GetStats(
            [FromQuery] StatsFilterRequest request)
        {
            var result = await _dashboardService.GetStatsAsync(UserId, request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// Gets operator's rejected questions with feedback and full hierarchy.
        /// Year → Syllabus → Grade → Subject → Unit → Chapter → ResourceType.
        /// </summary>
        /// <param name="request">Pagination and search parameters.</param>
        /// <returns>Paginated list of rejected questions with feedback.</returns>
        [HttpGet("corrections")]
        public async Task<ActionResult<CommonResponse<PagedResult<CorrectionListDto>>>>
            GetMyCorrections([FromQuery] PagedRequest request)
        {
            var result = await _correctionService.GetMyCorrectionsAsync(UserId, request);
            return Ok(result);
        }

        /// <summary>
        /// Gets full detail of a single rejected question with review history.
        /// Includes all review cycles, options, and complete hierarchy.
        /// </summary>
        /// <param name="questionId">Question ID.</param>
        /// <returns>Full correction detail with review history.</returns>
        [HttpGet("corrections/{questionId:guid}")]
        public async Task<ActionResult<CommonResponse<CorrectionDetailDto>>>
            GetCorrectionDetail(Guid questionId)
        {
            var result = await _correctionService.GetCorrectionDetailAsync(UserId, questionId);
            return result.Success ? Ok(result) : NotFound(result);
        }

        /// <summary>
        /// Fixes a rejected question and resubmits for review.
        /// Updates question text/options, changes status back to Submitted,
        /// re-increments TaskAssignment.CompletedCount.
        /// </summary>
        /// <param name="questionId">Question ID.</param>
        /// <param name="dto">Updated question data.</param>
        /// <returns>Success/failure result.</returns>
        [HttpPut("corrections/{questionId:guid}")]
        public async Task<ActionResult<CommonResponse<bool>>>
            FixAndResubmit(Guid questionId, [FromBody] FixQuestionDto dto)
        {
            var result = await _correctionService.FixAndResubmitAsync(
                UserId, questionId, dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
