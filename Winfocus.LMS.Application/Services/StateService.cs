namespace Winfocus.LMS.Application.Services
{
    using Microsoft.Extensions.Logging;
    using Winfocus.LMS.Application.DTOs;
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

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>StateDto.</returns>
        public async Task<CommonResponse<StateDto>> GetByIdAsync(Guid id)
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

        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>StateDto.</returns>
        /// <exception cref="InvalidOperationException">State code already exists. </exception>
        public async Task<StateDto> CreateAsync(CreateMasterStateRequest request)
        {
            var country = await _countryRepository.GetByIdAsync(request.countryid);

            if (country == null)
            {
                throw new InvalidOperationException("Country not found");
            }

            if (!country.IsActive)
            {
                throw new InvalidOperationException("Cannot create state for inactive country");
            }

            var state = new State
            {
                Name = request.name,
                CountryId = request.countryid,
                CreatedBy = request.userId,
                CreatedAt = DateTime.UtcNow,
            };

            var created = await _repository.AddAsync(state);
            _logger.LogInformation(
           "State created successfully. StateId: {StateId}",
           created.Id);
            return Map(created);
        }

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="request">The request.</param>
        /// <exception cref="KeyNotFoundException">State not found.</exception>
        /// <returns>task.</returns>
        public async Task<StateDto> UpdateAsync(Guid id, CreateMasterStateRequest request)
        {
            _logger.LogInformation("Updating state Id: {StateId}", id);
            var state = await _repository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException("State not found");

            state.Name = request.name;
            state.UpdatedBy = request.userId;
            state.UpdatedAt = DateTime.UtcNow;

            return Map(await _repository.UpdateAsync(state));
        }

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>task.</returns>
        public async Task<bool> DeleteAsync(Guid id)
        {
            _logger.LogInformation("Deleting state Id: {StateId}", id);
            return await _repository.DeleteAsync(id);
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
          CreatedBy = c.CreatedBy,
          CreatedAt = c.CreatedAt,
          UpdatedAt = c.UpdatedAt,
          UpdatedBy = c.UpdatedBy,
          Country = c.Country == null ? null : new CountryDto(
              c.Country.Id,
              c.Country.Name,
              c.Country.IsoAlpha3,
              c.Country.IsoNumeric,
              c.Country.Centres?
                  .Select(x => new CentreDto(
                      x.Id,
                      x.Name,
                      x.CenterType.ToString()
                  ))
                  .ToList() ?? new List<CentreDto>()
          )
      };
    }
}
