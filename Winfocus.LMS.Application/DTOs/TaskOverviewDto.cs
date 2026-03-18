using System;
using System.Collections.Generic;
using System.Text;

namespace Winfocus.LMS.Application.DTOs
{
    /// <summary>
    /// Represents overall dashboard statistics for task assignments.
    /// </summary>
    public class TaskOverviewDto
    {
        public int TotalTasks { get; set; }

        public int ActiceTasks { get; set; }

        public int CompletedTasks { get; set; }

        public int OverdueTasks { get; set; }

        public int TotalQuestionsAssigned { get; set; }

        public int totalQuestionsCompleted { get; set; }

        public decimal completionRate { get; set; }

        public List<OperatorStatus> operatorStatus { get; set; }
    }

    /// <summary>
    /// Represents operator-wise task statistics.
    /// </summary>
    public class OperatorStatus
    {
        public Guid OperatorId { get; set; }

        public string OperatorName { get; set; }

        public int ActiveTasks { get; set; }

        public int CompletedQuestions { get; set; }

        public int TotalAssigned { get; set; }

        public decimal CompletionRate { get; set; }
    }
}
