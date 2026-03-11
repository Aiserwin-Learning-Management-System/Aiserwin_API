namespace Winfocus.LMS.API.Controllers
{
    using Asp.Versioning;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Winfocus.LMS.Application.DTOs;
    using Winfocus.LMS.Application.DTOs.Fees;
    using Winfocus.LMS.Application.Interfaces;
    using Winfocus.LMS.Application.Services;
    using Winfocus.LMS.Domain.Enums;

    /// <summary>
    /// FeeController – handles all fee-related operations including
    /// fee page display, fee selection, and discount CRUD management.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public sealed class FeeController : BaseController
    {
        private readonly IFeeService _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="FeeController"/> class.
        /// </summary>
        /// <param name="service">The fee service.</param>
        public FeeController(IFeeService service)
        {
            _service = service;
        }

        /// <summary>
        /// Gets the fee page for a student.
        /// Returns the pricing table with all discount flags and calculated amounts.
        /// Registration fee is excluded from calculations.
        /// </summary>
        /// <param name="studentId">The student identifier.</param>
        /// <returns>Task&lt;ActionResult&lt;FeePageResponseDto&gt;&gt;.</returns>
        [HttpGet("{studentId}")]
        public async Task<ActionResult<FeePageResponseDto>> GetFeePage(Guid studentId)
        {
            var result = await _service.GetFeePageAsync(studentId);
            return Ok(result);
        }

        /// <summary>
        /// Selects a fee plan for a student.
        /// Persists all three discount types (scholarship, seasonal, manual)
        /// and returns the fee summary.
        /// </summary>
        /// <param name="request">The select fee request.</param>
        /// <returns>Task&lt;ActionResult&lt;FeeSummaryDto&gt;&gt;.</returns>
        [HttpPost("select")]
        public async Task<ActionResult<FeeSummaryDto>> SelectFee(
            SelectFeeRequestDto request)
        {
            var result = await _service.SelectFeeAsync(request);
            return Ok(result);
        }

        /// <summary>
        /// Gets all discount entries for a student (across all fee selections).
        /// Returns 3 entries per selection (Scholarship, Seasonal, Manual).
        /// </summary>
        /// <param name="studentId">The student identifier.</param>
        /// <returns>Task&lt;ActionResult&lt;IReadOnlyList&lt;DiscountDetailDto&gt;&gt;&gt;.</returns>
        [HttpGet("discounts/student/{studentId}")]
        public async Task<ActionResult<IReadOnlyList<DiscountDetailDto>>>
            GetDiscountsByStudent(Guid studentId)
        {
            var result = await _service.GetDiscountsByStudentAsync(studentId);
            return Ok(result);
        }

        /// <summary>
        /// Gets discount entries for a specific student fee selection.
        /// Returns 3 entries (Scholarship, Seasonal, Manual).
        /// </summary>
        /// <param name="selectionId">The student fee selection identifier.</param>
        /// <returns>Task&lt;ActionResult&lt;IReadOnlyList&lt;DiscountDetailDto&gt;&gt;&gt;.</returns>
        [HttpGet("discounts/selection/{selectionId}")]
        public async Task<ActionResult<IReadOnlyList<DiscountDetailDto>>>
            GetDiscountsBySelection(Guid selectionId)
        {
            var result = await _service.GetDiscountsBySelectionAsync(selectionId);
            return Ok(result);
        }

        /// <summary>
        /// Creates or updates a discount on a student fee selection.
        /// Valid discount types: Scholarship, Seasonal, Manual.
        /// Recalculates and persists the final amount.
        /// </summary>
        /// <param name="request">The update discount request.</param>
        /// <returns>Task&lt;ActionResult&lt;FeeSummaryDto&gt;&gt;.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPut("discounts")]
        public async Task<ActionResult<FeeSummaryDto>> UpdateDiscount(
            UpdateDiscountRequestDto request)
        {
            var result = await _service.UpdateDiscountAsync(request);
            return Ok(result);
        }

        /// <summary>
        /// Removes (deactivates and zeroes) a specific discount from a student fee selection.
        /// </summary>
        /// <param name="selectionId">The student fee selection identifier.</param>
        /// <param name="discountType">The discount type (Scholarship, Seasonal, Manual).</param>
        /// <returns>Task&lt;ActionResult&lt;FeeSummaryDto&gt;&gt;.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpDelete("discounts/{selectionId}/{discountType}")]
        public async Task<ActionResult<FeeSummaryDto>> RemoveDiscount(
            Guid selectionId,
            DiscountType discountType)
        {
            var result = await _service.RemoveDiscountAsync(selectionId, discountType);
            return Ok(result);
        }

        /// <summary>
        /// Gets the seasonal discount for a fee plan.
        /// </summary>
        /// <param name="feePlanId">The fee plan identifier.</param>
        /// <returns>Task&lt;ActionResult&lt;DiscountDetailDto&gt;&gt;.</returns>
        [HttpGet("discounts/plan/seasonal/{feePlanId}")]
        public async Task<ActionResult<DiscountDetailDto>> GetSeasonalDiscountOnPlan(
            Guid feePlanId)
        {
            var result = await _service.GetSeasonalDiscountOnPlanAsync(feePlanId);
            return Ok(result);
        }

        /// <summary>
        /// Updates the seasonal discount on a fee plan (plan-level setting).
        /// </summary>
        /// <param name="request">The update seasonal discount request.</param>
        /// <returns>Task&lt;IActionResult&gt;.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPut("discounts/plan/seasonal")]
        public async Task<IActionResult> UpdateSeasonalDiscountOnPlan(
            UpdateSeasonalDiscountRequestDto request)
        {
            await _service.UpdateSeasonalDiscountOnPlanAsync(request);
            return NoContent();
        }

        /// <summary>
        /// Removes the seasonal discount from a fee plan (sets to 0% and inactive).
        /// </summary>
        /// <param name="feePlanId">The fee plan identifier.</param>
        /// <returns>Task&lt;IActionResult&gt;.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpDelete("discounts/plan/seasonal/{feePlanId}")]
        public async Task<IActionResult> RemoveSeasonalDiscountOnPlan(Guid feePlanId)
        {
            await _service.RemoveSeasonalDiscountOnPlanAsync(feePlanId);
            return NoContent();
        }

        /// <summary>
        /// Creates a new FeePlan with optional discounts.
        /// </summary>
        /// <param name="request">The fee plan identifier.</param>
        /// <returns>FeePlanDto;.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost]
        public async Task<ActionResult<FeePlanDto>> Create(
            [FromBody] CreateFeePlanRequestDto request)
        {
            var updatedRequest = request with
            {
                userid = UserId
            };
            var result = await _service.CreateAsync(updatedRequest);

            return CreatedAtAction(
                nameof(GetById),
                new { id = result.Id },
                result);
        }

        /// <summary>
        /// Retrieves all fee plans.
        /// </summary>
        /// <returns>A list of fee plans.</returns>
        /// <response code="200">Returns the list of fee plans.</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<FeePlanDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<FeePlanDto>>> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        /// <summary>
        /// Retrieves a fee plan by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the fee plan.</param>
        /// <returns>The matching fee plan.</returns>
        /// <response code="200">Returns the requested fee plan.</response>
        /// <response code="404">If the fee plan is not found.</response>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(FeePlanDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<FeePlanDto>> GetById(Guid id)
        {
            var result = await _service.GetByIdAsync(id);

            if (result == null)
                return NotFound("FeePlan not found.");

            return Ok(result);
        }

        /// <summary>
        /// Updates the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="request">The request.</param>
        /// <returns>result.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<FeePlanDto>> Update(
            Guid id,
            [FromBody] CreateFeePlanRequestDto request)
        {
            var updatedRequest = request with
            {
                userid = UserId
            };
            var result = await _service.UpdateAsync(id, updatedRequest);

            if (result == null)
                return NotFound("FeePlan not found.");

            return Ok(result);
        }

        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>result.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<CommonResponse<bool>>> Delete(Guid id)
        {
            var result = await _service.DeleteAsync(id);
            return Ok(result);
        }
    }
}
