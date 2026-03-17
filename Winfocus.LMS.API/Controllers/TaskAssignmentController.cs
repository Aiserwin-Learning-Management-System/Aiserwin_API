using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Winfocus.LMS.Application.DTOs;
using Winfocus.LMS.Application.DTOs.Common;
using Winfocus.LMS.Application.DTOs.Masters;
using Winfocus.LMS.Application.Interfaces;
using Winfocus.LMS.Application.Services;

namespace Winfocus.LMS.API.Controllers
{
    /// <summary>
    /// Handles authentication endpoints.
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class TaskAssignmentController : BaseController
    {
        private readonly ITaskAssignmentService _taskAssignmentService;

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskAssignmentController"/> class.
        /// </summary>
        /// <param name="taskAssignmentService">The taskAssignment Service.</param>
        public TaskAssignmentController(ITaskAssignmentService taskAssignmentService)
        {
            _taskAssignmentService = taskAssignmentService;
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns>SyllabusDto list.</returns>
        [HttpGet]
        public async Task<ActionResult<CommonResponse<TaskResponseDto>>> GetAll()
        {
            return Ok(await _taskAssignmentService.GetAllAsync());
        }

        /// <summary>
        /// Creates the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>TaskResponseDto.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost]
        public async Task<ActionResult<CommonResponse<TaskResponseDto>>> Create(
            CreateTaskDto request)
        {
            request.Createdby = UserId;
            var updatedRequest = request;
            var created = await _taskAssignmentService.CreateAsync(updatedRequest);
            return Ok(created);
        }

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>TaskResponseDto by id.</returns>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<CommonResponse<TaskResponseDto>>> Get(Guid id)
        {
            var result = await _taskAssignmentService.GetByIdAsync(id);
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
        public async Task<ActionResult<CommonResponse<TaskResponseDto>>> Update(
            Guid id,
            CreateTaskDto request)
        {
            request.Createdby = UserId;
            var updatedRequest = request;
            var updated = await _taskAssignmentService.UpdateAsync(id, updatedRequest);
            return Ok(updated);
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
            var response = await _taskAssignmentService.DeleteAsync(id);
            return Ok(response);
        }

        /// <summary>
        /// Retrieves filtered task with pagination.
        /// </summary>
        /// <param name="request">The paged request.</param>
        /// <returns>Paginated list of task.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet("filter")]
        public async Task<ActionResult<CommonResponse<PagedResult<TaskResponseDto>>>> GetFiltered(
            [FromQuery] PagedRequest request)
        {
            var result = await _taskAssignmentService.GetFilteredAsync(request);
            return Ok(result);
        }

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="operatorid">The identifier.</param>
        /// <returns>TaskResponseDto by id.</returns>
        [HttpGet("/operator/{operatorid:guid?}")]
        public async Task<ActionResult<CommonResponse<TaskResponseDto>>> GetByOperator(Guid operatorid)
        {
            var result = await _taskAssignmentService.GetByOperatorIdAsync(operatorid);
            return Ok(result);
        }
    }
}
