using System;
using System.Collections.Generic;
using System.Text;

namespace Winfocus.LMS.Application.DTOs
{
    /// <summary>
    /// Request for creating or updating a ExamGradeRequestDto.
    /// </summary>
    public sealed record class ExamGradeRequestDto(string name, string description, Guid syllabusId, Guid userid);
}
