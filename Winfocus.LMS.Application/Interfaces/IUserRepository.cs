namespace Winfocus.LMS.Application.Interfaces
{
    using Winfocus.LMS.Domain.Entities;

    /// <summary>
    /// Defines persistence operations for users.
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Usernames the exists asynchronous.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns>bool.</returns>
        Task<bool> UsernameExistsAsync(string username);

        /// <summary>
        /// Gets the by username asynchronous.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns>user.</returns>
        Task<User?> GetByUsernameAsync(string username);

        /// <summary>
        /// Adds the asynchronous.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>task.</returns>
        Task AddAsync(User user);
    }
}
