namespace Winfocus.LMS.Application.Services
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Winfocus.LMS.Application.DTOs;
    using Winfocus.LMS.Application.DTOs.Common;
    using Winfocus.LMS.Application.DTOs.Review;
    using Winfocus.LMS.Application.Helpers;
    using Winfocus.LMS.Application.Interfaces;

    /// <summary>
    /// QuestionCorrectionService.
    /// </summary>
    /// <seealso cref="Winfocus.LMS.Application.Interfaces.IQuestionCorrectionService" />
    public sealed class QuestionCorrectionService : IQuestionCorrectionService
    {
        private readonly IQuestionReviewRepository _repo;
        private readonly IOperatorDashboardRepository _dashRepo;
        private readonly ILogger<QuestionCorrectionService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="QuestionCorrectionService"/> class.
        /// </summary>
        /// <param name="repo">The repo.</param>
        /// <param name="dashRepo">The dash repo.</param>
        /// <param name="logger">The logger.</param>
        public QuestionCorrectionService(
            IQuestionReviewRepository repo,
            IOperatorDashboardRepository dashRepo,
            ILogger<QuestionCorrectionService> logger)
        {
            _repo = repo;
            _dashRepo = dashRepo;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<CommonResponse<PagedResult<CorrectionListDto>>>
            GetMyCorrectionsAsync(Guid userId, PagedRequest request)
        {
            try
            {
                var registration = await _dashRepo.GetOperatorRegistrationAsync(userId);
                if (registration == null)
                {
                    return CommonResponse<PagedResult<CorrectionListDto>>
                        .FailureResponse("Operator registration not found.");
                }

                var query = _repo.QueryRejectedByOperator(registration.Id);

                if (!string.IsNullOrWhiteSpace(request.SearchText))
                {
                    var search = request.SearchText.Trim().ToLower();
                    query = query.Where(q => q.QuestionText.ToLower().Contains(search));
                }

                var totalCount = await query.CountAsync();

                if (totalCount == 0)
                {
                    return CommonResponse<PagedResult<CorrectionListDto>>.SuccessResponse(
                        "No corrections pending.",
                        new PagedResult<CorrectionListDto>(
                            new List<CorrectionListDto>(), 0, request.Limit, request.Offset));
                }

                var questions = await query
                    .OrderByDescending(q => q.UpdatedAt ?? q.CreatedAt)
                    .Skip(request.Offset)
                    .Take(request.Limit)
                    .ToListAsync();

                var dtoList = questions.Select((q, i) =>
                {
                    var latestRejection = q.Reviews?
                        .Where(r => r.Action == 1)
                        .OrderByDescending(r => r.ReviewedAt)
                        .FirstOrDefault();

                    return new CorrectionListDto
                    {
                        QuestionId = q.Id,
                        QuestionNumber = request.Offset + i + 1,
                        QuestionText = q.QuestionText.Length > 150
                            ? q.QuestionText[..150] + "..." : q.QuestionText,
                        QuestionType = HierarchyMapper.MapQuestionType(q.QuestionType),
                        TaskCode = HierarchyMapper.GetTaskCode(q.TaskAssignment),
                        RejectedAt = latestRejection?.ReviewedAt ?? q.UpdatedAt ?? q.CreatedAt,
                        Feedback = latestRejection?.Feedback,
                        ReviewerRole = latestRejection?.ReviewerRole ?? "Unknown",
                        ReviewCycle = q.Reviews?.Count(r => r.Action == 1) ?? 0,

                        // ── Full Hierarchy ────────────────────
                        Year = HierarchyMapper.GetYear(q.TaskAssignment),
                        Syllabus = HierarchyMapper.GetSyllabus(q.TaskAssignment),
                        Grade = HierarchyMapper.GetGrade(q.TaskAssignment),
                        Subject = HierarchyMapper.GetSubject(q.TaskAssignment),
                        Unit = HierarchyMapper.GetUnit(q.TaskAssignment),
                        Chapter = HierarchyMapper.GetChapter(q.TaskAssignment),
                        ResourceType = HierarchyMapper.GetResourceType(q.TaskAssignment),
                    };
                }).ToList();

                return CommonResponse<PagedResult<CorrectionListDto>>.SuccessResponse(
                    $"{totalCount} corrections pending.",
                    new PagedResult<CorrectionListDto>(
                        dtoList, totalCount, request.Limit, request.Offset));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading corrections for user {UserId}", userId);
                return CommonResponse<PagedResult<CorrectionListDto>>
                    .FailureResponse($"An error occurred: {ex.Message}");
            }
        }

        /// <inheritdoc/>
        public async Task<CommonResponse<CorrectionDetailDto>>
            GetCorrectionDetailAsync(Guid userId, Guid questionId)
        {
            try
            {
                var registration = await _dashRepo.GetOperatorRegistrationAsync(userId);
                if (registration == null)
                {
                    return CommonResponse<CorrectionDetailDto>
                        .FailureResponse("Operator registration not found.");
                }

                var q = await _repo.GetQuestionWithDetailsAsync(questionId);
                if (q == null)
                {
                    return CommonResponse<CorrectionDetailDto>.FailureResponse("Question not found.");
                }

                if (q.OperatorId != registration.Id)
                {
                    return CommonResponse<CorrectionDetailDto>
                        .FailureResponse("This question does not belong to you.");
                }

                var latestRejection = q.Reviews?
                    .Where(r => r.Action == 1)
                    .OrderByDescending(r => r.ReviewedAt)
                    .FirstOrDefault();

                var detail = new CorrectionDetailDto
                {
                    QuestionId = q.Id,
                    TaskId = q.TaskId,
                    QuestionText = q.QuestionText,
                    QuestionType = HierarchyMapper.MapQuestionType(q.QuestionType),
                    Marks = q.Marks,
                    CorrectAnswer = q.CorrectAnswer,
                    CorrectAnswerText = q.CorrectAnswerText,
                    Reference = q.Reference,
                    TaskCode = HierarchyMapper.GetTaskCode(q.TaskAssignment),
                    LatestFeedback = latestRejection?.Feedback ?? "",
                    ReviewCycle = q.Reviews?.Count(r => r.Action == 1) ?? 0,

                    // ── Full Hierarchy ────────────────────────
                    Year = HierarchyMapper.GetYear(q.TaskAssignment),
                    Syllabus = HierarchyMapper.GetSyllabus(q.TaskAssignment),
                    Grade = HierarchyMapper.GetGrade(q.TaskAssignment),
                    Subject = HierarchyMapper.GetSubject(q.TaskAssignment),
                    Unit = HierarchyMapper.GetUnit(q.TaskAssignment),
                    Chapter = HierarchyMapper.GetChapter(q.TaskAssignment),
                    ResourceType = HierarchyMapper.GetResourceType(q.TaskAssignment),

                    Options = q.Options?.Select(o => new QuestionOptionDto
                    {
                        Id = o.Id,
                        OptionLabel = o.OptionLabel,
                        OptionText = o.OptionText,
                        IsCorrect = o.IsCorrect,
                    }).OrderBy(o => o.OptionLabel).ToList() ?? new (),

                    ReviewHistory = HierarchyMapper.BuildReviewHistory(q.Reviews),
                };

                return CommonResponse<CorrectionDetailDto>.SuccessResponse(
                    "Correction detail loaded.", detail);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading correction {QuestionId}", questionId);
                return CommonResponse<CorrectionDetailDto>
                    .FailureResponse($"An error occurred: {ex.Message}");
            }
        }

        /// <inheritdoc/>
        public async Task<CommonResponse<bool>> FixAndResubmitAsync(
            Guid userId, Guid questionId, FixQuestionDto dto)
        {
            try
            {
                var registration = await _dashRepo.GetOperatorRegistrationAsync(userId);
                if (registration == null)
                {
                    return CommonResponse<bool>.FailureResponse("Operator registration not found.");
                }

                var question = await _repo.GetQuestionWithDetailsAsync(questionId);
                if (question == null)
                {
                    return CommonResponse<bool>.FailureResponse("Question not found.");
                }

                if (question.OperatorId != registration.Id)
                {
                    return CommonResponse<bool>.FailureResponse("This question does not belong to you.");
                }

                if (question.Status != 4)
                {
                    return CommonResponse<bool>.FailureResponse(
                        "Only rejected questions can be fixed and resubmitted.");
                }

                // Update question fields
                question.QuestionText = dto.QuestionText;
                question.Marks = dto.Marks;
                question.CorrectAnswer = dto.CorrectAnswer;
                question.CorrectAnswerText = dto.CorrectAnswerText;
                question.Reference = dto.Reference;
                question.Status = 1; // Resubmit
                question.UpdatedAt = DateTime.UtcNow;
                question.UpdatedBy = userId;

                // Update MCQ options if provided
                if (dto.Options != null && dto.Options.Any() && question.Options != null)
                {
                    foreach (var optDto in dto.Options)
                    {
                        if (optDto.Id.HasValue)
                        {
                            var existing = question.Options
                                .FirstOrDefault(o => o.Id == optDto.Id.Value);
                            if (existing != null)
                            {
                                existing.OptionLabel = optDto.OptionLabel;
                                existing.OptionText = optDto.OptionText;
                                existing.IsCorrect = optDto.IsCorrect;
                            }
                        }
                    }
                }

                // Re-increment task completed count
                if (question.TaskAssignment != null)
                {
                    question.TaskAssignment.CompletedCount++;
                    question.TaskAssignment.UpdatedAt = DateTime.UtcNow;
                    await _repo.UpdateTaskAssignmentAsync(question.TaskAssignment);
                }

                await _repo.UpdateQuestionAsync(question);

                _logger.LogInformation(
                    "Question {QuestionId} fixed and resubmitted by operator {OperatorId}",
                    questionId, registration.Id);

                return CommonResponse<bool>.SuccessResponse(
                    "Question fixed and resubmitted for review.", true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fixing question {QuestionId}", questionId);
                return CommonResponse<bool>.FailureResponse($"An error occurred: {ex.Message}");
            }
        }
    }
}
