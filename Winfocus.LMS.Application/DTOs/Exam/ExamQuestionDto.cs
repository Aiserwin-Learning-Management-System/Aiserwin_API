using System;
using System.Collections.Generic;
using System.Text;
using Winfocus.LMS.Application.DTOs.Masters;
using Winfocus.LMS.Application.DTOs.Question;

namespace Winfocus.LMS.Application.DTOs.Exam
{
    /// <summary>
    /// DTO representing an exam-question mapping with question details.
    /// </summary>
    public class ExamQuestionDto : BaseClassDTO
    {
        /// <summary>
        /// Gets or sets the exam identifier.
        /// </summary>
        public Guid ExamId { get; set; }

        /// <summary>
        /// Gets or sets the question identifier.
        /// </summary>
        public Guid QuestionId { get; set; }

        /// <summary>
        /// Gets or sets the question details if included.
        /// </summary>
        public QuestionResponseDto? Question { get; set; }
    }
}
