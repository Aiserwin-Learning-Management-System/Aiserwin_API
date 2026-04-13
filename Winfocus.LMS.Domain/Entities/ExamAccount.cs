using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Winfocus.LMS.Domain.Common;

namespace Winfocus.LMS.Domain.Entities
{
    /// <summary>
    /// Represents an exam account configuration for scheduling and filtering exams.
    /// </summary>
    public class ExamAccount : BaseEntity
    {
        /// <summary>
        /// Gets or sets the activation start date from which this exam account becomes valid.
        /// </summary>
        public DateTime ActivationStartDate { get; set; }

        /// <summary>
        /// Gets or sets the activation end date until which this exam account remains valid.
        /// </summary>
        public DateTime ActivationEndDate { get; set; }

        /// <summary>
        /// Gets or sets optional batch identifier associated with the exam.
        /// </summary>
        public Guid? BatchId { get; set; }

        /// <summary>
        /// Gets or sets optional student identifier associated with the exam.
        /// </summary>
        public Guid? StudentId { get; set; }

        /// <summary>
        /// Gets or sets the scheduled date of the exam.
        /// </summary>
        public DateTime ExamDate { get; set; }

        /// <summary>
        /// Gets or sets subject identifier for the exam.
        /// </summary>
        public Guid SubjectId { get; set; }

        /// <summary>
        /// Gets or sets resource identifier (e.g., study material or content reference).
        /// </summary>
        public Guid ResourceId { get; set; }

        /// <summary>
        /// Gets or sets unit identifier within the subject.
        /// </summary>
        public Guid UnitId { get; set; }

        /// <summary>
        /// Gets or sets chapter identifier within the unit.
        /// </summary>
        public Guid ChapterId { get; set; }

        /// <summary>
        /// Gets or sets type of questions included in the exam (e.g., MCQ, descriptive).
        /// </summary>
        public Guid QuestionTypeId { get; set; }

        /// <summary>
        /// Gets or sets identifier of the exam associated with this account.
        /// </summary>
        public Guid ExamId { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the exam.
        /// </summary>
        public Exam Exam { get; set; } = default!;

        /// <summary>
        /// Gets or sets the navigation property to the QuestionTypeConfig.
        /// </summary>
        public QuestionTypeConfig QuestionTypeConfig { get; set; } = default!;

        /// <summary>
        /// Gets or sets the navigation property to the exam chapter.
        /// </summary>
        public ExamChapter Chapter { get; set; } = default!;

        /// <summary>
        /// Gets or sets the navigation property to the exam unit.
        /// </summary>
        public ExamUnit Unit { get; set; } = default!;

        /// <summary>
        /// Gets or sets the navigation property to the content resource type.
        /// </summary>
        public ContentResourceType ResourceType { get; set; } = default!;

        /// <summary>
        /// Gets or sets the navigation property to the subject.
        /// </summary>
        public Subject Subject { get; set; } = default!;

        /// <summary>
        /// Gets or sets the navigation property to the student.
        /// </summary>
        public Student Student { get; set; } = default!;

        /// <summary>
        /// Gets or sets the navigation property to the batch.
        /// </summary>
        public Batch Batch { get; set; } = default!;

        /// <summary>
        /// Gets or sets the foreign key to QuestionTypeConfig.
        /// </summary>
        public Guid QuestionTypeConfigId { get; set; }

        /// <summary>
        /// Gets or sets the foreign key to ContentResourceType.
        /// </summary>
        public Guid ResourceTypeId { get; set; }
    }
}
