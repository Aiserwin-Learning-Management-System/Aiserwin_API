using System;
using System.Collections.Generic;
using System.Text;

namespace Winfocus.LMS.Application.DTOs.Exam
{
    /// <summary>
    /// Request to create or update ExamQuestion mapping.
    /// </summary>
    public sealed record ExamQuestionRequest(Guid ExamId, Guid QuestionId, Guid UserId);
}
