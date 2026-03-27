using System;
using System.Collections.Generic;
using System.Text;

namespace Winfocus.LMS.Application.DTOs.Exam
{
    /// <summary>
    /// Request for creating or updating a exam.
    /// </summary>
    public sealed record ExamRequest(
         Guid countryId, Guid centerId, Guid syllabusId, int mode, Guid gradeId, Guid streamId, Guid courseId, Guid unitId, Guid chapterId, Guid questionTypeId, string? examTitle, string? examQuestionNumber, DateTime examDate, string? examDuration, double totalMark, double passingMark, int status, Guid userid);
}
