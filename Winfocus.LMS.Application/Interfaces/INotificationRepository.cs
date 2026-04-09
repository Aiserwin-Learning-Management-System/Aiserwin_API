namespace Winfocus.LMS.Application.Interfaces;

using Winfocus.LMS.Domain.Entities;

/// <summary>
/// Defines data access operations for the <see cref="Notification"/> entity.
/// </summary>
public interface INotificationRepository
{
    /// <summary>
    /// Adds a new notification to the database.
    /// </summary>
    Task AddAsync(Notification notification, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a notification by its unique identifier.
    /// </summary>
    Task<Notification?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all notifications for a specific user, sorted by creation date descending.
    /// </summary>
    Task<IReadOnlyList<Notification>> GetByUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves unread notifications for a specific user, sorted newest first.
    /// </summary>
    Task<IReadOnlyList<Notification>> GetUnreadByUserIdAsync(
        Guid userId,
        int limit = 20,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Saves any pending changes to the database.
    /// </summary>
    Task SaveChangesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Marks all unread notifications for a user as read using bulk update.
    /// Returns the number of notifications updated.
    /// </summary>
    Task<int> MarkAllReadAsync(Guid userId, CancellationToken cancellationToken = default);
}
