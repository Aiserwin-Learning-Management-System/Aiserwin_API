namespace Winfocus.LMS.Application.DTOs.Auth
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Request data for user registration.
    /// </summary>
    public sealed record RegisterRequestDto(
        [Required] string username,
        [Required][EmailAddress] string email,
        IReadOnlyList<string>? roleNames);
}
