using System;
using System.Collections.Generic;
using System.Text;
using Winfocus.LMS.Domain.Entities;
using Winfocus.LMS.Domain.Enums;
using TaskStatus = Winfocus.LMS.Domain.Enums.TaskStatus;

namespace Winfocus.LMS.Application.DTOs
{
    /// <summary>
    /// DTO used by DTP Admin to create a new question typing task.
    /// Contains all academic context required for the operator to type questions.
    /// </summary>
    public class CreateTaskDto
    {
        /// <summary>
        /// Gets or sets operator ID to whom the task will be assigned.
        /// Must belong to an approved DTP operator.
        /// </summary>
        public Guid OperatorId { get; set; }

        /// <summary>
        /// Gets or sets the assigned by.
        /// </summary>
        /// <value>
        /// The assigned by.
        /// </value>
        public Guid AssignedBy { get; set; }

        /// <summary>
        /// Gets or sets the resource type identifier.
        /// </summary>
        /// <value>
        /// The resource type identifier.
        /// </value>
        public Guid ResourceTypeId { get; set; }

        /// <summary>
        /// Gets or sets type of questions (MCQ, Numerical, Theory etc.)
        /// </summary>
        public int QuestionType { get; set; }

        /// <summary>
        /// Gets or sets academic year for the questions.
        /// </summary>
        public int? Year { get; set; }

        /// <summary>
        /// Gets or sets syllabus id.
        /// </summary>
        public Guid SyllabusId { get; set; }

        /// <summary>
        /// Gets or sets grade or class level.
        /// </summary>
        public Guid GradeId { get; set; }

        /// <summary>
        /// Gets or sets the subject identifier.
        /// </summary>
        /// <value>
        /// The subject identifier.
        /// </value>
        public Guid SubjectId { get; set; }

        /// <summary>
        /// Gets or sets the unit identifier.
        /// </summary>
        /// <value>
        /// The unit identifier.
        /// </value>
        public Guid? UnitId { get; set; }

        /// <summary>
        /// Gets or sets chapter or topic reference.
        /// </summary>
        public Guid ChapterId { get; set; }

        /// <summary>
        /// Gets or sets total number of questions assigned in the task.
        /// </summary>
        public int TotalQuestions { get; set; }

        /// <summary>
        /// Gets or sets the completed count.
        /// </summary>
        /// <value>
        /// The completed count.
        /// </value>
        public int CompletedCount { get; set; } = 0;

        /// <summary>
        /// Gets or sets deadline for task completion.
        /// </summary>
        public DateTime Deadline { get; set; }

        /// <summary>
        /// Gets or sets task priority.
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// Gets or sets additional instructions for the operator.
        /// </summary>
        public string? Instructions { get; set; } = null!;

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public int Status { get; set; }

        /// <summary>
        /// Gets or sets Createdby.
        /// </summary>
        public Guid Createdby { get; set; }
    }
}
