namespace Winfocus.LMS.Application.Interfaces
{
    using Winfocus.LMS.Application.DTOs.Common;
    using Winfocus.LMS.Domain.Entities;

    /// <summary>
    /// Repository interface for UserLoginLog data access operations.
    /// </summary>
    public interface IUserLoginLogRepository
    {
        /// <summary>
        /// Adds a new login log entity to the database.
        /// </summary>
        /// <param name="entity">The login log entity to add.</param>
        /// <returns>The added login log entity with its generated ID.</returns>
        Task<UserLoginLog> AddAsync(UserLoginLog entity);

        /// <summary>
        /// Retrieves a login log by its ID.
        /// </summary>
        /// <param name="logId">The login log identifier.</param>
        /// <returns>The login log entity if found; otherwise, <c>null</c>.</returns>
        Task<UserLoginLog?> GetByIdAsync(Guid logId);

        /// <summary>
        /// Retrieves paginated, sorted, and filtered login logs for a specific user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="request">The pagination, sorting, and filtering parameters.</param>
        /// <returns>A paginated result of login logs.</returns>
        Task<PagedResult<UserLoginLog>> GetByUserIdAsync(Guid userId, PagedRequest request);

        /// <summary>
        /// Updates an existing login log entity.
        /// </summary>
        /// <param name="entity">The login log entity with updated values.</param>
        /// <returns>A task that represents the asynchronous update operation.</returns>
        Task UpdateAsync(UserLoginLog entity);
    }
}
