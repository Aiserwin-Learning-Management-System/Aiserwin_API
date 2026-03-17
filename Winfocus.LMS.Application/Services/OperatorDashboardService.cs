namespace Winfocus.LMS.Application.Services
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Winfocus.LMS.Application.DTOs;
    using Winfocus.LMS.Application.DTOs.Common;
    using Winfocus.LMS.Application.DTOs.Dashboard;
    using Winfocus.LMS.Application.Interfaces;
    using Winfocus.LMS.Domain.Entities;

    /// <summary>
    /// OperatorDashboardService.
    /// </summary>
    /// <seealso cref="Winfocus.LMS.Application.Interfaces.IOperatorDashboardService" />
    public sealed class OperatorDashboardService : IOperatorDashboardService
    {
        private readonly IOperatorDashboardRepository _repo;
        private readonly IFileStorageService _fileStorage;
        private readonly ILogger<OperatorDashboardService> _logger;

        private static readonly string[] FullNameFields = { "full_name", "fullname", "name" };
        private static readonly string[] FirstNameFields = { "first_name", "firstname" };
        private static readonly string[] LastNameFields = { "last_name", "lastname" };
        private static readonly string[] EmailFields = { "email", "email_address" };
        private static readonly string[] PhoneFields = { "phone", "mobile", "phone_number", "mobile_number" };
        private static readonly string[] PhotoFields = { "profile_photo", "photo", "profile_image" };

        /// <summary>
        /// Initializes a new instance of the <see cref="OperatorDashboardService"/> class.
        /// </summary>
        /// <param name="repo">The repo.</param>
        /// <param name="fileStorage">The file storage.</param>
        /// <param name="logger">The logger.</param>
        public OperatorDashboardService(
            IOperatorDashboardRepository repo,
            IFileStorageService fileStorage,
            ILogger<OperatorDashboardService> logger)
        {
            _repo = repo;
            _fileStorage = fileStorage;
            _logger = logger;
        }

        private async Task<(StaffRegistration? registration, string? error)>
            ValidateOperator(Guid userId)
        {
            // Check user's staff category
            var categoryName = await _repo.GetUserStaffCategoryAsync(userId);

            if (string.IsNullOrEmpty(categoryName) ||
                !categoryName.Contains("DTP", StringComparison.OrdinalIgnoreCase))
            {
                return (null, "Access denied. This dashboard is for DTP staff only.");
            }

            var registration = await _repo.GetOperatorRegistrationAsync(userId);

            if (registration == null)
            {
                return (null, "Operator registration not found. Please complete registration first.");
            }

            return (registration, null);
        }

        /// <inheritdoc/>
        public async Task<CommonResponse<DashboardDto>> GetDashboardAsync(
            Guid userId, string period = "monthly")
        {
            try
            {
                var (registration, error) = await ValidateOperator(userId);
                if (registration == null)
                    return CommonResponse<DashboardDto>.FailureResponse(error!);

                var operatorId = registration.Id;
                var (fromDate, toDate, periodLabel) = GetDateRange(period);

                // Build all sections
                var profile = await BuildProfileAsync(registration);
                var stats = await BuildStatsAsync(operatorId, fromDate, toDate, periodLabel);
                var tasks = await BuildActiveTasksAsync(operatorId);
                var corrections = await BuildCorrectionsAsync(operatorId);

                var dashboard = new DashboardDto
                {
                    Profile = profile,
                    Productivity = stats,
                    ActiveTasks = tasks,
                    Corrections = corrections
                };

                _logger.LogInformation(
                    "Dashboard loaded for operator {OperatorId}", operatorId);

                return CommonResponse<DashboardDto>.SuccessResponse(
                    "Dashboard loaded successfully.", dashboard);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading dashboard for UserId {UserId}", userId);
                return CommonResponse<DashboardDto>
                    .FailureResponse($"An error occurred: {ex.Message}");
            }
        }

        /// <inheritdoc/>
        public async Task<CommonResponse<OperatorProfileDto>> GetProfileAsync(Guid userId)
        {
            try
            {
                var (registration, error) = await ValidateOperator(userId);
                if (registration == null)
                    return CommonResponse<OperatorProfileDto>.FailureResponse(error!);

                var profile = await BuildProfileAsync(registration);
                return CommonResponse<OperatorProfileDto>.SuccessResponse(
                    "Profile loaded.", profile);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading profile for UserId {UserId}", userId);
                return CommonResponse<OperatorProfileDto>
                    .FailureResponse($"An error occurred: {ex.Message}");
            }
        }

        /// <inheritdoc/>
        public async Task<CommonResponse<PagedResult<ActiveTaskDto>>> GetMyTasksAsync(
            Guid userId, OperatorTaskFilterRequest request)
        {
            try
            {
                var (registration, error) = await ValidateOperator(userId);
                if (registration == null)
                    return CommonResponse<PagedResult<ActiveTaskDto>>.FailureResponse(error!);

                var query = _repo.QueryTasks(registration.Id);

                // ── Filters ──
                if (request.Status.HasValue)
                    query = query.Where(t => t.Status == request.Status.Value);

                if (request.Priority.HasValue)
                    query = query.Where(t => t.Priority == request.Priority.Value);

                if (request.Active.HasValue)
                    query = query.Where(t => t.IsActive == request.Active.Value);

                if (request.StartDate.HasValue)
                    query = query.Where(t => t.CreatedAt >= request.StartDate.Value);

                if (request.EndDate.HasValue)
                    query = query.Where(t => t.CreatedAt <= request.EndDate.Value);

                if (!string.IsNullOrWhiteSpace(request.SearchText))
                {
                    var search = request.SearchText.Trim().ToLower();
                    query = query.Where(t =>
                        t.Subject.Name.ToLower().Contains(search) ||
                        t.Syllabus.Name.ToLower().Contains(search) ||
                        (t.Chapter != null && t.Chapter.Name.ToLower().Contains(search)));
                }

                // ── Count ──
                var totalCount = await query.CountAsync();

                if (totalCount == 0)
                {
                    return CommonResponse<PagedResult<ActiveTaskDto>>.SuccessResponse(
                        "No tasks found.",
                        new PagedResult<ActiveTaskDto>(
                            new List<ActiveTaskDto>(), 0, request.Limit, request.Offset));
                }

                // ── Sort ──
                var isDesc = request.SortOrder
                    .Equals("desc", StringComparison.OrdinalIgnoreCase);

                query = request.SortBy?.ToLower() switch
                {
                    "deadline" => isDesc
                        ? query.OrderByDescending(t => t.Deadline)
                        : query.OrderBy(t => t.Deadline),
                    "priority" => isDesc
                        ? query.OrderByDescending(t => t.Priority)
                        : query.OrderBy(t => t.Priority),
                    "status" => isDesc
                        ? query.OrderByDescending(t => t.Status)
                        : query.OrderBy(t => t.Status),
                    "subject" => isDesc
                        ? query.OrderByDescending(t => t.Subject.Name)
                        : query.OrderBy(t => t.Subject.Name),
                    _ => query.OrderBy(t => t.Deadline)
                };

                // ── Paginate ──
                var tasks = await query
                    .Skip(request.Offset)
                    .Take(request.Limit)
                    .ToListAsync();

                var dtoList = tasks.Select(MapToActiveTask).ToList();

                return CommonResponse<PagedResult<ActiveTaskDto>>.SuccessResponse(
                    "Tasks loaded successfully.",
                    new PagedResult<ActiveTaskDto>(
                        dtoList, totalCount, request.Limit, request.Offset));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading tasks for UserId {UserId}", userId);
                return CommonResponse<PagedResult<ActiveTaskDto>>
                    .FailureResponse($"An error occurred: {ex.Message}");
            }
        }

        /// <inheritdoc/>
        public async Task<CommonResponse<ProductivityStatsDto>> GetStatsAsync(
            Guid userId, StatsFilterRequest request)
        {
            try
            {
                var (registration, error) = await ValidateOperator(userId);
                if (registration == null)
                    return CommonResponse<ProductivityStatsDto>.FailureResponse(error!);

                var (fromDate, toDate, periodLabel) = GetDateRange(
                    request.Period, request.StartDate, request.EndDate);

                var stats = await BuildStatsAsync(
                    registration.Id, fromDate, toDate, periodLabel);

                return CommonResponse<ProductivityStatsDto>.SuccessResponse(
                    "Stats loaded.", stats);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading stats for UserId {UserId}", userId);
                return CommonResponse<ProductivityStatsDto>
                    .FailureResponse($"An error occurred: {ex.Message}");
            }
        }

        private async Task<OperatorProfileDto> BuildProfileAsync(StaffRegistration reg)
        {
            // Build lookup from registration values
            var values = new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase);

            foreach (var v in reg.Values)
            {
                var key = v.FieldName?.Trim().ToLower() ?? "";
                if (!string.IsNullOrEmpty(key) && !values.ContainsKey(key))
                {
                    values[key] = v.Value;
                }
            }

            // Extract fields
            var fullName = FindValue(values, FullNameFields);

            // If no full_name, try combining first + last
            if (string.IsNullOrWhiteSpace(fullName))
            {
                var first = FindValue(values, FirstNameFields);
                var last = FindValue(values, LastNameFields);

                if (!string.IsNullOrWhiteSpace(first))
                {
                    fullName = string.IsNullOrWhiteSpace(last)
                        ? first : $"{first} {last}";
                }
            }

            var email = FindValue(values, EmailFields);
            var phone = FindValue(values, PhoneFields);
            var photoPath = FindValue(values, PhotoFields);

            // Generate EmployeeId
            var seqNum = await _repo.GetRegistrationSequenceAsync(reg.Id);
            var year = reg.CreatedAt.Year;
            var employeeId = $"REG-{year}-{seqNum:D5}";

            return new OperatorProfileDto
            {
                RegistrationId = reg.Id,
                FullName = fullName ?? "Unknown",
                EmployeeId = employeeId,
                Role = "DTP Operator",
                Email = email,
                Phone = phone,
                ProfilePhoto = photoPath,
                PhotoUrl = !string.IsNullOrEmpty(photoPath)
                    ? _fileStorage.GetFileUrl(photoPath)
                    : null,
                StaffCategory = reg.StaffCategory?.Name ?? "DTP",
                JoinedAt = reg.CreatedAt
            };
        }

        private async Task<ProductivityStatsDto> BuildStatsAsync(
            Guid operatorId, DateTime? fromDate, DateTime? toDate, string periodLabel)
        {
            var counts = await _repo.GetQuestionCountsByStatusAsync(
                operatorId, fromDate, toDate);

            var totalTarget = await _repo.GetTotalQuestionsTargetAsync(
                operatorId, fromDate, toDate);

            var taskCount = await _repo.GetTaskCountAsync(
                operatorId, fromDate, toDate);

            // QuestionStatus: Draft=0, Submitted=1, UnderReview=2, Approved=3, Rejected=4
            var draft = counts.GetValueOrDefault(0, 0);
            var submitted = counts.GetValueOrDefault(1, 0);
            var underReview = counts.GetValueOrDefault(2, 0);
            var approved = counts.GetValueOrDefault(3, 0);
            var rejected = counts.GetValueOrDefault(4, 0);

            var completed = submitted + underReview + approved;
            var pending = totalTarget - completed - rejected - draft;
            if (pending < 0) pending = 0;

            var achievement = totalTarget > 0
                ? Math.Round((decimal)completed / totalTarget * 100, 1)
                : 0;

            return new ProductivityStatsDto
            {
                Period = periodLabel,
                TotalTasks = taskCount,
                QuestionsToEnter = totalTarget,
                QuestionsCompleted = completed,
                PendingQuestions = pending,
                RejectedQuestions = rejected,
                DraftQuestions = draft,
                ApprovedQuestions = approved,
                TargetAchievement = achievement,
                ProgressBar = (int)achievement
            };
        }

        private async Task<List<ActiveTaskDto>> BuildActiveTasksAsync(Guid operatorId)
        {
            var tasks = await _repo.GetActiveTasksAsync(operatorId);
            return tasks.Select(MapToActiveTask).ToList();
        }

        private async Task<CorrectionsSummaryDto> BuildCorrectionsAsync(Guid operatorId)
        {
            var rejected = await _repo.GetRejectedQuestionsAsync(operatorId, take: 5);

            var summary = new CorrectionsSummaryDto
            {
                PendingCount = rejected.Count
            };

            if (rejected.Any())
            {
                var latestReview = rejected
                    .SelectMany(q => q.Reviews)
                    .Where(r => r.Action == 1) // Rejected
                    .OrderByDescending(r => r.ReviewedAt)
                    .FirstOrDefault();

                summary.LatestRejection = latestReview?.Feedback;
                summary.LatestRejectionDate = latestReview?.ReviewedAt;

                summary.RecentRejections = rejected.Select(q =>
                {
                    var review = q.Reviews
                        .Where(r => r.Action == 1)
                        .OrderByDescending(r => r.ReviewedAt)
                        .FirstOrDefault();

                    return new RejectedQuestionDto
                    {
                        QuestionId = q.Id,
                        QuestionPreview = q.QuestionText.Length > 100
                            ? q.QuestionText[..100] + "..."
                            : q.QuestionText,
                        Feedback = review?.Feedback,
                        RejectedAt = review?.ReviewedAt ?? q.UpdatedAt ?? q.CreatedAt,
                        ReviewerRole = review?.ReviewerRole ?? "Unknown"
                    };
                }).ToList();
            }

            return summary;
        }

        private static ActiveTaskDto MapToActiveTask(TaskAssignment t)
        {
            var daysRemaining = (t.Deadline.Date - DateTime.UtcNow.Date).Days;
            var progress = t.TotalQuestions > 0
                ? (int)Math.Round((double)t.CompletedCount / t.TotalQuestions * 100)
                : 0;

            return new ActiveTaskDto
            {
                TaskId = t.Id,
                TaskCode = $"T-{t.CreatedAt:yyyyMMdd}-{t.Id.ToString()[..4].ToUpper()}",
                QuestionType = t.QuestionType switch
                {
                    0 => "MCQ",
                    1 => "Short Answer",
                    2 => "Long Answer",
                    _ => "Unknown"
                },
                ResourceType = t.ResourceType?.Name ?? "N/A",
                Syllabus = t.Syllabus?.Name ?? "N/A",
                Grade = t.Grade?.Name ?? "N/A",
                Subject = t.Subject?.Name ?? "N/A",
                Unit = t.Unit?.Name,
                Chapter = t.Chapter?.Name,
                TotalQuestions = t.TotalQuestions,
                CompletedQuestions = t.CompletedCount,
                ProgressPercentage = progress,
                Deadline = t.Deadline,
                DaysRemaining = daysRemaining,
                Priority = t.Priority switch
                {
                    0 => "Low",
                    1 => "Medium",
                    2 => "High",
                    _ => "Medium"
                },
                Status = t.Status switch
                {
                    0 => "Pending",
                    1 => "InProgress",
                    2 => "Completed",
                    3 => "Overdue",
                    _ => "Unknown"
                },
                Instructions = t.Instructions
            };
        }

        private static string? FindValue(
            Dictionary<string, string?> values, string[] fieldNames)
        {
            foreach (var name in fieldNames)
            {
                if (values.TryGetValue(name, out var value)
                    && !string.IsNullOrWhiteSpace(value))
                {
                    return value;
                }
            }
            return null;
        }

        private static (DateTime? FromDate, DateTime? ToDate, string Label) GetDateRange(
            string period,
            DateTime? customStart = null,
            DateTime? customEnd = null)
        {
            var now = DateTime.UtcNow;

            return period.ToLower() switch
            {
                "daily" => (
                    now.Date,
                    now.Date.AddDays(1).AddTicks(-1),
                    "Today"),

                "weekly" => (
                    now.Date.AddDays(-(int)now.DayOfWeek),
                    now.Date.AddDays(7 - (int)now.DayOfWeek).AddTicks(-1),
                    "This Week"),

                "monthly" => (
                    new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc),
                    new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc)
                        .AddMonths(1).AddTicks(-1),
                    "This Month"),

                "custom" when customStart.HasValue && customEnd.HasValue => (
                    customStart.Value.Date,
                    customEnd.Value.Date.AddDays(1).AddTicks(-1),
                    $"{customStart.Value:dd MMM} - {customEnd.Value:dd MMM yyyy}"),

                "all" => (null, null, "All Time"),

                _ => (
                    new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc),
                    new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc)
                        .AddMonths(1).AddTicks(-1),
                    "This Month")
            };
        }
    }
}
