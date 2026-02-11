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
    /// Provides business operations for <see cref="BatchTimingSaturday"/> entities.
    /// </summary>
    public class BatchTimingSaturdayService : IBatchTimingSaturdayService
    {
        private readonly IBatchTimingSaturdayRepository _repository;
        private readonly ILogger<BatchTimingSaturdayService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="BatchTimingSaturdayService"/> class.
        /// </summary>
        /// <param name="repository">Repository used for data access.</param>
        /// <param name="logger">Logger.</param>
        public BatchTimingSaturdayService(IBatchTimingSaturdayRepository repository, ILogger<BatchTimingSaturdayService> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>BatchTimingSaturdayDto.</returns>
        public async Task<IReadOnlyList<BatchTimingSaturdayDto>> GetAllAsync()
        {
            _logger.LogInformation("Fetching all Batches");
            var batchtiming = await _repository.GetAllAsync();
            _logger.LogInformation("Fetched {Count} batches", batchtiming.Count());
            return batchtiming.Select(Map).ToList();
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>BatchTimingSaturdayDto.</returns>
        public async Task<BatchTimingSaturdayDto?> GetByIdAsync(Guid id)
        {
            _logger.LogInformation("Fetching batchtiming by Id: {Id}", id);
            var batchtiming = await _repository.GetByIdAsync(id);
            _logger.LogInformation("batch fetched successfully for Id: {Id}", id);
            return batchtiming == null ? null : Map(batchtiming);
        }

        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>BatchTimingSaturdayDto.</returns>
        public async Task<BatchTimingSaturdayDto> CreateAsync(BatchTimingRequest request)
        {
            var batchtiming = new BatchTimingSaturday
            {
                BatchTime = request.batchTime,
                SubjectId = request.subjectId,
                CreatedBy = request.userId,
                CreatedAt = DateTime.UtcNow,
            };

            var created = await _repository.AddAsync(batchtiming);
            _logger.LogInformation(
           "Batch Timing for saturday created successfully. Id: {BatchTimingId}",
           created.Id);
            return Map(created);
        }

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="request">The request.</param>
        /// <exception cref="KeyNotFoundException">Batch Timing not found.</exception>
        /// <returns>task.</returns>
        public async Task UpdateAsync(Guid id, BatchTimingRequest request)
        {
            _logger.LogInformation("Updating batch Id: {BatchTimingId}", id);
            var batchtiming = await _repository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException("BatchTiming not found");

            batchtiming.BatchTime = request.batchTime;
            batchtiming.SubjectId = request.subjectId;
            batchtiming.UpdatedBy = request.userId;
            batchtiming.UpdatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(batchtiming);
            _logger.LogInformation(
           "BatchTiming updated successfully. BatchTimingId: {BatchTimingId}",
           id);
        }

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>task.</returns>
        public async Task DeleteAsync(Guid id)
        {
            _logger.LogInformation("Deleting BatchTiming Id: {Id}", id);
            await _repository.DeleteAsync(id);
            _logger.LogInformation(
           "BatchTiming deleted successfully. BatchTimingId: {BatchTimingId}",
           id);
        }


        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="subjectid">The identifier.</param>
        /// <returns>BatchTimingSaturdayDto.</returns>
        public async Task<List<BatchTimingSaturdayDto>> GetBySubjectIdAsync(Guid subjectid)
        {
            var batchTimings = await _repository.GetBySubjectIdAsync(subjectid);
            return Map(batchTimings);
        }

        private static List<BatchTimingSaturdayDto> Map(IEnumerable<BatchTimingSaturday> batchTimingMTFs)
        {
            return batchTimingMTFs.Select(Map).ToList();
        }

        private static BatchTimingSaturdayDto Map(BatchTimingSaturday c) =>
    new BatchTimingSaturdayDto
    {
        Id = c.Id,
        BatchTime = c.BatchTime,
        SubjectId = c.SubjectId,
        CreatedBy = c.CreatedBy,
        CreatedAt = c.CreatedAt,
        UpdatedAt = c.UpdatedAt,
        UpdatedBy = c.UpdatedBy,
        IsActive = c.IsActive,
        Subject = c.Subject == null ? null : new SubjectDto
        {
            Id = c.Subject.Id,
            SubjectName = c.Subject.SubjectName,
            SubjectCode = c.Subject.SubjectCode          
        }
    };
    }
}
