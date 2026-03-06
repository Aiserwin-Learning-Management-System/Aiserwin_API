namespace Winfocus.LMS.Application.Services
{
    using Microsoft.Extensions.Logging;
    using Microsoft.EntityFrameworkCore;
    using Winfocus.LMS.Application.DTOs;
    using Winfocus.LMS.Application.DTOs.Common;
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
                    PhoneCode = request.phonecode,
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
                country.PhoneCode = request.phonecode;

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

        /// <summary>
        /// Gets filtered countries with pagination support.
        /// </summary>
        /// <param name="request">The paged request.</param>
        /// <returns>Paginated countries result.</returns>
        public async Task<CommonResponse<PagedResult<CountryDto>>> GetFilteredAsync(
            PagedRequest request)
        {
            try
            {
                _logger.LogInformation(
                    "Fetching filtered countries. Filters => Active:{Active}, " +
                    "Search:{SearchText}, SortBy:{SortBy}, SortOrder:{SortOrder}, " +
                    "Limit:{Limit}, Offset:{Offset}",
                    request.Active, request.SearchText, request.SortBy,
                    request.SortOrder, request.Limit, request.Offset);

                var query = _repository.Query();

                // ── Filters ──
                if (request.Active.HasValue)
                    query = query.Where(x => x.IsActive == request.Active.Value);

                if (request.StartDate.HasValue)
                    query = query.Where(x => x.CreatedAt >= request.StartDate.Value);

                if (request.EndDate.HasValue)
                    query = query.Where(x => x.CreatedAt <= request.EndDate.Value);

                // ── Search on Course, Stream, Grade, AND Syllabus Name ──
                if (!string.IsNullOrWhiteSpace(request.SearchText))
                {
                    var searchTerm = request.SearchText.Trim().ToLower();
                    query = query.Where(x =>
                        x.Name.ToLower().Contains(searchTerm));
                }

                // ── Total Count ──
                var totalCount = await query.CountAsync();

                if (totalCount == 0)
                {
                    return CommonResponse<PagedResult<CountryDto>>.SuccessResponse(
                        "No countries found.",
                        new PagedResult<CountryDto>(
                            new List<CountryDto>(), 0, request.Limit, request.Offset));
                }

                // ── Dynamic Sorting ──
                var isDesc = request.SortOrder.Equals("desc", StringComparison.OrdinalIgnoreCase);

                query = request.SortBy.ToLower() switch
                {
                    "name" => isDesc ? query.OrderByDescending(x => x.Name)
                                             : query.OrderBy(x => x.Name),

                    "isactive" => isDesc ? query.OrderByDescending(x => x.IsActive)
                                             : query.OrderBy(x => x.IsActive),

                    "createdat" => isDesc ? query.OrderByDescending(x => x.CreatedAt)
                                             : query.OrderBy(x => x.CreatedAt),

                    _ => isDesc ? query.OrderByDescending(x => x.CreatedAt)
                                             : query.OrderBy(x => x.CreatedAt),
                };

                // ── Pagination ──
                var courses = await query
                    .Skip(request.Offset)
                    .Take(request.Limit)
                    .ToListAsync();

                var dtoList = courses.Select(Map).ToList();

                _logger.LogInformation(
                    "Returning {Count} of {Total} countries",
                    dtoList.Count, totalCount);

                return CommonResponse<PagedResult<CountryDto>>.SuccessResponse(
                    "Countries fetched successfully.",
                    new PagedResult<CountryDto>(
                        dtoList, totalCount, request.Limit, request.Offset));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching filtered countries.");
                return CommonResponse<PagedResult<CountryDto>>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        private static CountryDto Map(Country c) =>
          new CountryDto
          {
              Id = c.Id,
              Name = c.Name,
              Code = c.Code,
              IsActive = c.IsActive,
              PhoneCode = c.PhoneCode,
          };
    }
}
