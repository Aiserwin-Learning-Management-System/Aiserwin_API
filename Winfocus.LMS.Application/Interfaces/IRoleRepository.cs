namespace Winfocus.LMS.Application.Interfaces
{
    using Winfocus.LMS.Domain.Entities;

    /// <summary>
    /// Defines persistence operations for roles.
    /// </summary>
    public interface IRoleRepository
    {
        /// <summary>
        /// Gets the by name asynchronous.
        /// </summary>
        /// <param name="roleName">Name of the role.</param>
        /// <returns>Role.</returns>
        Task<Role?> GetByNameAsync(string roleName);
    }
}
