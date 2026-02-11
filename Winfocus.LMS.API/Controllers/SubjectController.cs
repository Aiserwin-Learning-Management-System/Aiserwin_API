namespace Winfocus.LMS.API.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Winfocus.LMS.Application.DTOs;
    using Winfocus.LMS.Application.DTOs.Masters;
    using Winfocus.LMS.Application.Interfaces;

    /// <summary>
    /// SubjectController.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [ApiController]
    [Route("api/[controller]")]
    public class SubjectController : ControllerBase
    {
        private readonly ISubjectService _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="SubjectController"/> class.
        /// </summary>
        /// <param name="service">The service.</param>
        public SubjectController(ISubjectService service)
        {
            _service = service;
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns>SubjectDto list.</returns>
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<SubjectDto>>> GetAll()
            => Ok(await _service.GetAllAsync());

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>SubjectDto list.</returns>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<IReadOnlyList<SubjectDto>>> Get(Guid id)
            => Ok(await _service.GetByIdAsync(id));

        /// <summary>
        /// Gets the by stream.
        /// </summary>
        /// <param name="streamId">The stream identifier.</param>
        /// <returns>SubjectDto list.</returns>
        [HttpGet("stream/{streamId:guid}")]
        public async Task<ActionResult<IReadOnlyList<SubjectDto>>> GetByStream(Guid streamId)
            => Ok(await _service.GetByStreamAsync(streamId));

        /// <summary>
        /// Creates the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>SubjectDto.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost]
        public async Task<ActionResult<SubjectDto>> Create(SubjectRequest request)
            => Ok(await _service.CreateAsync(request));

        /// <summary>
        /// Updates the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="request">The request.</param>
        /// <returns>result.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, SubjectRequest request)
        {
            await _service.UpdateAsync(id, request);
            return NoContent();
        }

        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>result.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
