using System;
using System.Collections.Generic;
using System.Text;
using Winfocus.LMS.Application.DTOs.Masters;

namespace Winfocus.LMS.Application.DTOs.Fees
{
    /// <summary>
    /// Represents a FeePlan with its associated discounts.
    /// </summary>
    public class FeePlanDto : BaseClassDTO
    {

        /// <summary>
        /// Gets or sets the related Course identifier.
        /// </summary>
        public Guid CourseId { get; set; }

        /// <summary>
        /// Gets or sets the name of the fee plan.
        /// </summary>
        public string PlanName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the tuition fee amount.
        /// </summary>
        public decimal TuitionFee { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether installment payment is allowed.
        /// </summary>
        public bool IsInstallmentAllowed { get; set; }

        /// <summary>
        /// Gets or sets the type of the payment.
        /// </summary>
        /// <value>The type of the payment.</value>
        public string PaymentType { get; set; } = null!;

        /// <summary>
        /// Gets or sets the course duration in years.
        /// </summary>
        /// <value>course duration in years.</value>
        public int DurationinYears { get; set; }

        /// <summary>
        /// Gets or sets the Subject identifier.
        /// </summary>
        /// <value>The Subject identifier.</value>
        public Guid SubjectId { get; set; }

        /// <summary>
        /// Gets or sets the syllabus identifier.
        /// </summary>
        /// <value>The syllabus identifier.</value>
        public Guid SyllabusId { get; set; }

        /// <summary>
        /// Gets or sets the Grade identifier.
        /// </summary>
        /// <value>The Grade identifier.</value>
        public Guid GradeId { get; set; }

        /// <summary>
        /// Gets or sets the Stream identifier.
        /// </summary>
        /// <value>The Stream identifier.</value>
        public Guid StreamId { get; set; }

        /// <summary>
        /// Gets or sets the Country identifier.
        /// </summary>
        /// <value>The Country identifier.</value>
        public Guid CountryId { get; set; }

        /// <summary>
        /// Gets or sets the State identifier.
        /// </summary>
        /// <value>The State identifier.</value>
        public Guid StateId { get; set; }

        /// <summary>
        /// Gets or sets the Modeofstudy identifier.
        /// </summary>
        /// <value>The Modeofstudy identifier.</value>
        public Guid ModeofstudyId { get; set; }

        /// <summary>
        /// Gets or sets the Center identifier.
        /// </summary>
        /// <value>The Center identifier.</value>
        public Guid CenterId { get; set; }

        /// <summary>
        /// Gets or sets the list of discounts associated with this FeePlan.
        /// </summary>
        public IReadOnlyCollection<FeePlanDiscountDto> Discounts { get; set; }
            = new List<FeePlanDiscountDto>();
    }
}
