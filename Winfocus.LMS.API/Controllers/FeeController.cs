namespace Winfocus.LMS.API.Controllers
{
    using Asp.Versioning;
    using Microsoft.AspNetCore.Mvc;
    using Winfocus.LMS.Application.DTOs.Fees;
    using Winfocus.LMS.Application.Interfaces;

    /// <summary>
    /// FeeController.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public sealed class FeeController : ControllerBase
    {
        private readonly IFeeService _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="FeeController"/> class.
        /// </summary>
        /// <param name="service">The service.</param>
        public FeeController(IFeeService service)
        {
            _service = service;
        }

        /// <summary>
        /// Gets the fee page.
        /// </summary>
        /// <param name="studentId">The student identifier.</param>
        /// <returns>Task&lt;IActionResult&gt;.</returns>
        [HttpGet("{studentId}")]
        public async Task<ActionResult<IReadOnlyList<FeePageResponseDto>>> GetFeePage(Guid studentId)
        {
            var result = await _service.GetFeePageAsync(studentId);
            return Ok(result);
        }

        /// <summary>
        /// Selects the fee.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Task&lt;IActionResult&gt;.</returns>
        [HttpPost("select")]
        public async Task<IActionResult> SelectFee(SelectFeeRequestDto request)
        {
            var result = await _service.SelectFeeAsync(request);
            return Ok(result);
        }
    }
}
