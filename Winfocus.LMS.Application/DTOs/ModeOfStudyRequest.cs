using System.ComponentModel.DataAnnotations;

namespace Winfocus.LMS.Application.DTOs
{
    /// <summary>
    /// Request for creating or updating a country.
    /// </summary>
    public sealed record ModeOfStudyRequest(
        [Required] string name, [Required] Guid countryid, Guid userId);
}
