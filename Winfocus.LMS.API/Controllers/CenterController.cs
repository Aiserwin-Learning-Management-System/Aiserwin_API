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
    using Winfocus.LMS.Domain.Entities;

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
        /// <param name="countryId">The countryId.</param>
        /// <returns>CenterDto list.</returns>
        [HttpGet("{countryId:guid?}")]
        public async Task<ActionResult<CommonResponse<List<CenterDto>>>> GetAll(Guid countryId)
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                if (User.IsInRole("SuperAdmin"))
                {
                    return Ok(await _centerService.GetAllAsync());
                }

                if (UserId != Guid.Empty)
                {
                    return Ok(await _centerService.GetByCountryAsync(CountryId));
                }
            }

            return Ok(await _centerService.GetByCountryAsync(countryId));
        }

        // => Ok(await _centerService.GetAllAsync());

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="countryid">The countryId.</param>
        /// <returns>CenterDto by id.</returns>
        [HttpGet("{id:guid}/{countryid:guid?}")]
        public async Task<ActionResult<CommonResponse<CenterDto>>> Get(Guid id, Guid countryid)
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                if (User.IsInRole("SuperAdmin"))
                {
                    return Ok(await _centerService.GetByIdAsync(id, Guid.Empty));
                }

                if (UserId != Guid.Empty)
                {
                    return Ok(await _centerService.GetByIdAsync(id, CountryId));
                }
            }

            return Ok(await _centerService.GetByIdAsync(id, countryid));
        }

        //=> Ok(await _centerService.GetByIdAsync(id));

        /// <summary>
        /// Creates the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>CenterDto.</returns>
        [Authorize(Roles = "Admin,SuperAdmin,CountryAdmin")]
        [HttpPost]
        public async Task<ActionResult<CommonResponse<CenterDto>>> Create(
            CenterRequestDto request)
        {
            var isSuperAdmin = User.IsInRole("SuperAdmin");
            if (!isSuperAdmin && CountryId != request.countryid)
            {
                return StatusCode(403, "You are not allowed to create data for this center.");
            }

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
        [Authorize(Roles = "Admin,SuperAdmin,CountryAdmin")]
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<CommonResponse<CenterDto>>> Update(
            Guid id,
            CenterRequestDto request)
        {
            var updatedRequest = request with
            {
                userId = UserId
            };
            var isSuperAdmin = User.IsInRole("SuperAdmin");
            if (!isSuperAdmin && CountryId != request.countryid)
            {
                return StatusCode(403, "You are not allowed to create data for this center.");
            }

            var updated = await _centerService.UpdateAsync(id, updatedRequest);
            return Ok(updated);
        }

        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>result.</returns>
        [Authorize(Roles = "Admin,SuperAdmin,CountryAdmin")]
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<CommonResponse<bool>>> Delete(Guid id)
        {
            var isSuperAdmin = User.IsInRole("SuperAdmin");
            if (isSuperAdmin)
            {
                return await _centerService.DeleteAsync(id, Guid.Empty);
            }

            var result = await _centerService.DeleteAsync(id, CountryId);
            return Ok(result);
        }

        /// <summary>
        /// Gets centre by country, mode of study and state.
        /// </summary>
        /// <param name="request">Filter parameters.</param>
        /// <returns> list of CentreDto.</returns>
        [HttpGet("{countryId:guid}/{modeOfStudyId:guid}/{stateId:guid?}")]
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
        [Authorize(Roles = "Admin,SuperAdmin,CountryAdmin")]
        [HttpGet("filter")]
        public async Task<ActionResult<CommonResponse<PagedResult<CenterDto>>>> GetFiltered(
            [FromQuery] PagedRequest request)
        {
            var isSuperAdmin = User.IsInRole("SuperAdmin");
            if (isSuperAdmin)
            {
                return Ok(await _centerService.GetFilteredAsync(request, Guid.Empty));
            }

            var result = await _centerService.GetFilteredAsync(request, CountryId);
            return Ok(result);
        }
    }
}
