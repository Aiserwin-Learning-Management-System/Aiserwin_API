namespace Winfocus.LMS.API.Controllers
{
    using Asp.Versioning;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Winfocus.LMS.Application.DTOs;
    using Winfocus.LMS.Application.DTOs.Common;
    using Winfocus.LMS.Application.DTOs.Registration;
    using Winfocus.LMS.Application.Interfaces;

    /// <summary>
    /// Handles staff registration submission and retrieval.
    /// Supports both JSON and multipart/form-data submissions.
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/staff-registrations")]
    public sealed class StaffRegistrationsController : BaseController
    {
        private readonly IStaffRegistrationService _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="StaffRegistrationsController"/> class.
        /// </summary>
        /// <param name="service">The service.</param>
        public StaffRegistrationsController(IStaffRegistrationService service)
        {
            _service = service;
        }

        /// <summary>
        /// Submits a registration form with file uploads (multipart/form-data).
        /// </summary>
        /// <param name="request">Form values and files.</param>
        /// <returns>Created registration details.</returns>
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<CommonResponse<RegistrationResponseDto>>>
            SubmitWithFiles([FromForm] SubmitRegistrationRequest request)
        {
            var result = await _service.SubmitAsync(request, UserId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// Submits a registration form without files (JSON body).
        /// </summary>
        /// <param name="request">Form values as JSON.</param>
        /// <returns>Created registration details.</returns>
        [HttpPost("json")]
        [Consumes("application/json")]
        public async Task<ActionResult<CommonResponse<RegistrationResponseDto>>>
            SubmitJson([FromBody] SubmitRegistrationRequest request)
        {
            var result = await _service.SubmitAsync(request, UserId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// Gets a single registration with full field values and labels.
        /// </summary>
        /// <param name="id">Registration ID.</param>
        /// <returns>Registration detail with values.</returns>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<CommonResponse<RegistrationDetailDto>>>
            GetById(Guid id)
        {
            var result = await _service.GetByIdAsync(id);
            return result.Success ? Ok(result) : NotFound(result);
        }

        /// <summary>
        /// Lists registrations with pagination and filters.
        /// </summary>
        /// <param name="request">Filter and pagination parameters.</param>
        /// <returns>Paginated list of registrations.</returns>
        [HttpGet]
        [Authorize(Roles = "Admin,SuperAdmin,CountryAdmin")]
        public async Task<ActionResult<CommonResponse<PagedResult<RegistrationResponseDto>>>>
            GetFiltered([FromQuery] StaffRegistrationFilterRequest request)
        {
            var result = await _service.GetFilteredAsync(request);
            return Ok(result);
        }
    }
}
