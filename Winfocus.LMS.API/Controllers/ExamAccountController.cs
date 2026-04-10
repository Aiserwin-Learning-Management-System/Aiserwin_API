using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Winfocus.LMS.Application.DTOs.Common;
using Winfocus.LMS.Application.Interfaces;
using Winfocus.LMS.Application.DTOs;

namespace Winfocus.LMS.API.Controllers
{
    /// <summary>
    /// Controller responsible for managing ExamAccount resources.
    /// Provides CRUD and filtering endpoints.
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ExamAccountController : BaseController
    {
        private readonly IExamAccountService _service;
        private readonly ILogger<ExamAccountController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExamAccountController"/> class.
        /// </summary>
        /// <param name="service">Service for exam account operations.</param>
        /// <param name="logger">Logger instance.</param>
        public ExamAccountController(IExamAccountService service, ILogger<ExamAccountController> logger)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves all exam accounts.
        /// </summary>
        /// <returns>CommonResponse containing list of <see cref="ExamAccountDto"/>.</returns>
        [HttpGet]
        public async Task<ActionResult<CommonResponse<List<ExamAccountDto>>>> GetAll()
        {
            try
            {
                // Return all exam accounts
                return Ok(await _service.GetAllAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAll for ExamAccount");
                return StatusCode(500, CommonResponse<List<ExamAccountDto>>.FailureResponse($"An error occurred: {ex.Message}"));
            }
        }

        /// <summary>
        /// Creates a new exam account.
        /// </summary>
        /// <param name="request">The exam account create request.</param>
        /// <returns>Created <see cref="ExamAccountDto"/> wrapped in CommonResponse.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost]
        public async Task<ActionResult<CommonResponse<ExamAccountDto>>> Create(ExamAccountRequest request)
        {
            try
            {
                var updated = request with { userId = UserId };
                var created = await _service.CreateAsync(updated);
                return Ok(created);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Create for ExamAccount");
                return StatusCode(500, CommonResponse<ExamAccountDto>.FailureResponse($"An error occurred: {ex.Message}"));
            }
        }

        /// <summary>
        /// Gets an exam account by id.
        /// </summary>
        /// <param name="id">Exam account id.</param>
        /// <returns>ExamAccountDto wrapped in CommonResponse.</returns>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<CommonResponse<ExamAccountDto>>> Get(Guid id)
        {
            try
            {
                // No center id validation required
                var result = await _service.GetByIdAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Get for ExamAccount {Id}", id);
                return StatusCode(500, CommonResponse<ExamAccountDto>.FailureResponse($"An error occurred: {ex.Message}"));
            }
        }

        /// <summary>
        /// Updates an existing exam account.
        /// </summary>
        /// <param name="id">Exam account id.</param>
        /// <param name="request">Update request.</param>
        /// <returns>Updated ExamAccountDto wrapped in CommonResponse.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<CommonResponse<ExamAccountDto>>> Update(Guid id, ExamAccountRequest request)
        {
            try
            {
                var updated = request with { userId = UserId };
                var res = await _service.UpdateAsync(id, updated);
                return Ok(res);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Update for ExamAccount {Id}", id);
                return StatusCode(500, CommonResponse<ExamAccountDto>.FailureResponse($"An error occurred: {ex.Message}"));
            }
        }

        /// <summary>
        /// Deletes (soft) the specified exam account.
        /// </summary>
        /// <param name="id">Exam account id.</param>
        /// <returns>Boolean result wrapped in CommonResponse.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<CommonResponse<bool>>> Delete(Guid id)
        {
            try
            {
                // No center id validation required
                var result = await _service.DeleteAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Delete for ExamAccount {Id}", id);
                return StatusCode(500, CommonResponse<bool>.FailureResponse($"An error occurred: {ex.Message}"));
            }
        }

        /// <summary>
        /// Retrieves filtered and paginated exam accounts.
        /// </summary>
        /// <param name="request">Paged request containing filters, sort and pagination info.</param>
        /// <returns>PagedResult of <see cref="ExamAccountDto"/> wrapped in CommonResponse.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet("filter")]
        public async Task<ActionResult<CommonResponse<PagedResult<ExamAccountDto>>>> GetFiltered([FromQuery] PagedRequest request)
        {
            try
            {
                // No center id validation required
                var result = await _service.GetFilteredAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetFiltered for ExamAccount");
                return StatusCode(500, CommonResponse<PagedResult<ExamAccountDto>>.FailureResponse($"An error occurred: {ex.Message}"));
            }
        }
    }
}
