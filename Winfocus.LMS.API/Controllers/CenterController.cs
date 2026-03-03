namespace Winfocus.LMS.API.Controllers
{
    using Asp.Versioning;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Winfocus.LMS.Application.DTOs;
    using Winfocus.LMS.Application.DTOs.Common;
    using Winfocus.LMS.Application.DTOs.Masters;
    using Winfocus.LMS.Application.Interfaces;
    using Winfocus.LMS.Application.Services;

    /// <summary>
    /// Handles authentication endpoints.
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CenterController : BaseController
    {
        private readonly ICenterService _centerService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CenterController"/> class.
        /// </summary>
        /// <param name="centerService">The center service.</param>
        public CenterController(ICenterService centerService)
        {
            _centerService = centerService;
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns>CenterDto list.</returns>
        [HttpGet]
        public async Task<ActionResult<CommonResponse<List<CenterDto>>>> GetAll()
           => Ok(await _centerService.GetAllAsync());

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>CenterDto by id.</returns>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<CommonResponse<CenterDto>>> Get(Guid id)
         => Ok(await _centerService.GetByIdAsync(id));

        /// <summary>
        /// Creates the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>CenterDto.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost]
        public async Task<ActionResult<CommonResponse<CenterDto>>> Create(
            CenterRequestDto request)
        {
            var updatedRequest = request with
            {
                userId = UserId
            };
            var created = await _centerService.CreateAsync(updatedRequest);
            return Ok(created);
        }

        /// <summary>
        /// Updates the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="request">The request.</param>
        /// <returns>result.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<CommonResponse<CenterDto>>> Update(
            Guid id,
            CenterRequestDto request)
        {
            var updatedRequest = request with
            {
                userId = UserId
            };
            var updated = await _centerService.UpdateAsync(id, updatedRequest);
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
            var result = await _centerService.DeleteAsync(id);
            return Ok(result);
        }

        /// <summary>
        /// Gets centre by country, mode of study and state.
        /// </summary>
        /// <param name="request">Filter parameters.</param>
        /// <returns> list of CentreDto.</returns>
        [HttpGet("{countryId:guid}/{modeOfStudyId:guid}/{stateId:guid}")]
        public async Task<CommonResponse<List<CenterDto>>> Get([FromRoute] CenterGetRequest request)
        {
            return await _centerService.GetByFilterAsync(
                request.countryId,
                request.modeOfStudyId,
                request.stateId);
        }

        /// <summary>
        /// Retrieves filtered Center with pagination.
        /// </summary>
        /// <param name="request">The paged request.</param>
        /// <returns>Paginated list of Center.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet("filter")]
        public async Task<ActionResult<CommonResponse<PagedResult<CenterDto>>>> GetFiltered(
            [FromQuery] PagedRequest request)
        {
            var result = await _centerService.GetFilteredAsync(request);
            return Ok(result);
        }
    }
}
