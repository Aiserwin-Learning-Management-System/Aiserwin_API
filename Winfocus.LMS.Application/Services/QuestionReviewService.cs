namespace Winfocus.LMS.Application.Services
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Winfocus.LMS.Application.DTOs;
    using Winfocus.LMS.Application.DTOs.Common;
    using Winfocus.LMS.Application.DTOs.Review;
    using Winfocus.LMS.Application.Helpers;
    using Winfocus.LMS.Application.Interfaces;
    using Winfocus.LMS.Domain.Entities;

    /// <summary>
    /// QuestionReviewService.
    /// </summary>
    /// <seealso cref="Winfocus.LMS.Application.Interfaces.IQuestionReviewService" />
    public sealed class QuestionReviewService : IQuestionReviewService
    {
        private readonly IQuestionReviewRepository _repo;
        private readonly ILogger<QuestionReviewService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="QuestionReviewService"/> class.
        /// </summary>
        /// <param name="repo">The repo.</param>
        /// <param name="logger">The logger.</param>
        public QuestionReviewService(
            IQuestionReviewRepository repo,
            ILogger<QuestionReviewService> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        /// <summary>
        /// Gets the pending reviews asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>The pending reviews asynchronous.</returns>
        public async Task<CommonResponse<PagedResult<ReviewQuestionListDto>>>
            GetPendingReviewsAsync(ReviewFilterRequest request)
        {
            try
            {
                var query = _repo.QuerySubmittedQuestions();

                // ── Hierarchy Filters (cascading) ────────────────
                if (request.Year.HasValue)
                    query = query.Where(q => q.TaskAssignment.Year == request.Year.Value);

                if (request.SyllabusId.HasValue)
                    query = query.Where(q => q.TaskAssignment.SyllabusId == request.SyllabusId.Value);

                if (request.GradeId.HasValue)
                    query = query.Where(q => q.TaskAssignment.GradeId == request.GradeId.Value);

                if (request.SubjectId.HasValue)
                    query = query.Where(q => q.TaskAssignment.SubjectId == request.SubjectId.Value);

                if (request.UnitId.HasValue)
                    query = query.Where(q => q.TaskAssignment.UnitId == request.UnitId.Value);

                if (request.ChapterId.HasValue)
                    query = query.Where(q => q.TaskAssignment.ChapterId == request.ChapterId.Value);

                if (request.ResourceTypeId.HasValue)
                    query = query.Where(q => q.TaskAssignment.ResourceTypeId == request.ResourceTypeId.Value);

                // ── Other Filters ────────────────────────────────
                if (request.OperatorId.HasValue)
                    query = query.Where(q => q.OperatorId == request.OperatorId.Value);

                if (request.QuestionType.HasValue)
                    query = query.Where(q => q.QuestionType == request.QuestionType.Value);

                if (!string.IsNullOrWhiteSpace(request.SearchText))
                {
                    var search = request.SearchText.Trim().ToLower();
                    query = query.Where(q =>
                        q.QuestionText.ToLower().Contains(search));
                }

                if (request.StartDate.HasValue)
                    query = query.Where(q => q.CreatedAt >= request.StartDate.Value);

                if (request.EndDate.HasValue)
                    query = query.Where(q => q.CreatedAt <= request.EndDate.Value);

                var totalCount = await query.CountAsync();

                if (totalCount == 0)
                {
                    return CommonResponse<PagedResult<ReviewQuestionListDto>>.SuccessResponse(
                        "No questions pending review.",
                        new PagedResult<ReviewQuestionListDto>(
                            new List<ReviewQuestionListDto>(), 0, request.Limit, request.Offset));
                }

                // ── Sort ─────────────────────────────────────────
                var isDesc = request.SortOrder.Equals("desc", StringComparison.OrdinalIgnoreCase);
                query = request.SortBy?.ToLower() switch
                {
                    "syllabus" => isDesc
                        ? query.OrderByDescending(q => q.TaskAssignment.Syllabus.Name)
                        : query.OrderBy(q => q.TaskAssignment.Syllabus.Name),
                    "grade" => isDesc
                        ? query.OrderByDescending(q => q.TaskAssignment.Grade.Name)
                        : query.OrderBy(q => q.TaskAssignment.Grade.Name),
                    "subject" => isDesc
                        ? query.OrderByDescending(q => q.TaskAssignment.Subject.Name)
                        : query.OrderBy(q => q.TaskAssignment.Subject.Name),
                    "marks" => isDesc
                        ? query.OrderByDescending(q => q.Marks)
                        : query.OrderBy(q => q.Marks),
                    "questiontype" => isDesc
                        ? query.OrderByDescending(q => q.QuestionType)
                        : query.OrderBy(q => q.QuestionType),
                    _ => isDesc
                        ? query.OrderByDescending(q => q.CreatedAt)
                        : query.OrderBy(q => q.CreatedAt)
                };

                var questions = await query
                    .Skip(request.Offset)
                    .Take(request.Limit)
                    .ToListAsync();

                var dtoList = questions.Select((q, i) => new ReviewQuestionListDto
                {
                    QuestionId = q.Id,
                    QuestionNumber = request.Offset + i + 1,
                    QuestionText = q.QuestionText.Length > 150
                        ? q.QuestionText[..150] + "..." : q.QuestionText,
                    QuestionType = HierarchyMapper.MapQuestionType(q.QuestionType),
                    OperatorName = HierarchyMapper.GetOperatorName(q.Operator),
                    OperatorId = q.OperatorId,
                    Marks = q.Marks,
                    SubmittedAt = q.UpdatedAt ?? q.CreatedAt,
                    ReviewCycle = (q.Reviews?.Count(r => r.Action == 1) ?? 0) + 1,
                    TaskCode = HierarchyMapper.GetTaskCode(q.TaskAssignment),

                    // ── Full Hierarchy ────────────────────────
                    Year = HierarchyMapper.GetYear(q.TaskAssignment),
                    Syllabus = HierarchyMapper.GetSyllabus(q.TaskAssignment),
                    Grade = HierarchyMapper.GetGrade(q.TaskAssignment),
                    Subject = HierarchyMapper.GetSubject(q.TaskAssignment),
                    Unit = HierarchyMapper.GetUnit(q.TaskAssignment),
                    Chapter = HierarchyMapper.GetChapter(q.TaskAssignment),
                    ResourceType = HierarchyMapper.GetResourceType(q.TaskAssignment),
                }).ToList();

                return CommonResponse<PagedResult<ReviewQuestionListDto>>.SuccessResponse(
                    "Pending reviews loaded.",
                    new PagedResult<ReviewQuestionListDto>(
                        dtoList, totalCount, request.Limit, request.Offset));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading pending reviews.");
                return CommonResponse<PagedResult<ReviewQuestionListDto>>
                    .FailureResponse($"An error occurred: {ex.Message}");
            }
        }

        /// <inheritdoc/>
        public async Task<CommonResponse<ReviewQuestionDetailDto>>
            GetQuestionForReviewAsync(Guid questionId)
        {
            try
            {
                var q = await _repo.GetQuestionWithDetailsAsync(questionId);

                if (q == null)
                {
                    return CommonResponse<ReviewQuestionDetailDto>
                        .FailureResponse("Question not found.");
                }

                var detail = new ReviewQuestionDetailDto
                {
                    QuestionId = q.Id,
                    TaskId = q.TaskId,
                    QuestionText = q.QuestionText,
                    QuestionType = HierarchyMapper.MapQuestionType(q.QuestionType),
                    Marks = q.Marks,
                    CorrectAnswer = q.CorrectAnswer,
                    CorrectAnswerText = q.CorrectAnswerText,
                    Reference = q.Reference,
                    OperatorName = HierarchyMapper.GetOperatorName(q.Operator),
                    Status = HierarchyMapper.MapStatus(q.Status),
                    SubmittedAt = q.UpdatedAt ?? q.CreatedAt,
                    TaskCode = HierarchyMapper.GetTaskCode(q.TaskAssignment),

                    // ── Full Hierarchy with IDs ──────────────
                    Year = HierarchyMapper.GetYear(q.TaskAssignment),
                    Syllabus = HierarchyMapper.GetSyllabus(q.TaskAssignment),
                    SyllabusId = q.TaskAssignment?.SyllabusId,
                    Grade = HierarchyMapper.GetGrade(q.TaskAssignment),
                    GradeId = q.TaskAssignment?.GradeId,
                    Subject = HierarchyMapper.GetSubject(q.TaskAssignment),
                    SubjectId = q.TaskAssignment?.SubjectId,
                    Unit = HierarchyMapper.GetUnit(q.TaskAssignment),
                    UnitId = q.TaskAssignment?.UnitId,
                    Chapter = HierarchyMapper.GetChapter(q.TaskAssignment),
                    ChapterId = q.TaskAssignment?.ChapterId,
                    ResourceType = HierarchyMapper.GetResourceType(q.TaskAssignment),
                    ResourceTypeId = q.TaskAssignment?.ResourceTypeId,

                    Options = q.Options?.Select(o => new QuestionOptionDto
                    {
                        Id = o.Id,
                        OptionLabel = o.OptionLabel,
                        OptionText = o.OptionText,
                        IsCorrect = o.IsCorrect,
                    }).OrderBy(o => o.OptionLabel).ToList() ?? new (),

                    ReviewHistory = HierarchyMapper.BuildReviewHistory(q.Reviews),
                };

                return CommonResponse<ReviewQuestionDetailDto>.SuccessResponse(
                    "Question loaded.", detail);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading question {QuestionId}", questionId);
                return CommonResponse<ReviewQuestionDetailDto>
                    .FailureResponse($"An error occurred: {ex.Message}");
            }
        }

        /// <inheritdoc/>
        public async Task<CommonResponse<bool>> ApproveAsync(
            Guid questionId, Guid reviewerId, string reviewerRole, ApproveQuestionDto? dto)
        {
            try
            {
                var question = await _repo.GetQuestionWithDetailsAsync(questionId);

                if (question == null)
                {
                    return CommonResponse<bool>.FailureResponse("Question not found.");
                }

                if (question.Status != 1)
                {
                    return CommonResponse<bool>.FailureResponse(
                        $"Question is '{HierarchyMapper.MapStatus(question.Status)}', not 'Submitted'.");
                }

                question.Status = 3; // Approved
                question.UpdatedAt = DateTime.UtcNow;
                question.UpdatedBy = reviewerId;

                var review = new QuestionReview
                {
                    QuestionId = questionId,
                    ReviewerId = reviewerId,
                    ReviewerRole = reviewerRole,
                    Action = 0, // Approved
                    Feedback = dto?.Remarks,
                    ReviewedAt = DateTime.UtcNow,
                };

                await _repo.UpdateQuestionAsync(question);
                await _repo.AddReviewAsync(review);

                _logger.LogInformation(
                    "Question {QuestionId} approved by {Role} {ReviewerId}",
                    questionId,
                    reviewerRole,
                    reviewerId);

                return CommonResponse<bool>.SuccessResponse(
                    "Question approved successfully.", true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error approving question {QuestionId}", questionId);
                return CommonResponse<bool>.FailureResponse($"An error occurred: {ex.Message}");
            }
        }

        /// <inheritdoc/>
        public async Task<CommonResponse<bool>> RejectAsync(
            Guid questionId, Guid reviewerId, string reviewerRole, RejectQuestionDto dto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dto.Feedback))
                {
                    return CommonResponse<bool>.FailureResponse(
                        "Feedback is required when rejecting a question.");
                }

                var question = await _repo.GetQuestionWithDetailsAsync(questionId);

                if (question == null)
                {
                    return CommonResponse<bool>.FailureResponse("Question not found.");
                }

                if (question.Status != 1)
                {
                    return CommonResponse<bool>.FailureResponse(
                        $"Question is '{HierarchyMapper.MapStatus(question.Status)}', not 'Submitted'.");
                }

                question.Status = 4; // Rejected
                question.UpdatedAt = DateTime.UtcNow;
                question.UpdatedBy = reviewerId;

                // Decrement task completed count
                if (question.TaskAssignment != null && question.TaskAssignment.CompletedCount > 0)
                {
                    question.TaskAssignment.CompletedCount--;
                    question.TaskAssignment.UpdatedAt = DateTime.UtcNow;
                    await _repo.UpdateTaskAssignmentAsync(question.TaskAssignment);
                }

                var review = new QuestionReview
                {
                    QuestionId = questionId,
                    ReviewerId = reviewerId,
                    ReviewerRole = reviewerRole,
                    Action = 1, // Rejected
                    Feedback = dto.Feedback,
                    ReviewedAt = DateTime.UtcNow,
                };

                await _repo.UpdateQuestionAsync(question);
                await _repo.AddReviewAsync(review);

                _logger.LogInformation(
                    "Question {QuestionId} rejected by {Role}. Feedback: {Feedback}",
                    questionId,
                    reviewerRole,
                    dto.Feedback);

                return CommonResponse<bool>.SuccessResponse(
                    "Question rejected with feedback.", true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error rejecting question {QuestionId}", questionId);
                return CommonResponse<bool>.FailureResponse($"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets the review stats asynchronous.
        /// </summary>
        /// <returns>The review stats asynchronous.</returns>
        public async Task<CommonResponse<ReviewStatsDto>> GetReviewStatsAsync()
        {
            try
            {
                var stats = new ReviewStatsDto
                {
                    TotalPending = await _repo.CountByStatusAsync(1),
                    ApprovedToday = await _repo.CountReviewedTodayAsync(0),
                    RejectedToday = await _repo.CountReviewedTodayAsync(1),
                    TotalApprovedAllTime = await _repo.CountAllByActionAsync(0),
                    TotalRejectedAllTime = await _repo.CountAllByActionAsync(1),
                };
                stats.TotalReviewedToday = stats.ApprovedToday + stats.RejectedToday;

                return CommonResponse<ReviewStatsDto>.SuccessResponse("Review stats loaded.", stats);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading review stats.");
                return CommonResponse<ReviewStatsDto>.FailureResponse($"An error occurred: {ex.Message}");
            }
        }
    }
}
