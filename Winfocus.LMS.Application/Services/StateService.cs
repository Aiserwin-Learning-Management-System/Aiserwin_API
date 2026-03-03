namespace Winfocus.LMS.Application.Services
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Winfocus.LMS.Application.DTOs;
    using Winfocus.LMS.Application.DTOs.Common;
    using Winfocus.LMS.Application.DTOs.Masters;
    using Winfocus.LMS.Application.Interfaces;
    using Winfocus.LMS.Domain.Entities;

    /// <summary>
    /// Provides business operations for <see cref="State"/> entities.
    /// </summary>
    public sealed class StateService : IStateService
    {
        private readonly IStateRepository _repository;
        private readonly ILogger<StateService> _logger;
        private readonly ICountryRepository _countryRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="StateService"/> class.
        /// </summary>
        /// <param name="repository">Repository used for data access.</param>
        /// <param name="countryRepository">countryRepository used for data access.</param>
        /// <param name="logger">Logger.</param>
        public StateService(IStateRepository repository, ILogger<StateService> logger, ICountryRepository countryRepository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _countryRepository = countryRepository;
        }

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>StateDto.</returns>
        public async Task<CommonResponse<List<StateDto>>> GetAllAsync()
        {
            try
            {
                _logger.LogInformation("Fetching all states");
                var states = await _repository.GetAllAsync();
                _logger.LogInformation("Fetched {Count} states", states.Count());
                var mappeddata = states.Select(Map).ToList();
                if (mappeddata.Any())
                {
                    return CommonResponse<List<StateDto>>.SuccessResponse("Fetched all states", mappeddata);
                }
                else
                {
                    return CommonResponse<List<StateDto>>.FailureResponse("States not found");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching states");
                return CommonResponse<List<StateDto>>.FailureResponse($"Failed to fetch states. Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>StateDto.</returns>
        public async Task<CommonResponse<StateDto>> GetByIdAsync(Guid id)
        {
            try
            {
                _logger.LogInformation("Fetching state by Id: {StateId}", id);
                var state = await _repository.GetByIdAsync(id);
                _logger.LogInformation("State fetched successfully for Id: {StateId}", id);
                var mappeddata = state == null ? null : Map(state);
                if (mappeddata != null)
                {
                    return CommonResponse<StateDto>.SuccessResponse("Fetched state by Id", mappeddata);
                }
                else
                {
                    return CommonResponse<StateDto>.FailureResponse("State not found");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching state by Id: {StateId}", id);
                return CommonResponse<StateDto>.FailureResponse($"Failed to fetch state. Error: {ex.Message}");
            }
         }

        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>StateDto.</returns>
        /// <exception cref="InvalidOperationException">State code already exists. </exception>
        public async Task<CommonResponse<StateDto>> CreateAsync(CreateMasterStateRequest request)
        {
            try
            {
                var country = await _countryRepository.GetByIdAsync(request.countryid);

                if (country == null)
                {
                    return CommonResponse<StateDto>.FailureResponse("Failed to create State, because the invalid country");
                }

                var state = new State
                {
                    Name = request.name,
                    CountryId = request.countryid,
                    ModeOfStudyId = request.modeofstudyid,
                    CreatedBy = request.userId,
                    CreatedAt = DateTime.UtcNow,
                };

                var created = await _repository.AddAsync(state);
                _logger.LogInformation(
               "State created successfully. StateId: {StateId}",
               created.Id);
                var mapped = Map(created);
                if (mapped == null)
                {
                    return CommonResponse<StateDto>.FailureResponse("Failed to create State.");
                }
                else
                {
                    return CommonResponse<StateDto>.SuccessResponse("State created successfully.", mapped);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating state");
                return CommonResponse<StateDto>.FailureResponse($"Failed to create State. Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="request">The request.</param>
        /// <exception cref="KeyNotFoundException">State not found.</exception>
        /// <returns>task.</returns>
        public async Task<CommonResponse<StateDto>> UpdateAsync(Guid id, CreateMasterStateRequest request)
        {
            try
            {
                _logger.LogInformation("Updating state Id: {StateId}", id);
                var state = await _repository.GetByIdAsync(id);
                if (state == null)
                {
                    return CommonResponse<StateDto>.FailureResponse("State not found");
                }

                state.Name = request.name;
                state.UpdatedBy = request.userId;
                state.UpdatedAt = DateTime.UtcNow;

                var mappeddata = Map(await _repository.UpdateAsync(state));
                if (mappeddata != null)
                {
                    return CommonResponse<StateDto>.SuccessResponse("USuccessfully updated", mappeddata);
                }
                else
                {
                    return CommonResponse<StateDto>.FailureResponse("Failed to update");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating state Id: {StateId}", id);
                return CommonResponse<StateDto>.FailureResponse($"Failed to update State. Error: {ex.Message}");
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
                _logger.LogInformation("Deleting state Id: {Id}", id);
                var result = await _repository.DeleteAsync(id);

                if (result)
                {
                    return CommonResponse<bool>.SuccessResponse(
                        "State deleted successfully", true);
                }

                return CommonResponse<bool>.FailureResponse(
                    "State not found or already deleted");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting State Id: {Id}", id);
                return CommonResponse<bool>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="countryid">The identifier.</param>
        /// <returns>StateDto.</returns>
        public async Task<CommonResponse<List<StateDto>>> GetByCountryIdAsync(Guid countryid)
        {
            _logger.LogInformation("Fetching states for CountryId: {CountryId}", countryid);

            var states = await _repository.GetByCountryIdAsync(countryid);

            var mappeddata = Map(states);

            if (mappeddata != null)
            {
                return CommonResponse<List<StateDto>>.SuccessResponse("Fetched state by country Id", mappeddata);
            }
            else
            {
                _logger.LogWarning("No states found for CountryId: {CountryId}", countryid);
                return CommonResponse<List<StateDto>>.FailureResponse("State not found");
            }
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>StateDto.</returns>
        public async Task<CommonResponse<PagedResult<StateDto>>> GetFilteredAsync(
           PagedRequest request)
        {
            try
            {
                _logger.LogInformation(
                    "Fetching filtered states. Filters => Active:{Active}, " +
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
                    return CommonResponse<PagedResult<StateDto>>.SuccessResponse(
                        "No state found.",
                        new PagedResult<StateDto>(
                            new List<StateDto>(), 0, request.Limit, request.Offset));
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
                var stateList = await query
                    .Skip(request.Offset)
                    .Take(request.Limit)
                    .ToListAsync();

                var dtoList = stateList.Select(Map).ToList();

                _logger.LogInformation(
                    "Returning {Count} of {Total} state",
                    dtoList.Count, totalCount);

                return CommonResponse<PagedResult<StateDto>>.SuccessResponse(
                    "State details fetched successfully.",
                    new PagedResult<StateDto>(
                        dtoList, totalCount, request.Limit, request.Offset));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching filtered in state.");
                return CommonResponse<PagedResult<StateDto>>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        private static List<StateDto> Map(IEnumerable<State> states)
        {
            return states.Select(Map).ToList();
        }

        private static StateDto Map(State c) =>
      new StateDto
      {
          Id = c.Id,
          Name = c.Name,
          CountryId = c.CountryId,
          ModeOfStudyId = c.ModeOfStudyId,
          IsActive = c.IsActive,
          Country = c.Country == null ? null : new CountryDto
          {
              Id = c.Country.Id,
              Name = c.Country.Name
          },
          ModeOfStudy = c.ModeOfStudy == null ? null : new ModeOfStudyDto
          {
              Id = c.ModeOfStudy.Id,
              Name = c.ModeOfStudy.Name
          }
      };
    }
}
