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
        public async Task<IReadOnlyList<ModeOfStudyDto>> GetAllAsync()
        {
            _logger.LogInformation("Fetching all mode of studies");
            var modeOfStudies = await _repository.GetAllAsync();
            _logger.LogInformation("Fetched {Count} mode of studies", modeOfStudies.Count());
            return modeOfStudies.Select(Map).ToList();
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ModeOfStudyDto.</returns>
        public async Task<ModeOfStudyDto?> GetByIdAsync(Guid id)
        {
            _logger.LogInformation("Fetching mode of study by Id: {ModeOfStudyId}", id);
            var modeOfStudies = await _repository.GetByIdAsync(id);
            _logger.LogInformation("Mode of study fetched successfully for Id: {ModeOfStudyId}", id);
            return modeOfStudies == null ? null : Map(modeOfStudies);
        }

        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>ModeOfStudyDto.</returns>
        /// <exception cref="InvalidOperationException">mode of study already exists.</exception>
        public async Task<ModeOfStudyDto> CreateAsync(ModeOfStudyRequest request)
        {
            _logger.LogInformation("Creating mode of study with Code: {ModeCode}", request.code);
            if (await _repository.ExistsByCodeAsync(request.code))
            {
                _logger.LogWarning(
                "Mode of study creation failed. Code already exists: {ModeCode}",
                request.code);
                throw new InvalidOperationException("Mode of study code already exists");
            }

            var modeOfStudy = new ModeOfStudy
            {
                ModeName = request.name,
                ModeCode = request.code,
                CreatedAt = DateTime.UtcNow,
            };

            var created = await _repository.AddAsync(modeOfStudy);
            _logger.LogInformation(
           "Mode of study created successfully. ModeOfStudyId: {ModeOfStudyId}, Code: {ModeCode}",
           created.Id,
           created.ModeCode);

            return Map(created);
        }

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="request">The request.</param>
        /// <exception cref="KeyNotFoundException">mode of study not found.</exception>
        /// <returns>task.</returns>
        public async Task UpdateAsync(Guid id, ModeOfStudyRequest request)
        {
            _logger.LogInformation("Updating mode of study Id: {ModeOfStudyId}", id);
            var modeOfStudy = await _repository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException("Mode of study not found");

            modeOfStudy.ModeName = request.name;
            modeOfStudy.ModeCode = request.code;
            modeOfStudy.UpdatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(modeOfStudy);
            _logger.LogInformation(
           "Mode of study updated successfully. ModeOfStudyId: {ModeOfStudyId}",
           id);
        }

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>task.</returns>
        public async Task DeleteAsync(Guid id)
        {
            _logger.LogInformation("Deleting mode of study Id: {ModeOfStudyId}", id);
            await _repository.DeleteAsync(id);
            _logger.LogInformation(
           "Mode of study deleted successfully. ModeOfStudyId: {ModeOfStudyId}",
           id);
        }

        private static ModeOfStudyDto Map(ModeOfStudy c) =>
            new ModeOfStudyDto
            {
                Id = c.Id,
                ModeName = c.ModeName,
                ModeCode = c.ModeCode,
                StateId = c.StateId,
            };
    }
}
