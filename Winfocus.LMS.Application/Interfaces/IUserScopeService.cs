using System;
using System.Collections.Generic;
using System.Text;

namespace Winfocus.LMS.Application.Interfaces
{
    /// <summary>
    /// Provides information about the currently authenticated user's scope.
    /// Scope includes role, country, and center identifiers extracted from JWT claims.
    /// This service is used to enforce data access restrictions across the application.
    /// </summary>
    public interface IUserScopeService
    {
        /// <summary>
        /// Gets the role of the current user.
        /// Example: SuperAdmin, CountryAdmin, CenterAdmin, Staff.
        /// </summary>
        string Role { get; }

        /// <summary>
        /// Gets the CountryId associated with the user.
        /// Null means the user is not restricted to a specific country.
        /// </summary>
        Guid? CountryId { get; }

        /// <summary>
        /// Gets the CenterId associated with the user.
        /// Null means the user is not restricted to a specific center.
        /// </summary>
        Guid? CenterId { get; }
    }
}
