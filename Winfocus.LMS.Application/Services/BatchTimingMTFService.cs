namespace Winfocus.LMS.Application.Services
{
    using Microsoft.Extensions.Logging;
    using Winfocus.LMS.Application.DTOs;
    using Winfocus.LMS.Application.DTOs.Masters;
    using Winfocus.LMS.Application.Interfaces;
    using Winfocus.LMS.Domain.Entities;

    /// <summary>
    /// Provides business operations for <see cref="BatchTimingMTF"/> entities.
    /// </summary>
    public class BatchTimingMTFService : IBatchTimingMTFService
    {
        private readonly IBatchTimingMTFRepository _repository;
        private readonly ILogger<BatchTimingMTFService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="BatchTimingMTFService"/> class.
        /// </summary>
        /// <param name="repository">Repository used for data access.</param>
        /// <param name="logger">Logger.</param>
        public BatchTimingMTFService(IBatchTimingMTFRepository repository, ILogger<BatchTimingMTFService> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>BatchTimingMTFDto.</returns>
        public async Task<IReadOnlyList<BatchTimingMTFDto>> GetAllAsync()
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
        public async Task<BatchTimingMTFDto?> GetByIdAsync(Guid id)
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
        public async Task<BatchTimingMTFDto> CreateAsync(BatchTimingRequest request)
        {
            var batchtiming = new BatchTimingMTF
            {
                BatchTime = request.batchTime,
                SubjectId = request.subjectId,
                CreatedBy = request.userId,
                CreatedAt = DateTime.UtcNow,
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
        /// <returns>BatchTimingMTFDto.</returns>
        public async Task<List<BatchTimingMTFDto>> GetBySubjectIdAsync(Guid subjectid)
        {
            var batchTimings = await _repository.GetBySubjectIdAsync(subjectid);
            return Map(batchTimings);
        }

        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>BatchTimingMTFDto.</returns>
        /// <exception cref="InvalidOperationException">batch code already exists. </exception>
        public async Task BatchTimingSubjectCreate(SubjectBatchTimingRequest request)
        {
            var batchtiming = new SubjectBatchTimingMTF
            {
                SubjectId = request.subjectId,
                SubjectBatchTimingMTFs = request.Batchtimingids
                    .Distinct()
                    .Select(id => new SubjectBatchTimingMTF { SubjectId = id })
                    .ToList(),
            };

            await _repository.BatchTimingSubjectCreate(batchtiming);
            _logger.LogInformation(
           "Batch Timing for monaday to friday for subject created successfully.");
        }

        private static List<BatchTimingMTFDto> Map(IEnumerable<BatchTimingMTF> batchTimingMTFs)
        {
            return batchTimingMTFs.Select(Map).ToList();
        }

        private static BatchTimingMTFDto Map(BatchTimingMTF c) =>
    new BatchTimingMTFDto
    {
        Id = c.Id,
        BatchTime = c.BatchTime.ToString("dd/MM/yyyy hh:mm tt"),
        SubjectId = c.SubjectId,
        CreatedAt = c.CreatedAt,
        CreatedBy = c.CreatedBy,
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
