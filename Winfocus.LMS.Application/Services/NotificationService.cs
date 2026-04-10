namespace Winfocus.LMS.Application.Services;

using Microsoft.Extensions.Logging;
using Winfocus.LMS.Application.DTOs;
using Winfocus.LMS.Application.DTOs.Common;
using Winfocus.LMS.Application.DTOs.Notifications;
using Winfocus.LMS.Application.Interfaces;
using Winfocus.LMS.Domain.Entities;
using Winfocus.LMS.Domain.Enums;

/// <summary>
/// Provides notification business logic: persistence and SignalR push.
/// Any service in the project can inject <see cref="INotificationService"/> to
/// send a real-time notification to any user.
/// </summary>
public sealed class NotificationService : INotificationService
{
    private readonly INotificationRepository _repository;
    private readonly INotificationHubContext _hubContext;
    private readonly ILogger<NotificationService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="NotificationService"/> class.
    /// </summary>
    /// <param name="repository">Notification data access.</param>
    /// <param name="hubContext">SignalR hub context for real-time push.</param>
    /// <param name="logger">Logger instance.</param>
    public NotificationService(
        INotificationRepository repository,
        INotificationHubContext hubContext,
        ILogger<NotificationService> logger)
    {
        _repository = repository;
        _hubContext = hubContext;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<NotificationDto> CreateAsync(
        Guid userId,
        NotificationType type,
        string message,
        NotificationPriority priority = NotificationPriority.Normal,
        string? payload = null,
        string? actionUrl = null,
        DateTime? expiresAt = null)
    {
        var notification = new Notification
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Type = type,
            Message = message,
            IsRead = false,
            Priority = priority,
            Payload = payload,
            ActionUrl = actionUrl,
            ExpiresAt = expiresAt,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = userId,
            IsActive = true,
            IsDeleted = false,
        };

        await _repository.AddAsync(notification);

        _logger.LogInformation(
            "Notification created for user {UserId}: {Message}",
            userId, message);

        var dto = MapToDto(notification);

        // Push to the user via SignalR (fire-and-forget, don't block on hub delivery)
        _ = Task.Run(async () =>
        {
            try
            {
                var signalDto = MapToSignalDto(notification);
                await _hubContext.NotifyUserAsync(userId, signalDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Failed to push notification {Id} to SignalR hub for user {UserId}",
                    notification.Id, userId);
            }
        });

        return dto;
    }

    /// <inheritdoc/>
    public async Task<CommonResponse<PagedResult<NotificationDto>>> GetAllAsync(
        Guid userId, PagedRequest request)
    {
        var query = (await _repository.GetByUserIdAsync(userId)).AsQueryable();

        // Apply filters
        if (request.Active.HasValue)
        {
            query = query.Where(n => n.IsActive == request.Active.Value);
        }

        if (!string.IsNullOrWhiteSpace(request.SearchText))
        {
            query = query.Where(n => n.Message.Contains(request.SearchText));
        }

        var totalCount = query.Count();

        // Sort
        query = request.SortOrder.ToLowerInvariant() == "desc"
            ? ApplySort(query, request.SortBy, descending: true)
            : ApplySort(query, request.SortBy, descending: false);

        var items = query
            .Skip(request.Offset)
            .Take(request.Limit)
            .Select(MapToDto)
            .ToList();

        return CommonResponse<PagedResult<NotificationDto>>.SuccessResponse(
            "Notifications retrieved.",
            new PagedResult<NotificationDto>(items, totalCount, request.Limit, request.Offset));
    }

    /// <inheritdoc/>
    public async Task<NotificationUnreadDto> GetUnreadAsync(Guid userId)
    {
        var unreadList = await _repository.GetUnreadByUserIdAsync(userId);
        var unread = unreadList.Select(MapToDto).ToList();
        return new NotificationUnreadDto(unread.Count, unread);
    }

    /// <inheritdoc/>
    public async Task<CommonResponse<bool>> MarkAsReadAsync(Guid notificationId, Guid userId)
    {
        var notification = await _repository.GetByIdAsync(notificationId);

        if (notification is null || notification.UserId != userId)
        {
            return CommonResponse<bool>.FailureResponse("Notification not found.");
        }

        notification.IsRead = true;
        notification.UpdatedAt = DateTime.UtcNow;
        await _repository.SaveChangesAsync();

        _logger.LogInformation(
            "Notification {Id} marked as read for user {UserId}",
            notificationId, userId);

        return CommonResponse<bool>.SuccessResponse("Notification marked as read.", true);
    }

    /// <inheritdoc/>
    public async Task<CommonResponse<bool>> MarkAllReadAsync(Guid userId)
    {
        var count = await _repository.MarkAllReadAsync(userId);

        _logger.LogInformation(
            "Marked {Count} notifications as read for user {UserId}",
            count, userId);

        return CommonResponse<bool>.SuccessResponse(
            $"{count} notification(s) marked as read.", true);
    }

    // ── Private helpers ────────────────────────────────────────────────────────

    private static NotificationDto MapToDto(Notification n) => new(
        n.Id,
        n.UserId,
        n.Type,
        n.Message,
        n.IsRead,
        n.Payload,
        n.Priority,
        n.ActionUrl,
        n.ExpiresAt,
        n.CreatedAt,
        n.UpdatedAt);

    private static NotificationSignalDto MapToSignalDto(Notification n) => new(
        n.Id,
        n.UserId,
        n.Type,
        n.Message,
        n.IsRead,
        n.Payload,
        n.Priority,
        n.ActionUrl,
        n.ExpiresAt,
        n.CreatedAt);

    private static IQueryable<Notification> ApplySort(
        IQueryable<Notification> query, string? sortBy, bool descending)
    {
        var ordered = sortBy?.ToLowerInvariant() switch
        {
            "message" => descending
                ? query.OrderByDescending(n => n.Message)
                : query.OrderBy(n => n.Message),
            "type" => descending
                ? query.OrderByDescending(n => n.Type)
                : query.OrderBy(n => n.Type),
            "priority" => descending
                ? query.OrderByDescending(n => n.Priority)
                : query.OrderBy(n => n.Priority),
            "isread" => descending
                ? query.OrderByDescending(n => n.IsRead)
                : query.OrderBy(n => n.IsRead),
            "createdat" => descending
                ? query.OrderByDescending(n => n.CreatedAt)
                : query.OrderBy(n => n.CreatedAt),
            _ => descending
                ? query.OrderByDescending(n => n.CreatedAt)
                : query.OrderBy(n => n.CreatedAt),
        };
        return ordered;
    }
}
