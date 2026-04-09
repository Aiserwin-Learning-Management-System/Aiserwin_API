namespace Winfocus.LMS.API.Controllers
{
    using Asp.Versioning;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Winfocus.LMS.Application.DTOs;
    using Winfocus.LMS.Application.DTOs.Common;
    using Winfocus.LMS.Application.DTOs.Fees;
    using Winfocus.LMS.Application.Interfaces;

    /// <summary>
    /// FeeController.
    /// </summary>
    /// <seealso cref="Winfocus.LMS.API.Controllers.BaseController" />
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public sealed class FeeController : BaseController
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

        // ═══════════════════════════════════════════════════════
        //  FEE PLAN CRUD
        // ═══════════════════════════════════════════════════════

        /// <summary>
        /// Creates a new FeePlan with optional discounts.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Created FeePlan.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost]
        public async Task<ActionResult<FeePlanDto>> Create(
            [FromBody] CreateFeePlanRequestDto request)
        {
            var updated = request with { userid = UserId };
            var result = await _service.CreateAsync(updated);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        /// <summary>
        /// Retrieves all fee plans.
        /// </summary>
        /// <returns>List of FeePlanDto.</returns>
        [HttpGet]
        public async Task<ActionResult<List<FeePlanDto>>> GetAll()
            => Ok(await _service.GetAllAsync());

        /// <summary>
        /// Retrieves a fee plan by ID.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>FeePlanDto if found; otherwise, NotFound.</returns>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<FeePlanDto>> GetById(Guid id)
        {
            var result = await _service.GetByIdAsync(id);
            return result == null ? NotFound("FeePlan not found.") : Ok(result);
        }

        /// <summary>
        /// Updates a fee plan.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="request">The request.</param>
        /// <returns>Updated FeePlanDto if successful; otherwise, NotFound or BadRequest.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<FeePlanDto>> Update(
            Guid id, [FromBody] CreateFeePlanRequestDto request)
        {
            var updated = request with { userid = UserId };
            var result = await _service.UpdateAsync(id, updated);
            return result == null ? NotFound("FeePlan not found.") : Ok(result);
        }

        /// <summary>
        /// Deletes a fee plan (soft delete).
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>True if deleted; otherwise, NotFound.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<CommonResponse<bool>>> Delete(Guid id)
            => Ok(await _service.DeleteAsync(id));

        /// <summary>
        /// Filtered/paginated fee plans.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Paged result of FeePlanDto.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet("filter")]
        public async Task<ActionResult<CommonResponse<PagedResult<FeePlanDto>>>> GetFiltered(
            [FromQuery] PagedRequest request)
            => Ok(await _service.GetFilteredAsync(request));

        // ═══════════════════════════════════════════════════════
        //  DISCOUNT REQUESTS
        // ═══════════════════════════════════════════════════════

        /// <summary>
        /// Retrieves all students who have submitted a manual discount request.
        /// </summary>
        /// <returns>List of discount request entries.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet("discount-requests")]
        public async Task<ActionResult<CommonResponse<List<DiscountRequestDto>>>>
            GetDiscountRequests()
        {
            var result = await _service.GetDiscountRequestsAsync();
            return result.Success ? Ok(result) : BadRequest(result);
        }

        // ═══════════════════════════════════════════════════════
        //  ADMIN STUDENT FEE MANAGEMENT
        // ═══════════════════════════════════════════════════════

        /// <summary>
        /// Admin view: courses, available discounts, and current assignments
        /// for a specific student.
        /// </summary>
        /// <param name="studentId">The student identifier.</param>
        /// <returns>AdminStudentFeePageDto with course and discount details.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet("student-admin/{studentId:guid}")]
        public async Task<ActionResult<CommonResponse<AdminStudentFeePageDto>>>
            GetAdminStudentFeePage(Guid studentId)
        {
            var result = await _service.GetAdminStudentFeePageAsync(studentId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// Admin assigns discounts to a student for a course.
        /// Replaces any previous assignments for this student+course.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>True if successful; otherwise, BadRequest.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost("assign-discounts")]
        public async Task<ActionResult<CommonResponse<bool>>> AssignDiscounts(
            [FromBody] AssignDiscountsRequestDto request)
        {
            request.UserId = UserId;
            var result = await _service.AssignDiscountsAsync(request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// Admin bulk-updates discount assignments for a student+course.
        /// Same as assign — replaces existing.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>True if successful; otherwise, BadRequest.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPut("assign-discounts")]
        public async Task<ActionResult<CommonResponse<bool>>> UpdateDiscountAssignments(
            [FromBody] AssignDiscountsRequestDto request)
        {
            request.UserId = UserId;
            var result = await _service.AssignDiscountsAsync(request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// Admin removes all discount assignments for a student+course.
        /// </summary>
        /// <param name="studentId">The student identifier.</param>
        /// <param name="courseId">The course identifier.</param>
        /// <returns>True if successful; otherwise, BadRequest.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpDelete("assign-discounts/{studentId:guid}/{courseId:guid}")]
        public async Task<ActionResult<CommonResponse<bool>>> RemoveDiscounts(
            Guid studentId, Guid courseId)
        {
            var result = await _service.RemoveDiscountsAsync(studentId, courseId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        // ═══════════════════════════════════════════════════════
        //  STUDENT PORTAL
        // ═══════════════════════════════════════════════════════

        /// <summary>
        /// Student fee listing page.
        /// Shows all courses with payment types, durations, applied discounts,
        /// and calculated amounts including per-installment breakdown.
        /// </summary>
        /// <param name="studentId">The student identifier.</param>
        /// <returns>StudentFeePageDto with course and fee details.</returns>
        [HttpGet("student-page/{studentId:guid}")]
        public async Task<ActionResult<CommonResponse<StudentFeePageDto>>>
            GetStudentFeePage(Guid studentId)
        {
            var result = await _service.GetStudentFeePageAsync(studentId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// Student confirms fee plan selection.
        /// Creates StudentFeeSelection, discount snapshots, and installment schedule.
        /// Requires declaration acceptance.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>ConfirmFeeResponseDto with selection details if successful; otherwise, BadRequest.</returns>
        [HttpPost("confirm")]
        public async Task<ActionResult<CommonResponse<ConfirmFeeResponseDto>>>
            ConfirmFeeSelection([FromBody] ConfirmFeeRequestDto request)
        {
            var result = await _service.ConfirmFeeSelectionAsync(request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        // ═══════════════════════════════════════════════════════
        //  PAYMENT TRACKING
        // ═══════════════════════════════════════════════════════

        /// <summary>
        /// Gets installment schedule for a confirmed fee selection.
        /// </summary>
        /// <param name="selectionId">The selection identifier.</param>
        /// <returns>List of InstallmentScheduleDto if successful; otherwise, BadRequest.</returns>
        [HttpGet("installments/{selectionId:guid}")]
        public async Task<ActionResult<CommonResponse<List<InstallmentScheduleDto>>>>
            GetInstallments(Guid selectionId)
        {
            var result = await _service.GetInstallmentsAsync(selectionId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// Admin records a payment against a specific installment.
        /// Automatically updates parent selection status.
        /// </summary>
        /// <param name="installmentId">The installment identifier.</param>
        /// <param name="request">The request.</param>
        /// <returns>Updated InstallmentScheduleDto if successful; otherwise, BadRequest.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPut("installments/{installmentId:guid}/pay")]
        public async Task<ActionResult<CommonResponse<InstallmentScheduleDto>>>
            RecordPayment(Guid installmentId, [FromBody] RecordPaymentRequestDto request)
        {
            var result = await _service.RecordPaymentAsync(installmentId, request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// Overall payment summary for a student across all confirmed fee selections.
        /// Shows totals, remaining balances, and next due dates.
        /// </summary>
        /// <param name="studentId">The student identifier.</param>
        /// <returns>PaymentSummaryDto with aggregated payment details if successful; otherwise, BadRequest.</returns>
        [HttpGet("payment-summary/{studentId:guid}")]
        public async Task<ActionResult<CommonResponse<PaymentSummaryDto>>>
            GetPaymentSummary(Guid studentId)
        {
            var result = await _service.GetPaymentSummaryAsync(studentId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        // ═══════════════════════════════════════════════════════
        //  SELECTIONS FILTER (replaces StudentFeeSelectionController)
        // ═══════════════════════════════════════════════════════

        /// <summary>
        /// Filtered/paginated list of all student fee selections.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Paged result of StudentFeeSelectionListDto.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet("selections/filter")]
        public async Task<ActionResult<CommonResponse<PagedResult<StudentFeeSelectionListDto>>>>
            GetSelectionsFiltered([FromQuery] PagedRequest request)
            => Ok(await _service.GetSelectionsFilteredAsync(request));
    }
}
