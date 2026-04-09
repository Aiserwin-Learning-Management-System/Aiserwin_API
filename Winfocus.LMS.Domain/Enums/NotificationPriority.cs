namespace Winfocus.LMS.Domain.Enums;

/// <summary>
/// Represents the urgency level of a notification.
/// </summary>
public enum NotificationPriority
{
    /// <summary>Low urgency — informational only.</summary>
    Low = 0,

    /// <summary>Normal urgency — standard notifications.</summary>
    Normal = 1,

    /// <summary>High urgency — requires immediate attention.</summary>
    High = 2,
}
