using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Winfocus.LMS.Application.DTOs;
using Winfocus.LMS.Application.Interfaces;

namespace Winfocus.LMS.API.Controllers
{
    /// <summary>
    /// API controller responsible for managing registration forms.
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class RegistrationFormsController : BaseController
    {
        /// <summary>
        /// Service used to handle registration form business logic.
        /// </summary>
        private readonly IRegistrationFormService _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistrationFormsController"/> class.
        /// </summary>
        /// <param name="service">
        /// Service responsible for managing registration forms.
        /// </param>
        public RegistrationFormsController(IRegistrationFormService service)
        {
            _service = service;
        }

        /// <summary>
        /// Creates a new registration form.
        /// </summary>
        /// <param name="dto">
        /// Data transfer object containing form details,
        /// selected field groups, and standalone fields.
        /// </param>
        /// <returns>
        /// Returns the identifier of the newly created form.
        /// </returns>
        /// <response code="201">Form created successfully.</response>
        /// <response code="400">Invalid request data.</response>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateRegistrationFormDto dto)
        {
            var id = await _service.CreateAsync(dto);

            return CreatedAtAction(nameof(GetById), new { id }, id);
        }

        /// <summary>
        /// Retrieves all registration forms.
        /// </summary>
        /// <returns>
        /// A list of registration form summaries.
        /// </returns>
        /// <response code="200">Forms retrieved successfully.</response>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var forms = await _service.GetAllAsync();
            return Ok(forms);
        }

        /// <summary>
        /// Retrieves a specific registration form including its groups and fields.
        /// </summary>
        /// <param name="id">
        /// Unique identifier of the registration form.
        /// </param>
        /// <returns>
        /// Detailed form structure including grouped and standalone fields.
        /// </returns>
        /// <response code="200">Form retrieved successfully.</response>
        /// <response code="404">Form not found.</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var form = await _service.GetByIdAsync(id);
            return Ok(form);
        }

        /// <summary>
        /// Updates an existing registration form.
        /// </summary>
        /// <param name="id">
        /// Identifier of the form to update.
        /// </param>
        /// <param name="dto">
        /// Updated form configuration including groups and standalone fields.
        /// </param>
        /// <returns>
        /// No content if update is successful.
        /// </returns>
        /// <response code="204">Form updated successfully.</response>
        /// <response code="404">Form not found.</response>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, CreateRegistrationFormDto dto)
        {
            await _service.UpdateAsync(id, dto);

            return NoContent();
        }

        /// <summary>
        /// Performs a soft delete of a registration form.
        /// </summary>
        /// <param name="id">
        /// Identifier of the form to delete.
        /// </param>
        /// <returns>
        /// No content if deletion is successful.
        /// </returns>
        /// <response code="204">Form deleted successfully.</response>
        /// <response code="404">Form not found.</response>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.DeleteAsync(id);

            return NoContent();
        }
    }
}
