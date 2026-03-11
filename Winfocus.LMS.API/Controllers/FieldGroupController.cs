using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Winfocus.LMS.Application.DTOs;
using Winfocus.LMS.Application.DTOs.Common;
using Winfocus.LMS.Application.Interfaces;

namespace Winfocus.LMS.API.Controllers
{
    /// <summary>
    /// FieldGroupController.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class FieldGroupController : BaseController
    {
        private readonly IFieldGroupServices _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldGroupController"/> class.
        /// </summary>
        /// <param name="service">The service.</param>
        public FieldGroupController(IFieldGroupServices service)
        {
            _service = service;
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns>FieldGroupDto list.</returns>
        [HttpGet]
        public async Task<ActionResult<CommonResponse<FieldGroupDto>>> GetAll()
            => Ok(await _service.GetAllAsync());

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>FieldGroupDto by id.</returns>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<CommonResponse<FieldGroupDto>>> Get(Guid id)
         => Ok(await _service.GetByIdAsync(id));

        /// <summary>
        /// Creates the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>FieldGroupDto.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost]
        public async Task<ActionResult<CommonResponse<FieldGroupDto>>> Create(
            CreateFieldGroupRequest request)
        {
            Guid userid = UserId;
            var created = await _service.CreateAsync(request, userid);
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
        public async Task<ActionResult<CommonResponse<FieldGroupDto>>> Update(
            Guid id,
            CreateFieldGroupRequest request)
        {
            Guid userid = UserId;
            var updated = await _service.UpdateAsync(id, request, userid);
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
            var result = await _service.DeleteAsync(id);
            return Ok(result);
        }

        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>result.</returns>
        [HttpGet("{id:guid}/fields")]
        public async Task<IActionResult> GetFieldsByGroupId(Guid id)
        {
            var result = await _service.GetFieldsByGroupIdAsync(id);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves filtered FieldGroup with pagination.
        /// </summary>
        /// <param name="request">The paged request.</param>
        /// <returns>Paginated list of FieldGroup.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet("filter")]
        public async Task<ActionResult<CommonResponse<PagedResult<FieldGroupDto>>>> GetFiltered(
            [FromQuery] PagedRequest request)
        {
            var result = await _service.GetFilteredAsync(request);
            return Ok(result);
        }
    }
}
