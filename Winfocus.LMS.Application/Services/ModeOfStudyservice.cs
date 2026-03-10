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
    /// Provides business operations for <see cref="ModeOfStudy"/> entities.
    /// </summary>
    public sealed class ModeOfStudyService : IModeOfStudyService
    {
        private readonly IModeOfStudyRepository _repository;
        private readonly ILogger<ModeOfStudyService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModeOfStudyService"/> class.
        /// </summary>
        /// <param name="repository">Repository used for data access.</param>
        /// <param name="logger">Logger.</param>
        public ModeOfStudyService(IModeOfStudyRepository repository, ILogger<ModeOfStudyService> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>ModeOfStudyDto.</returns>
        public async Task<CommonResponse<List<ModeOfStudyDto>>> GetAllAsync()
        {
            try
            {
                _logger.LogInformation("Fetching all mode of studies");
                var modeOfStudies = await _repository.GetAllAsync();
                _logger.LogInformation("Fetched {Count} mode of studies", modeOfStudies.Count());
                var mapped = modeOfStudies.Select(Map).ToList();
                if (mapped.Any())
                {
                    return CommonResponse<List<ModeOfStudyDto>>.SuccessResponse("Fetching all mode of studies", mapped);
                }
                else
                {
                    return CommonResponse<List<ModeOfStudyDto>>.FailureResponse("no mode of studies");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching mode of studies.");
                return CommonResponse<List<ModeOfStudyDto>>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }

        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="countryId">The countryId.</param>
        /// <returns>ModeOfStudyDto.</returns>
        public async Task<CommonResponse<ModeOfStudyDto>> GetByIdAsync(Guid id, Guid countryId)
        {
            try
            {
                _logger.LogInformation("Fetching mode of study by Id: {ModeOfStudyId}", id);
                var modeOfStudies = await _repository.GetByIdAsync(id, countryId);
                _logger.LogInformation("Mode of study fetched successfully for Id: {ModeOfStudyId}", id);
                var mapped = modeOfStudies == null ? null : Map(modeOfStudies);
                if (mapped != null)
                {
                    return CommonResponse<ModeOfStudyDto>.SuccessResponse("Mode of study by id", mapped);
                }
                else
                {
                    return CommonResponse<ModeOfStudyDto>.FailureResponse("mode of study not found");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching mode of study.");
                return CommonResponse<ModeOfStudyDto>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>ModeOfStudyDto.</returns>
        /// <exception cref="InvalidOperationException">mode of study already exists.</exception>
        public async Task<CommonResponse<ModeOfStudyDto>> CreateAsync(ModeOfStudyRequest request)
        {
            try
            {
                var modeOfStudy = new ModeOfStudy
                {
                    CountryId = request.countryid,
                    Name = request.name,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = request.userId,
                };

                var created = await _repository.AddAsync(modeOfStudy);
                _logger.LogInformation(
               "Mode of study created successfully. ModeOfStudyId: {ModeOfStudyId}",
               created.Id);

                var mapped = Map(created);
                if (mapped != null)
                {
                    _logger.LogInformation(
                      "Course created successfully with Id: {CourseId}",
                      created.Id);
                    return CommonResponse<ModeOfStudyDto>.SuccessResponse("Course created successfully", mapped);
                }
                else
                {
                    return CommonResponse<ModeOfStudyDto>.FailureResponse("Failed to create mode of study");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating mode of study");
                return CommonResponse<ModeOfStudyDto>.FailureResponse("An error occurred while creating mode of study");
            }
        }

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="request">The request.</param>
        /// <exception cref="KeyNotFoundException">mode of study not found.</exception>
        /// <returns>task.</returns>
        public async Task<CommonResponse<ModeOfStudyDto>> UpdateAsync(Guid id, ModeOfStudyRequest request)
        {
            try
            {
                _logger.LogInformation("Updating mode of study Id: {Id}", id);

                var batch = await _repository.GetByIdAsync(id, request.countryid);
                if (batch == null)
                {
                    return CommonResponse<ModeOfStudyDto>.FailureResponse("mode of study not found");
                }

                batch.Name = request.name;
                batch.CountryId = request.countryid;
                batch.UpdatedAt = DateTime.UtcNow;
                batch.UpdatedBy = request.userId;

                var updated = await _repository.UpdateAsync(batch);

                _logger.LogInformation("mode of study updated Id: {Id}", id);
                return CommonResponse<ModeOfStudyDto>.SuccessResponse(
                    "mode of study updated successfully", Map(updated));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating mode of study Id: {Id}", id);
                return CommonResponse<ModeOfStudyDto>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="countryId">The countryId.</param>
        /// <returns>task.</returns>
        public async Task<CommonResponse<bool>> DeleteAsync(Guid id, Guid countryId)
        {
            try
            {
                _logger.LogInformation("Deleting mode of study Id: {ModeOfStudyId}", id);
                var result = await _repository.DeleteAsync(id, countryId);

                if (result)
                {
                    return CommonResponse<bool>.SuccessResponse(
                        "mode of study deleted successfully", true);
                }

                return CommonResponse<bool>.FailureResponse(
                    "mode of study not found or already deleted");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting mode of study Id: {Id}", id);
                return CommonResponse<bool>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="countryid">The identifier.</param>
        /// <returns>ModeOfStudyDto.</returns>
        public async Task<List<ModeOfStudyDto>> GetByCountryIdAsync(Guid countryid)
        {
            _logger.LogInformation("Fetching Modeof study for CountryId: {CountryId}", countryid);

            var modeofstudy = await _repository.GetByCountryIdAsync(countryid);

            if (!modeofstudy.Any())
                _logger.LogWarning("No modeofstudy found for StateId: {StateId}", countryid);

            return Map(modeofstudy);
        }

        /// <summary>
        /// Gets filtered modeofstudy with pagination support.
        /// </summary>
        /// <param name="request">The paged request.</param>
        /// <param name="countryId">The countryId.</param>
        /// <returns>Paginated modeofstudy result.</returns>
        public async Task<CommonResponse<PagedResult<ModeOfStudyDto>>> GetFilteredAsync(
         PagedRequest request, Guid countryId)
        {
            try
            {
                _logger.LogInformation(
                    "Fetching filtered grades. Filters => Active:{Active}, " +
                    "Search:{SearchText}, SortBy:{SortBy}, SortOrder:{SortOrder}, " +
                    "Limit:{Limit}, Offset:{Offset}",
                    request.Active, request.SearchText, request.SortBy,
                    request.SortOrder, request.Limit, request.Offset);

                var query = _repository.Query(countryId);

                // ── Filters ──
                if (request.Active.HasValue)
                    query = query.Where(x => x.IsActive == request.Active.Value);

                if (request.StartDate.HasValue)
                    query = query.Where(x => x.CreatedAt >= request.StartDate.Value);

                if (request.EndDate.HasValue)
                    query = query.Where(x => x.CreatedAt <= request.EndDate.Value);

                if (!string.IsNullOrWhiteSpace(request.SearchText))
                {
                    var searchTerm = request.SearchText.Trim().ToLower();
                    query = query.Where(x =>
                        x.Name.ToLower().Contains(searchTerm) ||
                        x.Country.Name.ToLower().Contains(searchTerm));
                }

                // ── Total Count ──
                var totalCount = await query.CountAsync();

                if (totalCount == 0)
                {
                    return CommonResponse<PagedResult<ModeOfStudyDto>>.SuccessResponse(
                        "No mode of study found.",
                        new PagedResult<ModeOfStudyDto>(
                            new List<ModeOfStudyDto>(), 0, request.Limit, request.Offset));
                }

                // ── Dynamic Sorting ──
                var isDesc = request.SortOrder.Equals("desc", StringComparison.OrdinalIgnoreCase);

                query = request.SortBy.ToLower() switch
                {
                    "name" => isDesc ? query.OrderByDescending(x => x.Name)
                                             : query.OrderBy(x => x.Name),

                    "countryname" => isDesc ? query.OrderByDescending(x => x.Country.Name)
                                             : query.OrderBy(x => x.Country.Name),

                    "isactive" => isDesc ? query.OrderByDescending(x => x.IsActive)
                                             : query.OrderBy(x => x.IsActive),

                    "createdat" => isDesc ? query.OrderByDescending(x => x.CreatedAt)
                                             : query.OrderBy(x => x.CreatedAt),

                    _ => isDesc ? query.OrderByDescending(x => x.CreatedAt)
                                             : query.OrderBy(x => x.CreatedAt),
                };

                // ── Pagination ──
                var modeofstudy = await query
                    .Skip(request.Offset)
                    .Take(request.Limit)
                    .ToListAsync();

                var dtoList = modeofstudy.Select(Map).ToList();

                _logger.LogInformation(
                    "Returning {Count} of {Total} mode of study",
                    dtoList.Count, totalCount);

                return CommonResponse<PagedResult<ModeOfStudyDto>>.SuccessResponse(
                    "Mode of study fetched successfully.",
                    new PagedResult<ModeOfStudyDto>(
                        dtoList, totalCount, request.Limit, request.Offset));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching filtered Mode of study.");
                return CommonResponse<PagedResult<ModeOfStudyDto>>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        private static List<ModeOfStudyDto> Map(IEnumerable<ModeOfStudy> modeofstudy)
        {
            return modeofstudy.Select(Map).ToList();
        }

        private static ModeOfStudyDto Map(ModeOfStudy c) =>
            new ModeOfStudyDto
            {
                Id = c.Id,
                Name = c.Name,
                CountryId = c.CountryId,
                IsActive = c.IsActive,
                Country = c.Country == null ? null : new CountryDto
                {
                    Id = c.Country.Id,
                    Name = c.Country.Name
                }
            };
    }
}
