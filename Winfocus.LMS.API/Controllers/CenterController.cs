namespace Winfocus.LMS.API.Controllers
{
    using Asp.Versioning;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Winfocus.LMS.Application.DTOs;
    using Winfocus.LMS.Application.DTOs.Masters;
    using Winfocus.LMS.Application.Interfaces;

    /// <summary>
    /// Handles authentication endpoints.
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CenterController : BaseController
    {
        private readonly ICentreService _centerService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CenterController"/> class.
        /// </summary>
        /// <param name="centerService">The center service.</param>
        public CenterController(ICentreService centerService)
        {
            _centerService = centerService;
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns>CenterDto list.</returns>
        [HttpGet]
        public async Task<CommonResponse<List<CentreDto>>> GetAll()
        {
            return await _centerService.GetAllAsync();
        }

            //=> Ok(await _centerService.GetAllAsync());

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>CenterDto by id.</returns>
        [HttpGet("{id:guid}")]
        public async Task<CommonResponse<CentreDto>> Get(Guid id)
        {
            return await _centerService.GetByIdAsync(id);
        }

        /// <summary>
        /// Creates the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>CenterDto.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost]
        public async Task<CommonResponse<CentreDto>> Create(
            CenterRequestDto request)
        {
            var updatedRequest = request with
            {
                userId = UserId
            };
            return await _centerService.CreateAsync(updatedRequest);
        }

        /// <summary>
        /// Updates the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="request">The request.</param>
        /// <returns>result.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPut("{id:guid}")]
        public async Task<CommonResponse<CentreDto>> Update(
            Guid id,
            CenterRequestDto request)
        {
            var updatedRequest = request with
            {
                userId = UserId
            };
            return await _centerService.UpdateAsync(id, updatedRequest);
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
            var result = await _centerService.DeleteAsync(id);
            if (result)
            {
                return CommonResponse<bool>.SuccessResponse("Center deleted successfully.", true);
            }
            else
            {
                return CommonResponse<bool>.FailureResponse("Failed to delete Center.");
            }
        }

        /// <summary>
        /// Gets centre by mode of study and state.
        /// </summary>
        /// <param name="modeofid">Mode of study identifier.</param>
        /// <param name="stateid">State identifier.</param>
        /// <returns>CentreDto.</returns>
        [HttpGet("{modeofid:guid}/{stateid:guid}")]
        public async Task<CommonResponse<CentreDto>> Get(Guid modeofid, Guid stateid)
        {
            return await _centerService.GetByFilterAsync(modeofid, stateid);
        }
    }
}
