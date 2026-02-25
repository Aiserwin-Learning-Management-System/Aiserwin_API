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
                return CommonResponse<GradeDto>.FailureResponse("batch timing not found");
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
        /// Retrieves grades based on multiple filter criteria with pagination support.
        /// </summary>
        /// <param name="syllabusId">Syllabus identifier used to filter grades.</param>
        /// <param name="startDate">Filters grades created on or after this date.</param>
        /// <param name="endDate">Filters grades created on or before this date.</param>
        /// <param name="active">Indicates whether to filter active or inactive grades.</param>
        /// <param name="searchText">Search keyword applied to grades name.</param>
        /// <param name="limit">Number of records to return (page size).</param>
        /// <param name="offset">Number of records to skip.</param>
        /// <param name="sortOrder">Sorting order ("asc" or "desc").</param>
        /// <returns>
        /// A <see cref="CommonResponse{T}"/> containing a paginated list of
        /// <see cref="GradeDto"/> objects.
        /// </returns>
        public async Task<CommonResponse<PagedResult<GradeDto>>> GetFilteredAsync(
       Guid? syllabusId,
       DateTime? startDate,
       DateTime? endDate,
       bool? active,
       string? searchText,
       int limit,
       int offset,
       string sortOrder)
        {
            try
            {
                _logger.LogInformation(
                             "Fetching filtered grades. Filters =>  SyllabusId:{SyllabusId}, Active:{Active}, Search:{SearchText}, Limit:{Limit}, Offset:{Offset}, SortOrder:{SortOrder}",
                             syllabusId, active, searchText, limit, offset, sortOrder);
                var query = _repository.Query();

                if (syllabusId.HasValue)
                {
                    query = query.Where(c =>
                        c.Stream.Grade.SyllabusId == syllabusId);
                }

                if (active.HasValue)
                    query = query.Where(x => x.IsActive == active);

                if (startDate.HasValue)
                    query = query.Where(x => x.CreatedAt >= startDate.Value);

                if (endDate.HasValue)
                    query = query.Where(x => x.CreatedAt <= endDate.Value);

                if (!string.IsNullOrWhiteSpace(searchText))
                {
                    query = query.Where(x =>
                        x.Name.Contains(searchText));
                }

                var totalCount = await query.CountAsync();

                _logger.LogInformation(
                "Filtered grades count: {TotalCount}",
                totalCount);

                query = sortOrder.ToLower() == "desc"
                    ? query.OrderByDescending(x => x.CreatedAt)
                    : query.OrderBy(x => x.CreatedAt);

                var grades = await query
                    .Skip(offset)
                    .Take(limit)
                    .ToListAsync();

                var dtoList = grades.Select(c => new GradeDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    IsActive = c.IsActive,
                }).ToList();

                _logger.LogInformation(
               "Returning {ReturnedCount} grades (Offset:{Offset}, Limit:{Limit})",
               dtoList.Count, offset, limit);
                return CommonResponse<PagedResult<GradeDto>>.SuccessResponse(
                "Grades fetched successfully",
                new PagedResult<GradeDto>(
                    dtoList,
                    totalCount,
                    limit,
                    offset));
            }
            catch (Exception ex)
            {
                _logger.LogError(
                   ex,
                   "Error occurred while fetching filtered grades. Filters => SyllabusId:{SyllabusId}, Active:{Active}, Search:{SearchText}, Limit:{Limit}, Offset:{Offset}, SortOrder:{SortOrder}",
                   syllabusId, active, searchText, limit, offset, sortOrder);
                return CommonResponse<PagedResult<GradeDto>>.FailureResponse($"An error occurred while fetching grades: {ex.Message}");
            }
        }

        private static List<GradeDto> Map(IEnumerable<Grade> grades)
        {
            return grades.Select(Map).ToList();
        }

        private static GradeDto Map(Grade c) =>
     new GradeDto
     {
         Id = c.Id,
         Name = c.Name,
         SyllabusId = c.SyllabusId,
         UpdatedBy = c.UpdatedBy,
         UpdatedAt = c.UpdatedAt,
         CreatedBy = c.CreatedBy,
         CreatedAt = c.CreatedAt,
         Syllabus = c.Syllabus == null ? null : new SyllabusDto
         {
             Id = c.Syllabus.Id,
             Name = c.Syllabus.Name,
         },
     };
}
}
