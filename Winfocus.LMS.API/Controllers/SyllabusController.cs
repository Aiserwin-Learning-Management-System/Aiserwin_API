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
        /// <param name="centerId">The centerId.</param>
        /// <returns>SyllabusDto list.</returns>
        [HttpGet("{centerId:guid?}")]
        public async Task<ActionResult<CommonResponse<SyllabusDto>>> GetAll(Guid centerId)
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                if (User.IsInRole("SuperAdmin"))
                {
                    return Ok(await _syllabusService.GetByCenterIdAsync(Guid.Empty));
                }

                if (UserId != Guid.Empty)
                {
                    var centerIdFromToken = CenterId;
                    return Ok(await _syllabusService.GetByCenterIdAsync(centerIdFromToken));
                }
            }

            return Ok(await _syllabusService.GetByCenterIdAsync(centerId));
        }

        //=> Ok(await _syllabusService.GetAllAsync());

        /// <summary>
        /// Creates the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>SyllabusDto.</returns>
        [Authorize(Roles = "Admin,SuperAdmin,CenterAdmin")]
        [HttpPost]
        public async Task<ActionResult<CommonResponse<SyllabusDto>>> Create(
            SyllabusRequest request)
        {
            var isSuperAdmin = User.IsInRole("SuperAdmin");
            if (!isSuperAdmin && CenterId != request.CenterId)
            {
                return StatusCode(403, "You are not allowed to create data for this center.");
            }

            var updatedRequest = request with
            {
                UserId = UserId
            };
            var created = await _syllabusService.CreateAsync(updatedRequest);
            return Ok(created);
        }

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="centerid">centerid.</param>
        /// <returns>SyllabusDto by id.</returns>
        [HttpGet("{id:guid}/center/{centerid:guid?}")]
        public async Task<ActionResult<CommonResponse<SyllabusDto>>> Get(Guid id, Guid centerid)
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                if (User.IsInRole("SuperAdmin"))
                {
                    return Ok(await _syllabusService.GetByIdAsync(id, Guid.Empty));
                }

                if (UserId != Guid.Empty)
                {
                    return Ok(await _syllabusService.GetByIdAsync(id, CenterId));
                }
            }

            var result = await _syllabusService.GetByIdAsync(id, centerid);
            return Ok(result);
        }

        /// <summary>
        /// Updates the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="request">The request.</param>
        /// <returns>result.</returns>
        [Authorize(Roles = "Admin,SuperAdmin,CenterAdmin")]
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<CommonResponse<SyllabusDto>>> Update(
            Guid id,
            SyllabusRequest request)
        {
            var isSuperAdmin = User.IsInRole("SuperAdmin");
            if (!isSuperAdmin && CenterId != request.CenterId)
            {
                return StatusCode(403, "You are not allowed to create data for this center.");
            }

            var updatedRequest = request with
            {
                UserId = UserId
            };
            var updated = await _syllabusService.UpdateAsync(id, updatedRequest);
            return Ok(updated);
        }

        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>result.</returns>
        [Authorize(Roles = "Admin,SuperAdmin, CenterAdmin")]
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<CommonResponse<bool>>> Delete(Guid id)
        {
            var response = await _syllabusService.DeleteAsync(id, CenterId);
            return Ok(response);
        }

        /// <summary>
        /// Retrieves filtered syllabuses with pagination.
        /// </summary>
        /// <param name="request">The paged request.</param>
        /// <returns>Paginated list of syllabuses.</returns>
        [Authorize(Roles = "Admin,SuperAdmin, CenterAdmin")]
        [HttpGet("filter")]
        public async Task<ActionResult<CommonResponse<PagedResult<SyllabusDto>>>> GetFiltered(
            [FromQuery] PagedRequest request)
        {
            var result = await _syllabusService.GetFilteredAsync(request, CenterId);
            return Ok(result);
        }

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="centerId">The identifier.</param>
        /// <returns>GradeDto by id.</returns>
        [HttpGet("by-center/{centerid:guid}")]
        public async Task<ActionResult<CommonResponse<List<SyllabusDto>>>> GetBySyllabusId(Guid centerId)
        {
            var result = await _syllabusService.GetByCenterIdAsync(centerId);
            return Ok(result);
        }

    }
}
