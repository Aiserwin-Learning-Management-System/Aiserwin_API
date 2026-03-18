using System;
using System.Collections.Generic;
using System.Text;

namespace Winfocus.LMS.Application.DTOs
{
    public sealed record class ExamSubjectRequestDto(string name, string description, string code,Guid gradeId, Guid userid);
}
