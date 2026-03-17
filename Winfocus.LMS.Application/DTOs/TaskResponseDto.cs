using System;
using System.Collections.Generic;
using System.Text;
using Winfocus.LMS.Application.DTOs.Masters;
using Winfocus.LMS.Domain.Entities;
using Winfocus.LMS.Domain.Enums;
using TaskStatus = Winfocus.LMS.Domain.Enums.TaskStatus;

namespace Winfocus.LMS.Application.DTOs
{
    /// <summary>
    /// Represents the detailed response returned for a task assignment.
    /// This DTO includes operator information, task progress, deadline metrics,
    /// and completion statistics used in task views and dashboards.
    /// </summary>
    public class TaskResponseDto : BaseClassDTO
    {
        /// <summary>
        /// Gets or sets the AssignedBy.
        /// </summary>
        public Guid AssignedBy { get; set; }

        /// <summary>
        /// Gets or sets the name of the operator assigned to the task.
        /// </summary>
        /// <value>
        /// The full name of the DTP operator responsible for typing the questions.
        /// </value>
        public Guid OperatorId { get; set; }

        /// <summary>
        /// Gets or sets the resource type identifier.
        /// </summary>
        /// <value>
        /// The resource type identifier.
        /// </value>
        public Guid ResourceTypeId { get; set; }

        /// <summary>
        /// Gets or sets the type of question assigned in the task.
        /// </summary>
        public int QuestionType { get; set; }

        /// <summary>
        /// Gets or sets the Year.
        /// </summary>
        public int? Year { get; set; }

        /// <summary>
        /// Gets or sets the syllabus identifier.
        /// </summary>
        /// <value>
        /// The syllabus identifier.
        /// </value>
        public Guid SyllabusId { get; set; }

        /// <summary>
        /// Gets or sets the grade identifier.
        /// </summary>
        /// <value>
        /// The grade identifier.
        /// </value>
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
        /// Gets or sets the chapter.
        /// </summary>
        /// <value>
        /// The chapter.
        /// </value>
        public Guid? ChapterId { get; set; }

        /// <summary>
        /// Gets or sets the total number of questions assigned in the task.
        /// </summary>
        /// <value>
        /// The total count of questions that the operator must complete.
        /// </value>
        public int TotalQuestions { get; set; }

        /// <summary>
        /// Gets or sets the number of questions completed by the operator.
        /// </summary>
        /// <value>
        /// The count of questions that have already been typed and submitted.
        /// </value>
        public int CompletedCount { get; set; }

        /// <summary>
        /// Gets or sets the deadline for completing the task.
        /// </summary>
        /// <value>
        /// The date and time by which the operator must finish typing the assigned questions.
        /// </value>
        public DateTime Deadline { get; set; }

        /// <summary>
        /// Gets or sets the number of days remaining until the task deadline.
        /// </summary>
        /// <value>
        /// A calculated value representing how many days are left before the deadline.
        /// </value>
        public int DaysRemaining { get; set; }

        /// <summary>
        /// Gets or sets the priority.
        /// </summary>
        /// <value>
        /// The priority.
        /// </value>
        public int Priority { get; set; }

        /// <summary>
        /// Gets or sets the instructions.
        /// </summary>
        /// <value>
        /// The instructions.
        /// </value>
        public string? Instructions { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public int Status { get; set; }

        /// <summary>
        /// Gets or sets the operator.
        /// </summary>
        /// <value>
        /// The operator.
        /// </value>
        public StaffRegistration Operator { get; set; } = null!;

        /// <summary>
        /// Gets or sets the type of the resource.
        /// </summary>
        /// <value>
        /// The type of the resource.
        /// </value>
        public ContentResourceType ResourceType { get; set; } = default!;

        /// <summary>
        /// Gets or sets the syllabus.
        /// </summary>
        /// <value>
        /// The syllabus.
        /// </value>
        public ExamSyllabus Syllabus { get; set; } = default!;

        /// <summary>
        /// Gets or sets the grade.
        /// </summary>
        /// <value>
        /// The grade.
        /// </value>
        public ExamGrade Grade { get; set; } = default!;

        /// <summary>
        /// Gets or sets the subject.
        /// </summary>
        /// <value>
        /// The subject.
        /// </value>
        public ExamSubject Subject { get; set; } = default!;

        /// <summary>
        /// Gets or sets the unit.
        /// </summary>
        /// <value>
        /// The unit.
        /// </value>
        public ExamUnit? Unit { get; set; }

        /// <summary>
        /// Gets or sets the chapter.
        /// </summary>
        /// <value>
        /// The chapter.
        /// </value>
        public ExamChapter? Chapter { get; set; }
    }
}
