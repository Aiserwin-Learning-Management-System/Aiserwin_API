namespace Winfocus.LMS.Application.Services
{
    using Microsoft.AspNetCore.Http.HttpResults;
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
        /// <param name="centerId">The centerId.</param>
        /// <returns>GradeDto.</returns>
        public async Task<CommonResponse<List<GradeDto>>> GetAllAsync(Guid centerId)
        {
            try
            {
                _logger.LogInformation("Fetching all Grdaes");
                var grades = await _repository.GetAllAsync(centerId);
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching grades.");
                return CommonResponse<List<GradeDto>>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>GradeDto.</returns>
        public async Task<CommonResponse<GradeDto>> GetByIdAsync(Guid id)
        {
            try
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
            } catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching grades.");
                return CommonResponse<GradeDto>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="centerId">The centerId.</param>
        /// <returns>GradeDto.</returns>
        public async Task<CommonResponse<GradeDto>> GetByIdCenterIdAsync(Guid id, Guid centerId)
        {
            try
            {
                var grades = await _repository.GetByIdCenterIdAsync(id, centerId);
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching grades.");
                return CommonResponse<GradeDto>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>GradeDto.</returns>
        /// <exception cref="InvalidOperationException">grade code already exists.</exception>
        public async Task<CommonResponse<GradeDto>> CreateAsync(GradeRequest request)
        {
            try
            {
                var grades = new Grade
                {
                    Name = request.name,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = request.userId,
                    SyllabusId = request.syllabusid,
                };

                var created = await _repository.AddAsync(grades);
                var result = new GradeDto
                {
                    Id = created.Id,
                    Name = created.Name,
                    SyllabusId = created.SyllabusId,
                };

                _logger.LogInformation(
               "Grades created successfully. Id: {Id}",
               created.Id);
                return CommonResponse<GradeDto>.SuccessResponse(
                  "Grade created successfully", result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating Grade: {Name}", request.name);
                return CommonResponse<GradeDto>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="request">The request.</param>
        /// <exception cref="KeyNotFoundException">Grade not found.</exception>
        /// <returns>task.</returns>
        public async Task<CommonResponse<GradeDto>> UpdateAsync(Guid id, GradeRequest request)
        {
            try
            {
                _logger.LogInformation("Updating Grade Id: {Id}", id);

                var batch = await _repository.GetByIdAsync(id);
                if (batch == null)
                {
                    return CommonResponse<GradeDto>.FailureResponse("Grade not found");
                }

                batch.Name = request.name;
                batch.SyllabusId = request.syllabusid;
                batch.UpdatedAt = DateTime.UtcNow;
                batch.UpdatedBy = request.userId;

                var updated = await _repository.UpdateAsync(batch);
                var result = new GradeDto
                {
                    Id = updated.Id,
                    Name = updated.Name,
                    SyllabusId = updated.SyllabusId,
                };

                _logger.LogInformation("Grade updated Id: {Id}", id);
                return CommonResponse<GradeDto>.SuccessResponse(
                    "Grade updated successfully", result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating grade Id: {Id}", id);
                return CommonResponse<GradeDto>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="centerId">The centerId.</param>
        /// <returns>task.</returns>
        public async Task<CommonResponse<bool>> DeleteAsync(Guid id, Guid centerId)
        {
            try
            {
                _logger.LogInformation("Deleting grade Id: {Id}", id);
                var result = await _repository.DeleteAsync(id, centerId);

                if (result)
                {
                    return CommonResponse<bool>.SuccessResponse(
                        "Grade deleted successfully", true);
                }

                return CommonResponse<bool>.FailureResponse(
                    "Grade not found or already deleted");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting Grade Id: {Id}", id);
                return CommonResponse<bool>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="syllabusid">The identifier.</param>
        /// <returns>GradeDto.</returns>
        public async Task<CommonResponse<List<GradeDto>>> GetBySyllabusIdAsync(Guid syllabusid)
        {
            try
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
            catch (Exception ex) {
                _logger.LogError(ex, "Error fetching Grade");
                return CommonResponse<List<GradeDto>>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets filtered grades with pagination support.
        /// Search works on both Grade Name and Syllabus Name.
        /// </summary>
        /// <param name="request">The paged request.</param>
        /// <param name="centerId">The centerId.</param>
        /// <returns>Paginated grade result.</returns>
        public async Task<CommonResponse<PagedResult<GradeDto>>> GetFilteredAsync(
            PagedRequest request, Guid centerId)
        {
            try
            {
                _logger.LogInformation(
                    "Fetching filtered grades. Filters => Active:{Active}, " +
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
                CenterId = c.Syllabus.CenterId,
                StateId = c.Syllabus.Center.StateId,
                ModeOfStudyId = c.Syllabus.Center.ModeOfStudyId,
                CountryId = c.Syllabus.Center.CountryId,
                Syllabus = c.Syllabus == null ? null : new SyllabusDto
                {
                    Id = c.Syllabus.Id,
                    Name = c.Syllabus.Name,
                    IsActive = c.Syllabus.IsActive,
                },
            };
    }
}
