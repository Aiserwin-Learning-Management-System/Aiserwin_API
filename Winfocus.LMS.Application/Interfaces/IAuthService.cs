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
    }
}
