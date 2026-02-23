namespace Winfocus.LMS.Application.DTOs.Auth
{
    public sealed record ResetPasswordDto(
    string token,
    string newPassword);
}
