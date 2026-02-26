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
    /// GradeService.
    /// </summary>
    public sealed class GradeService : IGradeService
    {
        private readonly IGradeRepository _repository;
        private readonly ILogger<GradeService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="GradeService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="logger">The logger.</param>
        public GradeService(
            IGradeRepository repository,
            ILogger<GradeService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>GradeDto.</returns>
        public async Task<CommonResponse<List<GradeDto>>> GetAllAsync()
        {
            _logger.LogInformation("Fetching all Grdaes");
            var grades = await _repository.GetAllAsync();
            var mapped = grades.Select(Map).ToList();
            if (mapped.Any())
            {
                return CommonResponse<List<GradeDto>>.SuccessResponse("Fetching all Grades", mapped);
            }
            else
            {
                return CommonResponse<List<GradeDto>>.FailureResponse("Grades not found");
            }
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>GradeDto.</returns>
        public async Task<CommonResponse<GradeDto>> GetByIdAsync(Guid id)
        {
            var grades = await _repository.GetByIdAsync(id);
            var mapped = grades == null ? null : Map(grades);
            if (mapped != null)
            {
                return CommonResponse<GradeDto>.SuccessResponse("Fetching the grade", mapped);
            }
            else
            {
                return CommonResponse<GradeDto>.FailureResponse("Grade not found");
            }
        }

        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>GradeDto.</returns>
        /// <exception cref="InvalidOperationException">grade code already exists.</exception>
        public async Task<GradeDto> CreateAsync(GradeRequest request)
        {
            var grades = new Grade
            {
                Name = request.name,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = request.userId,
                SyllabusId = request.syllabusid,
            };

            var created = await _repository.AddAsync(grades);
            return Map(created);
        }

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="request">The request.</param>
        /// <exception cref="KeyNotFoundException">Grade not found.</exception>
        /// <returns>task.</returns>
        public async Task<GradeDto> UpdateAsync(Guid id, GradeRequest request)
        {
            var grade = await _repository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException("Grade not found");

            grade.Name = request.name;
            grade.SyllabusId = request.syllabusid;
            grade.UpdatedBy = request.userId;
            grade.UpdatedAt = DateTime.UtcNow;

            return Map(await _repository.UpdateAsync(grade));
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
        /// <param name="syllabusid">The identifier.</param>
        /// <returns>GradeDto.</returns>
        public async Task<CommonResponse<List<GradeDto>>> GetBySyllabusIdAsync(Guid syllabusid)
        {
            var grades = await _repository.GetBySyllabusIdAsync(syllabusid);
            var mapped = Map(grades);
            if (mapped != null)
            {
                return CommonResponse<List<GradeDto>>.SuccessResponse("Fetching the grade by syllabus", mapped);
            }
            else
            {
                return CommonResponse<List<GradeDto>>.FailureResponse("grade not found");
            }
        }

        /// <summary>
        /// Gets filtered grades with pagination support.
        /// Search works on both Grade Name and Syllabus Name.
        /// </summary>
        /// <param name="request">The paged request.</param>
        /// <returns>Paginated grade result.</returns>
        public async Task<CommonResponse<PagedResult<GradeDto>>> GetFilteredAsync(
            PagedRequest request)
        {
            try
            {
                _logger.LogInformation(
                    "Fetching filtered grades. Filters => Active:{Active}, " +
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

                // ── Search on BOTH Grade Name AND Syllabus Name ──
                if (!string.IsNullOrWhiteSpace(request.SearchText))
                {
                    var searchTerm = request.SearchText.Trim().ToLower();
                    query = query.Where(x =>
                        x.Name.ToLower().Contains(searchTerm) ||
                        x.Syllabus.Name.ToLower().Contains(searchTerm));
                }

                // ── Total Count ──
                var totalCount = await query.CountAsync();

                if (totalCount == 0)
                {
                    return CommonResponse<PagedResult<GradeDto>>.SuccessResponse(
                        "No grades found.",
                        new PagedResult<GradeDto>(
                            new List<GradeDto>(), 0, request.Limit, request.Offset));
                }

                // ── Dynamic Sorting ──
                var isDesc = request.SortOrder.Equals("desc", StringComparison.OrdinalIgnoreCase);

                query = request.SortBy.ToLower() switch
                {
                    "name" => isDesc ? query.OrderByDescending(x => x.Name)
                                             : query.OrderBy(x => x.Name),

                    "syllabusname" => isDesc ? query.OrderByDescending(x => x.Syllabus.Name)
                                             : query.OrderBy(x => x.Syllabus.Name),

                    "isactive" => isDesc ? query.OrderByDescending(x => x.IsActive)
                                             : query.OrderBy(x => x.IsActive),

                    "createdat" => isDesc ? query.OrderByDescending(x => x.CreatedAt)
                                             : query.OrderBy(x => x.CreatedAt),

                    _ => isDesc ? query.OrderByDescending(x => x.CreatedAt)
                                             : query.OrderBy(x => x.CreatedAt),
                };

                // ── Pagination ──
                var grades = await query
                    .Skip(request.Offset)
                    .Take(request.Limit)
                    .ToListAsync();

                var dtoList = grades.Select(Map).ToList();

                _logger.LogInformation(
                    "Returning {Count} of {Total} grades",
                    dtoList.Count, totalCount);

                return CommonResponse<PagedResult<GradeDto>>.SuccessResponse(
                    "Grades fetched successfully.",
                    new PagedResult<GradeDto>(
                        dtoList, totalCount, request.Limit, request.Offset));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching filtered grades.");
                return CommonResponse<PagedResult<GradeDto>>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Maps grade entity list to DTO list.
        /// </summary>
        private static List<GradeDto> Map(IEnumerable<Grade> grades)
        {
            return grades.Select(Map).ToList();
        }

        /// <summary>
        /// Maps grade entity to DTO.
        /// </summary>
        private static GradeDto Map(Grade c) =>
            new GradeDto
            {
                Id = c.Id,
                Name = c.Name,
                IsActive = c.IsActive,
                SyllabusId = c.SyllabusId,
                CreatedBy = c.CreatedBy,
                CreatedAt = c.CreatedAt,
                UpdatedBy = c.UpdatedBy,
                UpdatedAt = c.UpdatedAt,
                Syllabus = c.Syllabus == null ? null : new SyllabusDto
                {
                    Id = c.Syllabus.Id,
                    Name = c.Syllabus.Name,
                    IsActive = c.Syllabus.IsActive,
                },
            };
    }
}
