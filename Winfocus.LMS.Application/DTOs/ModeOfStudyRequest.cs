namespace Winfocus.LMS.Application.DTOs
{
    /// <summary>
    /// Request for creating or updating a country.
    /// </summary>
    public sealed record ModeOfStudyRequest(
        string name,
        string code, Guid stateid);
}
