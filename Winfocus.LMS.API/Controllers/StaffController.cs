using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Asp.Versioning;
using Winfocus.LMS.Application.DTOs.Common;
using Winfocus.LMS.Application.DTOs.DtpAdmin;
using Winfocus.LMS.Application.Interfaces;
using Winfocus.LMS.Application.DTOs;

namespace Winfocus.LMS.API.Controllers
{
    /// <summary>
    /// Handles authentication endpoints.
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/staff")]
    [Authorize(Roles = "SuperAdmin,CountryAdmin,CenterAdmin,Admin,Staff")]
    public class StaffController : ControllerBase
    {
    private readonly ITeachersService _teachersService;

        /// <summary>
        /// Initializes a new instance of the <see cref="StaffController"/> class.
        /// </summary>
        /// <param name="teachersService">The batch service.</param>
    public StaffController(ITeachersService teachersService)
        {
            _teachersService = teachersService;
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <returns>list TeachersByCategoryDto.</returns>
    [HttpGet("teachers-by-category")]
    public async Task<ActionResult<CommonResponse<List<TeachersByCategoryDto>>>> GetTeachersByCategory([FromQuery] string? category)
        {
            var data = await _teachersService.GetTeachersByCategoryAsync(category);
            return Ok(CommonResponse<List<TeachersByCategoryDto>>.SuccessResponse("Teachers loaded.", data));
        }
    }
}
