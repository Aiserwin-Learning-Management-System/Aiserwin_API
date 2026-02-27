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
    public class AcademicYearController : BaseController
    {
        private readonly IAcademicYearService _academiyearService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AcademicYearController"/> class.
        /// </summary>
        /// <param name="academiyearService">The academic year service.</param>
        public AcademicYearController(IAcademicYearService academiyearService)
        {
            _academiyearService = academiyearService;
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns>academic year list.</returns>
        [HttpGet]
        public async Task<ActionResult<CommonResponse<AcademicYearDto>>> GetAll()
            => Ok(await _academiyearService.GetAllAsync());

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>AcademicYearDto by id.</returns>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<CommonResponse<AcademicYearDto>>> Get(Guid id)
           => Ok(await _academiyearService.GetByIdAsync(id));

        /// <summary>
        /// Creates the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>AcademicYearDto.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost]
        public async Task<ActionResult<CommonResponse<AcademicYearDto>>> Create(
            AcademicYearRequest request)
        {
            var updatedRequest = request with
            {
                userid = UserId
            };
            var created = await _academiyearService.CreateAsync(updatedRequest);
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
        public async Task<ActionResult<CommonResponse<AcademicYearDto>>> Update(
            Guid id,
            AcademicYearRequest request)
        {
            var updatedRequest = request with
            {
                userid = UserId
            };
            var updated = await _academiyearService.UpdateAsync(id, updatedRequest);
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
            var result = await _academiyearService.DeleteAsync(id);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves filtered academic year with pagination.
        /// </summary>
        /// <param name="request">The paged request.</param>
        /// <returns>Paginated list of academic year.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet("filter")]
        public async Task<ActionResult<CommonResponse<PagedResult<AcademicYearDto>>>> GetFiltered(
            [FromQuery] PagedRequest request)
        {
            var result = await _academiyearService.GetFilteredAsync(request);
            return Ok(result);
        }
    }
}
