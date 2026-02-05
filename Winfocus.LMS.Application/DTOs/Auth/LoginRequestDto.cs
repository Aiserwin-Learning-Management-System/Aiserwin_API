namespace Winfocus.LMS.Application.DTOs.Auth
{
    using System.Text.Json.Serialization;

    /// <summary>
    /// Request model for user login.
    /// </summary>
    public class LoginRequestDto
    {
        /// <summary>
        /// Gets or sets the username or email used to authenticate.
        /// </summary>
        [JsonPropertyName("username")]
        public string Username { get; set; } = null!;

        /// <summary>
        /// Gets or sets the password used to authenticate.
        /// </summary>
        [JsonPropertyName("password")]
        public string Password { get; set; } = null!;
    }
}
