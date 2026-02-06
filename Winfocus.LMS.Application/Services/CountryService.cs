namespace Winfocus.LMS.Application.Services
{
    using Microsoft.Extensions.Logging;
    using Winfocus.LMS.Application.DTOs;
    using Winfocus.LMS.Application.Interfaces;
    using Winfocus.LMS.Domain.Entities;

    /// <summary>
    /// CountryService.
    /// </summary>
    /// <seealso cref="Winfocus.LMS.Application.Interfaces.ICountryService" />
    public sealed class CountryService : ICountryService
    {
        private readonly ICountryRepository _repository;
        private readonly ILogger<CountryService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="CountryService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="logger">The logger.</param>
        public CountryService(
            ICountryRepository repository,
            ILogger<CountryService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>CountryDto.</returns>
        public async Task<IReadOnlyList<CountryDto>> GetAllAsync()
        {
            _logger.LogInformation("Fetching all countries");
            var countries = await _repository.GetAllAsync();
            return countries.Select(Map).ToList();
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>CountryDto.</returns>
        public async Task<CountryDto?> GetByIdAsync(Guid id)
        {
            var country = await _repository.GetByIdAsync(id);
            return country == null ? null : Map(country);
        }

        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>CountryDto.</returns>
        /// <exception cref="InvalidOperationException">Country code already exists.</exception>
        public async Task<CountryDto> CreateAsync(CreateCountryRequest request)
        {
            if (await _repository.ExistsByCodeAsync(request.code))
            {
                throw new InvalidOperationException("Country code already exists");
            }

            var country = new Country
            {
                Name = request.name,
                Code = request.code,
                CreatedAt = DateTime.UtcNow,
            };

            var created = await _repository.AddAsync(country);
            return Map(created);
        }

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="request">The request.</param>
        /// <exception cref="KeyNotFoundException">Country not found.</exception>
        /// <returns>task.</returns>
        public async Task UpdateAsync(Guid id, CreateCountryRequest request)
        {
            var country = await _repository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException("Country not found");

            country.Name = request.name;
            country.Code = request.code;
            country.UpdatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(country);
        }

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>task.</returns>
        public async Task DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
        }

        private static CountryDto Map(Country c) =>
            new (
                c.Id,
                c.Name,
                c.Code,
                c.IsoAlpha3,
                c.IsoNumeric,
                c.Centres.Select(x =>
                    new CentreDto(x.Id, x.Name, x.Type.ToString()))
                .ToList()
            );
    }
}
