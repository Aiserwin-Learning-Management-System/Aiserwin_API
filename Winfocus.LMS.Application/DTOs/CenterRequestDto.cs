namespace Winfocus.LMS.Application.DTOs
{
    /// <summary>
    /// Request for creating or updating a country.
    /// </summary>
    public sealed record CenterRequestDto(
        string name,
        string code, Guid modeofstudy, Guid userId);
}
