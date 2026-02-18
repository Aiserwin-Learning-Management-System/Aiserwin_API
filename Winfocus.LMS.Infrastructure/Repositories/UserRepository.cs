using Microsoft.EntityFrameworkCore;
using Winfocus.LMS.Application.Interfaces;
using Winfocus.LMS.Domain.Entities;
using Winfocus.LMS.Infrastructure.Data;

namespace Winfocus.LMS.Infrastructure.Repositories
{
    /// <summary>
    /// EF Core implementation of <see cref="IUserRepository"/>.
    /// </summary>
    public sealed class UserRepository : IUserRepository
    {
        private readonly AppDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public UserRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
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
    }
}
