namespace Winfocus.LMS.Application.Services
{
    using Microsoft.Extensions.Logging;
    using Winfocus.LMS.Application.DTOs;
    using Winfocus.LMS.Application.DTOs.Masters;
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
        public async Task<CommonResponse<List<CountryDto>>> GetAllAsync()
        {
            _logger.LogInformation("Fetching all countries");
            var countries = await _repository.GetAllAsync();
            var mapped = countries.Select(Map).ToList();
            if (mapped.Any())
            {
                return CommonResponse<List<CountryDto>>.SuccessResponse("fetching all countries", mapped);
            }
            else
            {
                return CommonResponse<List<CountryDto>>.FailureResponse("no countries found");
            }
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>CountryDto.</returns>
        public async Task<CommonResponse<CountryDto>> GetByIdAsync(Guid id)
        {
            var country = await _repository.GetByIdAsync(id);
            var mappeddata = country == null ? null : Map(country);
            if (mappeddata != null)
            {
                return CommonResponse<CountryDto>.SuccessResponse("fetching country by id", mappeddata);
            }
            else
            {
                return CommonResponse<CountryDto>.FailureResponse("country not found");
            }
        }

        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>CountryDto.</returns>
        /// <exception cref="InvalidOperationException">Country code already exists.</exception>
        public async Task<CountryDto> CreateAsync(CreateCountryRequest request)
        {
            var country = new Country
            {
                Name = request.name,
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
        public async Task<CountryDto> UpdateAsync(Guid id, CreateCountryRequest request)
        {
            var country = await _repository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException("Country not found");

            country.Name = request.name;
            country.UpdatedAt = DateTime.UtcNow;

            return Map(await _repository.UpdateAsync(country));
        }

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>task.</returns>
        public async Task<bool> DeleteAsync(Guid id)
        {
             return await _repository.DeleteAsync(id);
        }

        private static CountryDto Map(Country c) =>
            new (
                c.Id,
                c.Name,
                c.IsoAlpha3,
                c.IsoNumeric,
                c.Centres.Select(x =>
                    new CentreDto(x.Id, x.Name, x.CenterType.ToString()))
                .ToList());
    }
}
