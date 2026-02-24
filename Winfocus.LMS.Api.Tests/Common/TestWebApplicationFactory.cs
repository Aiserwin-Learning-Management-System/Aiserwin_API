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
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactory&lt;Program&gt;" />
    public sealed class TestWebApplicationFactory : WebApplicationFactory<Program>
    {
        private const string _databaseName = "AuthControllerTestsDb";

        public string? CapturedActivationToken { get; private set; }

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

                // Replace IEmailService with mock
                var emailDescriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(IEmailService));
                if (emailDescriptor != null)
                {
                    services.Remove(emailDescriptor);
                }

                var emailServiceMock = new Mock<IEmailService>();
                emailServiceMock
                    .Setup(e => e.SendActivationEmailAsync(
                        It.IsAny<string>(),
                        It.IsAny<string>(),
                        It.IsAny<string>()))
                    .Callback<string, string, string>((email, username, token) =>
                    {
                        CapturedActivationToken = token;
                    })
                    .Returns(Task.CompletedTask);

                services.AddSingleton(emailServiceMock.Object);

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
