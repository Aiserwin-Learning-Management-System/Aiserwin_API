namespace Winfocus.LMS.Application.Interfaces
{
    using Winfocus.LMS.Application.DTOs;
    using Winfocus.LMS.Application.DTOs.Common;
    using Winfocus.LMS.Application.DTOs.DtpAdmin;
    using Winfocus.LMS.Domain.Entities;

    /// <summary>
    /// Repository interface for Daily Activity Report data access.
    /// </summary>
    public interface IDarRepository
    {
        /// <summary>
        /// Creates a new Daily Activity Report asynchronously.
        /// </summary>
        /// <param name="dar">The Daily Activity Report to create.</param>
        /// <returns>The created Daily Activity Report.</returns>
        Task<DailyActivityReport> CreateAsync(DailyActivityReport dar);

        /// <summary>
        /// Updates an existing Daily Activity Report asynchronously.
        /// </summary>
        /// <param name="dar">The Daily Activity Report to update.</param>
        /// <returns>The updated Daily Activity Report.</returns>
        Task<DailyActivityReport> UpdateAsync(DailyActivityReport dar);

        /// <summary>
        /// Gets a Daily Activity Report by ID asynchronously.
        /// </summary>
        /// <param name="id">The DAR identifier.</param>
        /// <returns>The Daily Activity Report or null if not found.</returns>
        Task<DailyActivityReport?> GetByIdAsync(Guid id);

        /// <summary>
        /// Gets today's Daily Activity Report for an operator asynchronously.
        /// </summary>
        /// <param name="operatorId">The operator identifier.</param>
        /// <param name="date">The date to search for.</param>
        /// <returns>The Daily Activity Report or null if not found.</returns>
        Task<DailyActivityReport?> GetTodayByOperatorAsync(Guid operatorId, DateOnly date);

        /// <summary>
        /// Gets paginated Daily Activity Reports for an operator asynchronously.
        /// </summary>
        /// <param name="operatorId">The operator identifier.</param>
        /// <param name="request">The pagination request.</param>
        /// <returns>Tuple of items and total count.</returns>
        Task<(List<DailyActivityReport> items, int totalCount)> GetOperatorDarsAsync(
            Guid operatorId, PagedRequest request);

        /// <summary>
        /// Gets paginated Daily Activity Reports for admin view with filtering asynchronously.
        /// </summary>
        /// <param name="filter">The filter request with date range and operator ID.</param>
        /// <returns>Tuple of items and total count.</returns>
        Task<(List<DailyActivityReport> items, int totalCount)> GetAllDarsAsync(
            DarFilterRequest filter);

        /// <summary>
        /// Gets a queryable collection of Daily Activity Reports for advanced filtering.
        /// </summary>
        /// <returns>Queryable Daily Activity Reports.</returns>
        IQueryable<DailyActivityReport> Query();
    }
}
