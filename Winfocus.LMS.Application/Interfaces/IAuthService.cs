namespace Winfocus.LMS.Application.Interfaces
{
    using Winfocus.LMS.Application.DTOs;
    using Winfocus.LMS.Application.DTOs.Auth;

    /// <summary>
    /// Defines authentication operations.
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Registers the asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>AuthResponseDto.</returns>
        Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request);

        /// <summary>
        /// Logins the asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>AuthResponseDto.</returns>
        Task<AuthResponseDto> LoginAsync(LoginRequestDto request);

        /// <summary>
        /// Sets the password asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Task.</returns>
        Task SetPasswordAsync(SetPasswordDto request);

        /// <summary>
        /// Forgots the password asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <exception cref="CustomException">Invalid email.</exception>
        /// <returns>Task.</returns>
        Task ForgotPasswordAsync(ForgotPasswordDto request);

        /// <summary>
        /// Resets the password asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <exception cref="CustomException">
        /// Invalid token. - INVALID_TOKEN
        /// or
        /// Invalid token purpose. - INVALID_TOKEN
        /// or
        /// Token already used. - TOKEN_ALREADY_USED
        /// or
        /// Token expired. - TOKEN_EXPIRED
        /// or
        /// User not found. - USER_NOT_FOUND.
        /// </exception>
        /// <returns>Task.</returns>
        Task ResetPasswordAsync(ResetPasswordDto request);
    }
}
