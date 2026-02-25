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
            try
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
            catch (Exception ex)
            {
                return CommonResponse<List<CountryDto>>.FailureResponse($"An error occurred while fetching countries: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>CountryDto.</returns>
        public async Task<CommonResponse<CountryDto>> GetByIdAsync(Guid id)
        {
            try
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
            catch (Exception ex)
            {
                return CommonResponse<CountryDto>.FailureResponse($"An error occurred while fetching country: {ex.Message}");
            }
        }

        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="userid">The userid.</param>
        /// <returns>CountryDto.</returns>
        /// <exception cref="InvalidOperationException">Country code already exists.</exception>
        public async Task<CommonResponse<CountryDto>> CreateAsync(CreateCountryRequest request, Guid userid)
        {
            try
            {
                bool codeExists = await _repository.ExistsByNameAsync(request.name);
                if (codeExists)
                {
                    return CommonResponse<CountryDto>.FailureResponse("Failed to create country, because the name already exist.");
                }

                var country = new Country
                {
                    Name = request.name,
                    Code = request.code,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = userid,
                };

                var created = await _repository.AddAsync(country);
                var mapped = Map(created);
                if (mapped == null)
                {
                    return CommonResponse<CountryDto>.FailureResponse("Failed to create country.");
                }
                else
                {
                    return CommonResponse<CountryDto>.SuccessResponse("Country created successfully.", mapped);
                }
            }
            catch (Exception ex)
            {
                return CommonResponse<CountryDto>.FailureResponse($"An error occurred while creating country: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="request">The request.</param>
        /// <param name="userid">The userid.</param>
        /// <exception cref="KeyNotFoundException">Country not found.</exception>
        /// <returns>task.</returns>
        public async Task<CommonResponse<CountryDto>> UpdateAsync(Guid id, CreateCountryRequest request, Guid userid)
        {
            try
            {
                var country = await _repository.GetByIdAsync(id);
                if (country == null)
                {
                    return CommonResponse<CountryDto>.FailureResponse("Country not found.");
                }

                country.Name = request.name;
                country.Code = request.code;
                country.UpdatedAt = DateTime.UtcNow;
                country.UpdatedBy = userid;

                var mapped = Map(await _repository.UpdateAsync(country));
                if (mapped == null)
                {
                    return CommonResponse<CountryDto>.FailureResponse("Failed to update country.");
                }
                else
                {
                    return CommonResponse<CountryDto>.SuccessResponse("Country updated successfully.", mapped);
                }
            }
            catch (Exception ex)
            {
                return CommonResponse<CountryDto>.FailureResponse($"An error occurred while updating country: {ex.Message}");
            }
        }

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>task.</returns>
        public async Task<CommonResponse<bool>> DeleteAsync(Guid id)
        {
            try
            {
                bool res = await _repository.DeleteAsync(id);
                if (!res)
                {
                    return CommonResponse<bool>.FailureResponse("Failed to delete country.");
                }
                else
                {
                    return CommonResponse<bool>.SuccessResponse("Country deleted successfully.", true);
                }
            }
            catch (Exception ex)
            {
                return CommonResponse<bool>.FailureResponse($"An error occurred while deleting country: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets the by name asynchronous.
        /// </summary>
        /// <param name="name">The identifier.</param>
        /// <returns>CountryDto.</returns>
        public async Task<bool> ExistsByNameAsync(string name)
        {
            return await _repository.ExistsByNameAsync(name);
        }

        private static CountryDto Map(Country c) =>
            new (
                c.Id,
                c.Name,
                c.Centres.Select(x =>
                    new CentreDto(x.Id, x.Name, x.CenterType.ToString()))
                .ToList());
    }
}
