using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Winfocus.LMS.Application.DTOs;
using Winfocus.LMS.Application.DTOs.Masters;
using Winfocus.LMS.Application.Interfaces;
using Winfocus.LMS.Application.Services;
using Winfocus.LMS.Domain.Entities;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Winfocus.LMS.API.Controllers
{
    /// <summary>
    /// Handles authentication endpoints.
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class StreamController : BaseController
    {
        private readonly IStreamService _streamService;

        /// <summary>
        /// Initializes a new instance of the <see cref="StreamController"/> class.
        /// </summary>
        /// <param name="streamService">The state service.</param>
        public StreamController(IStreamService streamService)
        {
            _streamService = streamService;
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns>StreamDto list.</returns>
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<StreamDto>>> GetAll()
            => Ok(await _streamService.GetAllAsync());

        /// <summary>
        /// Creates the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>StreamDto.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost]
        public async Task<CommonResponse<StreamDto>> Create(
            StreamRequest request)
        {
            var updatedRequest = request with
            {
                userId = UserId
            };
            var created = await _streamService.CreateAsync(updatedRequest);
            if (created == null)
            {
                return CommonResponse<StreamDto>.FailureResponse("Failed to create Stream.");
            }
            else
            {
                return CommonResponse<StreamDto>.SuccessResponse("Stream created successfully.", created);
            }
        }

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>StreamDto by id.</returns>
        [HttpGet("{id:guid}")]
        public async Task<CommonResponse<StreamDto>> Get(Guid id)
        {
            var result = await _streamService.GetByIdAsync(id);
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
        public async Task<CommonResponse<StreamDto>> Update(
            Guid id,
            StreamRequest request)
        {
            var updatedRequest = request with
            {
                userId = UserId
            };
            var updated = await _streamService.UpdateAsync(id, updatedRequest);
            if (updated == null)
            {
                return CommonResponse<StreamDto>.FailureResponse("Failed to create batchtiming for monday to friday.");
            }
            else
            {
                return CommonResponse<StreamDto>.SuccessResponse("Batchtiming for monday to friday created successfully.", updated);
            }
        }

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="gradeid">The identifier.</param>
        /// <returns>StreamDto by id.</returns>
        [HttpGet("by-grade/{gradeid:guid}")]
        public async Task<CommonResponse<List<StreamDto>>> GetByGradeId(Guid gradeid)
        {
            var result = await _streamService.GetByGradeIdAsync(gradeid);
            if (result == null)
            {
                return CommonResponse<List<StreamDto>>.FailureResponse("Stream not found for this grade.");
            }
            else
            {
                return CommonResponse<List<StreamDto>>.SuccessResponse("Fetches steams for this grade.", result);
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
            bool result = await _streamService.DeleteAsync(id);
            if (result)
            {
                return CommonResponse<bool>.SuccessResponse("Stream deleted successfully.", true);
            }
            else
            {
                return CommonResponse<bool>.FailureResponse("Failed to delete Stream.");
            }
        }
    }
}
