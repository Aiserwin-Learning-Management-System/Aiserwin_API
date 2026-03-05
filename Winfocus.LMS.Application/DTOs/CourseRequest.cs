namespace Winfocus.LMS.Application.DTOs
{
    /// <summary>
    /// Request model for creating or updating a course.
    /// </summary>
    public sealed record CourseRequest(
        string coursename,
        Guid streamid,
        Guid userId, string courseCode);
}
