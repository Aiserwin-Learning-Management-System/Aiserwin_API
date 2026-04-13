using System;

namespace Winfocus.LMS.Application.DTOs
{
    /// <summary>
    /// Request DTO used to create or update an <see cref="Winfocus.LMS.Domain.Entities.ExamAccount"/>.
    /// </summary>
    /// <param name="activationStartDate">Activation start date.</param>
    /// <param name="activationEndDate">Activation end date.</param>
    /// <param name="batchId">Optional batch identifier.</param>
    /// <param name="studentId">Optional student identifier.</param>
    /// <param name="examDate">Scheduled exam date.</param>
    /// <param name="subjectId">Subject identifier.</param>
    /// <param name="resourceId">Resource identifier.</param>
    /// <param name="unitId">Exam unit identifier.</param>
    /// <param name="chapterId">Exam chapter identifier.</param>
    /// <param name="questionTypeId">Question type identifier.</param>
    /// <param name="examId">Associated exam identifier.</param>
    /// <param name="userId">Identifier of the user performing the action.</param>
    public sealed record ExamAccountRequest(
        DateTime activationStartDate,
        DateTime activationEndDate,
        Guid? batchId,
        Guid? studentId,
        DateTime examDate,
        Guid subjectId,
        Guid resourceId,
        Guid unitId,
        Guid chapterId,
        Guid questionTypeId,
        Guid examId,
        Guid userId);
}
