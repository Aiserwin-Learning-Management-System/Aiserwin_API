namespace Winfocus.LMS.Infrastructure.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using Winfocus.LMS.Application.Interfaces;
    using Winfocus.LMS.Domain.Entities;
    using Winfocus.LMS.Infrastructure.Data;

    /// <summary>
    /// EF Core implementation of <see cref="IUserSessionRepository"/>.
    /// Handles all database interactions for user active sessions.
    /// </summary>
    public sealed class UserSessionRepository : IUserSessionRepository
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserSessionRepository"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        public UserSessionRepository(AppDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc />
        public async Task<UserActiveSession?> GetActiveSessionByUserIdAsync(
            Guid userId)
        {
            return await _context.UserActiveSessions
                .Where(s => s.UserId == userId
                         && s.IsActive
                         && !s.IsRevoked
                         && s.ExpiresAt > DateTimeOffset.UtcNow)
                .OrderByDescending(s => s.LoginAt)
                .FirstOrDefaultAsync();
        }

        /// <inheritdoc />
        public async Task<UserActiveSession?> GetBySessionIdAsync(
            string sessionId)
        {
            return await _context.UserActiveSessions
                .FirstOrDefaultAsync(s => s.SessionId == sessionId);
        }

        /// <inheritdoc />
        public async Task AddAsync(UserActiveSession session)
        {
            _context.UserActiveSessions.Add(session);
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc />
        public async Task UpdateAsync(UserActiveSession session)
        {
            _context.UserActiveSessions.Update(session);
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc />
        public async Task RevokeAllUserSessionsAsync(Guid userId)
        {
            var activeSessions = await _context.UserActiveSessions
                .Where(s => s.UserId == userId
                         && s.IsActive
                         && !s.IsRevoked
                         && s.ExpiresAt > DateTimeOffset.UtcNow)
                .ToListAsync();

            foreach (var session in activeSessions)
            {
                session.IsRevoked = true;
                session.LogoutAt = DateTimeOffset.UtcNow;
                session.UpdatedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
        }
    }
}
