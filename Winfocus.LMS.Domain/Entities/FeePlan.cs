namespace Winfocus.LMS.Domain.Entities
{
    using Winfocus.LMS.Domain.Common;

    /// <summary>
    /// FeePlan.
    /// </summary>
    /// <seealso cref="Winfocus.LMS.Domain.Common.BaseEntity" />
    public sealed class FeePlan : BaseEntity
    {
        /// <summary>
        /// Gets the course identifier.
        /// </summary>
        /// <value>
        /// The course identifier.
        /// </value>
        public Guid CourseId { get; private set; }

        /// <summary>
        /// Gets the name of the plan.
        /// </summary>
        /// <value>
        /// The name of the plan.
        /// </value>
        public string PlanName { get; private set; } = null!;

        /// <summary>
        /// Gets the tuition fee.
        /// </summary>
        /// <value>
        /// The tuition fee.
        /// </value>
        public decimal TuitionFee { get; private set; }

        /// <summary>
        /// Gets the registration fee.
        /// </summary>
        /// <value>
        /// The registration fee.
        /// </value>
        public decimal RegistrationFee { get; private set; }

        /// <summary>
        /// Gets the scholarship percent.
        /// </summary>
        /// <value>
        /// The scholarship percent.
        /// </value>
        public decimal ScholarshipPercent { get; private set; }

        /// <summary>
        /// Gets the seasonal percent.
        /// </summary>
        /// <value>
        /// The seasonal percent.
        /// </value>
        public decimal SeasonalPercent { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance is installment allowed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is installment allowed; otherwise, <c>false</c>.
        /// </value>
        public bool IsInstallmentAllowed { get; private set; }

        /// <summary>
        /// Gets the course.
        /// </summary>
        /// <value>
        /// The course.
        /// </value>
        public Course Course { get; private set; } = null!;

        /// <summary>
        /// Gets the installments.
        /// </summary>
        /// <value>
        /// The installments.
        /// </value>
        public ICollection<FeeInstallment> Installments { get; private set; }
            = new List<FeeInstallment>();
    }
}
