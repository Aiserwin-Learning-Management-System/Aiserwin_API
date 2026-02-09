namespace Winfocus.LMS.Api.Tests.Common
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Winfocus.LMS.Domain.Entities;
    using Winfocus.LMS.Infrastructure.Data;

    /// <summary>
    /// Custom web application factory for integration testing.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactory&lt;Program&gt;" />
    public sealed class TestWebApplicationFactory
    : WebApplicationFactory<Program>
    {
        private const string _databaseName = "AuthControllerTestsDb";

        /// <summary>
        /// Gives a fixture an opportunity to configure the application before it gets built.
        /// </summary>
        /// <param name="builder">The <see cref="T:Microsoft.AspNetCore.Hosting.IWebHostBuilder" /> for the application.</param>
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");

            builder.ConfigureServices(services =>
            {
                // Remove existing DbContext registrations
                var descriptors = services
                    .Where(d =>
                        d.ServiceType == typeof(AppDbContext) ||
                        d.ServiceType == typeof(DbContextOptions<AppDbContext>))
                    .ToList();

                foreach (var d in descriptors)
                {
                    services.Remove(d);
                }

                // SINGLE SHARED DATABASE
                services.AddDbContext<AppDbContext>(options =>
                    options.UseInMemoryDatabase(_databaseName));

                // Build provider ONCE
                var sp = services.BuildServiceProvider();

                using var scope = sp.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                db.Database.EnsureCreated();

                // Seed roles ONCE
                if (!db.Roles.Any())
                {
                    db.Roles.AddRange(
                        new Role { Name = "Student", IsActive = true },
                        new Role { Name = "Admin", IsActive = true });
                    db.SaveChanges();
                }
            });
        }
    }
}
