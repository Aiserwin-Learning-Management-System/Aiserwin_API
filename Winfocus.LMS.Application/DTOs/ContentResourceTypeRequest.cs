using System;
using System.Collections.Generic;
using System.Text;

namespace Winfocus.LMS.Application.DTOs
{

    /// <summary>
    /// Request for creating or updating a country.
    /// </summary>
    public sealed record ContentResourceTypeRequest(string name, string description,
        Guid chapterId, Guid userId);
}
