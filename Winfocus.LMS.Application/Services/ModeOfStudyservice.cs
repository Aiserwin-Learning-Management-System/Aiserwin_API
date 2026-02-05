using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Winfocus.LMS.Application.Interfaces;
using Winfocus.LMS.Domain.Entities;

namespace Winfocus.LMS.Application.Services
{
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

        /// <inheritdoc />
        public Task<ModeOfStudy?> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                _logger.LogWarning("GetByIdAsync called with empty Guid.");
                return Task.FromResult<ModeOfStudy?>(null);
            }

            return _repository.GetByIdAsync(id);
        }

        /// <inheritdoc />
        public Task<List<ModeOfStudy>> GetAllAsync()
        {
            return _repository.GetAllAsync();
        }

        /// <inheritdoc />
        public async Task<ModeOfStudy> CreateAsync(ModeOfStudy mode)
        {
            if (mode is null)
            {
                throw new ArgumentNullException(nameof(mode));
            }

            if (string.IsNullOrWhiteSpace(mode.ModeName))
            {
                throw new ArgumentException("ModeOfStudy name is required.", nameof(mode));
            }

            mode.CreatedAt = DateTime.UtcNow;
            mode.Id = mode.Id == Guid.Empty ? Guid.NewGuid() : mode.Id;

            var created = await _repository.CreateAsync(mode).ConfigureAwait(false);
            _logger.LogInformation("Created mode of study {ModeId}", created.Id);
            return created;
        }

        /// <inheritdoc />
        public async Task<ModeOfStudy?> UpdateAsync(ModeOfStudy mode)
        {
            if (mode is null)
            {
                throw new ArgumentNullException(nameof(mode));
            }

            if (mode.Id == Guid.Empty)
            {
                throw new ArgumentException("ModeOfStudy Id is required for update.", nameof(mode));
            }

            var updated = await _repository.UpdateAsync(mode).ConfigureAwait(false);
            if (updated is null)
            {
                _logger.LogWarning("Update failed; mode not found: {ModeId}", mode.Id);
                return null;
            }

            _logger.LogInformation("Updated mode of study {ModeId}", updated.Id);
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
                _logger.LogInformation("Soft-deleted mode of study {ModeId}", id);
            }
            else
            {
                _logger.LogWarning("Soft-delete failed; mode not found: {ModeId}", id);
            }

            return result;
        }
    }
}
