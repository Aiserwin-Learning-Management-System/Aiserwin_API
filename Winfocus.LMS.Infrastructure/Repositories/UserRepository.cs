namespace Winfocus.LMS.Infrastructure.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Winfocus.LMS.Application.Interfaces;
    using Winfocus.LMS.Application.Services;
    using Winfocus.LMS.Domain.Entities;
    using Winfocus.LMS.Infrastructure.Data;

    /// <summary>
    /// EF Core implementation of <see cref="IUserRepository"/>.
    /// </summary>
    public sealed class UserRepository : IUserRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger<UserRepository> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="logger">The logger.</param>
        public UserRepository(AppDbContext dbContext, ILogger<UserRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        /// <summary>
        /// Usernames the exists asynchronous.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns>
        /// bool.
        /// </returns>
        public Task<bool> UsernameExistsAsync(string username)
        {
            return _dbContext.Users.AnyAsync(u => u.Username == username);
        }

        /// <summary>
        /// Gets the by username asynchronous.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns>
        /// user.
        /// </returns>
        public Task<User?> GetByUsernameAsync(string username)
        {
            return _dbContext.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => (u.Username == username || u.Email == username) && u.IsActive);
        }

        /// <summary>
        /// Adds the asynchronous.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>
        /// Task.
        /// </returns>
        public async Task AddAsync(User user)
        {
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// user.
        /// </returns>
        public Task<User?> GetByIdAsync(Guid id)
        {
            return _dbContext.Users
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>
        /// Task.
        /// </returns>
        public async Task UpdateAsync(User user)
        {
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Gets the by names asynchronous.
        /// </summary>
        /// <param name="roleNames">The role names.</param>
        /// <returns>
        /// List of roles.
        public async Task<IReadOnlyList<Role>> GetByNamesAsync(IReadOnlyList<string> roleNames)
        {
            return await _dbContext.Roles
                .Where(r => roleNames.Contains(r.Name))
                .ToListAsync();
        }

        /// <summary>
        /// Gets the by email asynchronous.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns>
        /// user.
        /// </returns>
        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _dbContext.Users
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        /// <summary>
        /// Counts existing users whose username starts with the given prefix
        /// and who were created in the specified year. This enables the sequential
        /// suffix generation (e.g., arjun_2601, arjun_2602).
        /// </summary>
        /// <param name="usernamePrefix">
        /// The lowercase first-name part of the username (e.g., "arjun").
        /// </param>
        /// <param name="year">
        /// The 4-digit registration year used in the username (e.g., 2026).
        /// The last 2 digits of this year are embedded in the username suffix (e.g., "26").
        /// </param>
        /// <returns>
        /// The total count of matching users, which determines the next available sequence number.
        /// </returns>
        public async Task<int> CountUsernamesByPrefixAndYearAsync(string usernamePrefix, int year)
        {
            try
            {
                var yearSuffix = (year % 100).ToString("D2");
                var pattern = $"{usernamePrefix}_{yearSuffix}";

                _logger.LogDebug("Counting usernames with pattern '{Pattern}' for year {Year}", pattern, year);

                var count = await _dbContext.Users
                    .CountAsync(u => u.Username.StartsWith(pattern));

                _logger.LogDebug("Found {Count} existing usernames matching pattern '{Pattern}'", count, pattern);

                return count;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error counting usernames for prefix '{Prefix}' and year {Year}", usernamePrefix, year);
                throw;
            }
        }

        /// <summary>
        /// Determines whether the specified email address is already registered.
        /// </summary>
        /// <param name="email">The email address to check (case-insensitive).</param>
        /// <returns><c>true</c> if the email is taken; otherwise, <c>false</c>.</returns>
        public async Task<bool> EmailExistsAsync(string email)
        {
            try
            {
                _logger.LogDebug("Checking email existence for {Email}", email);
                return await _dbContext.Users
                    .AnyAsync(u => u.Email.ToLower() == email.ToLower());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking email existence for {Email}", email);
                throw;
            }
        }
    }
}
