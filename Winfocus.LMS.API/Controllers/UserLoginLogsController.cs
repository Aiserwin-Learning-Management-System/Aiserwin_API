namespace Winfocus.LMS.API.Controllers
{
    using Asp.Versioning;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Winfocus.LMS.Application.DTOs.Common;
    using Winfocus.LMS.Application.DTOs.LoginLog;
    using Winfocus.LMS.Application.Interfaces;

    /// <summary>
    /// UserLoginLogsController.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class UserLoginLogsController : ControllerBase
    {
        private readonly IUserLoginLogService _loginLogService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserLoginLogsController"/> class.
        /// </summary>
        /// <param name="loginLogService">The login log service.</param>
        public UserLoginLogsController(IUserLoginLogService loginLogService)
        {
            _loginLogService = loginLogService;
        }

        /// <summary>
        /// Records a new login attempt.
        /// </summary>
        /// <param name="dto">The login log details.</param>
        /// <returns>The created login log entry.</returns>
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(UserLoginLogDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddLog([FromBody] CreateLoginLogDto dto)
        {
            dto.IpAddress ??= HttpContext.Connection.RemoteIpAddress?.ToString();
            dto.UserAgent ??= Request.Headers.UserAgent.ToString();

            var result = await _loginLogService.AddLogAsync(dto);
            return CreatedAtAction(nameof(GetByUserId), new { userId = result.UserId }, result);
        }

        /// <summary>
        /// Gets paginated login logs for a specific user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="request">The pagination, sorting, and filtering parameters.</param>
        /// <returns>A paginated result of login logs.</returns>
        [HttpGet("user/{userId:guid}")]
        [ProducesResponseType(typeof(PagedResult<UserLoginLogDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByUserId(
            Guid userId,
            [FromQuery] PagedRequest request)
        {
            var result = await _loginLogService.GetLogsByUserIdAsync(userId, request);
            return Ok(result);
        }

        /// <summary>
        /// Soft-deletes a specific login log entry.
        /// </summary>
        /// <param name="logId">The login log identifier.</param>
        /// <returns><c>true</c> if the log was successfully deleted; otherwise, <c>false</c>.</returns>
        [HttpDelete("{logId:guid}")]
        [Authorize(Roles = "SuperAdmin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteLog(Guid logId)
        {
            var deleted = await _loginLogService.DeleteLogAsync(logId);
            return deleted ? NoContent() : NotFound();
        }
    }
}
