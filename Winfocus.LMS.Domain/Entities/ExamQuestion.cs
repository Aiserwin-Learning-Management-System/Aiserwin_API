using System;
using System.Collections.Generic;
using System.Text;
using Winfocus.LMS.Domain.Common;

namespace Winfocus.LMS.Domain.Entities
{
    /// <summary>
    /// ExamQuestion.
    /// </summary>
    public class ExamQuestion : BaseEntity
    {
        /// <summary>
        /// Gets or sets the question identifier.
        /// </summary>
        /// <value>
        /// The question identifier.
        /// </value>
        public Guid QuestionId { get; set; }

        /// <summary>
        /// Gets or sets the question.
        /// </summary>
        /// <value>
        /// The question.
        /// </value>
        public Question Question { get; set; } = null!;

        /// <summary>
        /// Gets or sets the exam identifier.
        /// </summary>
        /// <value>
        /// The question identifier.
        /// </value>
        public Guid ExamId { get; set; }

        /// <summary>
        /// Gets or sets the exam.
        /// </summary>
        /// <value>
        /// The exam.
        /// </value>
        public Exam Exam { get; set; } = null!;
    }
}
