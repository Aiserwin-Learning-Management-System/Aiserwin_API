namespace Winfocus.LMS.Domain.Entities
{
    using Winfocus.LMS.Domain.Common;
    using Winfocus.LMS.Domain.Enums;

    /// <summary>
    /// Daily activity report submitted by a DTP operator.
    /// One report per operator per date enforced by unique constraint.
    /// </summary>
    public class DailyActivityReport : BaseEntity
    {
        /// <summary>
        /// Gets or sets the operator identifier.
        /// </summary>
        /// <value>
        /// The operator identifier.
        /// </value>
        public Guid OperatorId { get; set; }

        /// <summary>
        /// Gets or sets the task identifier.
        /// </summary>
        /// <value>
        /// The task identifier.
        /// </value>
        public Guid? TaskId { get; set; }

        /// <summary>
        /// Gets or sets the report date.
        /// </summary>
        /// <value>
        /// The report date.
        /// </value>
        public DateOnly ReportDate { get; set; }

        /// <summary>
        /// Gets or sets the questions typed.
        /// </summary>
        /// <value>
        /// The questions typed.
        /// </value>
        public int QuestionsTyped { get; set; } = 0;

        /// <summary>
        /// Gets or sets the time spent hours.
        /// </summary>
        /// <value>
        /// The time spent hours.
        /// </value>
        public decimal TimeSpentHours { get; set; } = 0;

        /// <summary>
        /// Gets or sets the issues faced.
        /// </summary>
        /// <value>
        /// The issues faced.
        /// </value>
        public string? IssuesFaced { get; set; }

        /// <summary>
        /// Gets or sets the remarks.
        /// </summary>
        /// <value>
        /// The remarks.
        /// </value>
        public string? Remarks { get; set; }

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
        /// Gets or sets the task assignment.
        /// </summary>
        /// <value>
        /// The task assignment.
        /// </value>
        public TaskAssignment? TaskAssignment { get; set; }
    }
}
