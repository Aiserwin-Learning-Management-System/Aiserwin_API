namespace Winfocus.LMS.Api.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Winfocus.LMS.Application.Interfaces;
    using Winfocus.LMS.Domain.Entities;

    /// <summary>
    /// API controller for managing countries.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class CountriesController : ControllerBase
    {
        private readonly ICountryRepository _countryRepository;
        private readonly ILogger<CountriesController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="CountriesController"/> class.
        /// </summary>
        /// <param name="countryRepository">The country repository.</param>
        /// <param name="logger">The logger instance.</param>
        public CountriesController(ICountryRepository countryRepository, ILogger<CountriesController> logger)
        {
            _countryRepository = countryRepository;
            _logger = logger;
        }

        /// <summary>
        /// Gets all countries.
        /// </summary>
        /// <returns>A list of countries.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Fetching all countries at {Time}", DateTime.UtcNow);
            var list = await _countryRepository.GetAllAsync();
            _logger.LogInformation("Fetched {Count} countries", list.Count());
            var dto = list.Select(c => new { c.Id, c.Name, c.Code });
            return Ok(dto);
        }

        /// <summary>
        /// Gets a country by its unique identifier.
        /// </summary>
        /// <param name="id">The country identifier.</param>
        /// <returns>The country details.</returns>
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var c = await _countryRepository.GetByIdAsync(id);
            if (c == null)
            {
                return NotFound();
            }

            return Ok(new { c.Id, c.Name, c.Code, Centres = c.Centres.Select(x => new { x.Id, x.Name, x.Type }) });
        }

        /// <summary>
        /// Creates a new country.
        /// </summary>
        /// <param name="req">The country creation request.</param>
        /// <returns>The created country.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCountryRequest req)
        {
            var country = new Country { Name = req.name, Code = req.code };
            var created = await _countryRepository.AddAsync(country);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, new { created.Id, created.Name, created.Code });
        }

        /// <summary>
        /// Updates an existing country.
        /// </summary>
        /// <param name="id">The country identifier.</param>
        /// <param name="req">The country update request.</param>
        /// <returns>No content if successful.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] CreateCountryRequest req)
        {
            var existing = await _countryRepository.GetByIdAsync(id);
            if (existing == null)
            {
                return NotFound();
            }

            existing.Name = req.name;
            existing.Code = req.code;
            await _countryRepository.UpdateAsync(existing);
            return NoContent();
        }

        /// <summary>
        /// Deletes a country by its unique identifier.
        /// </summary>
        /// <param name="id">The country identifier.</param>
        /// <returns>No content if successful.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _countryRepository.DeleteAsync(id);
            return NoContent();
        }
    }

    /// <summary>
    /// Request model for creating or updating a country.
    /// </summary>
    /// <param name="name">The name of the country.</param>
    /// <param name="code">The code of the country.</param>
    public record CreateCountryRequest(string name, string code);
}
