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
    /// Provides business operations for <see cref="BatchTimingSunday"/> entities.
    /// </summary>
    public class BatchTimingSundayService : IBatchTimingSundayService
    {
        private readonly IBatchTimingSundayRepository _repository;
        private readonly ILogger<BatchTimingSundayService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="BatchTimingSundayService"/> class.
        /// </summary>
        /// <param name="repository">Repository used for data access.</param>
        /// <param name="logger">Logger.</param>
        public BatchTimingSundayService(IBatchTimingSundayRepository repository, ILogger<BatchTimingSundayService> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>BatchTimingSundayDto.</returns>
        public async Task<IReadOnlyList<BatchTimingSundayDto>> GetAllAsync()
        {
            _logger.LogInformation("Fetching all Batches");
            var batchtiming = await _repository.GetAllAsync();
            _logger.LogInformation("Fetched {Count} Batches", batchtiming.Count());
            return batchtiming.Select(Map).ToList();
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>BatchTimingMTFDto.</returns>
        public async Task<BatchTimingSundayDto?> GetByIdAsync(Guid id)
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
        /// <returns>BatchTimingMTFDto.</returns>
        /// <exception cref="InvalidOperationException">batch code already exists. </exception>
        public async Task<BatchTimingSundayDto> CreateAsync(BatchTimingRequest request)
        {
            var batchtiming = new BatchTimingSunday
            {
                BatchTime = request.batchTime,
                SubjectId = request.subjectId,
            };

            var created = await _repository.AddAsync(batchtiming);
            _logger.LogInformation(
           "Batch Timing for monaday to friday created successfully. Id: {BatchTimingId}",
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

        private static BatchTimingSundayDto Map(BatchTimingSunday c) =>
   new BatchTimingSundayDto
   {
       Id = c.Id,
       BatchTime = c.BatchTime,
       SubjectId = c.SubjectId,
       Subject = c.Subject == null ? null : new SubjectDto
       {
           Id = c.Subject.Id,
           SubjectName = c.Subject.SubjectName,
           SubjectCode = c.Subject.SubjectCode,
           CourseId = c.Subject.CourseId,
           Course = c.Subject.Course == null ? null : new CourseDto
           {
               CourseCode = c.Subject.Course.CourseCode
           }
       }
   };
    }
}
