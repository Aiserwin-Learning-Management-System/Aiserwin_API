using System;
using System.Collections.Generic;
using System.Text;

namespace Winfocus.LMS.Application.DTOs
{
    /// <summary>
    /// Request for creating or updating a subject to batchtiming.
    /// </summary>
    public sealed record SubjectBatchTimingRequest(
        Guid subjectId, List<Guid> Batchtimingids, Guid userid);
}
