using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    public class StudentFeeSelectionController : BaseController
    {
        private readonly IStudentFeeSelectionService _iStudentFeeSelectionService;

        /// <summary>
        /// Initializes a new instance of the <see cref="StudentFeeSelectionController"/> class.
        /// </summary>
        /// <param name="istudentFeeSelection">The doubt_clear service.</param>
        public StudentFeeSelectionController(IStudentFeeSelectionService istudentFeeSelection)
        {
            _iStudentFeeSelectionService = istudentFeeSelection;
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns>StudentFeeSelectionDto list.</returns>
        [HttpGet]
        public async Task<ActionResult<CommonResponse<StudentFeeSelectionDto>>> GetAll()
            => Ok(await _iStudentFeeSelectionService.GetAllAsync());

        /// <summary>
        /// Creates the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>StudentFeeSelectionDto.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost]
        public async Task<ActionResult<CommonResponse<StudentFeeSelectionDto>>> Create(
            StudentFeeSelectionRequest request)
        {
            var updatedRequest = request with
            {
                userId = UserId
            };
            var created = await _iStudentFeeSelectionService.CreateAsync(updatedRequest);
            return Ok(created);
        }

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>StudentFeeSelectionDto by id.</returns>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<CommonResponse<StudentFeeSelectionDto>>> Get(Guid id)
        {
            var result = await _iStudentFeeSelectionService.GetByIdAsync(id);
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
        public async Task<ActionResult<CommonResponse<StudentFeeSelectionDto>>> Update(
            Guid id,
            StudentFeeSelectionRequest request)
        {
            var updatedRequest = request with
            {
                userId = UserId
            };
            var updated = await _iStudentFeeSelectionService.UpdateAsync(id, updatedRequest);
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
        => Ok(await _iStudentFeeSelectionService.DeleteAsync(id));


        /// <summary>
        /// Retrieves filtered with pagination.
        /// </summary>
        /// <param name="request">The paged request.</param>
        /// <returns>Paginated list of student fee selection.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet("filter")]
        public async Task<ActionResult<CommonResponse<PagedResult<StudentFeeSelectionDto>>>> GetFiltered(
            [FromQuery] PagedRequest request)
        {
            var result = await _iStudentFeeSelectionService.GetFilteredAsync(request);
            return Ok(result);
        }


    }
}
