namespace Winfocus.LMS.Application.DTOs.Auth
{
    public sealed record SetPasswordDto(
    string token,
    string password);
}
