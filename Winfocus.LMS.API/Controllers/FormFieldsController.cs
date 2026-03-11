using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Winfocus.LMS.Application.DTOs;
using Winfocus.LMS.Application.Interfaces;

namespace Winfocus.LMS.API.Controllers
{
    /// <summary>
    /// API controller responsible for managing dynamic form fields.
    /// Supports creation, retrieval, update, deletion, grouping, and metadata operations.
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class FormFieldsController : BaseController
    {
        private readonly IFormFieldService _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="FormFieldsController"/> class.
        /// </summary>
        /// <param name="service">Service responsible for form field business logic.</param>
        public FormFieldsController(IFormFieldService service)
        {
            _service = service;
        }

        /// <summary>
        /// Creates a new dynamic form field.
        /// The field can optionally belong to a field group.
        /// Dropdown, radio, and checkbox fields may include selectable options.
        /// </summary>
        /// <param name="dto">The form field creation request.</param>
        /// <returns>The created form field with group and option details.</returns>
        /// <response code="200">Form field created successfully.</response>
        /// <response code="400">Invalid request data or group identifier.</response>
        [HttpPost]
        public async Task<IActionResult> Create(CreateFormFieldDto dto)
        {
            var result = await _service.CreateAsync(dto);

            return Ok(result);
        }

        /// <summary>
        /// Retrieves all dynamic form fields.
        /// Includes both grouped and standalone fields.
        /// </summary>
        /// <returns>A list of form fields with group information.</returns>
        /// <response code="200">Returns the list of form fields.</response>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();

            return Ok(result);
        }

        /// <summary>
        /// Retrieves a specific form field by its identifier.
        /// Includes selectable options if applicable.
        /// </summary>
        /// <param name="id">The unique identifier of the form field.</param>
        /// <returns>The form field details.</returns>
        /// <response code="200">Form field retrieved successfully.</response>
        /// <response code="404">Form field not found.</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await _service.GetByIdAsync(id);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        /// <summary>
        /// Updates an existing form field.
        /// Allows modifying validation rules, field group, and selectable options.
        /// </summary>
        /// <param name="id">The unique identifier of the form field.</param>
        /// <param name="dto">The updated field information.</param>
        /// <returns>The updated form field.</returns>
        /// <response code="200">Form field updated successfully.</response>
        /// <response code="404">Form field not found.</response>
        /// <response code="400">Invalid update request.</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateFormFieldDto dto)
        {
            var result = await _service.UpdateAsync(id, dto);

            return Ok(result);
        }

        /// <summary>
        /// Soft deletes a form field.
        /// The record remains in the database but is marked as deleted.
        /// </summary>
        /// <param name="id">The identifier of the form field.</param>
        /// <returns>No content if deletion is successful.</returns>
        /// <response code="204">Form field deleted successfully.</response>
        /// <response code="404">Form field not found.</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.DeleteAsync(id);

            return NoContent();
        }

        /// <summary>
        /// Moves a form field to a different group or removes it from a group.
        /// </summary>
        /// <param name="id">The identifier of the form field.</param>
        /// <param name="dto">The group assignment information.</param>
        /// <returns>No content if the move operation succeeds.</returns>
        /// <response code="204">Field moved successfully.</response>
        /// <response code="400">Invalid group identifier.</response>
        /// <response code="404">Form field not found.</response>
        [HttpPatch("{id}/group")]
        public async Task<IActionResult> MoveGroup(Guid id, MoveFieldToGroupDto dto)
        {
            await _service.MoveFieldAsync(id, dto);

            return NoContent();
        }

        /// <summary>
        /// Retrieves all standalone form fields that are not assigned to any group.
        /// </summary>
        /// <returns>A list of ungrouped form fields.</returns>
        /// <response code="200">Returns standalone fields.</response>
        [HttpGet("ungrouped")]
        public async Task<IActionResult> GetUngrouped()
        {
            var result = await _service.GetUngroupedAsync();

            return Ok(result);
        }

        /// <summary>
        /// Retrieves all supported form field types.
        /// Used by frontend applications to build dynamic forms.
        /// </summary>
        /// <returns>A list of field type enumeration values.</returns>
        /// <response code="200">Returns all available field types.</response>
        [HttpGet("types")]
        public async Task<IActionResult> GetTypes()
        {
            var result = await _service.GetFieldTypesAsync();

            return Ok(result);
        }
    }
}
