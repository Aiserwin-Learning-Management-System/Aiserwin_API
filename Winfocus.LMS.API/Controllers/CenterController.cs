using Microsoft.AspNetCore.Mvc;
using Winfocus.LMS.Application.Interfaces;

namespace Winfocus.LMS.API.Controllers
{
    /// <summary>
    /// Handles authentication endpoints.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class CenterController : Controller
    {
        private readonly ICentreService _centerService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CenterController"/> class.
        /// </summary>
        /// <param name="centerService">The center service.</param>
        public CenterController(ICentreService centerService)
        {
            _centerService = centerService;
        }
    }
}
