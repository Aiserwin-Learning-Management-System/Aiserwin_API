namespace Winfocus.LMS.Infrastructure.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using Winfocus.LMS.Application.Interfaces;
    using Winfocus.LMS.Domain.Entities;
    using Winfocus.LMS.Infrastructure.Data;

    /// <summary>
    /// EF Core implementation of <see cref="IRoleRepository"/>.
    /// </summary>
    public sealed class RoleRepository : IRoleRepository
    {
        private readonly AppDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public RoleRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Gets the by name asynchronous.
        /// </summary>
        /// <param name="roleName">Name of the role.</param>
        /// <returns>
        /// Role.
        /// </returns>
        public Task<Role?> GetByNameAsync(string roleName)
        {
            return _dbContext.Roles
                .FirstOrDefaultAsync(r => r.Name == roleName && r.IsActive);
        }
    }
}
