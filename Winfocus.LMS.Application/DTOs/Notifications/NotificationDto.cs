namespace Winfocus.LMS.Application.DTOs.Notifications;

using Winfocus.LMS.Domain.Enums;

/// <summary>
/// DTO returned by notification list endpoints.
/// </summary>
public sealed record NotificationDto(
    Guid Id,
    Guid UserId,
    NotificationType Type,
    string Message,
    bool IsRead,
    string? Payload,
    NotificationPriority Priority,
    string? ActionUrl,
    DateTime? ExpiresAt,
    DateTime CreatedAt,
    DateTime? UpdatedAt);

/// <summary>
/// DTO sent over SignalR to the client.
/// </summary>
public sealed record NotificationSignalDto(
    Guid Id,
    Guid UserId,
    NotificationType Type,
    string Message,
    bool IsRead,
    string? Payload,
    NotificationPriority Priority,
    string? ActionUrl,
    DateTime? ExpiresAt,
    DateTime CreatedAt);

/// <summary>
/// DTO returned by the unread count endpoint.
/// </summary>
public sealed record NotificationUnreadDto(
    int UnreadCount,
    IReadOnlyList<NotificationDto> UnreadNotifications);
