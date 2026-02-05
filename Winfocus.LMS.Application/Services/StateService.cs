using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
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

        /// <summary>
        /// Initializes a new instance of the <see cref="StateService"/> class.
        /// </summary>
        /// <param name="repository">Repository used for data access.</param>
        /// <param name="logger">Logger.</param>
        public StateService(IStateRepository repository, ILogger<StateService> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc />
        public Task<State?> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                _logger.LogWarning("GetByIdAsync called with empty Guid.");
                return Task.FromResult<State?>(null);
            }

            return _repository.GetByIdAsync(id);
        }

        /// <inheritdoc />
        public Task<State?> GetByCountryIdAsync(Guid countryid)
        {
            if (countryid == Guid.Empty)
            {
                _logger.LogWarning("GetByIdAsync called with empty Guid.");
                return Task.FromResult<State?>(null);
            }

            return _repository.GetByCountryAsync(countryid);
        }

        /// <inheritdoc />
        public Task<List<State>> GetAllAsync()
        {
            return _repository.GetAllAsync();
        }

        /// <inheritdoc />
        public async Task<State> CreateAsync(State state)
        {
            if (state is null)
            {
                throw new ArgumentNullException(nameof(state));
            }

            // Business rules (example): ensure required name present
            if (string.IsNullOrWhiteSpace(state.StateName))
            {
                throw new ArgumentException("State name is required.", nameof(state));
            }

            // Ensure audit defaults
            state.CreatedAt = DateTime.UtcNow;
            if (state.Id == Guid.Empty)
            {
                state.Id = Guid.NewGuid();
            }

            var created = await _repository.CreateAsync(state).ConfigureAwait(false);
            _logger.LogInformation("Created state {StateId}", created.Id);
            return created;
        }

        /// <inheritdoc />
        public async Task<State?> UpdateAsync(State state)
        {
            if (state is null)
            {
                throw new ArgumentNullException(nameof(state));
            }

            if (state.Id == Guid.Empty)
            {
                throw new ArgumentException("State Id is required for update.", nameof(state));
            }

            var updated = await _repository.UpdateAsync(state).ConfigureAwait(false);
            if (updated is null)
            {
                _logger.LogWarning("Update failed; state not found: {StateId}", state.Id);
                return null;
            }

            _logger.LogInformation("Updated state {StateId}", updated.Id);
            return updated;
        }

        /// <inheritdoc />
        public async Task<bool> DeleteAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                _logger.LogWarning("DeleteAsync called with empty Guid.");
                return false;
            }

            var result = await _repository.DeleteAsync(id).ConfigureAwait(false);
            if (result)
            {
                _logger.LogInformation("Soft-deleted state {StateId}", id);
            }
            else
            {
                _logger.LogWarning("Soft-delete failed; state not found: {StateId}", id);
            }

            return result;
        }
    }
}
