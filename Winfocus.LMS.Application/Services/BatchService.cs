using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using Winfocus.LMS.Application.DTOs;
using Winfocus.LMS.Application.DTOs.Masters;
using Winfocus.LMS.Application.Interfaces;
using Winfocus.LMS.Domain.Entities;

namespace Winfocus.LMS.Application.Services
{
    /// <summary>
    /// Provides business operations for <see cref="Batch"/> entities.
    /// </summary>
    public class BatchService : IBatchService
    {
        private readonly IBatchRepository _repository;
        private readonly ILogger<BatchService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="BatchService"/> class.
        /// </summary>
        /// <param name="repository">Repository used for data access.</param>
        /// <param name="logger">Logger.</param>
        public BatchService(IBatchRepository repository, ILogger<BatchService> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>BatchDto.</returns>
        public async Task<IReadOnlyList<BatchDto>> GetAllAsync()
        {
            _logger.LogInformation("Fetching all Batches");
            var batch = await _repository.GetAllAsync();
            _logger.LogInformation("Fetched {Count} Batches", batch.Count());
            return batch.Select(Map).ToList();
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>BatchTimingMTFDto.</returns>
        public async Task<BatchDto?> GetByIdAsync(Guid id)
        {
            _logger.LogInformation("Fetching batch by Id: {Id}", id);
            var batch = await _repository.GetByIdAsync(id);
            _logger.LogInformation("batch fetched successfully for Id: {Id}", id);
            return batch == null ? null : Map(batch);
        }

        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>BatchDto.</returns>
        public async Task<BatchDto> CreateAsync(BatchRequest request)
        {
            var batchtiming = new Batch
            {
                Name = request.name,
                SubjectId = request.subjectId,
                CreatedBy = request.userId,
                CreatedAt = DateTime.UtcNow,
            };

            var created = await _repository.AddAsync(batchtiming);
            _logger.LogInformation(
           "Batch for monaday to friday created successfully. Id: {Id}",
           created.Id);
            return Map(created);
        }

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="request">The request.</param>
        /// <exception cref="KeyNotFoundException">Batch not found.</exception>
        /// <returns>task.</returns>
        public async Task UpdateAsync(Guid id, BatchRequest request)
        {
            _logger.LogInformation("Updating batch Id: {BatchId}", id);
            var batchtiming = await _repository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException("Batch not found");

            batchtiming.Name = request.name;
            batchtiming.SubjectId = request.subjectId;
            batchtiming.UpdatedBy = request.userId;
            batchtiming.UpdatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(batchtiming);
            _logger.LogInformation(
           "Batch updated successfully. BatchId: {BatchId}",
           id);
        }

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>task.</returns>
        public async Task DeleteAsync(Guid id)
        {
            _logger.LogInformation("Deleting Batch Id: {Id}", id);
            await _repository.DeleteAsync(id);
            _logger.LogInformation(
           "Batch deleted successfully. BatchId: {BatchId}",
           id);
        }

        private static BatchDto Map(Batch c) =>
   new BatchDto
   {
       Id = c.Id,
       Name = c.Name,
       SubjectId = c.SubjectId,
       CreatedAt = c.CreatedAt,
       CreatedBy = c.CreatedBy,
       UpdatedAt = c.UpdatedAt,
       UpdatedBy = c.UpdatedBy,
       IsActive = c.IsActive,
       Subject = c.Subject == null ? null : new SubjectDto
       {
           Id = c.Subject.Id,
           Name = c.Subject.Name,
       }
   };
    }
}
