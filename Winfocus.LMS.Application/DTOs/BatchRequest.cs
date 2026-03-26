using System;
using System.Collections.Generic;
using System.Text;

namespace Winfocus.LMS.Application.DTOs
{
    /// <summary>
    /// Request for creating or updating a batch.
    /// </summary>
    public sealed record BatchRequest(string name,
        Guid subjectId, Guid userId);
}
