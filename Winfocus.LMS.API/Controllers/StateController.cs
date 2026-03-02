using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Winfocus.LMS.Application.DTOs;
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
        /// <returns>StateDto list.</returns>
        [HttpGet]
        public async Task<ActionResult<CommonResponse<List<StateDto>>>> GetAll()
            => Ok(await _stateService.GetAllAsync());

        /// <summary>
        /// Creates the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>StateDto.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost]
        public async Task<ActionResult<CommonResponse<StateDto>>> Create(
            CreateMasterStateRequest request)
        {
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
        /// <returns>StateDto by id.</returns>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<CommonResponse<StateDto>>> Get(Guid id)
        {
            var result = await _stateService.GetByIdAsync(id);
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
        public async Task<ActionResult<CommonResponse<StateDto>>> Update(
            Guid id,
            CreateMasterStateRequest request)
        {
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
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpDelete("{id:guid}")]
        public async Task<CommonResponse<bool>> Delete(Guid id)
        {
            return await _stateService.DeleteAsync(id);
        }
    }
}
