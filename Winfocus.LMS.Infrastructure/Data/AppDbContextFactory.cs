namespace Winfocus.LMS.Infrastructure.Data
{
    using System.IO;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Design;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// AppDbContextFactory.
    /// </summary>
    /// <seealso cref="Microsoft.EntityFrameworkCore.Design.IDesignTimeDbContextFactory&lt;Winfocus.LMS.Infrastructure.Data.AppDbContext&gt;" />
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        /// <summary>
        /// Creates a new instance of a derived context.
        /// </summary>
        /// <param name="args">Arguments provided by the design-time service.</param>
        /// <returns>
        /// An instance of <typeparamref name="TContext" />.
        /// </returns>
        public AppDbContext CreateDbContext(string[] args)
        {
            // Walk up from the assembly location to find the project root where
            // appsettings.json lives. Falls back to current directory if not found.
            var assemblyDir = Path.GetDirectoryName(typeof(AppDbContextFactory).Assembly.Location)
                              ?? Directory.GetCurrentDirectory();
            var basePath = assemblyDir;
            while (!string.IsNullOrEmpty(basePath) && !File.Exists(Path.Combine(basePath, "appsettings.json")))
            {
                basePath = Directory.GetParent(basePath)?.FullName;
            }
            if (string.IsNullOrEmpty(basePath))
                basePath = Directory.GetCurrentDirectory();

            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: true)
                .AddJsonFile("appsettings.Development.json", optional: true)
                .AddJsonFile("appsettings.Production.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            // Priority: command-line arg > env var > appsettings JSON
            var argConnection = args
                .Where(a => !string.IsNullOrWhiteSpace(a) && !a.StartsWith("--"))
                .FirstOrDefault();
            var connectionString = argConnection
                ?? configuration["DB_CONNECTION_STRING"]
                ?? configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException(
                    "Connection string not found. Provide it via --connection argument, "
                    + "DB_CONNECTION_STRING env var, or appsettings.json.");
            }

            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
