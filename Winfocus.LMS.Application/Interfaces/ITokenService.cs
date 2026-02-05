namespace Winfocus.LMS.Application.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Winfocus.LMS.Domain.Entities;

    /// <summary>
    /// ITokenService Interface.
    /// </summary>
    public interface ITokenService
    {
        /// <summary>
        /// Generates the token.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="roles">The roles.</param>
        /// <returns>string.</returns>
        string GenerateToken(User user, IReadOnlyList<string> roles);
    }
}
