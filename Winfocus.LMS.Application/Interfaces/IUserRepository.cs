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

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>user.</returns>
        Task<User?> GetByIdAsync(Guid id);

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>task.</returns>
        Task UpdateAsync(User user);

        /// <summary>
        /// Gets the by names asynchronous.
        /// </summary>
        /// <param name="roleNames">The role names.</param>
        /// <returns>Task&lt;IReadOnlyList&lt;Role&gt;&gt;.</returns>
        Task<IReadOnlyList<Role>> GetByNamesAsync(IReadOnlyList<string> roleNames);

        /// <summary>
        /// Gets the by email asynchronous.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns>user.</returns>
        Task<User?> GetByEmailAsync(string email);
    }
}
