namespace Winfocus.LMS.Infrastructure.Tests.Common
{
    using Microsoft.EntityFrameworkCore;
    using Winfocus.LMS.Infrastructure.Data;

    /// <summary>
    /// Base class for database context tests.
    /// </summary>
    public abstract class DbContextTestBase
    {
        /// <summary>
        /// Creates the database context.
        /// </summary>
        /// <returns>The database context.</returns>
        protected AppDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .EnableSensitiveDataLogging()
                .Options;

            return new AppDbContext(options);
        }
    }
}
