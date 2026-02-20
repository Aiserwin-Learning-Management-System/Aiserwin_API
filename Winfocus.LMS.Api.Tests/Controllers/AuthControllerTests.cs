namespace Winfocus.LMS.Api.Tests.Controllers
{
    using System.Net;
    using System.Net.Http.Json;
    using FluentAssertions;
    using Winfocus.LMS.Api.Tests.Common;
    using Winfocus.LMS.Application.DTOs.Auth;

    /// <summary>
    /// Integration tests for AuthController.
    /// </summary>
    public sealed class AuthControllerTests
        : IClassFixture<TestWebApplicationFactory>
    {
        private readonly HttpClient _client;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthControllerTests"/> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        public AuthControllerTests(TestWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        /// <summary>
        /// Registers the returns ok for valid request.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Fact]
        public async Task Register_ReturnsOk_ForValidRequest()
        {
            var request = new RegisterRequestDto(
                "testuser",
                "test@winfocus.com",
                null
            );

            var response = await _client.PostAsJsonAsync(
                "/api/Auth/register",
                request);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        /// <summary>
        /// Logins the returns ok for valid credentials.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Fact]
        public async Task Login_ReturnsOk_ForValidCredentials()
        {
            // Register first
            await _client.PostAsJsonAsync(
                "/api/Auth/register",
                new RegisterRequestDto(
                    "loginuser",
                    "login@winfocus.com",
                    null));

            // Login
            var response = await _client.PostAsJsonAsync(
                "/api/Auth/login",
                new LoginRequestDto(
                    "loginuser",
                    "Password@123"));

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        /// <summary>
        /// Registers the returns bad request for invalid email.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Fact]
        public async Task Register_ReturnsBadRequest_ForInvalidEmail()
        {
            var request = new RegisterRequestDto(
                "user",
                "invalid-email",
                null);

            var response = await _client.PostAsJsonAsync(
                "/api/Auth/register",
                request);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        /// <summary>
        /// Logins the returns bad request for empty password.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Fact]
        public async Task Login_ReturnsBadRequest_ForEmptyPassword()
        {
            var request = new LoginRequestDto(
                "user",
                string.Empty);

            var response = await _client.PostAsJsonAsync(
                "/api/Auth/login",
                request);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
