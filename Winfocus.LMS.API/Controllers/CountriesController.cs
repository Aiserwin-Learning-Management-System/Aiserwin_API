using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Winfocus.LMS.Application.Interfaces;
using Winfocus.LMS.Domain.Entities;
using Winfocus.LMS.Infrastructure.Repositories;

namespace Winfocus.LMS.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CountriesController : ControllerBase
    {
        private readonly ICountryRepository _countryRepository;
        private readonly ILogger<CountriesController> _logger;
        public CountriesController(ICountryRepository countryRepository, ILogger<CountriesController> logger)
        {
            _countryRepository = countryRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Fetching all countries at {Time}", DateTime.UtcNow);
            var list = await _countryRepository.GetAllAsync();
            _logger.LogInformation("Fetched {Count} countries", list.Count());
            var dto = list.Select(c => new { c.Id, c.Name, c.Code });
            return Ok(dto);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var c = await _countryRepository.GetByIdAsync(id);
            if (c == null) return NotFound();
            return Ok(new { c.Id, c.Name, c.Code, Centres = c.Centres.Select(x => new { x.Id, x.Name, x.Type }) });
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCountryRequest req)
        {
            var country = new Country { Name = req.Name, Code = req.Code };
            var created = await _countryRepository.AddAsync(country);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, new { created.Id, created.Name, created.Code });
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] CreateCountryRequest req)
        {
            var existing = await _countryRepository.GetByIdAsync(id);
            if (existing == null) return NotFound();
            existing.Name = req.Name;
            existing.Code = req.Code;
            await _countryRepository.UpdateAsync(existing);
            return NoContent();
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _countryRepository.DeleteAsync(id);
            return NoContent();
        }
    }

    public record CreateCountryRequest(string Name, string Code);
}
