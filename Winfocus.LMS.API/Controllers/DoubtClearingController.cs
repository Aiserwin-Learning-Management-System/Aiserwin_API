using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Winfocus.LMS.Application.DTOs;
using Winfocus.LMS.Application.DTOs.Common;
using Winfocus.LMS.Application.DTOs.Masters;
using Winfocus.LMS.Application.DTOs.Students;
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
    public class DoubtClearingController : BaseController
    {
        private readonly IDoubtClearingService _doubtclearService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DoubtClearingController"/> class.
        /// </summary>
        /// <param name="doubtclearService">The doubt_clear service.</param>
        public DoubtClearingController(IDoubtClearingService doubtclearService)
        {
            _doubtclearService = doubtclearService;
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns>DoubtClearingDto list.</returns>
        [HttpGet]
        public async Task<ActionResult<CommonResponse<DoubtClearingDto>>> GetAll()
            => Ok(await _doubtclearService.GetAllAsync());

        /// <summary>
        /// Creates the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>DoubtClearingDto.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost]
        public async Task<ActionResult<CommonResponse<DoubtClearingDto>>> Create(
            DoubtClearingRequest request)
        {
            var updatedRequest = request with
            {
                userId = UserId
            };
            var created = await _doubtclearService.CreateAsync(updatedRequest);
            return Ok(created);
        }

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>DoubtClearingDto by id.</returns>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<CommonResponse<DoubtClearingDto>>> Get(Guid id)
        {
            var result = await _doubtclearService.GetByIdAsync(id);
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
        public async Task<ActionResult<CommonResponse<DoubtClearingDto>>> Update(
            Guid id,
            DoubtClearingRequest request)
        {
            var updatedRequest = request with
            {
                userId = UserId
            };
            var updated = await _doubtclearService.UpdateAsync(id, updatedRequest);
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
        => Ok(await _doubtclearService.DeleteAsync(id));

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="subjectid">The identifier.</param>
        /// <returns>DoubtClearingDto by id.</returns>
        [HttpGet("by-subject/{subjectid:guid}")]
        public async Task<ActionResult<CommonResponse<List<DoubtClearingDto>>>> GetBySubjectId(Guid subjectid)
        {
            var result = await _doubtclearService.GetBySubjectIdAsync(subjectid);
            return Ok(result);
        }


        /// <summary>
        /// Retrieves filtered batches for monday to friday with pagination.
        /// </summary>
        /// <param name="request">The paged request.</param>
        /// <returns>Paginated list of doubt clearing.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet("filter")]
        public async Task<ActionResult<CommonResponse<PagedResult<DoubtClearingDto>>>> GetFiltered(
            [FromQuery] PagedRequest request)
        {
            var result = await _doubtclearService.GetFilteredAsync(request);
            return Ok(result);
        }
    }
}
