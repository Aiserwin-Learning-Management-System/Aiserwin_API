using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
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
    public class GradeController : BaseController
    {
        private readonly IGradeService _gradeService;

        /// <summary>
        /// Initializes a new instance of the <see cref="GradeController"/> class.
        /// </summary>
        /// <param name="gradeService">The state service.</param>
        public GradeController(IGradeService gradeService)
        {
            _gradeService = gradeService;
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns>GradeDto list.</returns>
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<GradeDto>>> GetAll()
            => Ok(await _gradeService.GetAllAsync());

        /// <summary>
        /// Creates the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>GradeDto.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost]
        public async Task<CommonResponse<GradeDto>> Create(
            GradeRequest request)
        {
            var updatedRequest = request with
            {
                userId = UserId
            };
            var created = await _gradeService.CreateAsync(updatedRequest);
            if (created == null)
            {
                return CommonResponse<GradeDto>.FailureResponse("Failed to create Grade.");
            }
            else
            {
                return CommonResponse<GradeDto>.SuccessResponse("Grade created successfully.", created);
            }
        }

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>GradeDto by id.</returns>
        [HttpGet("{id:guid}")]
        public async Task<CommonResponse<GradeDto>> Get(Guid id)
        {
            var result = await _gradeService.GetByIdAsync(id);
            return result;
        }

        /// <summary>
        /// Updates the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="request">The request.</param>
        /// <returns>result.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPut("{id:guid}")]
        public async Task<CommonResponse<GradeDto>> Update(
            Guid id,
            GradeRequest request)
        {
            var updatedRequest = request with
            {
                userId = UserId
            };
            var updated = await _gradeService.UpdateAsync(id, updatedRequest);
            if (updated == null)
            {
                return CommonResponse<GradeDto>.FailureResponse("Failed to update Grade.");
            }
            else
            {
                return CommonResponse<GradeDto>.SuccessResponse("Grade updated successfully.", updated);
            }
        }

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="syllabusid">The identifier.</param>
        /// <returns>GradeDto by id.</returns>
        [HttpGet("by-syllabus/{syllabusid:guid}")]
        public async Task<CommonResponse<List<GradeDto>>> GetBySyllabusId(Guid syllabusid)
        {
            var result = await _gradeService.GetBySyllabusIdAsync(syllabusid);
            return result;
        }

        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>result.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpDelete("{id:guid}")]
        public async Task<CommonResponse<bool>> Delete(Guid id)
        {
            var result = await _gradeService.DeleteAsync(id);
            if (result)
            {
                return CommonResponse<bool>.SuccessResponse("Grade deleted successfully.", true);
            }
            else
            {
                return CommonResponse<bool>.FailureResponse("Failed to delete Grade.");
            }
        }

        /// <summary>
        /// Gets the filtered.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>result.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet("filter")]
        public async Task<ActionResult<CommonResponse<PagedResult<GradeDto>>>> GetFiltered(
        [FromQuery] PagedRequest request)
        {
            var result = await _gradeService.GetFilteredAsync(request);
            return Ok(result);
        }
    }
}
