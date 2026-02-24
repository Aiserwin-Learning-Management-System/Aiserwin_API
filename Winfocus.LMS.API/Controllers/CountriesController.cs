namespace Winfocus.LMS.API.Controllers
{
    using Asp.Versioning;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http.HttpResults;
    using Microsoft.AspNetCore.Mvc;
    using Winfocus.LMS.Application.DTOs;
    using Winfocus.LMS.Application.DTOs.Masters;
    using Winfocus.LMS.Application.Interfaces;

    /// <summary>
    /// CountriesController.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public sealed class CountriesController : ControllerBase
    {
        private readonly ICountryService _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="CountriesController"/> class.
        /// </summary>
        /// <param name="service">The service.</param>
        public CountriesController(ICountryService service)
        {
            _service = service;
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns>CountryDto list.</returns>
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<CountryDto>>> GetAll()
            => Ok(await _service.GetAllAsync());

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>CountryDto by id.</returns>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<CountryDto>> Get(Guid id)
        {
            var result = await _service.GetByIdAsync(id);
            return result == null ? NotFound() : Ok(result);
        }

        /// <summary>
        /// Creates the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>CountryDto.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost]
        public async Task<CommonResponse<CountryDto>> Create(
            CreateCountryRequest request)
        {
            var created = await _service.CreateAsync(request);
            if (created == null)
            {
                return CommonResponse<CountryDto>.FailureResponse("Failed to create country.");
            }
            else
            {
                return CommonResponse<CountryDto>.SuccessResponse("Country created successfully.", created);
            }
        }

        /// <summary>
        /// Updates the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="request">The request.</param>
        /// <returns>result.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPut("{id:guid}")]
        public async Task<CommonResponse<CountryDto>> Update(
            Guid id,
            CreateCountryRequest request)
        {
            var result = await _service.UpdateAsync(id, request);
            if (result == null)
            {
                return CommonResponse<CountryDto>.FailureResponse("Failed to update country.");
            }
            else
            {
                return CommonResponse<CountryDto>.SuccessResponse("Country updated successfully.", result);
            }
        }

        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>result.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpDelete("{id:guid}")]
        public async Task<CommonResponse<bool>> Delete(Guid id)
        {
            bool res = await _service.DeleteAsync(id);
            if (res)
            {
                return CommonResponse<bool>.SuccessResponse("Batchtiming for monday to friday deleted successfully.", true);
            }
            else
            {
                return CommonResponse<bool>.FailureResponse("Failed to delete batchtiming for monday to friday.");
            }
        }
    }
}
