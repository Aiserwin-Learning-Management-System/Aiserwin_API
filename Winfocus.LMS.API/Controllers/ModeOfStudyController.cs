using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Winfocus.LMS.Application.DTOs;
using Winfocus.LMS.Application.DTOs.Common;
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
    public class ModeOfStudyController : BaseController
    {
        private readonly IModeOfStudyService _modeofstudyService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModeOfStudyController"/> class.
        /// </summary>
        /// <param name="modeofstudyService">The modeofstudy service.</param>
        public ModeOfStudyController(IModeOfStudyService modeofstudyService)
        {
            _modeofstudyService = modeofstudyService;
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns>ModeOfStudyDto list.</returns>
        [HttpGet]
        public async Task<ActionResult<CommonResponse<List<ModeOfStudyDto>>>> GetAll()
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                var userid = UserId;
                if (Guid.Empty != userid)
                {
                    var countryid = CountryId;
                    return Ok(await _modeofstudyService.GetByCountryIdAsync(countryid));
                }
            }

            return Ok(await _modeofstudyService.GetAllAsync());
        }

         // => Ok(await _modeofstudyService.GetAllAsync());

        /// <summary>
        /// Creates the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>ModeOfStudyDto.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost]
        public async Task<ActionResult<CommonResponse<ModeOfStudyDto>>> Create(
            ModeOfStudyRequest request)
        {
            var updatedRequest = request with
            {
                userId = UserId
            };
            var created = await _modeofstudyService.CreateAsync(updatedRequest);
            return Ok(created);
        }

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ModeOfStudyDto by id.</returns>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<CommonResponse<ModeOfStudyDto>>> Get(Guid id)
        {
            var result = await _modeofstudyService.GetByIdAsync(id);
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
        public async Task<ActionResult<CommonResponse<ModeOfStudyDto>>> Update(
            Guid id,
            ModeOfStudyRequest request)
        {
            var updatedRequest = request with
            {
                userId = UserId
            };
            var updated = await _modeofstudyService.UpdateAsync(id, updatedRequest);
            return Ok(updated);
        }

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="countryid">The identifier.</param>
        /// <returns>ModeOfStudyDto by id.</returns>
        [HttpGet("by-country/{countryid:guid}")]
        public async Task<ActionResult<CommonResponse<List<ModeOfStudyDto>>>> GetByCountryId(Guid countryid)
        {
            var result = await _modeofstudyService.GetByCountryIdAsync(countryid);
            return Ok(result);
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
            var result = await _modeofstudyService.DeleteAsync(id);
            return Ok(result);
        }

        /// <summary>
        /// Gets the filtered.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>result.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet("filter")]
        public async Task<ActionResult<CommonResponse<PagedResult<ModeOfStudyDto>>>> GetFiltered(
        [FromQuery] PagedRequest request)
        {
            var result = await _modeofstudyService.GetFilteredAsync(request);
            return Ok(result);
        }
    }
}
