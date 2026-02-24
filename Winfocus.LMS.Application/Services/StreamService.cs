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
    public sealed class StreamService : IStreamService
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
        public async Task<CommonResponse<List<StreamDto>>> GetAllAsync()
        {
            _logger.LogInformation("Fetching all streams");
            var streams = await _repository.GetAllAsync();
            var mappeddata = streams.Select(Map).ToList();
            if (mappeddata.Any())
            {
                return CommonResponse<List<StreamDto>>.SuccessResponse("Fetched all streams", mappeddata);
            }
            else
            {
                return CommonResponse<List<StreamDto>>.FailureResponse("Streams not found");
            }
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>StreamDto.</returns>
        public async Task<CommonResponse<StreamDto>> GetByIdAsync(Guid id)
        {
            var streams = await _repository.GetByIdAsync(id);
            var mappeddata = streams == null ? null : Map(streams);
            if (mappeddata != null)
            {
                return CommonResponse<StreamDto>.SuccessResponse("Get Stream by id ", mappeddata);
            }
            else
            {
                return CommonResponse<StreamDto>.FailureResponse("Stream not found");
            }
        }

        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>StreamDto.</returns>
        /// <exception cref="InvalidOperationException">stream code already exists.</exception>
        /// <summary>
        /// Creates a new Stream along with its Course mappings.
        /// </summary>
        public async Task<StreamDto> CreateAsync(StreamRequest request)
        {
            var stream = new Streams
            {
                Name = request.name,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = request.userId,
                GradeId = request.gradeid,
                Courses = new List<Course>(),
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
        public async Task<StreamDto> UpdateAsync(Guid id, StreamRequest request)
        {
            var stream = await _repository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException("Stream not found");

            stream.Name = request.name;
            stream.GradeId = request.gradeid;
            stream.UpdatedBy = request.userId;
            stream.UpdatedAt = DateTime.UtcNow;

            return Map(await _repository.UpdateAsync(stream));
        }

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>task.</returns>
        public async Task<bool> DeleteAsync(Guid id)
        {
           return await _repository.DeleteAsync(id);
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="gradeid">The identifier.</param>
        /// <returns>StreamDto.</returns>
        public async Task<List<StreamDto>> GetByGradeIdAsync(Guid gradeid)
        {
            var streams = await _repository.GetByGradeIdAsync(gradeid);
            return Map(streams);
        }

        private static List<StreamDto> Map(IEnumerable<Streams> streams)
        {
            return streams.Select(Map).ToList();
        }

        private static StreamDto Map(Streams c) =>
         new StreamDto
         {
             Id = c.Id,
             Name = c.Name,
             GradeId = c.GradeId,
             CreatedBy = c.CreatedBy,
             CreatedAt = c.CreatedAt,
             UpdatedBy = c.UpdatedBy,
             UpdatedAt = c.UpdatedAt,
         };
    }
}
