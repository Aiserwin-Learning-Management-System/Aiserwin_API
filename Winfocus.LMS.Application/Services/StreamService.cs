namespace Winfocus.LMS.Application.Services
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Winfocus.LMS.Application.DTOs;
    using Winfocus.LMS.Application.DTOs.Common;
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
        /// <returns>StreamDto list.</returns>
        public async Task<CommonResponse<List<StreamDto>>> GetAllAsync()
        {
            _logger.LogInformation("Fetching all streams");
            var streams = await _repository.GetAllAsync();
            var mapped = streams.Select(Map).ToList();

            return CommonResponse<List<StreamDto>>.SuccessResponse(
                mapped.Any() ? "Fetched all streams" : "No streams found",
                mapped);
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>StreamDto.</returns>
        public async Task<CommonResponse<StreamDto>> GetByIdAsync(Guid id)
        {
            _logger.LogInformation("Fetching stream by Id: {Id}", id);
            var stream = await _repository.GetByIdAsync(id);

            if (stream == null)
            {
                return CommonResponse<StreamDto>.FailureResponse("Stream not found");
            }

            return CommonResponse<StreamDto>.SuccessResponse(
                "Stream fetched successfully", Map(stream));
        }

        /// <summary>
        /// Creates the asynchronous with duplicate check.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>StreamDto.</returns>
        public async Task<StreamDto> CreateAsync(StreamRequest request)
        {
            var existingStreams = await _repository.GetByGradeIdAsync(request.gradeid);
            var isDuplicate = existingStreams.Any(s =>
                s.Name.Equals(request.name, StringComparison.OrdinalIgnoreCase));

            if (isDuplicate)
            {
                throw new InvalidOperationException(
                    $"Stream '{request.name}' already exists under this grade.");
            }

            var stream = new Streams
            {
                Name = request.name,
                GradeId = request.gradeid,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = request.userId,
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
        /// <returns>StreamDto.</returns>
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
        /// <returns>bool.</returns>
        public async Task<bool> DeleteAsync(Guid id)
        {
            return await _repository.DeleteAsync(id);
        }

        /// <summary>
        /// Gets streams by grade identifier.
        /// </summary>
        /// <param name="gradeid">The grade identifier.</param>
        /// <returns>StreamDto list.</returns>
        public async Task<CommonResponse<List<StreamDto>>> GetByGradeIdAsync(Guid gradeid)
        {
            _logger.LogInformation("Fetching streams by GradeId: {GradeId}", gradeid);
            var streams = await _repository.GetByGradeIdAsync(gradeid);
            var mapped = streams.Select(Map).ToList();

            return CommonResponse<List<StreamDto>>.SuccessResponse(
                mapped.Any()
                    ? "Streams fetched successfully"
                    : "No streams found for this grade",
                mapped);
        }

        /// <summary>
        /// Gets filtered streams with pagination support.
        /// Search works on Stream Name, Grade Name, and Syllabus Name.
        /// </summary>
        /// <param name="request">The paged request.</param>
        /// <returns>Paginated stream result.</returns>
        public async Task<CommonResponse<PagedResult<StreamDto>>> GetFilteredAsync(
            PagedRequest request)
        {
            try
            {
                _logger.LogInformation(
                    "Fetching filtered streams. Filters => Active:{Active}, " +
                    "Search:{SearchText}, SortBy:{SortBy}, SortOrder:{SortOrder}, " +
                    "Limit:{Limit}, Offset:{Offset}",
                    request.Active, request.SearchText, request.SortBy,
                    request.SortOrder, request.Limit, request.Offset);

                var query = _repository.Query();

                // ── Filters ──
                if (request.Active.HasValue)
                    query = query.Where(x => x.IsActive == request.Active.Value);

                if (request.StartDate.HasValue)
                    query = query.Where(x => x.CreatedAt >= request.StartDate.Value);

                if (request.EndDate.HasValue)
                    query = query.Where(x => x.CreatedAt <= request.EndDate.Value);

                // ── Search on Stream Name, Grade Name, AND Syllabus Name ──
                if (!string.IsNullOrWhiteSpace(request.SearchText))
                {
                    var searchTerm = request.SearchText.Trim().ToLower();
                    query = query.Where(x =>
                        x.Name.ToLower().Contains(searchTerm) ||
                        x.Grade.Name.ToLower().Contains(searchTerm) ||
                        x.Grade.Syllabus.Name.ToLower().Contains(searchTerm));
                }

                // ── Total Count ──
                var totalCount = await query.CountAsync();

                if (totalCount == 0)
                {
                    return CommonResponse<PagedResult<StreamDto>>.SuccessResponse(
                        "No streams found.",
                        new PagedResult<StreamDto>(
                            new List<StreamDto>(), 0, request.Limit, request.Offset));
                }

                // ── Dynamic Sorting ──
                var isDesc = request.SortOrder.Equals("desc", StringComparison.OrdinalIgnoreCase);

                query = request.SortBy.ToLower() switch
                {
                    "name" => isDesc ? query.OrderByDescending(x => x.Name)
                                             : query.OrderBy(x => x.Name),

                    "gradename" => isDesc ? query.OrderByDescending(x => x.Grade.Name)
                                             : query.OrderBy(x => x.Grade.Name),

                    "syllabusname" => isDesc ? query.OrderByDescending(x => x.Grade.Syllabus.Name)
                                             : query.OrderBy(x => x.Grade.Syllabus.Name),

                    "isactive" => isDesc ? query.OrderByDescending(x => x.IsActive)
                                             : query.OrderBy(x => x.IsActive),

                    "createdat" => isDesc ? query.OrderByDescending(x => x.CreatedAt)
                                             : query.OrderBy(x => x.CreatedAt),

                    _ => isDesc ? query.OrderByDescending(x => x.CreatedAt)
                                             : query.OrderBy(x => x.CreatedAt),
                };

                // ── Pagination ──
                var streams = await query
                    .Skip(request.Offset)
                    .Take(request.Limit)
                    .ToListAsync();

                var dtoList = streams.Select(Map).ToList();

                _logger.LogInformation(
                    "Returning {Count} of {Total} streams",
                    dtoList.Count,
                    totalCount);

                return CommonResponse<PagedResult<StreamDto>>.SuccessResponse(
                    "Streams fetched successfully.",
                    new PagedResult<StreamDto>(
                        dtoList, totalCount, request.Limit, request.Offset));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching filtered streams.");
                return CommonResponse<PagedResult<StreamDto>>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Maps stream entity list to DTO list.
        /// </summary>
        private static List<StreamDto> Map(IEnumerable<Streams> streams)
        {
            return streams.Select(Map).ToList();
        }

        /// <summary>
        /// Maps stream entity to DTO.
        /// </summary>
        private static StreamDto Map(Streams c) =>
            new StreamDto
            {
                Id = c.Id,
                Name = c.Name,
                IsActive = c.IsActive,
                GradeId = c.GradeId,
                CreatedBy = c.CreatedBy,
                CreatedAt = c.CreatedAt,
                UpdatedBy = c.UpdatedBy,
                UpdatedAt = c.UpdatedAt,
                Grade = c.Grade == null ? null : new GradeDto
                {
                    Id = c.Grade.Id,
                    Name = c.Grade.Name,
                    IsActive = c.Grade.IsActive,
                    SyllabusId = c.Grade.SyllabusId,
                    Syllabus = c.Grade.Syllabus == null ? null : new SyllabusDto
                    {
                        Id = c.Grade.Syllabus.Id,
                        Name = c.Grade.Syllabus.Name,
                        IsActive = c.Grade.Syllabus.IsActive,
                    },
                },
            };
    }
}
