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
            // Priority: 1. --connection argument, 2. DB_CONNECTION_STRING env var, 3. appsettings.json
            var argConnection = args
                .Where(a => !string.IsNullOrWhiteSpace(a) && !a.StartsWith("--"))
                .FirstOrDefault();

            var connectionString = argConnection
                ?? Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");

            if (string.IsNullOrEmpty(connectionString))
            {
                // Fall back to appsettings.json (useful for local development).
                // Walk up from the assembly location to find the project root.
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
                    .Build();

                connectionString = configuration.GetConnectionString("DefaultConnection");
            }

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
