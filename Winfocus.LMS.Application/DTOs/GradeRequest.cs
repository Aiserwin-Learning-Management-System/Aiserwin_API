namespace Winfocus.LMS.Application.DTOs
{
    /// <summary>
    /// Request for creating or updating a grade.
    /// </summary>
    public sealed record GradeRequest(
        string name,
        string code, Guid syllabusid);
}
