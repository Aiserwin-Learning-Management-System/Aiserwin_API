namespace Winfocus.LMS.Application.DTOs
{
    /// <summary>
    /// Request model for creating or updating a course.
    /// </summary>
    public sealed record CourseRequest(
    string coursename,
    Guid subjectid, Guid gradeid, string cousedescription, string courseurl, int maxstudent, Guid academicyear, string status);
}
