namespace Winfocus.LMS.Application.DTOs
{
    /// <summary>
    /// Authentication response containing JWT access token.
    /// </summary>
    public sealed record AuthResponseDto(
        string accessToken,
        Guid userId,
        string username,
        string email,
        IReadOnlyList<string> roles,
        string profilepicture);
}
