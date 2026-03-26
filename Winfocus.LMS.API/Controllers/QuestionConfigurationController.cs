using System;
using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Winfocus.LMS.Application.DTOs;
using Winfocus.LMS.Application.DTOs.Common;
using Winfocus.LMS.Application.DTOs.QuestionConfig;
using Winfocus.LMS.Application.Interfaces;

namespace Winfocus.LMS.API.Controllers
{
    /// <summary>
    /// Handles Question ID Configuration operations.
    /// Admin selects academic hierarchy, gets auto-suggested code, and saves configurations.
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class QuestionConfigurationController : BaseController
    {
        private readonly IQuestionConfigurationService _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="QuestionConfigurationController"/> class.
        /// </summary>
        /// <param name="service">The question configuration service.</param>
        public QuestionConfigurationController(IQuestionConfigurationService service)
        {
            _service = service;
        }

        /// <summary>
        /// Generates a suggested Question Code based on the selected hierarchy.
        /// Does NOT save anything to the database.
        /// </summary>
        /// <param name="dto">The hierarchy selection containing 7 dropdown values.</param>
        /// <returns>The suggested Question Code and next sequence number.</returns>
        [HttpPost("suggest")]
        public async Task<ActionResult<CommonResponse<SuggestedCodeResponseDto>>> SuggestCode(
            [FromBody] SuggestQuestionCodeDto dto)
        {
            CommonResponse<SuggestedCodeResponseDto> result = await _service.SuggestCodeAsync(dto);
            return Ok(result);
        }

        /// <summary>
        /// Creates a new Question Configuration with the given Question Code.
        /// Validates all 8 FK IDs and ensures the code is unique.
        /// </summary>
        /// <param name="dto">The creation data including hierarchy IDs and Question Code.</param>
        /// <returns>The created configuration with resolved master names.</returns>
        [HttpPost]
        public async Task<ActionResult<CommonResponse<QuestionConfigurationDto>>> Create(
            [FromBody] CreateQuestionConfigurationDto dto)
        {
            CommonResponse<QuestionConfigurationDto> result = await _service.CreateAsync(dto, UserId);
            return Ok(result);
        }

        /// <summary>
        /// Checks whether a given Question Code is available (not already in use).
        /// Used for real-time uniqueness validation as admin types in the input field.
        /// </summary>
        /// <param name="code">The Question Code to check.</param>
        /// <returns>Availability status.</returns>
        [HttpGet("check-unique/{code}")]
        public async Task<ActionResult<CommonResponse<CodeAvailabilityDto>>> CheckUnique(
            [FromRoute] string code)
        {
            CommonResponse<CodeAvailabilityDto> result = await _service.CheckUniqueAsync(code);
            return Ok(result);
        }

        /// <summary>
        /// Gets a Question Configuration by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier.</param>
        /// <returns>The configuration with resolved master names.</returns>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<CommonResponse<QuestionConfigurationDto>>> Get(
            [FromRoute] Guid id)
        {
            CommonResponse<QuestionConfigurationDto> result = await _service.GetByIdAsync(id);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves filtered Question Configurations with pagination.
        /// Supports filtering by all master fields, search, date range, and sorting.
        /// </summary>
        /// <param name="request">The filter and pagination parameters.</param>
        /// <returns>Paginated list of Question Configurations.</returns>
        [HttpGet("filter")]
        public async Task<ActionResult<CommonResponse<PagedResult<QuestionConfigurationDto>>>> GetFiltered(
            [FromQuery] QuestionConfigurationFilterRequest request)
        {
            CommonResponse<PagedResult<QuestionConfigurationDto>> result =
                await _service.GetFilteredAsync(request);
            return Ok(result);
        }

        /// <summary>
        /// Soft deletes a Question Configuration by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the record to delete.</param>
        /// <returns>Success status.</returns>
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<CommonResponse<bool>>> Delete(
            [FromRoute] Guid id)
        {
            CommonResponse<bool> result = await _service.DeleteAsync(id, UserId);
            return Ok(result);
        }
    }
}
