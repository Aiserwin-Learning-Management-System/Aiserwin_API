namespace Winfocus.LMS.Application.DTOs
{
    /// <summary>
    /// Request data for user login.
    /// </summary>
    public sealed record LoginRequestDto(
        string username,
        string password
    );
}
