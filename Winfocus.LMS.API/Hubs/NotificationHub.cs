namespace Winfocus.LMS.API.Hubs;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Winfocus.LMS.Application.DTOs.Notifications;
using Winfocus.LMS.Application.Interfaces;

/// <summary>
/// SignalR hub for real-time user notifications.
/// Clients connect with their JWT bearer token and are automatically
/// placed into a group matching their user ID. The server pushes
/// notifications to each user group via <see cref="INotificationHubContext"/>.
/// </summary>
[Authorize]
public sealed class NotificationHub : Hub
{
    /// <summary>
    /// Adds the connected client to a SignalR group named after their user ID.
    /// SignalR extracts the user identifier from the validated JWT bearer token.
    /// </summary>
    public override async Task OnConnectedAsync()
    {
        var userId = Context.UserIdentifier;
        if (!string.IsNullOrEmpty(userId))
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, userId);
        }

        await base.OnConnectedAsync();
    }

    /// <summary>
    /// Removes the client from their user group on disconnect.
    /// </summary>
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = Context.UserIdentifier;
        if (!string.IsNullOrEmpty(userId))
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, userId);
        }

        await base.OnDisconnectedAsync(exception);
    }
}

/// <summary>
/// Implementation of <see cref="INotificationHubContext"/> that wraps SignalR hub context.
/// Registered as a singleton so it can be injected into application services.
/// </summary>
public sealed class NotificationHubContext : INotificationHubContext
{
    private readonly IHubContext<NotificationHub> _hubContext;

    public NotificationHubContext(IHubContext<NotificationHub> hubContext)
    {
        _hubContext = hubContext;
    }

    /// <inheritdoc/>
    public async Task NotifyUserAsync(Guid userId, NotificationSignalDto notification, CancellationToken cancellationToken = default)
    {
        await _hubContext.Clients
            .Group(userId.ToString())
            .SendAsync("ReceiveNotification", notification, cancellationToken);
    }
}
