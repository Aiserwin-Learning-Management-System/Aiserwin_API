namespace Winfocus.LMS.Infrastructure.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using Winfocus.LMS.Application.Interfaces;
    using Winfocus.LMS.Domain.Entities;
    using Winfocus.LMS.Infrastructure.Data;

    /// <summary>
    /// UserActivationTokenRepository.
    /// </summary>
    /// <seealso cref="Winfocus.LMS.Application.Interfaces.IUserActivationTokenRepository" />
    public sealed class UserActivationTokenRepository : IUserActivationTokenRepository
    {
        private readonly AppDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserActivationTokenRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public UserActivationTokenRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Adds the asynchronous.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns>Task.</returns>
        public async Task AddAsync(UserActivationToken token)
        {
            _dbContext.UserActivationTokens.Add(token);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Gets the by token asynchronous.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns>
        /// UserActivationToken.
        /// </returns>
        public Task<UserActivationToken?> GetByTokenAsync(string token)
        {
            return _dbContext.UserActivationTokens
                .FirstOrDefaultAsync(x => x.Token == token);
        }

        /// <summary>
        /// Invalidates the user tokens asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>Task.</returns>
        public async Task InvalidateUserTokensAsync(Guid userId)
        {
            var tokens = await _dbContext.UserActivationTokens
                .Where(x => x.UserId == userId && !x.IsUsed)
                .ToListAsync();

            foreach (var token in tokens)
            {
                token.IsUsed = true;
            }

            await _dbContext.SaveChangesAsync();
        }
    }
}
