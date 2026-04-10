namespace Winfocus.LMS.Domain.Entities;

using Winfocus.LMS.Domain.Common;
using Winfocus.LMS.Domain.Enums;

/// <summary>
/// Represents a user notification persisted to the database and pushed via SignalR.
/// </summary>
public sealed class Notification : BaseEntity
{
    /// <summary>
    /// The unique identifier of the user who owns this notification.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// The type/category of this notification.
    /// </summary>
    public NotificationType Type { get; set; }

    /// <summary>
    /// The notification message displayed to the user.
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Indicates whether this notification has been read by the user.
    /// </summary>
    public bool IsRead { get; set; }

    /// <summary>
    /// Optional JSON payload containing additional context data.
    /// </summary>
    public string? Payload { get; set; }

    /// <summary>
    /// The urgency level of the notification.
    /// </summary>
    public NotificationPriority Priority { get; set; }

    /// <summary>
    /// Optional URL to navigate to when the notification is clicked.
    /// </summary>
    public string? ActionUrl { get; set; }

    /// <summary>
    /// Optional expiration timestamp after which the notification is no longer relevant.
    /// </summary>
    public DateTime? ExpiresAt { get; set; }

    // Navigation property
    // public User? User { get; set; }
}
