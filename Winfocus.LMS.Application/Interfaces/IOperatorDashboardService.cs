namespace Winfocus.LMS.Application.Interfaces
{
    using Winfocus.LMS.Application.DTOs;
    using Winfocus.LMS.Application.DTOs.Common;
    using Winfocus.LMS.Application.DTOs.Dashboard;

    /// <summary>
    /// IOperatorDashboardService.
    /// </summary>
    public interface IOperatorDashboardService
    {
        /// <summary>
        /// Gets the dashboard asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="period">The period.</param>
        /// <returns></returns>
        Task<CommonResponse<DashboardDto>> GetDashboardAsync(Guid userId, string period = "monthly");

        /// <summary>
        /// Gets the profile asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        Task<CommonResponse<OperatorProfileDto>> GetProfileAsync(Guid userId);

        /// <summary>
        /// Gets my tasks asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        Task<CommonResponse<PagedResult<ActiveTaskDto>>> GetMyTasksAsync(Guid userId, OperatorTaskFilterRequest request);

        /// <summary>
        /// Gets the stats asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        Task<CommonResponse<ProductivityStatsDto>> GetStatsAsync(Guid userId, StatsFilterRequest request);
    }
}
