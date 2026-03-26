using System;
using System.Collections.Generic;
using System.Text;

namespace Winfocus.LMS.Domain.Enums
{
    /// <summary>
    /// Represents the current status of an exam.
    /// </summary>
    public enum ExamStatus
    {
        /// <summary>
        /// Exam is created but not yet scheduled or published.
        /// </summary>
        Draft = 1,

        /// <summary>
        /// Exam is scheduled but has not started yet.
        /// </summary>
        Scheduled = 2,

        /// <summary>
        /// Exam is currently in progress.
        /// </summary>
        Ongoing = 3,

        /// <summary>
        /// Exam has been completed by students.
        /// </summary>
        Completed = 4,

        /// <summary>
        /// Exam has been cancelled and will not take place.
        /// </summary>
        Cancelled = 5,
    }
}
