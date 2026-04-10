namespace Winfocus.LMS.API.Hubs;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;

/// <summary>
/// SignalR <see cref="IUserIdProvider"/> that extracts the user identifier
/// from the JWT <c>sub</c> claim on the incoming connection's
/// <see cref="HttpContext.User"/>.
/// </summary>
public sealed class JwtSubUserIdProvider : IUserIdProvider
{
    /// <inheritdoc/>
    public string? GetUserId(HubConnectionContext connection)
    {
        var subClaim = connection.User.FindFirst(JwtRegisteredClaimNames.Sub)
                       ?? connection.User.FindFirst(ClaimTypes.NameIdentifier);

        return subClaim?.Value;
    }
}
