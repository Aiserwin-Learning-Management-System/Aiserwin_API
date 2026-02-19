namespace Winfocus.LMS.API.Controllers
{
    using Asp.Versioning;
    using Microsoft.AspNetCore.Mvc;
    using Winfocus.LMS.Application.DTOs.Masters;
    using Winfocus.LMS.Application.Interfaces;
    using Winfocus.LMS.Application.Services;

    /// <summary>
    /// Handles authentication endpoints.
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class AcademicYearController : Controller
    {
        private readonly IAcademicYearService _academiyearService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AcademicYearController"/> class.
        /// </summary>
        /// <param name="academiyearService">The academic year service.</param>
        public AcademicYearController(IAcademicYearService academiyearService)
        {
            _academiyearService = academiyearService;
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns>academic year list.</returns>
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<AcademicYearDto>>> GetAll()
            => Ok(await _academiyearService.GetAllAsync());
    }
}
