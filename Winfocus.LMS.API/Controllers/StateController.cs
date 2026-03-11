using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Winfocus.LMS.Application.DTOs;
using Winfocus.LMS.Application.DTOs.Common;
using Winfocus.LMS.Application.DTOs.Masters;
using Winfocus.LMS.Application.Interfaces;
using Winfocus.LMS.Application.Services;
using Winfocus.LMS.Domain.Entities;

namespace Winfocus.LMS.API.Controllers
{
    /// <summary>
    /// Handles authentication endpoints.
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public sealed class StateController : BaseController
    {
        private readonly IStateService _stateService;

        /// <summary>
        /// Initializes a new instance of the <see cref="StateController"/> class.
        /// </summary>
        /// <param name="stateService">The state service.</param>
        public StateController(IStateService stateService)
        {
            _stateService = stateService;
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <param name="countryId">The identifier.</param>
        /// <returns>StateDto list.</returns>
        [HttpGet("{countryId:guid?}")]
        public async Task<ActionResult<CommonResponse<List<StateDto>>>> GetAll(Guid countryId)
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                if (UserId != Guid.Empty)
                {
                    return Ok(await _stateService.GetByCountryIdAsync(CountryId));
                }
            }

            return Ok(await _stateService.GetByCountryIdAsync(countryId));
        }

          //=> Ok(await _stateService.GetAllAsync());

        /// <summary>
        /// Creates the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>StateDto.</returns>
        [Authorize(Roles = "Admin,SuperAdmin,CountryAdmin")]
        [HttpPost]
        public async Task<ActionResult<CommonResponse<StateDto>>> Create(
            CreateMasterStateRequest request)
        {
            if (CountryId != request.countryid)
            {
                return StatusCode(403, "You are not allowed to create data for this country.");
            }

            var updatedRequest = request with
            {
                userId = UserId
            };
            var created = await _stateService.CreateAsync(updatedRequest);
            return Ok(created);
        }

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="countryid">The countryid.</param>
        /// <returns>StateDto by id.</returns>
        [HttpGet("{id:guid}/country/{countryid:guid?}")]
        public async Task<ActionResult<CommonResponse<StateDto>>> Get(Guid id, Guid countryid)
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                if (UserId != Guid.Empty)
                {
                    return Ok(await _stateService.GetByIdAsync(id, CountryId));
                }
            }

            var result = await _stateService.GetByIdAsync(id, countryid);
            return Ok(result);
        }

        /// <summary>
        /// Updates the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="request">The request.</param>
        /// <returns>result.</returns>
        [Authorize(Roles = "Admin,SuperAdmin,CountryAdmin")]
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<CommonResponse<StateDto>>> Update(
            Guid id,
            CreateMasterStateRequest request)
        {
            if (CountryId != request.countryid)
            {
                return StatusCode(403, "You are not allowed to create data for this country.");
            }
            var updatedRequest = request with
            {
                userId = UserId
            };
            var updated = await _stateService.UpdateAsync(id, updatedRequest);
            return Ok(updated);
        }

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="countryid">The identifier.</param>
        /// <returns>StateDto by id.</returns>
        [HttpGet("by-country/{countryid:guid}")]
        public async Task<CommonResponse<List<StateDto>>> GetByCountryId(Guid countryid)
        {
            var result = await _stateService.GetByCountryIdAsync(countryid);
            return result;
        }

        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>result.</returns>
        [Authorize(Roles = "Admin,SuperAdmin,CountryAdmin")]
        [HttpDelete("{id:guid}")]
        public async Task<CommonResponse<bool>> Delete(Guid id)
        {
            return await _stateService.DeleteAsync(id, CountryId);
        }

        /// <summary>
        /// Gets the filtered.
        /// Created On: 03/2026
        /// Created By: Aju Antony
        /// Task: Issues-175.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Result.</returns>
        [Authorize(Roles = "Admin,SuperAdmin,CountryAdmin")]
        [HttpGet("filter")]
        public async Task<ActionResult<CommonResponse<PagedResult<StateDto>>>> GetFiltered(
        [FromQuery] PagedRequest request)
        {
            var result = await _stateService.GetFilteredAsync(request, CountryId);
            return Ok(result);
        }
    }
}
