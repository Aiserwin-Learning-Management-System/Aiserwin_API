namespace Winfocus.LMS.API.Controllers
{
    using Asp.Versioning;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Winfocus.LMS.Application.DTOs;
    using Winfocus.LMS.Application.DTOs.Common;
    using Winfocus.LMS.Application.DTOs.Masters;
    using Winfocus.LMS.Application.Interfaces;

    /// <summary>
    /// Handles stream endpoints.
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
        /// <param name="streamService">The stream service.</param>
        public StreamController(IStreamService streamService)
        {
            _streamService = streamService;
        }

        /// <summary>
        /// Gets all streams.
        /// </summary>
        /// <returns>StreamDto list.</returns>
        [HttpGet]
        public async Task<ActionResult<CommonResponse<List<StreamDto>>>> GetAll()
            => Ok(await _streamService.GetAllAsync());

        /// <summary>
        /// Creates a new stream.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>StreamDto.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost]
        public async Task<ActionResult<CommonResponse<StreamDto>>> Create(
            StreamRequest request)
        {
            var updatedRequest = request with { userId = UserId };
            var created = await _streamService.CreateAsync(updatedRequest);

            if (created == null)
            {
                return Ok(CommonResponse<StreamDto>.FailureResponse("Failed to create stream."));
            }

            return Ok(CommonResponse<StreamDto>.SuccessResponse(
                "Stream created successfully.", created));
        }

        /// <summary>
        /// Gets a stream by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>StreamDto.</returns>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<CommonResponse<StreamDto>>> Get(Guid id)
        {
            var result = await _streamService.GetByIdAsync(id);
            return Ok(result);
        }

        /// <summary>
        /// Updates a stream.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="request">The request.</param>
        /// <returns>StreamDto.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<CommonResponse<StreamDto>>> Update(
            Guid id,
            StreamRequest request)
        {
            var updatedRequest = request with { userId = UserId };
            var updated = await _streamService.UpdateAsync(id, updatedRequest);

            if (updated == null)
            {
                return Ok(CommonResponse<StreamDto>.FailureResponse("Failed to update stream."));
            }

            return Ok(CommonResponse<StreamDto>.SuccessResponse(
                "Stream updated successfully.", updated));
        }

        /// <summary>
        /// Gets streams by grade identifier.
        /// </summary>
        /// <param name="gradeid">The grade identifier.</param>
        /// <returns>StreamDto list.</returns>
        [HttpGet("by-grade/{gradeid:guid}")]
        public async Task<ActionResult<CommonResponse<List<StreamDto>>>> GetByGradeId(
            Guid gradeid)
        {
            var result = await _streamService.GetByGradeIdAsync(gradeid);
            return Ok(result);
        }

        /// <summary>
        /// Deletes a stream.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>bool.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<CommonResponse<bool>>> Delete(Guid id)
        {
            var result = await _streamService.DeleteAsync(id);

            if (result)
            {
                return Ok(CommonResponse<bool>.SuccessResponse(
                    "Stream deleted successfully.", true));
            }

            return Ok(CommonResponse<bool>.FailureResponse(
                "Failed to delete stream."));
        }

        /// <summary>
        /// Retrieves filtered streams with pagination.
        /// </summary>
        /// <param name="request">The paged request.</param>
        /// <returns>Paginated list of streams.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet("filter")]
        public async Task<ActionResult<CommonResponse<PagedResult<StreamDto>>>> GetFiltered(
            [FromQuery] PagedRequest request)
        {
            var result = await _streamService.GetFilteredAsync(request);
            return Ok(result);
        }
    }
}
