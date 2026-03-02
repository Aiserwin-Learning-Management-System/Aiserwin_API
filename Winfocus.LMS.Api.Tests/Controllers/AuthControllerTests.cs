namespace Winfocus.LMS.Api.Tests.Controllers
{
    using FluentAssertions;
    using System.Net;
    using System.Net.Http.Json;
    using Winfocus.LMS.Api.Tests.Common;
    using Winfocus.LMS.Application.DTOs;
    using Winfocus.LMS.Application.DTOs.Auth;

    /// <summary>
    /// Integration tests for AuthController.
    /// </summary>
    public sealed class AuthControllerTests
        : IClassFixture<TestWebApplicationFactory>
    {
        private readonly HttpClient _client;
        private readonly TestWebApplicationFactory _factory;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthControllerTests"/> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        public AuthControllerTests(TestWebApplicationFactory factory)
        {
            _factory = factory;
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
                "/api/v1/Auth/register",
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
            // Register user
            var registerResponse = await _client.PostAsJsonAsync(
                "/api/v1/Auth/register",
                new RegisterRequestDto("loginuser", "login@winfocus.com", null));

            var registeredUser = await registerResponse.Content.ReadFromJsonAsync<AuthResponseDto>();

            // Use captured token from factory
            string activationToken = _factory.CapturedActivationToken!;

            // Set password with token
            var setPasswordResponse = await _client.PostAsJsonAsync(
                "/api/v1/Auth/set-password",
                new SetPasswordDto(activationToken, "Password@123"));

            setPasswordResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // Login
            var loginResponse = await _client.PostAsJsonAsync(
                "/api/v1/Auth/login",
                new LoginRequestDto(registeredUser.username, "Password@123", ipAddress: "192.168.1.100", userAgent: "Mozilla/5.0 (Windows NT 10.0; Win64; x64) Chrome/120.0.0.0"));

            loginResponse.StatusCode.Should().Be(HttpStatusCode.OK);
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
                "/api/v1/Auth/register",
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
                string.Empty,
                ipAddress: "192.168.1.100",
                userAgent: "Mozilla/5.0 (Windows NT 10.0; Win64; x64) Chrome/120.0.0.0");

            var response = await _client.PostAsJsonAsync(
                "/api/v1/Auth/login",
                request);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
