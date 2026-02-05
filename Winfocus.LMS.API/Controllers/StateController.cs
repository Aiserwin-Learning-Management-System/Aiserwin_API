using Microsoft.AspNetCore.Mvc;
using Winfocus.LMS.Application.Interfaces;
using Winfocus.LMS.Application.Services;

namespace Winfocus.LMS.API.Controllers
{
    /// <summary>
    /// Handles authentication endpoints.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public sealed class StateController : ControllerBase
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
        /// get the states by country.
        /// </summary>
        /// <param name="countryid">The request.</param>
        /// <returns>token.</returns>
        [HttpPost("GetByCountryId")]
        public async Task<IActionResult> GetByCountryId(Guid countryid)
        {
            var result = await _stateService.GetByCountryIdAsync(countryid);
            return Ok(result);
        }
    }
}
