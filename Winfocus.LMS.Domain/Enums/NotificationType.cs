namespace Winfocus.LMS.Domain.Enums;

/// <summary>
/// Represents the category or source of a notification.
/// </summary>
public enum NotificationType
{
    /// <summary>Discount-related notifications (e.g., fee reductions, promotional offers).</summary>
    Discount = 0,

    /// <summary>Order or enrollment-related notifications.</summary>
    Order = 1,

    /// <summary>General system-level notifications.</summary>
    System = 2,

    /// <summary>Priority alerts or warnings.</summary>
    Alert = 3,
}
