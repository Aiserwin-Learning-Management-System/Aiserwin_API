using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Winfocus.LMS.Application.DTOs;
using Winfocus.LMS.Application.DTOs.Teacher;
using Winfocus.LMS.Application.Interfaces;
    using Microsoft.AspNetCore.Authorization;
    using Winfocus.LMS.Application.DTOs.Common;

namespace Winfocus.LMS.API.Controllers
{
    /// <summary>
    /// API controller for teacher registration operations (create, read, update, soft delete).
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class TeacherRegistrationController : BaseController
    {
        private readonly ITeacherRegistrationService _service;
        private readonly ILogger<TeacherRegistrationController> _logger;
        private readonly Winfocus.LMS.Application.Interfaces.IAuthService _authService;

        /// <summary>
        /// Initializes a new instance of the <see cref="TeacherRegistrationController"/> class.
        /// </summary>
        /// <param name="service">The academic year service.</param>
        /// <param name="logger">The logger.</param>
        public TeacherRegistrationController(ITeacherRegistrationService service, ILogger<TeacherRegistrationController> logger, Winfocus.LMS.Application.Interfaces.IAuthService authService)
        {
            _service = service;
            _logger = logger;
            _authService = authService;
        }

        /// <summary>
        /// Retrieves filtered teacher registrations using the provided filter criteria.
        /// </summary>
        /// <param name="request">Filter request parameters.</param>
        /// <returns>Paged result containing matching teacher registrations.</returns>
        [Authorize(Roles = "Admin,SuperAdmin,CountryAdmin,CenterAdmin")]
        [HttpGet("teacher-filter")]
        public async Task<ActionResult<PagedResult<TeacherRegistrationDto>>> GetFiltered([FromQuery] Winfocus.LMS.Application.DTOs.Teacher.TeacherFilterRequest request)
        {
            try
            {
                var result = await _service.GetFilteredAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error filtering teachers");
                return StatusCode(500, null);
            }
        }

        /// <summary>
        /// Confirms a teacher registration (marks as submitted/pending).
        /// </summary>
        /// <param name="id">Teacher identifier.</param>
        /// <returns>Operation result indicating success or failure.</returns>
        [HttpPost("{id:guid}/confirm")]
        public async Task<ActionResult<CommonResponse<bool>>> Confirm(Guid id)
        {
            try
            {
                var response = await _service.TeacherConfirm(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error confirming teacher {Id}", id);
                return StatusCode(500, CommonResponse<bool>.FailureResponse("An error occurred while confirming the teacher."));
            }
        }

        /// <summary>
        /// Approves a teacher registration and creates a user account.
        /// </summary>
        /// <param name="id">Identifier of the approve teacher.</param>
        /// <returns>.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost("{id:guid}/approve")]
        public async Task<ActionResult<CommonResponse<bool>>> Approve(Guid id)
        {
            try
            {
                var response = await _service.TeacherApprove(id);

                // if approved, create user and link
                if (response != null && response.Success)
                {
                    var teacherResp = await _service.GetByIdAsync(id);
                    if (teacherResp != null && teacherResp.Success && teacherResp.Data != null)
                    {
                        var teacher = teacherResp.Data;
                        var username = teacher.FullName
                            ?.Trim()
                            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                            .FirstOrDefault();

                        var registerReq = new Winfocus.LMS.Application.DTOs.Auth.RegisterRequestDto(
                            username ?? string.Empty,
                            teacher.EmailAddress,
                            new List<string> { "Teacher" },
                            teacher.CountryId,
                            Guid.Empty,
                            teacher.EmploymentTypeId);

                        var authResult = await _authService.RegisterAsync(registerReq);
                        if (authResult != null && authResult.userId != Guid.Empty)
                        {
                            await _service.LinkUserAsync(id, authResult.userId);
                        }
                    }
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error approving teacher {Id}", id);
                return StatusCode(500, CommonResponse<bool>.FailureResponse("An error occurred while approving the teacher."));
            }
        }

        /// <summary>
        /// Creates a new teacher registration.
        /// </summary>
        /// <param name="request">Teacher registration request.</param>
        /// <returns>A <see cref="CommonResponse{T}"/> containing the created <see cref="TeacherRegistrationDto"/> on success or an error message.</returns>
        [HttpPost]
        public async Task<ActionResult<CommonResponse<TeacherRegistrationDto>>> Create(TeacherRegistrationRequest request)
        {
            try
            {
                var result = await _service.CreateAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating teacher registration");
                return StatusCode(500, CommonResponse<TeacherRegistrationDto>.FailureResponse("An error occurred while creating the teacher."));
            }
        }

        /// <summary>
        /// Gets a teacher registration by identifier.
        /// </summary>
        /// <param name="id">Teacher identifier.</param>
        /// <returns>A <see cref="CommonResponse{T}"/> containing the teacher DTO.</returns>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<CommonResponse<TeacherRegistrationDto>>> Get(Guid id)
        {
            try
            {
                var result = await _service.GetByIdAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching teacher by id {Id}", id);
                return StatusCode(500, CommonResponse<TeacherRegistrationDto>.FailureResponse("An error occurred while fetching the teacher."));
            }
        }

        /// <summary>
        /// Gets all teacher registrations.
        /// </summary>
        /// <returns>A <see cref="CommonResponse{T}"/> containing list of teacher DTOs.</returns>
        [HttpGet]
        public async Task<ActionResult<CommonResponse<List<TeacherRegistrationDto>>>> GetAll()
        {
            try
            {
                var result = await _service.GetAllAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all teachers");
                return StatusCode(500, CommonResponse<List<TeacherRegistrationDto>>.FailureResponse("An error occurred while fetching teachers."));
            }
        }

        /// <summary>
        /// Updates an existing teacher registration.
        /// </summary>
        /// <param name="id">Teacher identifier.</param>
        /// <param name="request">Updated teacher data.</param>
        /// <returns>.</returns>
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<CommonResponse<TeacherRegistrationDto>>> Update(Guid id, TeacherRegistrationRequest request)
        {
            try
            {
                var result = await _service.UpdateAsync(id, request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating teacher {Id}", id);
                return StatusCode(500, CommonResponse<TeacherRegistrationDto>.FailureResponse("An error occurred while updating the teacher."));
            }
        }

        /// <summary>
        /// Soft deletes a teacher registration (marks as deleted but does not remove from database).
        /// </summary>
        /// <param name="id">Teacher identifier.</param>
        /// <returns>.</returns>
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<CommonResponse<bool>>> SoftDelete(Guid id)
        {
            try
            {
                var result = await _service.SoftDeleteAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting teacher {Id}", id);
                return StatusCode(500, CommonResponse<bool>.FailureResponse("An error occurred while deleting the teacher."));
            }
        }
    }
}
