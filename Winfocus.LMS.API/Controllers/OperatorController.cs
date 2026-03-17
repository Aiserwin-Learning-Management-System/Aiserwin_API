namespace Winfocus.LMS.API.Controllers
{
    using Asp.Versioning;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Winfocus.LMS.Application.DTOs;
    using Winfocus.LMS.Application.DTOs.Common;
    using Winfocus.LMS.Application.DTOs.Dashboard;
    using Winfocus.LMS.Application.Interfaces;

    /// <summary>
    /// DTP Operator dashboard — profile, stats, tasks, corrections.
    /// Operator identified via JWT token (no operator ID in URL).
    /// DTP staff = User with Role "Staff" + StaffCategory containing "DTP".
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/DTPoperator")]
    [Authorize(Roles = "Staff,SuperAdmin")]
    public sealed class OperatorController : BaseController
    {
        private readonly IOperatorDashboardService _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="OperatorController"/> class.
        /// </summary>
        /// <param name="service">The dashboard service.</param>
        public OperatorController(IOperatorDashboardService service)
        {
            _service = service;
        }

        /// <summary>
        /// Gets the complete operator dashboard in a single API call.
        /// </summary>
        /// <param name="period">Stats period: daily, weekly, monthly, all</param>
        /// <returns>Complete dashboard data.</returns>
        [HttpGet("dashboard")]
        public async Task<ActionResult<CommonResponse<DashboardDto>>> GetDashboard(
            [FromQuery] string period = "monthly")
        {
            var result = await _service.GetDashboardAsync(UserId, period);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// Gets the operator's profile info extracted from registration data.
        /// </summary>
        /// <returns>Profile with name, email, phone, employee ID.</returns>
        [HttpGet("profile")]
        public async Task<ActionResult<CommonResponse<OperatorProfileDto>>> GetProfile()
        {
            var result = await _service.GetProfileAsync(UserId);
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
            var result = await _service.GetMyTasksAsync(UserId, request);
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
            var result = await _service.GetStatsAsync(UserId, request);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
