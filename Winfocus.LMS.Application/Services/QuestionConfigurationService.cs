namespace Winfocus.LMS.Application.Services
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Winfocus.LMS.Application.DTOs;
    using Winfocus.LMS.Application.DTOs.Common;
    using Winfocus.LMS.Application.DTOs.QuestionConfig;
    using Winfocus.LMS.Application.Interfaces;
    using Winfocus.LMS.Domain.Entities;

    /// <summary>
    /// Service implementation for Question Configuration operations.
    /// </summary>
    public sealed class QuestionConfigurationService : IQuestionConfigurationService
    {
        private readonly IQuestionConfigurationRepository _repository;
        private readonly IExamSyllabusRepository _syllabusRepository;
        private readonly IAcademicYearRepository _academicYearRepository;
        private readonly IExamGradeRepository _gradeRepository;
        private readonly IExamSubjectRepository _subjectRepository;
        private readonly IExamUnitRepository _unitRepository;
        private readonly IExamChapterRepository _chapterRepository;
        private readonly IContentResourceTypeRepository _resourceTypeRepository;
        private readonly IQuestionTypeConfigRepository _questionTypeRepository;
        private readonly ILogger<QuestionConfigurationService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="QuestionConfigurationService"/> class.
        /// </summary>
        /// <param name="repository">The question configuration repository.</param>
        /// <param name="syllabusRepository">The syllabus repository.</param>
        /// <param name="academicYearRepository">The academic year repository.</param>
        /// <param name="gradeRepository">The grade repository.</param>
        /// <param name="subjectRepository">The subject repository.</param>
        /// <param name="unitRepository">The unit repository.</param>
        /// <param name="chapterRepository">The chapter repository.</param>
        /// <param name="resourceTypeRepository">The resource type repository.</param>
        /// <param name="questionTypeRepository">The question type repository.</param>
        /// <param name="logger">The logger instance.</param>
        public QuestionConfigurationService(
            IQuestionConfigurationRepository repository,
            IExamSyllabusRepository syllabusRepository,
            IAcademicYearRepository academicYearRepository,
            IExamGradeRepository gradeRepository,
            IExamSubjectRepository subjectRepository,
            IExamUnitRepository unitRepository,
            IExamChapterRepository chapterRepository,
            IContentResourceTypeRepository resourceTypeRepository,
            IQuestionTypeConfigRepository questionTypeRepository,
            ILogger<QuestionConfigurationService> logger)
        {
            _repository = repository;
            _syllabusRepository = syllabusRepository;
            _academicYearRepository = academicYearRepository;
            _gradeRepository = gradeRepository;
            _subjectRepository = subjectRepository;
            _unitRepository = unitRepository;
            _chapterRepository = chapterRepository;
            _resourceTypeRepository = resourceTypeRepository;
            _questionTypeRepository = questionTypeRepository;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task<CommonResponse<SuggestedCodeResponseDto>> SuggestCodeAsync(
            SuggestQuestionCodeDto dto)
        {
            try
            {
                _logger.LogInformation("Generating suggested Question Code");

                ExamSyllabus? syllabus = await _syllabusRepository.GetByIdAsync(dto.SyllabusId, dto.AcademicYearId);
                if (syllabus == null)
                {
                    return CommonResponse<SuggestedCodeResponseDto>.FailureResponse("Syllabus not found");
                }

                AcademicYear? academicYear = await _academicYearRepository.GetByIdAsync(dto.AcademicYearId);
                if (academicYear == null)
                {
                    return CommonResponse<SuggestedCodeResponseDto>.FailureResponse("Academic Year not found");
                }

                ExamGrade? grade = await _gradeRepository.GetByIdAsync(dto.GradeId, dto.SyllabusId);
                if (grade == null)
                {
                    return CommonResponse<SuggestedCodeResponseDto>.FailureResponse("Grade not found");
                }

                ExamSubject? subject = await _subjectRepository.GetByIdAsync(dto.SubjectId, dto.GradeId);
                if (subject == null)
                {
                    return CommonResponse<SuggestedCodeResponseDto>.FailureResponse("Subject not found");
                }

                ExamUnit? unit = await _unitRepository.GetByIdAsync(dto.UnitId, dto.SubjectId);
                if (unit == null)
                {
                    return CommonResponse<SuggestedCodeResponseDto>.FailureResponse("Unit not found");
                }

                ExamChapter? chapter = await _chapterRepository.GetByIdAsync(dto.ChapterId, dto.UnitId);
                if (chapter == null)
                {
                    return CommonResponse<SuggestedCodeResponseDto>.FailureResponse("Chapter not found");
                }

                QuestionTypeConfig? questionType = await _questionTypeRepository.GetByIdAsync(dto.QuestionTypeId);
                if (questionType == null)
                {
                    return CommonResponse<SuggestedCodeResponseDto>.FailureResponse("Question Type not found");
                }

                int nextSequence = await _repository.GetNextSequenceAsync(
                    dto.SyllabusId, dto.AcademicYearId, dto.GradeId,
                    dto.SubjectId, dto.UnitId, dto.ChapterId, dto.QuestionTypeId);

                string code = BuildQuestionCode(
                    syllabus.Name, academicYear.Name, grade.Name,
                    subject.Code ?? subject.Name.Substring(0, Math.Min(3, subject.Name.Length)).ToUpper(),
                    unit.UnitNumber, chapter.ChapterNumber,
                    questionType.Name, nextSequence);

                _logger.LogInformation("Suggested code: {Code}", code);

                return CommonResponse<SuggestedCodeResponseDto>.SuccessResponse(
                    "Code suggested successfully",
                    new SuggestedCodeResponseDto
                    {
                        SuggestedCode = code,
                        NextSequence = nextSequence,
                    });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating suggested code");
                return CommonResponse<SuggestedCodeResponseDto>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        /// <inheritdoc />
        public async Task<CommonResponse<QuestionConfigurationDto>> CreateAsync(
            CreateQuestionConfigurationDto dto,
            Guid userId)
        {
            try
            {
                _logger.LogInformation("Creating Question Configuration with code: {Code}", dto.QuestionCode);

                bool codeExists = await _repository.CodeExistsAsync(dto.QuestionCode);
                if (codeExists)
                {
                    return CommonResponse<QuestionConfigurationDto>.FailureResponse(
                        $"Question Code '{dto.QuestionCode}' already exists");
                }

                int sequenceNumber = await _repository.GetNextSequenceAsync(
                    dto.SyllabusId, dto.AcademicYearId, dto.GradeId,
                    dto.SubjectId, dto.UnitId, dto.ChapterId, dto.QuestionTypeId);

                QuestionConfiguration entity = new QuestionConfiguration
                {
                    Id = Guid.NewGuid(),
                    SyllabusId = dto.SyllabusId,
                    AcademicYearId = dto.AcademicYearId,
                    GradeId = dto.GradeId,
                    SubjectId = dto.SubjectId,
                    UnitId = dto.UnitId,
                    ChapterId = dto.ChapterId,
                    ResourceTypeId = dto.ResourceTypeId,
                    QuestionTypeId = dto.QuestionTypeId,
                    QuestionCode = dto.QuestionCode,
                    SequenceNumber = sequenceNumber,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = userId,
                };

                await _repository.AddAsync(entity);

                QuestionConfiguration? loaded = await _repository.GetByIdAsync(entity.Id);

                _logger.LogInformation("Question Configuration created: {Code}", dto.QuestionCode);

                return CommonResponse<QuestionConfigurationDto>.SuccessResponse(
                    "Question Configuration created successfully",
                    Map(loaded!));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating Question Configuration");
                return CommonResponse<QuestionConfigurationDto>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        /// <inheritdoc />
        public async Task<CommonResponse<CodeAvailabilityDto>> CheckUniqueAsync(string code)
        {
            try
            {
                bool exists = await _repository.CodeExistsAsync(code);

                return CommonResponse<CodeAvailabilityDto>.SuccessResponse(
                    exists ? "Code already exists" : "Code is available",
                    new CodeAvailabilityDto
                    {
                        Code = code,
                        IsAvailable = !exists,
                        Message = exists ? "This Question ID already exists." : null,
                    });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking code uniqueness");
                return CommonResponse<CodeAvailabilityDto>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        /// <inheritdoc />
        public async Task<CommonResponse<QuestionConfigurationDto>> GetByIdAsync(Guid id)
        {
            try
            {
                _logger.LogInformation("Fetching Question Configuration by Id: {Id}", id);

                QuestionConfiguration? entity = await _repository.GetByIdAsync(id);
                if (entity == null)
                {
                    return CommonResponse<QuestionConfigurationDto>.FailureResponse(
                        "Question Configuration not found");
                }

                return CommonResponse<QuestionConfigurationDto>.SuccessResponse(
                    "Question Configuration fetched successfully",
                    Map(entity));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching Question Configuration");
                return CommonResponse<QuestionConfigurationDto>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        /// <inheritdoc />
        public async Task<CommonResponse<PagedResult<QuestionConfigurationDto>>> GetFilteredAsync(
            QuestionConfigurationFilterRequest request)
        {
            try
            {
                _logger.LogInformation(
                    "Fetching filtered Question Configurations. Search:{Search}, Limit:{Limit}, Offset:{Offset}",
                    request.SearchText, request.Limit, request.Offset);

                IQueryable<QuestionConfiguration> query = _repository.Query();

                if (request.SyllabusId.HasValue)
                {
                    query = query.Where(x => x.SyllabusId == request.SyllabusId.Value);
                }

                if (request.AcademicYearId.HasValue)
                {
                    query = query.Where(x => x.AcademicYearId == request.AcademicYearId.Value);
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

                if (request.QuestionTypeId.HasValue)
                {
                    query = query.Where(x => x.QuestionTypeId == request.QuestionTypeId.Value);
                }

                if (request.Active.HasValue)
                {
                    query = query.Where(x => x.IsActive == request.Active.Value);
                }

                if (!string.IsNullOrWhiteSpace(request.SearchText))
                {
                    string searchTerm = request.SearchText.Trim().ToLower();
                    query = query.Where(x =>
                        x.QuestionCode.ToLower().Contains(searchTerm) ||
                        x.Syllabus.Name.ToLower().Contains(searchTerm) ||
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
                    return CommonResponse<PagedResult<QuestionConfigurationDto>>.SuccessResponse(
                        "No question configurations found.",
                        new PagedResult<QuestionConfigurationDto>(
                            new List<QuestionConfigurationDto>(), 0, request.Limit, request.Offset));
                }

                bool isDesc = request.SortOrder.Equals("desc", StringComparison.OrdinalIgnoreCase);

                query = request.SortBy.ToLower() switch
                {
                    "questioncode" => isDesc
                        ? query.OrderByDescending(x => x.QuestionCode)
                        : query.OrderBy(x => x.QuestionCode),
                    "syllabusname" => isDesc
                        ? query.OrderByDescending(x => x.Syllabus.Name)
                        : query.OrderBy(x => x.Syllabus.Name),
                    "subjectname" => isDesc
                        ? query.OrderByDescending(x => x.Subject.Name)
                        : query.OrderBy(x => x.Subject.Name),
                    "chaptername" => isDesc
                        ? query.OrderByDescending(x => x.Chapter.Name)
                        : query.OrderBy(x => x.Chapter.Name),
                    "sequencenumber" => isDesc
                        ? query.OrderByDescending(x => x.SequenceNumber)
                        : query.OrderBy(x => x.SequenceNumber),
                    "isactive" => isDesc
                        ? query.OrderByDescending(x => x.IsActive)
                        : query.OrderBy(x => x.IsActive),
                    _ => isDesc
                        ? query.OrderByDescending(x => x.CreatedAt)
                        : query.OrderBy(x => x.CreatedAt),
                };

                List<QuestionConfiguration> items = await query
                    .Skip(request.Offset)
                    .Take(request.Limit)
                    .ToListAsync();

                List<QuestionConfigurationDto> dtoList = items.Select(Map).ToList();

                _logger.LogInformation(
                    "Returning {Count} of {Total} question configurations",
                    dtoList.Count, totalCount);

                return CommonResponse<PagedResult<QuestionConfigurationDto>>.SuccessResponse(
                    "Question configurations fetched successfully.",
                    new PagedResult<QuestionConfigurationDto>(
                        dtoList, totalCount, request.Limit, request.Offset));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching filtered question configurations");
                return CommonResponse<PagedResult<QuestionConfigurationDto>>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        /// <inheritdoc />
        public async Task<CommonResponse<bool>> DeleteAsync(Guid id, Guid userId)
        {
            try
            {
                _logger.LogInformation("Deleting Question Configuration Id: {Id}", id);

                bool result = await _repository.DeleteAsync(id, userId);

                if (result)
                {
                    return CommonResponse<bool>.SuccessResponse(
                        "Question Configuration deleted successfully", true);
                }

                return CommonResponse<bool>.FailureResponse(
                    "Question Configuration not found or already deleted");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting Question Configuration Id: {Id}", id);
                return CommonResponse<bool>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Builds the Question Code string from master data values.
        /// Format: [SYL]-[YYYY]-[GRD]-[SUB]-[UNIT]-[CH]-[TYPE]-[SEQ].
        /// </summary>
        /// <param name="syllabusName">The syllabus name.</param>
        /// <param name="academicYearName">The academic year name.</param>
        /// <param name="gradeName">The grade name.</param>
        /// <param name="subjectCode">The subject code.</param>
        /// <param name="unitNumber">The unit number.</param>
        /// <param name="chapterNumber">The chapter number.</param>
        /// <param name="questionTypeCode">The question type code.</param>
        /// <param name="sequenceNumber">The sequence number.</param>
        /// <returns>The formatted Question Code string.</returns>
        private static string BuildQuestionCode(
            string syllabusName,
            string academicYearName,
            string gradeName,
            string subjectCode,
            int unitNumber,
            int chapterNumber,
            string questionTypeCode,
            int sequenceNumber)
        {
            string yearPart = academicYearName.Length >= 4
                ? academicYearName.Substring(0, 4)
                : academicYearName;

            string gradePart = new string(gradeName.Where(char.IsDigit).ToArray());
            if (string.IsNullOrEmpty(gradePart))
            {
                gradePart = gradeName;
            }

            return $"{syllabusName.ToUpper()}" +
                   $"-{yearPart}" +
                   $"-{gradePart}" +
                   $"-{subjectCode.ToUpper()}" +
                   $"-U{unitNumber:D2}" +
                   $"-CH{chapterNumber:D2}" +
                   $"-{questionTypeCode.ToUpper()}" +
                   $"-{sequenceNumber:D4}";
        }

        /// <summary>
        /// Maps a QuestionConfiguration entity to a DTO.
        /// </summary>
        /// <param name="entity">The entity to map.</param>
        /// <returns>The mapped DTO.</returns>
        private static QuestionConfigurationDto Map(QuestionConfiguration entity)
        {
            return new QuestionConfigurationDto
            {
                Id = entity.Id,
                QuestionCode = entity.QuestionCode,
                SequenceNumber = entity.SequenceNumber,
                SyllabusName = entity.Syllabus.Name,
                AcademicYear = entity.AcademicYear.Name,
                GradeName = entity.Grade.Name,
                SubjectName = entity.Subject.Name,
                SubjectCode = entity.Subject.Code,
                UnitName = entity.Unit.Name,
                UnitNumber = entity.Unit.UnitNumber,
                ChapterName = entity.Chapter.Name,
                ChapterNumber = entity.Chapter.ChapterNumber,
                ResourceTypeName = entity.ResourceType.Name,
                IsActive = entity.IsActive,
                CreatedAt = entity.CreatedAt,
            };
        }
    }
}
