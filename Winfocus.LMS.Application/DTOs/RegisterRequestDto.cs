namespace Winfocus.LMS.Application.DTOs
{
    /// <summary>
    /// Request data for user registration.
    /// </summary>
    public sealed record RegisterRequestDto(
        string username,
        string email,
        string password,
        string roleName
    );
}
