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
    /// Handles authentication endpoints.
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class SyllabusController : BaseController
    {
        private readonly ISyllabusService _syllabusService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SyllabusController"/> class.
        /// </summary>
        /// <param name="syllabusService">The state service.</param>
        public SyllabusController(ISyllabusService syllabusService)
        {
            _syllabusService = syllabusService;
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns>SyllabusDto list.</returns>
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<SyllabusDto>>> GetAll()
            => Ok(await _syllabusService.GetAllAsync());

        /// <summary>
        /// Creates the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>SyllabusDto.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost]
        public async Task<ActionResult<CommonResponse<SyllabusDto>>> Create(
            SyllabusRequest request)
        {
            var updatedRequest = request with
            {
                UserId = UserId
            };
            var created = await _syllabusService.CreateAsync(updatedRequest);
            if (created == null)
            {
                return CommonResponse<SyllabusDto>.FailureResponse("Failed to create syllabus.");
            }
            else
            {
                return CommonResponse<SyllabusDto>.SuccessResponse("Syllabus created successfully.", created);
            }
        }

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>SyllabusDto by id.</returns>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<CommonResponse<SyllabusDto>>> Get(Guid id)
        {
            var result = await _syllabusService.GetByIdAsync(id);
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
        public async Task<ActionResult<CommonResponse<SyllabusDto>>> Update(
            Guid id,
            SyllabusRequest request)
        {
            var updatedRequest = request with
            {
                UserId = UserId
            };
            var updated = await _syllabusService.UpdateAsync(id, updatedRequest);
            if (updated == null)
            {
                return CommonResponse<SyllabusDto>.FailureResponse("Failed to update Syllabus.");
            }
            else
            {
                return CommonResponse<SyllabusDto>.SuccessResponse("Syllabus updated successfully.", updated);
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
            bool response = await _syllabusService.DeleteAsync(id);
            if (response)
            {
                return CommonResponse<bool>.SuccessResponse("Syllabus deleted successfully.", true);
            }
            else
            {
                return CommonResponse<bool>.FailureResponse("Failed to delete syllabus.");
            }
        }

        /// <summary>
        /// Retrieves filtered syllabuses with pagination.
        /// </summary>
        /// <param name="request">The paged request.</param>
        /// <returns>Paginated list of syllabuses.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet("filter")]
        public async Task<ActionResult<CommonResponse<PagedResult<SyllabusDto>>>> GetFiltered(
            [FromQuery] PagedRequest request)
        {
            var result = await _syllabusService.GetFilteredAsync(request);
            return Ok(result);
        }
    }
}
