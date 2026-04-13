using System;
using Winfocus.LMS.Application.DTOs.Masters;
using Winfocus.LMS.Application.DTOs.QuestionTypeConfig;
using Winfocus.LMS.Application.DTOs.Exam;
using Winfocus.LMS.Application.DTOs.Students;

namespace Winfocus.LMS.Application.DTOs
{
    /// <summary>
    /// Data transfer object representing an exam account.
    /// </summary>
    public class ExamAccountDto : Masters.BaseClassDTO
    {
        /// <summary>
        /// Gets or sets activation start date for the account.
        /// </summary>
        public DateTime ActivationStartDate { get; set; }

        /// <summary>
        /// Gets or sets activation end date for the account.
        /// </summary>
        public DateTime ActivationEndDate { get; set; }

        /// <summary>
        /// Gets or sets optional associated batch id.
        /// </summary>
        public Guid? BatchId { get; set; }

        /// <summary>
        /// Gets or sets associated batch data.
        /// </summary>
        public BatchDto? Batch { get; set; }

        /// <summary>
        /// Gets or sets optional student id associated with this account.
        /// </summary>
        public Guid? StudentId { get; set; }

        /// <summary>
        /// Gets or sets associated student data.
        /// </summary>
        public StudentDto? Student { get; set; }

        /// <summary>
        /// Gets or sets scheduled exam date for this account.
        /// </summary>
        public DateTime ExamDate { get; set; }

        /// <summary>
        /// Gets or sets subject identifier.
        /// </summary>
        public Guid SubjectId { get; set; }

        /// <summary>
        /// Gets or sets associated subject details.
        /// </summary>
        public SubjectDto? Subject { get; set; }

        /// <summary>
        /// Gets or sets resource identifier used by the exam.
        /// </summary>
        public Guid ResourceId { get; set; }

        /// <summary>
        /// Gets or sets content resource type details.
        /// </summary>
        public ContentResourceTypeDto? ResourceType { get; set; }

        /// <summary>
        /// Gets or sets unit identifier within the subject.
        /// </summary>
        public Guid UnitId { get; set; }

        /// <summary>
        /// Gets or sets exam unit details.
        /// </summary>
        public Masters.ExamUnitDto? Unit { get; set; }

        /// <summary>
        /// Gets or sets chapter identifier within the unit.
        /// </summary>
        public Guid ChapterId { get; set; }

        /// <summary>
        /// Gets or sets exam chapter details.
        /// </summary>
        public Masters.ExamChapterDto? Chapter { get; set; }

        /// <summary>
        /// Gets or sets question type identifier used for the exam.
        /// </summary>
        public Guid QuestionTypeId { get; set; }

        /// <summary>
        /// Gets or sets question type configuration details.
        /// </summary>
        public QuestionTypeConfigDto? QuestionTypeConfig { get; set; }

        /// <summary>
        /// Gets or sets associated exam identifier.
        /// </summary>
        public Guid ExamId { get; set; }

        /// <summary>
        /// Gets or sets associated exam data.
        /// </summary>
        public ExamDto? Exam { get; set; }
    }
}
