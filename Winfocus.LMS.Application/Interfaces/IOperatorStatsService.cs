namespace Winfocus.LMS.Application.Interfaces
{
    using Winfocus.LMS.Application.DTOs;
    using Winfocus.LMS.Application.DTOs.Stats;

    /// <summary>
    /// Service interface for operator productivity and performance statistics.
    /// </summary>
    public interface IOperatorStatsService
    {
        /// <summary>
        /// Gets detailed productivity statistics for a single operator.
        /// </summary>
        /// <param name="operatorId">The operator registration identifier.</param>
        /// <param name="filter">The period filter parameters.</param>
        /// <returns>Productivity stats wrapped in CommonResponse.</returns>
        Task<CommonResponse<OperatorProductivityDto>> GetProductivityAsync(
            Guid operatorId,
            OperatorStatsFilterDto filter);

        /// <summary>
        /// Gets comparison statistics for all operators (admin view).
        /// </summary>
        /// <param name="filter">The period filter parameters.</param>
        /// <returns>All operators comparison wrapped in CommonResponse.</returns>
        Task<CommonResponse<AllOperatorsStatsDto>> GetAllOperatorsStatsAsync(
            OperatorStatsFilterDto filter);

        /// <summary>
        /// Gets productivity statistics for the currently logged-in operator.
        /// Resolves the operator registration ID from the JWT user ID.
        /// </summary>
        /// <param name="userId">The user identifier from JWT token.</param>
        /// <param name="filter">The period filter parameters.</param>
        /// <returns>Productivity stats wrapped in CommonResponse.</returns>
        Task<CommonResponse<OperatorProductivityDto>> GetMyProductivityAsync(
            Guid userId,
            OperatorStatsFilterDto filter);
    }
}
