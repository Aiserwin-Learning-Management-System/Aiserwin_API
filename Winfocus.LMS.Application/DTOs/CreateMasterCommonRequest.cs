namespace Winfocus.LMS.Application.DTOs
{
    /// <summary>
    /// Request for creating or updating a country.
    /// </summary>
    public sealed record class CreateMasterStateRequest
    (string name, Guid countryid, Guid userId, Guid modeofstudyid);
}
