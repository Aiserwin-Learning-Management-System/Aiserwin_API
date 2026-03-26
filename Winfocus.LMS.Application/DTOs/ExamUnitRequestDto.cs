using System;
using System.Collections.Generic;
using System.Text;

namespace Winfocus.LMS.Application.DTOs
{
    /// <summary>
    /// ExamUnitRequestDto.
    /// </summary>
    public sealed record class ExamUnitRequestDto(string name, string description, int unitNumber, Guid subjectId, Guid userid);
}
