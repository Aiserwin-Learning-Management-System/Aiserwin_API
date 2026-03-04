namespace Winfocus.LMS.Application.Interfaces
{
    using Winfocus.LMS.Domain.Entities;

    /// <summary>
    /// Repository interface for UserActiveSession data access operations.
    /// </summary>
    public interface IUserSessionRepository
    {
        /// <summary>
        /// Gets the current active (non-revoked, non-expired) session for a user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>The active session if one exists; otherwise, <c>null</c>.</returns>
        Task<UserActiveSession?> GetActiveSessionByUserIdAsync(Guid userId);

        /// <summary>
        /// Gets a session by its unique session identifier (JWT JTI).
        /// </summary>
        /// <param name="sessionId">The session identifier.</param>
        /// <returns>The session if found; otherwise, <c>null</c>.</returns>
        Task<UserActiveSession?> GetBySessionIdAsync(string sessionId);

        /// <summary>
        /// Adds a new session to the database.
        /// </summary>
        /// <param name="session">The session entity to add.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task AddAsync(UserActiveSession session);

        /// <summary>
        /// Updates an existing session entity.
        /// </summary>
        /// <param name="session">The session entity with updated values.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task UpdateAsync(UserActiveSession session);

        /// <summary>
        /// Revokes all active sessions for a specific user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task RevokeAllUserSessionsAsync(Guid userId);
    }
}
