using Microsoft.EntityFrameworkCore;
using Winfocus.LMS.Application.DTOs;
using Winfocus.LMS.Application.DTOs.Common;
using Winfocus.LMS.Application.DTOs.DtpAdmin;
using Winfocus.LMS.Application.Interfaces;
using Winfocus.LMS.Domain.Entities;
using Winfocus.LMS.Infrastructure.Data;

namespace Winfocus.LMS.Infrastructure.Repositories
{
    /// <summary>
    /// Repository for Daily Activity Report data access.
    /// </summary>
    public sealed class DarRepository : IDarRepository
    {
        /// <summary>
        /// The application database context.
        /// </summary>
        private readonly AppDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="DarRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public DarRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Creates a new Daily Activity Report asynchronously.
        /// </summary>
        /// <param name="dar">The Daily Activity Report to create.</param>
        /// <returns>The created Daily Activity Report.</returns>
        public async Task<DailyActivityReport> CreateAsync(DailyActivityReport dar)
        {
            _dbContext.DailyActivityReports.Add(dar);
            await _dbContext.SaveChangesAsync();
            return dar;
        }

        /// <summary>
        /// Updates an existing Daily Activity Report asynchronously.
        /// </summary>
        /// <param name="dar">The Daily Activity Report to update.</param>
        /// <returns>The updated Daily Activity Report.</returns>
        public async Task<DailyActivityReport> UpdateAsync(DailyActivityReport dar)
        {
            dar.UpdatedAt = DateTime.UtcNow;
            _dbContext.DailyActivityReports.Update(dar);
            await _dbContext.SaveChangesAsync();
            return dar;
        }

        /// <summary>
        /// Gets a Daily Activity Report by ID asynchronously.
        /// </summary>
        /// <param name="id">The DAR identifier.</param>
        /// <returns>The Daily Activity Report or null if not found.</returns>
        public async Task<DailyActivityReport?> GetByIdAsync(Guid id)
        {
            return await _dbContext.DailyActivityReports
                .Include(d => d.TaskAssignment)
                .ThenInclude(t => t!.Subject)
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        }

        /// <summary>
        /// Gets today's Daily Activity Report for an operator asynchronously.
        /// </summary>
        /// <param name="operatorId">The operator identifier.</param>
        /// <param name="date">The date to search for.</param>
        /// <returns>The Daily Activity Report or null if not found.</returns>
        public async Task<DailyActivityReport?> GetTodayByOperatorAsync(Guid operatorId, DateOnly date)
        {
            return await _dbContext.DailyActivityReports
                .Include(d => d.TaskAssignment)
                .ThenInclude(t => t!.Subject)
                .FirstOrDefaultAsync(x =>
                    x.OperatorId == operatorId
                    && x.ReportDate == date
                    && !x.IsDeleted);
        }

        /// <summary>
        /// Gets paginated Daily Activity Reports for an operator asynchronously.
        /// </summary>
        /// <param name="operatorId">The operator identifier.</param>
        /// <param name="request">The pagination request.</param>
        /// <returns>Tuple of items and total count.</returns>
        public async Task<(List<DailyActivityReport> items, int totalCount)> GetOperatorDarsAsync(
            Guid operatorId, PagedRequest request)
        {
            var query = _dbContext.DailyActivityReports
                .Include(d => d.TaskAssignment)
                .ThenInclude(t => t!.Subject)
                .Where(x => x.OperatorId == operatorId && !x.IsDeleted);

            var totalCount = await query.CountAsync();

            // Apply sorting
            if (request.SortBy?.ToLower() == "reportdate")
            {
                query = request.SortOrder?.ToLower() == "desc"
                    ? query.OrderByDescending(x => x.ReportDate)
                    : query.OrderBy(x => x.ReportDate);
            }
            else
            {
                query = request.SortOrder?.ToLower() == "desc"
                    ? query.OrderByDescending(x => x.CreatedAt)
                    : query.OrderBy(x => x.CreatedAt);
            }

            // Apply pagination
            var items = await query
                .Skip(request.Offset)
                .Take(request.Limit)
                .ToListAsync();

            return (items, totalCount);
        }

        /// <summary>
        /// Gets paginated Daily Activity Reports for admin view with filtering asynchronously.
        /// </summary>
        /// <param name="filter">The filter request with date range and operator ID.</param>
        /// <returns>Tuple of items and total count.</returns>
        public async Task<(List<DailyActivityReport> items, int totalCount)> GetAllDarsAsync(
            DarFilterRequest filter)
        {
            var query = _dbContext.DailyActivityReports
                .Include(d => d.Operator)
                .Include(d => d.TaskAssignment)
                .ThenInclude(t => t!.Subject)
                .Where(x => !x.IsDeleted);

            // Filter by operator if specified
            if (filter.OperatorId.HasValue)
            {
                query = query.Where(x => x.OperatorId == filter.OperatorId.Value);
            }

            // Filter by date range
            if (filter.StartDate.HasValue)
            {
                var startDate = DateOnly.FromDateTime(filter.StartDate.Value);
                query = query.Where(x => x.ReportDate >= startDate);
            }

            if (filter.EndDate.HasValue)
            {
                var endDate = DateOnly.FromDateTime(filter.EndDate.Value);
                query = query.Where(x => x.ReportDate <= endDate);
            }

            var totalCount = await query.CountAsync();

            // Apply sorting
            if (filter.SortBy?.ToLower() == "reportdate")
            {
                query = filter.SortOrder?.ToLower() == "desc"
                    ? query.OrderByDescending(x => x.ReportDate)
                    : query.OrderBy(x => x.ReportDate);
            }
            else
            {
                query = filter.SortOrder?.ToLower() == "desc"
                    ? query.OrderByDescending(x => x.CreatedAt)
                    : query.OrderBy(x => x.CreatedAt);
            }

            // Apply pagination
            var items = await query
                .Skip(filter.Offset)
                .Take(filter.Limit)
                .ToListAsync();

            return (items, totalCount);
        }

        /// <summary>
        /// Gets a queryable collection of Daily Activity Reports for advanced filtering.
        /// </summary>
        /// <returns>Queryable Daily Activity Reports.</returns>
        public IQueryable<DailyActivityReport> Query()
        {
            return _dbContext.DailyActivityReports
                .AsNoTracking()
                .Where(x => !x.IsDeleted);
        }
    }
}
