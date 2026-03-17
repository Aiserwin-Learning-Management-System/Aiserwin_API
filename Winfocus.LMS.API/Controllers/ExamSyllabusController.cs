using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Winfocus.LMS.Application.DTOs;
using Winfocus.LMS.Application.DTOs.Common;
using Winfocus.LMS.Application.DTOs.Masters;
using Winfocus.LMS.Application.Interfaces;

namespace Winfocus.LMS.API.Controllers
{
    /// <summary>
    /// Handles authentication endpoints.
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ExamSyllabusController : BaseController
    {
        private readonly IExamSyllabusService _examsyllabusService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExamSyllabusController"/> class.
        /// </summary>
        /// <param name="examsyllabusService">The examsyllabusService service.</param>
        public ExamSyllabusController(IExamSyllabusService examsyllabusService)
        {
            _examsyllabusService = examsyllabusService;
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns>ExamSyllabusDto list.</returns>
        [HttpGet]
        public async Task<ActionResult<CommonResponse<ExamSyllabusDto>>> GetAll()
            => Ok(await _examsyllabusService.GetAllAsync());

        /// <summary>
        /// Creates the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>ExamSyllabusDto.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost]
        public async Task<ActionResult<CommonResponse<ExamSyllabusDto>>> Create(
            ExamSyllabusRequestDto request)
        {
            var updatedRequest = request with
            {
                userid = UserId
            };
            var created = await _examsyllabusService.CreateAsync(updatedRequest);
            return Ok(created);
        }

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="accademicYearID">The identifier.</param>
        /// <returns>ExamSyllabusDto by id.</returns>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<CommonResponse<ExamSyllabusDto>>> Get(Guid id, Guid accademicYearID)
        {
            var result = await _examsyllabusService.GetByIdAsync(id, accademicYearID);
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
        public async Task<ActionResult<CommonResponse<ExamSyllabusDto>>> Update(
            Guid id,
            ExamSyllabusRequestDto request)
        {
            var updatedRequest = request with
            {
                userid = UserId
            };
            var updated = await _examsyllabusService.UpdateAsync(id, updatedRequest);
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
        => Ok(await _examsyllabusService.DeleteAsync(id));

        /// <summary>
        /// Retrieves filtered batches for monday to friday with pagination.
        /// </summary>
        /// <param name="request">The paged request.</param>
        /// <returns>Paginated list of doubt clearing.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet("filter")]
        public async Task<ActionResult<CommonResponse<PagedResult<ExamSyllabusDto>>>> GetFiltered(
            [FromQuery] PagedRequest request)
        {
            var result = await _examsyllabusService.GetFilteredAsync(request);
            return Ok(result);
        }
    }
}
