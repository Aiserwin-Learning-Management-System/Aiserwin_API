using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Winfocus.LMS.Application.DTOs;
using Winfocus.LMS.Application.DTOs.Common;
using Winfocus.LMS.Application.DTOs.Masters;
using Winfocus.LMS.Application.Interfaces;
using Winfocus.LMS.Domain.Entities;

namespace Winfocus.LMS.API.Controllers
{
    /// <summary>
    /// Handles authentication endpoints.
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class StaffCategoryController : BaseController
    {
        private readonly IStaffCategoryService _staffcatgeoryService;

        /// <summary>
        /// Initializes a new instance of the <see cref="StaffCategoryController"/> class.
        /// </summary>
        /// <param name="staffcategoryService">The staff category service.</param>
        public StaffCategoryController(IStaffCategoryService staffcategoryService)
        {
            _staffcatgeoryService = staffcategoryService;
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns>staff category list.</returns>
        [HttpGet]
        public async Task<ActionResult<CommonResponse<StaffCategoryDto>>> GetAll()
            => Ok(await _staffcatgeoryService.GetAllAsync());

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Staff by id.</returns>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<CommonResponse<StaffCategoryDto>>> Get(Guid id)
           => Ok(await _staffcatgeoryService.GetByIdAsync(id));

        /// <summary>
        /// Creates the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>StaffCategoryDto.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost]
        public async Task<ActionResult<CommonResponse<StaffCategoryDto>>> Create(
            StaffCategoryRequestDto request)
        {
            var updatedRequest = request with
            {
                userId = UserId
            };
            var created = await _staffcatgeoryService.CreateAsync(updatedRequest);
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
        public async Task<ActionResult<CommonResponse<StaffCategoryDto>>> Update(
            Guid id,
            StaffCategoryRequestDto request)
        {
            var updatedRequest = request with
            {
                userId = UserId
            };
            var updated = await _staffcatgeoryService.UpdateAsync(id, updatedRequest);
            return Ok(updated);
        }

        /// <summary>s
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>result.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<CommonResponse<bool>>> Delete(Guid id)
        {
            var result = await _staffcatgeoryService.DeleteAsync(id);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves filtered staff Category with pagination.
        /// </summary>
        /// <param name="request">The paged request.</param>
        /// <returns>Paginated list of staff category.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet("filter")]
        public async Task<ActionResult<CommonResponse<PagedResult<StaffCategoryDto>>>> GetFiltered(
            [FromQuery] PagedRequest request)
        {
            var result = await _staffcatgeoryService.GetFilteredAsync(request);
            return Ok(result);
        }
    }
}
