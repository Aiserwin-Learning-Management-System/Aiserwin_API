namespace Winfocus.LMS.Application.DTOs.Auth
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Request data for user login.
    /// </summary>
    public sealed record LoginRequestDto(
        [Required] string username,
        [Required] string password,
        string? ipAddress,
        string? userAgent
    );
}
