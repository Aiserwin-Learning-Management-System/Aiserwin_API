namespace Winfocus.LMS.Infrastructure.Repositories
{
    using System.Linq.Expressions;
    using Microsoft.EntityFrameworkCore;
    using Winfocus.LMS.Application.DTOs.Common;
    using Winfocus.LMS.Application.Interfaces;
    using Winfocus.LMS.Domain.Entities;
    using Winfocus.LMS.Infrastructure.Data;

    /// <summary>
    /// Repository implementation for UserLoginLog data access.
    /// Handles all database interactions — no business logic here.
    /// </summary>
    public class UserLoginLogRepository : IUserLoginLogRepository
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserLoginLogRepository"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public UserLoginLogRepository(AppDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc />
        public async Task<UserLoginLog> AddAsync(UserLoginLog entity)
        {
            _context.UserLoginLogs.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        /// <inheritdoc />
        public async Task<UserLoginLog?> GetByIdAsync(Guid logId)
        {
            return await _context.UserLoginLogs
                .FirstOrDefaultAsync(x => x.Id == logId);
        }

        /// <inheritdoc />
        public async Task<PagedResult<UserLoginLog>> GetByUserIdAsync(
            Guid userId, PagedRequest request)
        {
            var query = _context.UserLoginLogs
                .AsNoTracking()
                .Where(x => x.UserId == userId);

            // Active status filter
            if (request.Active.HasValue)
            {
                query = query.Where(x => x.IsActive == request.Active.Value);
            }
            else
            {
                query = query.Where(x => x.IsActive);
            }

            // Date range filter
            if (request.StartDate.HasValue)
            {
                query = query.Where(x => x.LoginTimestamp >= request.StartDate.Value);
            }

            if (request.EndDate.HasValue)
            {
                query = query.Where(x => x.LoginTimestamp <= request.EndDate.Value);
            }

            // Search text filter
            if (!string.IsNullOrWhiteSpace(request.SearchText))
            {
                var search = request.SearchText.Trim().ToLower();
                query = query.Where(x =>
                    (x.IpAddress != null && x.IpAddress.ToLower().Contains(search)) ||
                    (x.UserAgent != null && x.UserAgent.ToLower().Contains(search)) ||
                    (x.FailureReason != null && x.FailureReason.ToLower().Contains(search)));
            }

            // Total count before pagination
            var totalCount = await query.CountAsync();

            // Dynamic sorting
            query = ApplySorting(query, request.SortBy, request.SortOrder);

            // Pagination
            var items = await query
                .Skip(request.Offset)
                .Take(request.Limit)
                .ToListAsync();

            return new PagedResult<UserLoginLog>(
                items: items,
                totalCount: totalCount,
                limit: request.Limit,
                offset: request.Offset);
        }

        /// <inheritdoc />
        public async Task UpdateAsync(UserLoginLog entity)
        {
            _context.UserLoginLogs.Update(entity);
            await _context.SaveChangesAsync();
        }

        private static IQueryable<UserLoginLog> ApplySorting(
            IQueryable<UserLoginLog> query, string sortBy, string sortOrder)
        {
            var isDescending = sortOrder.Equals("desc", StringComparison.OrdinalIgnoreCase);

            Expression<Func<UserLoginLog, object>> keySelector = sortBy.ToLower() switch
            {
                "logintimestamp" => x => x.LoginTimestamp,
                "ipaddress" => x => x.IpAddress!,
                "issuccessful" => x => x.IsSuccessful,
                "createdat" => x => x.CreatedAt,
                _ => x => x.LoginTimestamp
            };

            return isDescending
                ? query.OrderByDescending(keySelector)
                : query.OrderBy(keySelector);
        }
    }
}
