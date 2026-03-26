namespace Winfocus.LMS.API.Controllers
{
    using Asp.Versioning;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Winfocus.LMS.Application.DTOs;
    using Winfocus.LMS.Application.DTOs.Common;
    using Winfocus.LMS.Application.DTOs.QuestionTypeConfig;
    using Winfocus.LMS.Application.Interfaces;

    /// <summary>
    /// Handles Question Type Configuration CRUD operations.
    /// Admin defines question types (MCQ, Descriptive, etc.) mapped to academic hierarchy.
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize(Roles = "SuperAdmin,CountryAdmin,CenterAdmin,Admin")]
    public class QuestionTypeConfigController : BaseController
    {
        private readonly IQuestionTypeConfigService _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="QuestionTypeConfigController"/> class.
        /// </summary>
        /// <param name="service">The question type config service.</param>
        public QuestionTypeConfigController(IQuestionTypeConfigService service)
        {
            _service = service;
        }

        /// <summary>
        /// Creates a single Question Type Configuration.
        /// </summary>
        /// <param name="dto">The creation data with hierarchy IDs and question type name.</param>
        /// <returns>The created configuration.</returns>
        [HttpPost]
        public async Task<ActionResult<CommonResponse<QuestionTypeConfigDto>>> Create(
            [FromBody] CreateQuestionTypeConfigDto dto)
        {
            CommonResponse<QuestionTypeConfigDto> result = await _service.CreateAsync(dto, UserId);
            return Ok(result);
        }

        /// <summary>
        /// Creates multiple Question Type Configurations in a single request (Add More).
        /// </summary>
        /// <param name="dto">The bulk creation data.</param>
        /// <returns>List of created configurations.</returns>
        [HttpPost("bulk")]
        public async Task<ActionResult<CommonResponse<List<QuestionTypeConfigDto>>>> BulkCreate(
            [FromBody] BulkCreateQuestionTypeConfigDto dto)
        {
            CommonResponse<List<QuestionTypeConfigDto>> result =
                await _service.BulkCreateAsync(dto, UserId);
            return Ok(result);
        }

        /// <summary>
        /// Gets a Question Type Configuration by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier.</param>
        /// <returns>The configuration with resolved master names.</returns>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<CommonResponse<QuestionTypeConfigDto>>> Get(
            [FromRoute] Guid id)
        {
            CommonResponse<QuestionTypeConfigDto> result = await _service.GetByIdAsync(id);
            return Ok(result);
        }

        /// <summary>
        /// Updates an existing Question Type Configuration.
        /// </summary>
        /// <param name="id">The identifier to update.</param>
        /// <param name="dto">The updated data.</param>
        /// <returns>The updated configuration.</returns>
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<CommonResponse<QuestionTypeConfigDto>>> Update(
            [FromRoute] Guid id,
            [FromBody] CreateQuestionTypeConfigDto dto)
        {
            CommonResponse<QuestionTypeConfigDto> result =
                await _service.UpdateAsync(id, dto, UserId);
            return Ok(result);
        }

        /// <summary>
        /// Soft deletes a Question Type Configuration.
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

        /// <summary>
        /// Retrieves filtered Question Type Configurations with pagination.
        /// Supports filtering by hierarchy fields, search, date range, and sorting.
        /// </summary>
        /// <param name="request">The filter and pagination parameters.</param>
        /// <returns>Paginated list of question type configurations.</returns>
        [HttpGet("filter")]
        public async Task<ActionResult<CommonResponse<PagedResult<QuestionTypeConfigDto>>>> GetFiltered(
            [FromQuery] QuestionTypeConfigFilterRequest request)
        {
            CommonResponse<PagedResult<QuestionTypeConfigDto>> result =
                await _service.GetFilteredAsync(request);
            return Ok(result);
        }

        /// <summary>
        /// Gets question types available for a specific hierarchy combination.
        /// Used as a dropdown data source in task assignment and question ID configuration pages.
        /// </summary>
        /// <param name="query">The hierarchy combination to query.</param>
        /// <returns>List of question types for the given hierarchy.</returns>
        [HttpGet("by-hierarchy")]
        public async Task<ActionResult<CommonResponse<List<QuestionTypeConfigDto>>>> GetByHierarchy(
            [FromQuery] HierarchyQueryDto query)
        {
            CommonResponse<List<QuestionTypeConfigDto>> result =
                await _service.GetByHierarchyAsync(query);
            return Ok(result);
        }
    }
}
