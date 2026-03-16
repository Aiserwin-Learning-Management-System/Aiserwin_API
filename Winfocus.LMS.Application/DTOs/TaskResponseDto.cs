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
    public class TaskResponseDto :BaseClassDTO
    {
        /// <summary>
        /// Gets or sets the AssignedBy.
        /// </summary>
        public string AssignedBy { get; set; } = null!;

        /// <summary>
        /// Gets or sets the name of the operator assigned to the task.
        /// </summary>
        /// <value>
        /// The full name of the DTP operator responsible for typing the questions.
        /// </value>
        public Guid OperatorId { get; set; }

        /// <summary>
        /// Gets or sets the type of question assigned in the task.
        /// </summary>
        public QuestionType QuestionType { get; set; }

        /// <summary>
        /// Gets or sets the Year.
        /// </summary>
        public int? Year { get; set; }

        /// <summary>
        /// Gets or sets the chapter.
        /// </summary>
        /// <value>
        /// The chapter.
        /// </value>
        public string? Chapter { get; set; }

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
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public TaskStatus Status { get; set; } = TaskStatus.Pending;

        /// <summary>
        /// Gets or sets the operator.
        /// </summary>
        /// <value>
        /// The operator.
        /// </value>
        public StaffRegistration Operator { get; set; } = null!;

        /// <summary>
        /// Gets or sets the priority.
        /// </summary>
        /// <value>
        /// The priority.
        /// </value>
        public TaskPriority Priority { get; set; }

        /// <summary>
        /// Gets or sets the instructions.
        /// </summary>
        /// <value>
        /// The instructions.
        /// </value>
        public string? Instructions { get; set; }
    }
}
