namespace Winfocus.LMS.Application.DTOs
{
    /// <summary>
    /// Request for creating or updating a stream.
    /// </summary>
    public sealed record StreamRequest(
    /// <summary>
    /// Gets the name of the Stream.
    /// </summary>
    string name,

    /// <summary>
    /// Gets the identifier of the Grade associated with the Stream.
    /// </summary>
    Guid gradeid,

    /// <summary>
    /// Gets the list of Course identifiers to be mapped to the Stream.
    /// </summary>
    List<Guid> courseids,

        /// <summary>
        /// Gets the identifier of the Grade associated with the user.
        /// </summary>
    Guid userId,

    string streamcode);
}
