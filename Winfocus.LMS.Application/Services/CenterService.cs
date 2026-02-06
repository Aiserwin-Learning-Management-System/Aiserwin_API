using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Winfocus.LMS.Application.DTOs;
using Winfocus.LMS.Application.DTOs.Masters;
using Winfocus.LMS.Application.Interfaces;
using Winfocus.LMS.Domain.Entities;
using Winfocus.LMS.Domain.Enums;

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

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>CeneterDto.</returns>
        public async Task<IReadOnlyList<CentreDto>> GetAllAsync()
        {
            _logger.LogInformation("Fetching all Ceneters");
            var centres = await _repository.GetAllAsync();
            _logger.LogInformation("Fetched {Count} centres", centres.Count());
            return centres.Select(Map).ToList();
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>CenterDto.</returns>
        public async Task<CentreDto?> GetByIdAsync(Guid id)
        {
            _logger.LogInformation("Fetching centre by Id: {CentreId}", id);
            var centre = await _repository.GetByIdAsync(id);
            if (centre != null)
            {
                _logger.LogWarning("Centre not found for Id: {CentreId}", id);
            }
            else
            {
                _logger.LogInformation("Centre fetched successfully for Id: {CentreId}", id);
            }
            return centre == null ? null : Map(centre);
        }

        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>CenterDto.</returns>
        /// <exception cref="InvalidOperationException">Center code already exists.</exception>
        public async Task<CentreDto> CreateAsync(CenterRequestDto request)
        {
            _logger.LogInformation("Creating centre with Code: {CentreCode}", request.code);
            if (await _repository.ExistsByCodeAsync(request.code))
            {
                _logger.LogWarning("Centre creation failed. Code already exists: {CentreCode}", request.code);
                throw new InvalidOperationException("Center code already exists");
            }

            var centre = new Centre
            {
                Name = request.name,
                Code = request.code,
                CreatedAt = DateTime.UtcNow,
            };

            var created = await _repository.AddAsync(centre);
            _logger.LogInformation("Centre created successfully. CentreId: {CentreId}, Code: {CentreCode}", created.Id, created.Code);
            return Map(created);
        }

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="request">The request.</param>
        /// <exception cref="KeyNotFoundException">Center not found.</exception>
        /// <returns>task.</returns>
        public async Task UpdateAsync(Guid id, CenterRequestDto request)
        {
            _logger.LogInformation("Updating centre Id: {CentreId}", id);
            var center = await _repository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException("Center not found");

            center.Name = request.name;
            center.Code = request.code;
            center.UpdatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(center);
            _logger.LogInformation("Centre updated successfully. CentreId: {CentreId}", id);
        }

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>task.</returns>
        public async Task DeleteAsync(Guid id)
        {
            _logger.LogInformation("Deleting centre Id: {CentreId}", id);
            await _repository.DeleteAsync(id);
            _logger.LogInformation("Centre deleted successfully. CentreId: {CentreId}", id);
        }

        private static CentreDto Map(Centre c)
        {
            return new CentreDto(
                c.Id,
                c.Name,
                c.Code
            );
        }


    }
}
