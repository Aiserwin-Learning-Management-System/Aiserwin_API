namespace Winfocus.LMS.API.Controllers
{
    using Asp.Versioning;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Winfocus.LMS.Application.DTOs;
    using Winfocus.LMS.Application.DTOs.DtpAdmin;
    using Winfocus.LMS.Application.DTOs.Stats;
    using Winfocus.LMS.Application.Interfaces;

    /// <summary>
    /// DTP Admin — manage operators, view dynamic registration data.
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/dtp-admin")]
    [Authorize(Roles = "Staff,SuperAdmin,CountryAdmin,CenterAdmin")]
    public sealed class DtpAdminController : BaseController
    {
        private readonly IDtpAdminService _service;
        private readonly IOperatorStatsService _statsService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DtpAdminController"/> class.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="statsService">The operator stats service.</param>
        public DtpAdminController(IDtpAdminService service, IOperatorStatsService statsService)
        {
            _service = service;
            _statsService = statsService;
        }

        /// <summary>
        /// Gets column definitions from the active DTP registration form.
        /// Frontend uses this to build dynamic table headers.
        /// </summary>
        /// <returns>List of columns with metadata for frontend table construction.</returns>
        [HttpGet("operators/columns")]
        public async Task<ActionResult<CommonResponse<OperatorColumnsResponseDto>>>
            GetColumns()
        {
            var result = await _service.GetColumnsAsync();
            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// Lists all DTP operators with dynamic column values.
        /// Supports pagination, search, filtering, and sorting by any column.
        /// </summary>
        /// <param name="request">The request with pagination, search, filter, sort parameters.</param>
        /// <returns>Paginated list of operators with dynamic column data.</returns>
        [HttpGet("operators")]
        public async Task<ActionResult<CommonResponse<OperatorListResponseDto>>>
            GetOperators([FromQuery] DtpOperatorFilterRequest request)
        {
            var result = await _service.GetOperatorsAsync(request);
            return Ok(result);
        }

        /// <summary>
        /// Soft-deletes an operator registration.
        /// </summary>
        /// <param name="registrationId">The registration ID.</param>
        /// <returns>True if deletion was successful, false otherwise.</returns>
        [HttpDelete("operators/{registrationId:guid}")]
        public async Task<ActionResult<CommonResponse<bool>>>
            DeleteOperator(Guid registrationId)
        {
            var result = await _service.DeleteOperatorAsync(registrationId, UserId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// Toggles an operator's active/inactive status.
        /// </summary>
        /// <param name="registrationId">The registration ID.</param>
        /// <returns></returns>
        [HttpPatch("operators/{registrationId:guid}/toggle")]
        public async Task<ActionResult<CommonResponse<OperatorToggleResponseDto>>>
            ToggleOperator(Guid registrationId)
        {
            var result = await _service.ToggleOperatorAsync(registrationId, UserId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// Gets comparison statistics for all DTP operators.
        /// Admin-only endpoint.
        /// </summary>
        /// <param name="filter">Period filter parameters.</param>
        /// <returns>All operators comparison statistics.</returns>
        [HttpGet("stats/all-operators")]
        public async Task<ActionResult<CommonResponse<AllOperatorsStatsDto>>> GetAllOperatorsStats(
            [FromQuery] OperatorStatsFilterDto filter)
        {
            CommonResponse<AllOperatorsStatsDto> result =
                await _statsService.GetAllOperatorsStatsAsync(filter);
            return Ok(result);
        }

        /// <summary>
        /// Gets detailed productivity statistics for a specific operator.
        /// Admin-only endpoint.
        /// </summary>
        /// <param name="operatorId">The operator registration identifier.</param>
        /// <param name="filter">Period filter parameters.</param>
        /// <returns>Detailed productivity stats for the specified operator.</returns>
        [HttpGet("stats/operator/{operatorId:guid}")]
        public async Task<ActionResult<CommonResponse<OperatorProductivityDto>>> GetOperatorStats(
            [FromRoute] Guid operatorId,
            [FromQuery] OperatorStatsFilterDto filter)
        {
            CommonResponse<OperatorProductivityDto> result =
                await _statsService.GetProductivityAsync(operatorId, filter);
            return Ok(result);
        }
    }
}
