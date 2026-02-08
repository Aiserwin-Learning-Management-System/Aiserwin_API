using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Winfocus.LMS.Application.DTOs;
using Winfocus.LMS.Application.DTOs.Masters;
using Winfocus.LMS.Application.Interfaces;
using Winfocus.LMS.Domain.Entities;

namespace Winfocus.LMS.Application.Services
{
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
        public async Task<IReadOnlyList<StateDto>> GetAllAsync()
        {
            _logger.LogInformation("Fetching all states");
            var states = await _repository.GetAllAsync();
            _logger.LogInformation("Fetched {Count} states", states.Count());
            return states.Select(Map).ToList();
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>StateDto.</returns>
        public async Task<StateDto?> GetByIdAsync(Guid id)
        {
            _logger.LogInformation("Fetching state by Id: {StateId}", id);
            var state = await _repository.GetByIdAsync(id);
            _logger.LogInformation("State fetched successfully for Id: {StateId}", id);
            return state == null ? null : Map(state);
        }

        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>StateDto.</returns>
        /// <exception cref="InvalidOperationException">State code already exists. </exception>
        public async Task<StateDto> CreateAsync(CreateMasterStateRequest request)
        {
            _logger.LogInformation("Creating state with Code: {StateCode}", request.code);
            if (await _repository.ExistsByCodeAsync(request.code))
            {
                throw new InvalidOperationException("state code already exists");
            }

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
                StateName = request.name,
                StateCode = request.code,
                CountryId = request.countryid,
            };

            var created = await _repository.AddAsync(state);
            _logger.LogInformation(
           "State created successfully. StateId: {StateId}, Code: {StateCode}",
           created.Id,
           created.StateCode);
            return Map(created);
        }

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="request">The request.</param>
        /// <exception cref="KeyNotFoundException">State not found.</exception>
        /// <returns>task.</returns>
        public async Task UpdateAsync(Guid id, CreateMasterStateRequest request)
        {
            _logger.LogInformation("Updating state Id: {StateId}", id);
            var state = await _repository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException("State not found");

            state.StateName = request.name;
            state.StateCode = request.code;

            await _repository.UpdateAsync(state);
            _logger.LogInformation(
           "State updated successfully. StateId: {StateId}",
           id);
        }

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>task.</returns>
        public async Task DeleteAsync(Guid id)
        {
            _logger.LogInformation("Deleting state Id: {StateId}", id);
            await _repository.DeleteAsync(id);
            _logger.LogInformation(
           "State deleted successfully. StateId: {StateId}",
           id);
        }

        private static StateDto Map(State c) =>
      new StateDto
      {
          Id = c.Id,
          StateName = c.StateName,
          StateCode = c.StateCode,
          CountryId = c.CountryId,
          Country = c.Country == null ? null : new CountryDto(
              c.Country.Id,
              c.Country.Name,
              c.Country.Code,
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
