namespace Winfocus.LMS.Application.Interfaces
{
    using Winfocus.LMS.Application.DTOs.Common;
    using Winfocus.LMS.Application.DTOs.LoginLog;

    /// <summary>
    /// IUserLoginLogService.
    /// </summary>
    public interface IUserLoginLogService
    {
        /// <summary>
        /// Records a new login attempt.
        /// </summary>
        /// <param name="dto">The login log details.</param>
        /// <returns>The created login log entry.</returns>
        Task<UserLoginLogDto> AddLogAsync(CreateLoginLogDto dto);

        /// <summary>
        /// Retrieves paginated, sorted, and filtered login logs for a specific user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="request">The pagination, sorting, and filtering parameters.</param>
        /// <returns>A paginated result of login logs.</returns>
        Task<PagedResult<UserLoginLogDto>> GetLogsByUserIdAsync(Guid userId, PagedRequest request);

        /// <summary>
        /// Soft-deletes a specific login log entry.
        /// </summary>
        /// <param name="logId">The login log identifier.</param>
        /// <returns><c>true</c> if the log was successfully deleted; otherwise, <c>false</c>.</returns>
        Task<bool> DeleteLogAsync(Guid logId);
    }
}
