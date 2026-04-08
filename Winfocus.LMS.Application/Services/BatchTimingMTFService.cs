namespace Winfocus.LMS.Application.Services
{
    using Microsoft.AspNetCore.Http.HttpResults;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Winfocus.LMS.Application.DTOs;
    using Winfocus.LMS.Application.DTOs.Common;
    using Winfocus.LMS.Application.DTOs.Masters;
    using Winfocus.LMS.Application.DTOs.Students;
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
        /// <param name="centerId">The centerId.</param>
        /// <returns>BatchTimingMTFDto.</returns>
        public async Task<CommonResponse<List<BatchTimingMTFDto>>> GetAllAsync(Guid centerId)
        {
            _logger.LogInformation("Fetching all Batches monday to friday");
            var batchtiming = await _repository.GetAllAsync(centerId);
            _logger.LogInformation("Fetched {Count} Batches monday to friday", batchtiming.Count());
            var mappeddata = batchtiming.Select(Map).ToList();
            if (mappeddata.Any())
            {
                return CommonResponse<List<BatchTimingMTFDto>>.SuccessResponse("batch timing monday to friday", mappeddata);
            }
            else
            {
                return CommonResponse<List<BatchTimingMTFDto>>.FailureResponse("no batch timing found");
            }
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>BatchTimingMTFDto.</returns>
        public async Task<CommonResponse<BatchTimingMTFDto>> GetByIdAsync(Guid id)
        {
            _logger.LogInformation("Fetching batchtiming by Id: {Id}", id);
            var batchtiming = await _repository.GetByIdAsync(id);
            _logger.LogInformation("batch fetched successfully for Id: {Id}", id);
            var mappeddata = batchtiming == null ? null : Map(batchtiming);
            if (mappeddata != null)
            {
                return CommonResponse<BatchTimingMTFDto>.SuccessResponse("batch timing monday to friday", mappeddata);
            }
             else
            {
                return CommonResponse<BatchTimingMTFDto>.FailureResponse("batch timing not found");
            }
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="centerId">The centerId.</param>
        /// <returns>BatchTimingMTFDto.</returns>
        public async Task<CommonResponse<BatchTimingMTFDto>> GetByIdCenterIdAsync(Guid id, Guid centerId)
        {
            _logger.LogInformation("Fetching batchtiming by Id: {Id}", id);
            var batchtiming = await _repository.GetByIdCenterIdAsync(id,centerId);
            _logger.LogInformation("batch fetched successfully for Id: {Id}", id);
            var mappeddata = batchtiming == null ? null : Map(batchtiming);
            if (mappeddata != null)
            {
                return CommonResponse<BatchTimingMTFDto>.SuccessResponse("batch timing monday to friday", mappeddata);
            }
            else
            {
                return CommonResponse<BatchTimingMTFDto>.FailureResponse("batch timing not found");
            }
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
        public async Task<BatchTimingMTFDto> UpdateAsync(Guid id, BatchTimingRequest request)
        {
            _logger.LogInformation("Updating batch Id: {BatchTimingId}", id);
            var batchtiming = await _repository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException("BatchTiming not found");

            batchtiming.BatchTime = request.batchTime;
            batchtiming.SubjectId = request.subjectId;
            batchtiming.UpdatedBy = request.userId;
            batchtiming.UpdatedAt = DateTime.UtcNow;

            var updated = await _repository.UpdateAsync(batchtiming);
            _logger.LogInformation(
           "BatchTiming updated successfully. BatchTimingId: {BatchTimingId}",
           id);
            return Map(updated);
        }

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="centerId">The centerId.</param>
        /// <returns>task.</returns>
        public async Task<bool> DeleteAsync(Guid id, Guid centerId)
        {
            _logger.LogInformation("Deleting BatchTiming Id: {Id}", id);
            bool res = await _repository.DeleteAsync(id,centerId);
            if (res)
            {
                _logger.LogInformation("BatchTiming deleted successfully. BatchTimingId: {BatchTimingId}", id);
                return res;
            }
            else
            {
                _logger.LogWarning("BatchTiming deletion failed. BatchTimingId: {BatchTimingId}", id);
                return res;
            }
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

        /// <summary>
        /// Gets filtered batch timing for monday to frida with pagination support.
        /// Search works on Subject name, Course Name, Stream Name, Grade Name, and Syllabus Name.
        /// </summary>
        /// <param name="request">The paged request.</param>
        /// <param name="centerId">The centerId.</param>
        /// <returns>Paginated batch timing for monday to frida result.</returns>
        public async Task<CommonResponse<PagedResult<BatchTimingMTFDto>>> GetFilteredAsync(
            PagedRequest request, Guid centerId)
        {
            try
            {
                _logger.LogInformation(
                    "Fetching filtered batch timings. Filters => Active:{Active}, " +
                    "Search:{SearchText}, SortBy:{SortBy}, SortOrder:{SortOrder}, " +
                    "Limit:{Limit}, Offset:{Offset}",
                    request.Active, request.SearchText, request.SortBy,
                    request.SortOrder, request.Limit, request.Offset);

                var query = _repository.Query(centerId);

                // ── Filters ──
                if (request.Active.HasValue)
                    query = query.Where(x => x.IsActive == request.Active.Value);

                if (request.StartDate.HasValue)
                    query = query.Where(x => x.CreatedAt >= request.StartDate.Value);

                if (request.EndDate.HasValue)
                    query = query.Where(x => x.CreatedAt <= request.EndDate.Value);

                // ── Search on Subject, Course, Stream, Grade, AND Syllabus Name ──
                if (!string.IsNullOrWhiteSpace(request.SearchText))
                {
                    var searchTerm = request.SearchText.Trim().ToLower();
                    query = query.Where(x =>
                        x.BatchTime.ToString().Contains(searchTerm) ||
                        x.Subject.Name.ToLower().Contains(searchTerm) ||
                        x.Subject.Course.Name.ToLower().Contains(searchTerm) ||
                        x.Subject.Course.Stream.Name.ToLower().Contains(searchTerm) ||
                        x.Subject.Course.Stream.Grade.Name.ToLower().Contains(searchTerm) ||
                        x.Subject.Course.Stream.Grade.Syllabus.Name.ToLower().Contains(searchTerm));
                }

                // ── Total Count ──
                var totalCount = await query.CountAsync();

                if (totalCount == 0)
                {
                    return CommonResponse<PagedResult<BatchTimingMTFDto>>.SuccessResponse(
                        "No batch timings found.",
                        new PagedResult<BatchTimingMTFDto>(
                            new List<BatchTimingMTFDto>(), 0, request.Limit, request.Offset));
                }

                // ── Dynamic Sorting ──
                var isDesc = request.SortOrder.Equals("desc", StringComparison.OrdinalIgnoreCase);

                query = request.SortBy.ToLower() switch
                {
                    "name" => isDesc ? query.OrderByDescending(x => x.BatchTime)
                                             : query.OrderBy(x => x.BatchTime),
                    "subjectname" => isDesc ? query.OrderByDescending(x => x.Subject.Name)
                                             : query.OrderBy(x => x.Subject.Name),

                    "coursename" => isDesc ? query.OrderByDescending(x => x.Subject.Course.Name)
                                             : query.OrderBy(x => x.Subject.Course.Name),

                    "streamname" => isDesc ? query.OrderByDescending(x => x.Subject.Course.Stream.Name)
                                             : query.OrderBy(x => x.Subject.Course.Stream.Name),

                    "gradename" => isDesc ? query.OrderByDescending(x => x.Subject.Course.Stream.Grade.Name)
                                             : query.OrderBy(x => x.Subject.Course.Stream.Grade.Name),

                    "syllabusname" => isDesc ? query.OrderByDescending(x => x.Subject.Course.Stream.Grade.Syllabus.Name)
                                             : query.OrderBy(x => x.Subject.Course.Stream.Grade.Syllabus.Name),

                    "isactive" => isDesc ? query.OrderByDescending(x => x.IsActive)
                                             : query.OrderBy(x => x.IsActive),

                    "createdat" => isDesc ? query.OrderByDescending(x => x.CreatedAt)
                                             : query.OrderBy(x => x.CreatedAt),

                    _ => isDesc ? query.OrderByDescending(x => x.CreatedAt)
                                             : query.OrderBy(x => x.CreatedAt),
                };

                // ── Pagination ──
                var subjects = await query
                    .Skip(request.Offset)
                    .Take(request.Limit)
                    .ToListAsync();

                var dtoList = subjects.Select(Map).ToList();

                _logger.LogInformation(
                    "Returning {Count} of {Total} batch timing for monday to friday",
                    dtoList.Count, totalCount);

                return CommonResponse<PagedResult<BatchTimingMTFDto>>.SuccessResponse(
                    "batch timings fetched successfully.",
                    new PagedResult<BatchTimingMTFDto>(
                        dtoList, totalCount, request.Limit, request.Offset));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching filtered batch timings.");
                return CommonResponse<PagedResult<BatchTimingMTFDto>>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        private static List<BatchTimingMTFDto> Map(IEnumerable<BatchTimingMTF> batchTimingMTFs)
        {
            return batchTimingMTFs.Select(Map).ToList();
        }

        private static BatchTimingMTFDto Map(BatchTimingMTF c) =>
    new BatchTimingMTFDto
    {
        Id = c.Id,
        BatchTime = DateTime.SpecifyKind(c.BatchTime, DateTimeKind.Utc).ToString("yyyy-MM-ddTHH:mm:ss.fff'Z'"),
        BatchTimeDisplay = DateTime.SpecifyKind(c.BatchTime, DateTimeKind.Utc).ToString("hh:mm tt"),
        SubjectId = c.SubjectId,
        IsActive = c.IsActive,
        Subject = c.Subject == null ? null : new SubjectDto
        {
            Id = c.Subject.Id,
            Name = c.Subject.Name,
            CourseId = c.Subject.CourseId,
            Course = c.Subject.Course == null ? null : new CourseDto
            {
                Id = c.Subject.Course.Id,
                Name = c.Subject.Course.Name,
                IsActive = c.Subject.Course.IsActive,
                StreamId = c.Subject.Course.StreamId,
                Stream = c.Subject.Course.Stream == null ? null : new StreamDto
                {
                    Id = c.Subject.Course.Stream.Id,
                    Name = c.Subject.Course.Stream.Name,
                    IsActive = c.Subject.Course.Stream.IsActive,
                    GradeId = c.Subject.Course.Stream.GradeId,
                    Grade = c.Subject.Course.Stream.Grade == null ? null : new GradeDto
                    {
                        Id = c.Subject.Course.Stream.Grade.Id,
                        Name = c.Subject.Course.Stream.Grade.Name,
                        IsActive = c.Subject.Course.Stream.Grade.IsActive,
                        SyllabusId = c.Subject.Course.Stream.Grade.SyllabusId,
                        Syllabus = c.Subject.Course.Stream.Grade.Syllabus == null ? null : new SyllabusDto
                        {
                            Id = c.Subject.Course.Stream.Grade.Syllabus.Id,
                            Name = c.Subject.Course.Stream.Grade.Syllabus.Name,
                            IsActive = c.Subject.Course.Stream.Grade.Syllabus.IsActive
                        }
                    }
                }
            }
        }
    };
    }
}
