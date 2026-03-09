using System;
using System.Collections.Generic;
using System.Text;

namespace Winfocus.LMS.Application.DTOs
{
    public sealed record DoubtClearingRequest(
     DateTime startTime, DateTime endTime, Guid subjectId, Guid userId);
}
