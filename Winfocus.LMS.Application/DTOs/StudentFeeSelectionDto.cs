using System;
using System.Collections.Generic;
using System.Text;
using Winfocus.LMS.Application.DTOs.Fees;
using Winfocus.LMS.Application.DTOs.Masters;
using Winfocus.LMS.Application.DTOs.Students;

namespace Winfocus.LMS.Application.DTOs
{
    /// <summary>
    /// StudentFeeSelectionDto object containing master-level fields.
    /// </summary>
    public class StudentFeeSelectionDto : BaseClassDTO
    {
        /// <summary>
        /// Gets or sets the identifier of the student.
        /// </summary>
        public Guid StudentId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the course.
        /// </summary>
        public Guid CourseId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the fee plan.
        /// </summary>
        public Guid FeePlanId { get; set; }

        /// <summary>
        /// Gets or sets the scholarship percentage applied.
        /// </summary>
        public decimal ScholarshipPercent { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets whether scholarship is active.
        /// </summary>
        public bool IsScholarshipActive { get; set; }

        /// <summary>
        /// Gets or sets the seasonal discount percentage.
        /// </summary>
        public decimal SeasonalPercent { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets whether seasonal discount is active.
        /// </summary>
        public bool IsSeasonalActive { get; set; }

        /// <summary>
        /// Gets or sets the manual discount percentage.
        /// </summary>
        public decimal ManualDiscountPercent { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets whether manual discount is active.
        /// </summary>
        public bool IsManualDiscountActive { get; set; }

        /// <summary>
        /// Gets or sets the base fee amount.
        /// </summary>
        public decimal BaseFee { get; set; }

        /// <summary>
        /// Gets or sets the final calculated fee amount.
        /// </summary>
        public decimal FinalAmount { get; set; }

        /// <summary>
        /// Gets or sets the payment mode.
        /// </summary>
        public string PaymentMode { get; set; } = null!;

        /// <summary>
        /// Gets or sets the identifier of the associated Student.
        /// </summary>
        public StudentDto Student { get; set; } = null!;

        /// <summary>
        /// Gets or sets the identifier of the associated FeePlan.
        /// </summary>
        public FeePlanDto FeePlan { get; set; } = null!;
    }
}
