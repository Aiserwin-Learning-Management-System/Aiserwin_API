using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Winfocus.LMS.Application.DTOs;
using Winfocus.LMS.Application.DTOs.Masters;
using Winfocus.LMS.Application.Interfaces;
using Winfocus.LMS.Application.Services;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Winfocus.LMS.API.Controllers
{
    /// <summary>
    /// Handles authentication endpoints.
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class BatchTimingSaturdayController : BaseController
    {
        private readonly IBatchTimingSaturdayService _batchtimingService;

        /// <summary>
        /// Initializes a new instance of the <see cref="BatchTimingSaturdayController"/> class.
        /// </summary>
        /// <param name="batchtimingService">The BatchTimingSaturday service.</param>
        public BatchTimingSaturdayController(IBatchTimingSaturdayService batchtimingService)
        {
            _batchtimingService = batchtimingService;
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns>BatchTimingSaturdayDto list.</returns>
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<BatchTimingSaturdayDto>>> GetAll()
            => Ok(await _batchtimingService.GetAllAsync());

        /// <summary>
        /// Creates the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>BatchTimingSaturdayDto.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost]
        public async Task<CommonResponse<BatchTimingSaturdayDto>> Create(
            BatchTimingRequest request)
        {
            var updatedRequest = request with
            {
                userId = UserId
            };
            var created = await _batchtimingService.CreateAsync(updatedRequest);
            // return CreatedAtAction(nameof(Get), new { id = created.Id }, created);

            if (created == null)
            {
                return CommonResponse<BatchTimingSaturdayDto>.FailureResponse("Failed to create batchtiming for saturday.");
            }
            else
            {
                return CommonResponse<BatchTimingSaturdayDto>.SuccessResponse("Batchtiming for saturday created successfully.", created);
            }
        }

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>BatchTimingSaturdayDto by id.</returns>
        [HttpGet("{id:guid}")]
        public async Task<CommonResponse<BatchTimingSaturdayDto>> Get(Guid id)
        {
            var result = await _batchtimingService.GetByIdAsync(id);
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
        public async Task<CommonResponse<BatchTimingSaturdayDto>> Update(
            Guid id,
            BatchTimingRequest request)
        {
            var updatedRequest = request with
            {
                userId = UserId
            };
            var updated = await _batchtimingService.UpdateAsync(id, updatedRequest);
            if (updated == null)
            {
                return CommonResponse<BatchTimingSaturdayDto>.FailureResponse("Failed to create batchtiming for saturday.");
            }
            else
            {
                return CommonResponse<BatchTimingSaturdayDto>.SuccessResponse("Batchtiming for saturday created successfully.", updated);
            }
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
            var result = await _batchtimingService.DeleteAsync(id);
            if (result)
            {
                return CommonResponse<bool>.SuccessResponse("Batchtiming for saturday deleted successfully.", true);
            }
            else
            {
                return CommonResponse<bool>.FailureResponse("Failed to delete batchtiming for saturday.");
            }
        }

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="subjectid">The identifier.</param>
        /// <returns>BatchTimingSaturdayDto by id.</returns>
        [HttpGet("by-subject/{subjectid:guid}")]
        public async Task<CommonResponse<List<BatchTimingSaturdayDto>>> GetBySubjectId(Guid subjectid)
        {
            var result = await _batchtimingService.GetBySubjectIdAsync(subjectid);
            //return result == null ? NotFound() : Ok(result);
            if (result == null)
            {
                return CommonResponse<List<BatchTimingSaturdayDto>>.FailureResponse("Batchtiming for saturday not found for the given subject.");
            }
            else
            {
                return CommonResponse<List<BatchTimingSaturdayDto>>.SuccessResponse("fetching Batchtiming for saturday for the given subject.", result);
            }
        }

        /// <summary>
        /// Creates the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>result.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost("subject")]
        public async Task<IActionResult> BatchTimingSubjectCreate(
            SubjectBatchTimingRequest request)
        {
            var updatedRequest = request with
            {
                userid = UserId
            };
            await _batchtimingService.BatchTimingSubjectCreate(updatedRequest);
            return NoContent();
        }
    }
}
