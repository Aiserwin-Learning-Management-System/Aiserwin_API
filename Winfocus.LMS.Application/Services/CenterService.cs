using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Winfocus.LMS.Application.Interfaces;
using Winfocus.LMS.Domain.Entities;

namespace Winfocus.LMS.Application.Services
{
    /// <summary>
    /// Provides business operations for <see cref="Centre"/> entities.
    /// </summary>
    public sealed class CentreService : ICentreService
    {
        private readonly ICentreRepository _repository;
        private readonly ILogger<CentreService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="CentreService"/> class.
        /// </summary>
        /// <param name="repository">Repository used for data access.</param>
        /// <param name="logger">Logger instance.</param>
        public CentreService(ICentreRepository repository, ILogger<CentreService> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc />
        public Task<Centre?> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                _logger.LogWarning("GetByIdAsync called with empty Guid.");
                return Task.FromResult<Centre?>(null);
            }

            return _repository.GetByIdAsync(id);
        }

        /// <inheritdoc />
        public Task<List<Centre>> GetAllAsync()
        {
            return _repository.GetAllAsync();
        }

        /// <inheritdoc />
        public async Task<Centre> CreateAsync(Centre centre)
        {
            if (centre is null)
            {
                throw new ArgumentNullException(nameof(centre));
            }

            // Basic business validation: require a name
            if (string.IsNullOrWhiteSpace(centre.Name))
            {
                throw new ArgumentException("Centre name is required.", nameof(centre));
            }

            centre.CreatedAt = DateTime.UtcNow;
            if (centre.Id == Guid.Empty)
            {
                centre.Id = Guid.NewGuid();
            }

            var created = await _repository.CreateAsync(centre).ConfigureAwait(false);
            _logger.LogInformation("Created centre {CentreId}", created.Id);
            return created;
        }

        /// <inheritdoc />
        public async Task<Centre?> UpdateAsync(Centre centre)
        {
            if (centre is null)
            {
                throw new ArgumentNullException(nameof(centre));
            }

            if (centre.Id == Guid.Empty)
            {
                throw new ArgumentException("Centre Id is required for update.", nameof(centre));
            }

            var updated = await _repository.UpdateAsync(centre).ConfigureAwait(false);
            if (updated is null)
            {
                _logger.LogWarning("Update failed; centre not found: {CentreId}", centre.Id);
                return null;
            }

            _logger.LogInformation("Updated centre {CentreId}", updated.Id);
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
                _logger.LogInformation("Soft-deleted centre {CentreId}", id);
            }
            else
            {
                _logger.LogWarning("Soft-delete failed; centre not found: {CentreId}", id);
            }

            return result;
        }
    }
}
