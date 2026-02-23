namespace Winfocus.LMS.Application.DTOs
{
    /// <summary>
    /// Request for creating or updating a syllabus.
    /// </summary>
    public sealed record SyllabusRequest(
        string name, Guid centerid, Guid userId);
}
