namespace Winfocus.LMS.Application.Interfaces
{
    using Winfocus.LMS.Domain.Entities;

    /// <summary>
    /// Defines token generation operations.
    /// </summary>
    public interface ITokenService
    {
        /// <summary>
        /// Generates a JWT token for the specified user and roles.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="roles">The roles.</param>
        /// <param name="sessionId">
        /// <param name="permissions">The permissions.</param>
        /// The unique session identifier to embed as the JWT JTI claim.
        /// Used for session tracking and IP-based session locking.
        /// </param>
        /// <returns>The JWT token string.</returns>
        string GenerateToken(User user, IReadOnlyList<string> roles, string sessionId, IReadOnlyList<string> permissions);
    }
}
