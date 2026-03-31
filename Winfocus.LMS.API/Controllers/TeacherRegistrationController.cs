using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Winfocus.LMS.Application.DTOs;
using Winfocus.LMS.Application.DTOs.Teacher;
using Winfocus.LMS.Application.Interfaces;

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

        /// <summary>
        /// Initializes a new instance of the <see cref="TeacherRegistrationController"/> class.
        /// </summary>
        /// <param name="service">The academic year service.</param>
        /// <param name="logger">The logger.</param>
        public TeacherRegistrationController(ITeacherRegistrationService service, ILogger<TeacherRegistrationController> logger)
        {
            _service = service;
            _logger = logger;
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
