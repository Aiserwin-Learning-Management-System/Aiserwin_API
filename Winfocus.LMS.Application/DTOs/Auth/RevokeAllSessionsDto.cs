namespace Winfocus.LMS.Application.DTOs.Auth
{
    /// <summary>
    /// DTO for revoking all active sessions using credentials.
    /// Allows locked-out users to clear their sessions without a valid JWT.
    /// </summary>
    /// <param name="username">The username or email.</param>
    /// <param name="password">The account password.</param>
    public sealed record RevokeAllSessionsDto(
        string username,
        string password);
}
