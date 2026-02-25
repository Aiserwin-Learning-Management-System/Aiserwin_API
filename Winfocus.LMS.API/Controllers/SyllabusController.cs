using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Winfocus.LMS.Application.DTOs;
using Winfocus.LMS.Application.DTOs.Common;
using Winfocus.LMS.Application.DTOs.Masters;
using Winfocus.LMS.Application.Interfaces;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Winfocus.LMS.API.Controllers
{
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
        public async Task<CommonResponse<SyllabusDto>> Create(
            SyllabusRequest request)
        {
            var updatedRequest = request with
            {
                userId = UserId
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
        public async Task<CommonResponse<SyllabusDto>> Get(Guid id)
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
        public async Task<CommonResponse<SyllabusDto>> Update(
            Guid id,
            SyllabusRequest request)
        {
            var updatedRequest = request with
            {
                userId = UserId
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
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="centerid">The identifier.</param>
        /// <returns>SyllabusDto by id.</returns>
        [HttpGet("by-center/{centerid:guid}")]
        public async Task<ActionResult<SyllabusDto>> GetByCenterId(Guid centerid)
        {
            var result = await _syllabusService.GetByCenterIdAsync(centerid);
            return result == null ? NotFound() : Ok(result);
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
        /// <param name="startDate">Filter syllabuses created after this date.</param>
        /// <param name="endDate">Filter syllabuses created before this date.</param>
        /// <param name="active">Filter by active status.</param>
        /// <param name="searchText">Search keyword.</param>
        /// <param name="limit">Number of records to return.</param>
        /// <param name="offset">Number of records to skip.</param>
        /// <param name="sortOrder">Sorting order (asc/desc).</param>
        /// <returns>Paginated list of syllabuses.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet("filter")]
        public async Task<ActionResult<CommonResponse<PagedResult<SyllabusDto>>>> GetFiltered(
    DateTime? startDate,
    DateTime? endDate,
    bool active,
    string? searchText,
    int limit = 20,
    int offset = 0,
    string sortOrder = "asc")
        {
            var result = await _syllabusService.GetFilteredAsync(
                startDate,
                endDate,
                active,
                searchText,
                limit,
                offset,
                sortOrder);

            return Ok(result);
        }
    }
}
