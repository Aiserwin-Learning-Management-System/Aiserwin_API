namespace Winfocus.LMS.Application.Interfaces
{
    /// <summary>
    /// IEmailService.
    /// </summary>
    public interface IEmailService
    {
        /// <summary>
        /// Sends the activation email asynchronous.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="username">The username.</param>
        /// <param name="token">The token.</param>
        /// <returns>Task.</returns>
        Task SendActivationEmailAsync(string email, string username, string token);

        /// <summary>
        /// Sends the reset password email asynchronous.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="username">The username.</param>
        /// <param name="token">The token.</param>
        /// <returns>Task.</returns>
        Task SendResetPasswordEmailAsync(
    string email,
    string username,
    string token);
    }
}
