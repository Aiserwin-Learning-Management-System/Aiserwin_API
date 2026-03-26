using System;
using System.Collections.Generic;
using System.Text;

namespace Winfocus.LMS.Application.DTOs
{
    /// <summary>
    /// Request for filter center.
    /// </summary>
    public sealed record CenterGetRequest(Guid countryId,
    Guid modeOfStudyId,
    Guid? stateId);
}
