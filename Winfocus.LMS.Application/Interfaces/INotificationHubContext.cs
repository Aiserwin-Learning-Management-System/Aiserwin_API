namespace Winfocus.LMS.Application.Interfaces;

using Winfocus.LMS.Application.DTOs.Notifications;

/// <summary>
/// Abstraction over SignalR Hub context so the Application layer
/// can push notifications without depending on SignalR directly.
/// </summary>
public interface INotificationHubContext
{
    /// <summary>
    /// Pushes a notification to a specific user's SignalR group.
    /// </summary>
    /// <param name="userId">The target user's ID.</param>
    /// <param name="notification">The notification payload.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task NotifyUserAsync(Guid userId, NotificationSignalDto notification, CancellationToken cancellationToken = default);
}
