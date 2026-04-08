namespace Winfocus.LMS.Application.DTOs
{
    /// <summary>
    /// Request for creating or updating a country.
    /// </summary>
    public sealed record BatchTimingRequest(DateTimeOffset batchTime,
        Guid subjectId, Guid userId);
}
