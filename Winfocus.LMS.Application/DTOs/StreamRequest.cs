namespace Winfocus.LMS.Application.DTOs
{
    /// <summary>
    /// Request for creating or updating a stream.
    /// </summary>
    public sealed record StreamRequest(
        string name,
        string code, Guid gradeid);
}
