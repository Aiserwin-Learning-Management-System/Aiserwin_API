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
    public class GradeController : BaseController
    {
        private readonly IGradeService _gradeService;
        private readonly ISyllabusService _syllabusService;

        /// <summary>
        /// Initializes a new instance of the <see cref="GradeController"/> class.
        /// </summary>
        /// <param name="gradeService">The state service.</param>
        /// <param name="syllabusService">The syllabus service.</param>
        public GradeController(IGradeService gradeService, ISyllabusService syllabusService)
        {
            _gradeService = gradeService;
            _syllabusService = syllabusService;
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <param name="centerId">The centerId.</param>
        /// <returns>GradeDto list.</returns>
        [HttpGet("{centerId:guid?}")]
        public async Task<ActionResult<CommonResponse<GradeDto>>> GetAll(Guid centerId)
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                if (User.IsInRole("SuperAdmin"))
                {
                    return Ok(await _gradeService.GetAllAsync(Guid.Empty));
                }

                if (UserId != Guid.Empty)
                {
                    var countryIdFromToken = CenterId;
                    return Ok(await _gradeService.GetAllAsync(countryIdFromToken));
                }
            }

            return Ok(await _gradeService.GetAllAsync(centerId));
        }
          //  => Ok(await _gradeService.GetAllAsync());

        /// <summary>
        /// Creates the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>GradeDto.</returns>
        [Authorize(Roles = "Admin,SuperAdmin,CenterAdmin")]
        [HttpPost]
        public async Task<ActionResult<CommonResponse<GradeDto>>> Create(
            GradeRequest request)
        {
            var isSuperAdmin = User.IsInRole("SuperAdmin");
            var syllabus = await _syllabusService.GetByIdAsync(request.syllabusid, isSuperAdmin ? Guid.Empty : CenterId);
            if (syllabus?.Data == null)
            {
                return NotFound("syllabus not found.");
            }

            if (!isSuperAdmin && CenterId != syllabus.Data.CenterId)
            {
                return StatusCode(403, "You are not allowed to create data for this center.");
            }

            var updatedRequest = request with
            {
                userId = UserId
            };

            var created = await _gradeService.CreateAsync(updatedRequest);
            return Ok(created);
        }

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="centerid">The centerid.</param>
        /// <returns>GradeDto by id.</returns>
        [HttpGet("{id:guid}/center/{centerid:guid?}")]
        public async Task<ActionResult<CommonResponse<GradeDto>>> Get(Guid id, Guid centerid)
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                if (User.IsInRole("SuperAdmin"))
                {
                    return Ok(await _gradeService.GetByIdAsync(id));
                }

                if (UserId != Guid.Empty)
                {
                    return Ok(await _gradeService.GetByIdCenterIdAsync(id, CenterId));
                }
            }

            return Ok(await _gradeService.GetByIdCenterIdAsync(id, centerid));
        }

           //=> Ok(await _gradeService.GetByIdAsync(id));

        /// <summary>
        /// Updates the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="request">The request.</param>
        /// <returns>result.</returns>
        [Authorize(Roles = "Admin,SuperAdmin,CenterAdmin")]
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<CommonResponse<GradeDto>>> Update(
            Guid id,
            GradeRequest request)
        {
            var isSuperAdmin = User.IsInRole("SuperAdmin");
            var syllabus = await _syllabusService.GetByIdAsync(request.syllabusid, isSuperAdmin ? Guid.Empty : CenterId);
            if (syllabus?.Data == null)
            {
                return NotFound("syllabus not found.");
            }

            if (!isSuperAdmin && CenterId != syllabus.Data.CenterId)
            {
                return StatusCode(403, "You are not allowed to create data for this center.");
            }

            var updatedRequest = request with
            {
                userId = UserId
            };
            var updated = await _gradeService.UpdateAsync(id, updatedRequest);
            return Ok(updated);
        }

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="syllabusid">The identifier.</param>
        /// <returns>GradeDto by id.</returns>
        [HttpGet("by-syllabus/{syllabusid:guid}")]
        public async Task<ActionResult<CommonResponse<List<GradeDto>>>> GetBySyllabusId(Guid syllabusid)
        {
            var result = await _gradeService.GetBySyllabusIdAsync(syllabusid);
            return Ok(result);
        }

        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>result.</returns>
        [Authorize(Roles = "Admin,SuperAdmin,CenterAdmin")]
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<CommonResponse<bool>>> Delete(Guid id)
        {
            var isSuperAdmin = User.IsInRole("SuperAdmin");
            if (isSuperAdmin)
            {
                return await _gradeService.DeleteAsync(id, Guid.Empty);
            }

            var result = await _gradeService.DeleteAsync(id, CenterId);
            return Ok(result);
        }

        /// <summary>
        /// Gets the filtered.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>result.</returns>
        [Authorize(Roles = "Admin,SuperAdmin,CenterAdmin")]
        [HttpGet("filter")]
        public async Task<ActionResult<CommonResponse<PagedResult<GradeDto>>>> GetFiltered(
        [FromQuery] PagedRequest request)
        {
            var isSuperAdmin = User.IsInRole("SuperAdmin");
            if (isSuperAdmin)
            {
                return Ok(await _gradeService.GetFilteredAsync(request, Guid.Empty));
            }

            var result = await _gradeService.GetFilteredAsync(request, CenterId);
            return Ok(result);
        }
    }
}
