namespace Winfocus.LMS.Application.Services
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Extensions.Logging;
    using Winfocus.LMS.Application.DTOs;
    using Winfocus.LMS.Application.DTOs.Masters;
    using Winfocus.LMS.Application.Interfaces;
    using Winfocus.LMS.Domain.Entities;

    /// <summary>
    /// StreamService.
    /// </summary>
    public sealed class StreamService
    {
        private readonly IStreamRepository _repository;
        private readonly ILogger<StreamService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="StreamService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="logger">The logger.</param>
        public StreamService(
            IStreamRepository repository,
            ILogger<StreamService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>StreamDto.</returns>
        public async Task<IReadOnlyList<StreamDto>> GetAllAsync()
        {
            _logger.LogInformation("Fetching all syllabuses");
            var streams = await _repository.GetAllAsync();
            return streams.Select(Map).ToList();
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>StreamDto.</returns>
        public async Task<StreamDto?> GetByIdAsync(Guid id)
        {
            var streams = await _repository.GetByIdAsync(id);
            return streams == null ? null : Map(streams);
        }

        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>StreamDto.</returns>
        /// <exception cref="InvalidOperationException">stream code already exists.</exception>
        public async Task<StreamDto> CreateAsync(StreamRequest request)
        {
            if (await _repository.ExistsByCodeAsync(request.code))
            {
                throw new InvalidOperationException("Stream code already exists");
            }

            var stream = new Streams
            {
                StreamName = request.name,
                StreamCode = request.code,
                CreatedAt = DateTime.UtcNow,
            };

            var created = await _repository.AddAsync(stream);
            return Map(created);
        }

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="request">The request.</param>
        /// <exception cref="KeyNotFoundException">Stream not found.</exception>
        /// <returns>task.</returns>
        public async Task UpdateAsync(Guid id, StreamRequest request)
        {
            var grade = await _repository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException("Stream not found");

            grade.StreamName = request.name;
            grade.StreamCode = request.code;
            grade.UpdatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(grade);
        }

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>task.</returns>
        public async Task DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="gradeid">The identifier.</param>
        /// <returns>StreamDto.</returns>
        public async Task<StreamDto?> GetByGradeIdAsync(Guid gradeid)
        {
            var streams = await _repository.GetByGradeIdAsync(id);
            return streams == null ? null : Map(streams);
        }

        private static StreamDto Map(Streams c) =>
           new StreamDto
           {
               Id = c.Id,
               StreamName = c.StreamName,
               StreamCode = c.StreamCode,
               GradeId = c.GradeId,
           };
    }
}
