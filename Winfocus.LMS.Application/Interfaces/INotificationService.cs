namespace Winfocus.LMS.Application.Interfaces;

using Winfocus.LMS.Application.DTOs;
using Winfocus.LMS.Application.DTOs.Common;
using Winfocus.LMS.Application.DTOs.Notifications;
using Winfocus.LMS.Domain.Enums;

/// <summary>
/// Defines the contract for notification business logic.
/// Any service in the project can inject this to send real-time + persisted notifications.
/// </summary>
public interface INotificationService
{
    /// <summary>
    /// Creates a notification, persists it, and pushes it to the user via SignalR.
    /// </summary>
    /// <param name="userId">Target user ID.</param>
    /// <param name="type">Notification type.</param>
    /// <param name="message">Human-readable message.</param>
    /// <param name="priority">Priority level (default Normal).</param>
    /// <param name="payload">Optional JSON payload for rich data.</param>
    /// <param name="actionUrl">Optional navigation URL.</param>
    /// <param name="expiresAt">Optional expiration timestamp.</param>
    /// <returns>The created notification DTO.</returns>
    Task<NotificationDto> CreateAsync(
        Guid userId,
        NotificationType type,
        string message,
        NotificationPriority priority = NotificationPriority.Normal,
        string? payload = null,
        string? actionUrl = null,
        DateTime? expiresAt = null);

    /// <summary>
    /// Gets paginated notifications for a user (newest first).
    /// </summary>
    Task<CommonResponse<PagedResult<NotificationDto>>> GetAllAsync(Guid userId, PagedRequest request);

    /// <summary>
    /// Gets unread notification count and the unread notification list.
    /// </summary>
    Task<NotificationUnreadDto> GetUnreadAsync(Guid userId);

    /// <summary>
    /// Marks a single notification as read.
    /// </summary>
    Task<CommonResponse<bool>> MarkAsReadAsync(Guid notificationId, Guid userId);

    /// <summary>
    /// Marks all unread notifications for a user as read.
    /// </summary>
    Task<CommonResponse<bool>> MarkAllReadAsync(Guid userId);
}
