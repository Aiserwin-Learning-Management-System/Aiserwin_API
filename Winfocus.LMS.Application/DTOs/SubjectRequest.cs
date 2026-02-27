namespace Winfocus.LMS.Application.DTOs
{
    /// <summary>
    /// Request model for creating or updating a subject.
    /// </summary>
    public sealed record SubjectRequest(
        string name, Guid courseid, Guid userid);
}
