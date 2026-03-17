using System;
using System.Collections.Generic;
using System.Text;

namespace Winfocus.LMS.Application.DTOs
{
    /// <summary>
    /// Request for creating or updating a ExamSyllabus.
    /// </summary>

    public sealed record ExamSyllabusRequestDto(string name, string description, Guid accademicyearid, Guid userid);
}
