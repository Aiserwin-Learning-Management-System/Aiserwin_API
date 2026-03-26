namespace Winfocus.LMS.Api.Tests.Common
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Moq;
    using Winfocus.LMS.Application.Interfaces;
    using Winfocus.LMS.Domain.Entities;
    using Winfocus.LMS.Infrastructure.Data;

    /// <summary>
    /// Custom web application factory for integration testing.
    /// Shared across ALL controller test classes.
    /// Each service mock is exposed as a property so individual
    /// test classes can configure the expectations they need.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactory&lt;Program&gt;" />
    public sealed class TestWebApplicationFactory : WebApplicationFactory<Program>
    {
        private const string _databaseName = "SharedIntegrationTestsDb";

        /// <summary>
        /// Gets the captured activation token.
        /// </summary>
        /// <value>The captured activation token.</value>
        public string? CapturedActivationToken { get; private set; }

        /// <summary>
        /// Gets the mocked fee service.
        /// Test classes configure expectations on this before making HTTP requests.
        /// </summary>
        /// <value>The fee service mock.</value>
        public Mock<IFeeService> FeeServiceMock { get; } = new ();

        /// <summary>
        /// Gets the mocked email service.
        /// </summary>
        /// <value>The email service mock.</value>
        public Mock<IEmailService> EmailServiceMock { get; private set; } = null!;

        /// <summary>
        /// Gives a fixture an opportunity to configure the application before it gets built.
        /// </summary>
        /// <param name="builder">The <see cref="T:Microsoft.AspNetCore.Hosting.IWebHostBuilder" /> for the application.</param>
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");

            builder.ConfigureServices(services =>
            {
                // ── Remove existing DbContext registrations ──
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

                // ── Replace IEmailService with mock ──
                ReplaceService<IEmailService>(services, () =>
                {
                    EmailServiceMock = new Mock<IEmailService>();
                    EmailServiceMock
                        .Setup(e => e.SendActivationEmailAsync(
                            It.IsAny<string>(),
                            It.IsAny<string>(),
                            It.IsAny<string>()))
                        .Callback<string, string, string>((email, username, token) =>
                        {
                            CapturedActivationToken = token;
                        })
                        .Returns(Task.CompletedTask);

                    return EmailServiceMock.Object;
                });

                // ── Replace IFeeService with mock ──
                ReplaceService<IFeeService>(services, () => FeeServiceMock.Object);

                // ── Build provider and seed ──
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

        /// <summary>
        /// Removes an existing service registration and replaces it with a new one.
        /// </summary>
        /// <typeparam name="TService">The service interface type.</typeparam>
        /// <param name="services">The service collection.</param>
        /// <param name="factory">The factory that creates the replacement instance.</param>
        private static void ReplaceService<TService>(
            IServiceCollection services,
            Func<TService> factory)
            where TService : class
        {
            var descriptor = services
                .SingleOrDefault(d => d.ServiceType == typeof(TService));

            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            services.AddSingleton(factory());
        }
    }
}
