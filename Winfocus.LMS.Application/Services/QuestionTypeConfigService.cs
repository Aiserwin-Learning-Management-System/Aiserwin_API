namespace Winfocus.LMS.Application.Services
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Winfocus.LMS.Application.DTOs;
    using Winfocus.LMS.Application.DTOs.Common;
    using Winfocus.LMS.Application.DTOs.QuestionTypeConfig;
    using Winfocus.LMS.Application.Interfaces;
    using Winfocus.LMS.Domain.Entities;

    /// <summary>
    /// Service implementation for Question Type Configuration operations.
    /// </summary>
    public sealed class QuestionTypeConfigService : IQuestionTypeConfigService
    {
        private readonly IQuestionTypeConfigRepository _repository;
        private readonly ILogger<QuestionTypeConfigService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="QuestionTypeConfigService"/> class.
        /// </summary>
        /// <param name="repository">The question type config repository.</param>
        /// <param name="logger">The logger instance.</param>
        public QuestionTypeConfigService(
            IQuestionTypeConfigRepository repository,
            ILogger<QuestionTypeConfigService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task<CommonResponse<QuestionTypeConfigDto>> CreateAsync(
            CreateQuestionTypeConfigDto dto,
            Guid userId)
        {
            try
            {
                _logger.LogInformation("Creating Question Type Config: {Name}", dto.Name);

                bool isDuplicate = await _repository.IsDuplicateAsync(
                    dto.SyllabusId, dto.GradeId, dto.SubjectId,
                    dto.UnitId, dto.ChapterId, dto.ResourceTypeId,
                    dto.Name);

                if (isDuplicate)
                {
                    return CommonResponse<QuestionTypeConfigDto>.FailureResponse(
                        $"Question type '{dto.Name}' already exists for this hierarchy combination.");
                }

                QuestionTypeConfig entity = new QuestionTypeConfig
                {
                    Id = Guid.NewGuid(),
                    SyllabusId = dto.SyllabusId,
                    GradeId = dto.GradeId,
                    SubjectId = dto.SubjectId,
                    UnitId = dto.UnitId,
                    ChapterId = dto.ChapterId,
                    ResourceTypeId = dto.ResourceTypeId,
                    Name = dto.Name.Trim(),
                    Description = dto.Description,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = userId,
                };

                await _repository.AddAsync(entity);

                QuestionTypeConfig? loaded = await _repository.GetByIdAsync(entity.Id);

                _logger.LogInformation("Question Type Config created: {Name}", dto.Name);

                return CommonResponse<QuestionTypeConfigDto>.SuccessResponse(
                    "Question type created successfully.",
                    Map(loaded!));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating Question Type Config.");
                return CommonResponse<QuestionTypeConfigDto>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        /// <inheritdoc />
        public async Task<CommonResponse<List<QuestionTypeConfigDto>>> BulkCreateAsync(
            BulkCreateQuestionTypeConfigDto dto,
            Guid userId)
        {
            try
            {
                _logger.LogInformation("Bulk creating {Count} Question Type Configs", dto.Items.Count);

                List<QuestionTypeConfig> entities = new List<QuestionTypeConfig>();
                List<string> errors = new List<string>();

                for (int i = 0; i < dto.Items.Count; i++)
                {
                    CreateQuestionTypeConfigDto item = dto.Items[i];

                    bool isDuplicate = await _repository.IsDuplicateAsync(
                        item.SyllabusId, item.GradeId, item.SubjectId,
                        item.UnitId, item.ChapterId, item.ResourceTypeId,
                        item.Name);

                    // Also check within current batch.
                    bool duplicateInBatch = entities.Any(e =>
                        e.SyllabusId == item.SyllabusId &&
                        e.GradeId == item.GradeId &&
                        e.SubjectId == item.SubjectId &&
                        e.UnitId == item.UnitId &&
                        e.ChapterId == item.ChapterId &&
                        e.ResourceTypeId == item.ResourceTypeId &&
                        e.Name.Equals(item.Name.Trim(), StringComparison.OrdinalIgnoreCase));

                    if (isDuplicate || duplicateInBatch)
                    {
                        errors.Add($"Item {i + 1}: '{item.Name}' already exists for this hierarchy.");
                        continue;
                    }

                    entities.Add(new QuestionTypeConfig
                    {
                        Id = Guid.NewGuid(),
                        SyllabusId = item.SyllabusId,
                        GradeId = item.GradeId,
                        SubjectId = item.SubjectId,
                        UnitId = item.UnitId,
                        ChapterId = item.ChapterId,
                        ResourceTypeId = item.ResourceTypeId,
                        Name = item.Name.Trim(),
                        Description = item.Description,
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = userId,
                    });
                }

                if (errors.Any())
                {
                    return CommonResponse<List<QuestionTypeConfigDto>>.FailureResponse(
                        string.Join(" | ", errors));
                }

                await _repository.AddRangeAsync(entities);

                // Reload with navigations.
                List<QuestionTypeConfigDto> result = new List<QuestionTypeConfigDto>();
                foreach (QuestionTypeConfig entity in entities)
                {
                    QuestionTypeConfig? loaded = await _repository.GetByIdAsync(entity.Id);
                    if (loaded != null)
                    {
                        result.Add(Map(loaded));
                    }
                }

                _logger.LogInformation("Bulk created {Count} Question Type Configs.", result.Count);

                return CommonResponse<List<QuestionTypeConfigDto>>.SuccessResponse(
                    $"{result.Count} question types created successfully.",
                    result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in bulk creating Question Type Configs.");
                return CommonResponse<List<QuestionTypeConfigDto>>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        /// <inheritdoc />
        public async Task<CommonResponse<QuestionTypeConfigDto>> GetByIdAsync(Guid id)
        {
            try
            {
                QuestionTypeConfig? entity = await _repository.GetByIdAsync(id);
                if (entity == null)
                {
                    return CommonResponse<QuestionTypeConfigDto>.FailureResponse(
                        "Question type configuration not found.");
                }

                return CommonResponse<QuestionTypeConfigDto>.SuccessResponse(
                    "Fetched successfully.", Map(entity));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching Question Type Config Id: {Id}", id);
                return CommonResponse<QuestionTypeConfigDto>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        /// <inheritdoc />
        public async Task<CommonResponse<QuestionTypeConfigDto>> UpdateAsync(
            Guid id,
            CreateQuestionTypeConfigDto dto,
            Guid userId)
        {
            try
            {
                _logger.LogInformation("Updating Question Type Config Id: {Id}", id);

                QuestionTypeConfig? entity = await _repository.GetByIdAsync(id);
                if (entity == null)
                {
                    return CommonResponse<QuestionTypeConfigDto>.FailureResponse(
                        "Question type configuration not found.");
                }

                bool isDuplicate = await _repository.IsDuplicateAsync(
                    dto.SyllabusId, dto.GradeId, dto.SubjectId,
                    dto.UnitId, dto.ChapterId, dto.ResourceTypeId,
                    dto.Name, excludeId: id);

                if (isDuplicate)
                {
                    return CommonResponse<QuestionTypeConfigDto>.FailureResponse(
                        $"Question type '{dto.Name}' already exists for this hierarchy.");
                }

                entity.SyllabusId = dto.SyllabusId;
                entity.GradeId = dto.GradeId;
                entity.SubjectId = dto.SubjectId;
                entity.UnitId = dto.UnitId;
                entity.ChapterId = dto.ChapterId;
                entity.ResourceTypeId = dto.ResourceTypeId;
                entity.Name = dto.Name.Trim();
                entity.Description = dto.Description;
                entity.UpdatedAt = DateTime.UtcNow;
                entity.UpdatedBy = userId;

                await _repository.UpdateAsync(entity);

                QuestionTypeConfig? loaded = await _repository.GetByIdAsync(id);

                _logger.LogInformation("Question Type Config updated: {Id}", id);

                return CommonResponse<QuestionTypeConfigDto>.SuccessResponse(
                    "Updated successfully.", Map(loaded!));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating Question Type Config.");
                return CommonResponse<QuestionTypeConfigDto>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        /// <inheritdoc />
        public async Task<CommonResponse<bool>> DeleteAsync(Guid id, Guid userId)
        {
            try
            {
                _logger.LogInformation("Deleting Question Type Config Id: {Id}", id);

                bool result = await _repository.DeleteAsync(id, userId);

                if (result)
                {
                    return CommonResponse<bool>.SuccessResponse(
                        "Deleted successfully.", true);
                }

                return CommonResponse<bool>.FailureResponse(
                    "Question type not found or already deleted.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting Question Type Config.");
                return CommonResponse<bool>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        /// <inheritdoc />
        public async Task<CommonResponse<PagedResult<QuestionTypeConfigDto>>> GetFilteredAsync(
            QuestionTypeConfigFilterRequest request)
        {
            try
            {
                _logger.LogInformation("Fetching filtered Question Type Configs.");

                IQueryable<QuestionTypeConfig> query = _repository.Query();

                if (request.SyllabusId.HasValue)
                {
                    query = query.Where(x => x.SyllabusId == request.SyllabusId.Value);
                }

                if (request.GradeId.HasValue)
                {
                    query = query.Where(x => x.GradeId == request.GradeId.Value);
                }

                if (request.SubjectId.HasValue)
                {
                    query = query.Where(x => x.SubjectId == request.SubjectId.Value);
                }

                if (request.UnitId.HasValue)
                {
                    query = query.Where(x => x.UnitId == request.UnitId.Value);
                }

                if (request.ChapterId.HasValue)
                {
                    query = query.Where(x => x.ChapterId == request.ChapterId.Value);
                }

                if (request.ResourceTypeId.HasValue)
                {
                    query = query.Where(x => x.ResourceTypeId == request.ResourceTypeId.Value);
                }

                if (request.Active.HasValue)
                {
                    query = query.Where(x => x.IsActive == request.Active.Value);
                }

                if (!string.IsNullOrWhiteSpace(request.SearchText))
                {
                    string searchTerm = request.SearchText.Trim().ToLower();
                    query = query.Where(x =>
                        x.Name.ToLower().Contains(searchTerm) ||
                        x.Subject.Name.ToLower().Contains(searchTerm) ||
                        x.Chapter.Name.ToLower().Contains(searchTerm));
                }

                if (request.StartDate.HasValue)
                {
                    query = query.Where(x => x.CreatedAt >= request.StartDate.Value);
                }

                if (request.EndDate.HasValue)
                {
                    query = query.Where(x => x.CreatedAt <= request.EndDate.Value);
                }

                int totalCount = await query.CountAsync();

                if (totalCount == 0)
                {
                    return CommonResponse<PagedResult<QuestionTypeConfigDto>>.SuccessResponse(
                        "No question types found.",
                        new PagedResult<QuestionTypeConfigDto>(
                            new List<QuestionTypeConfigDto>(), 0, request.Limit, request.Offset));
                }

                bool isDesc = request.SortOrder.Equals("desc", StringComparison.OrdinalIgnoreCase);

                query = request.SortBy.ToLower() switch
                {
                    "name" => isDesc
                        ? query.OrderByDescending(x => x.Name)
                        : query.OrderBy(x => x.Name),
                    "subjectname" => isDesc
                        ? query.OrderByDescending(x => x.Subject.Name)
                        : query.OrderBy(x => x.Subject.Name),
                    "chaptername" => isDesc
                        ? query.OrderByDescending(x => x.Chapter.Name)
                        : query.OrderBy(x => x.Chapter.Name),
                    "isactive" => isDesc
                        ? query.OrderByDescending(x => x.IsActive)
                        : query.OrderBy(x => x.IsActive),
                    _ => isDesc
                        ? query.OrderByDescending(x => x.CreatedAt)
                        : query.OrderBy(x => x.CreatedAt),
                };

                List<QuestionTypeConfig> items = await query
                    .Skip(request.Offset)
                    .Take(request.Limit)
                    .ToListAsync();

                List<QuestionTypeConfigDto> dtoList = items.Select(Map).ToList();

                return CommonResponse<PagedResult<QuestionTypeConfigDto>>.SuccessResponse(
                    "Fetched successfully.",
                    new PagedResult<QuestionTypeConfigDto>(
                        dtoList, totalCount, request.Limit, request.Offset));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching filtered Question Type Configs.");
                return CommonResponse<PagedResult<QuestionTypeConfigDto>>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        /// <inheritdoc />
        public async Task<CommonResponse<List<QuestionTypeConfigDto>>> GetByHierarchyAsync(
            HierarchyQueryDto query)
        {
            try
            {
                _logger.LogInformation("Fetching question types by hierarchy.");

                List<QuestionTypeConfig> items = await _repository.GetByHierarchyAsync(
                    query.SyllabusId, query.GradeId, query.SubjectId,
                    query.UnitId, query.ChapterId, query.ResourceTypeId);

                if (!items.Any())
                {
                    return CommonResponse<List<QuestionTypeConfigDto>>.FailureResponse(
                        "No question types found for this hierarchy.");
                }

                List<QuestionTypeConfigDto> dtoList = items.Select(x => new QuestionTypeConfigDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    IsActive = x.IsActive,
                    CreatedAt = x.CreatedAt,
                }).ToList();

                return CommonResponse<List<QuestionTypeConfigDto>>.SuccessResponse(
                    "Fetched successfully.", dtoList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching question types by hierarchy.");
                return CommonResponse<List<QuestionTypeConfigDto>>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Maps a QuestionTypeConfig entity to a DTO with resolved master names.
        /// </summary>
        /// <param name="entity">The entity to map.</param>
        /// <returns>The mapped DTO.</returns>
        private static QuestionTypeConfigDto Map(QuestionTypeConfig entity)
        {
            return new QuestionTypeConfigDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                SyllabusName = entity.Syllabus?.Name ?? string.Empty,
                GradeName = entity.Grade?.Name ?? string.Empty,
                SubjectName = entity.Subject?.Name ?? string.Empty,
                SubjectCode = entity.Subject?.SubjectCode,
                UnitName = entity.Unit?.Name ?? string.Empty,
                ChapterName = entity.Chapter?.Name ?? string.Empty,
                ResourceTypeName = entity.ResourceType?.Name ?? string.Empty,
                IsActive = entity.IsActive,
                CreatedAt = entity.CreatedAt,
            };
        }
    }
}
