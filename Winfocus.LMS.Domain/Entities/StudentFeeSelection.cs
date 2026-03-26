namespace Winfocus.LMS.Domain.Entities
{
    using Winfocus.LMS.Domain.Common;
    using Winfocus.LMS.Domain.Enums;
    using Winfocus.LMS.Domain.Extensions;

    /// <summary>
    /// Represents a student's fee selection for a course, including applied discounts,
    /// payment type, installment details, and overall status.
    /// </summary>
    public sealed class StudentFeeSelection : BaseEntity
    {
        /// <summary>Gets the unique identifier of the student.</summary>
        public Guid StudentId { get; private set; }

        /// <summary>Gets the unique identifier of the course.</summary>
        public Guid CourseId { get; private set; }

        /// <summary>Gets the unique identifier of the fee plan.</summary>
        public Guid FeePlanId { get; private set; }

        /// <summary>Gets the yearly fee amount for the course.</summary>
        public decimal YearlyFee { get; private set; }

        /// <summary>Gets the selected duration of the course in years.</summary>
        public int SelectedDurationYears { get; private set; }

        /// <summary>Gets the total fee before applying discounts.</summary>
        public decimal TotalBeforeDiscount { get; private set; }

        /// <summary>Gets the total discount percentage applied.</summary>
        public decimal TotalDiscountPercent { get; private set; }

        /// <summary>Gets the total discount amount applied.</summary>
        public decimal TotalDiscountAmount { get; private set; }

        /// <summary>Gets the final payable amount after discounts.</summary>
        public decimal FinalAmount { get; private set; }

        /// <summary>Gets the payment type (e.g., full payment, installment).</summary>
        public PaymentType PaymentType { get; private set; }

        /// <summary>Gets the total number of installments selected.</summary>
        public int TotalInstallments { get; private set; }

        /// <summary>Gets the start date of the fee plan.</summary>
        public DateTime StartDate { get; private set; }

        /// <summary>Gets the end date of the fee plan.</summary>
        public DateTime EndDate { get; private set; }

        /// <summary>Gets the current status of the fee selection.</summary>
        public FeeSelectionStatus Status { get; private set; }

        /// <summary>Gets the student associated with this fee selection.</summary>
        public Student Student { get; private set; } = null!;

        /// <summary>Gets the course associated with this fee selection.</summary>
        public Course Course { get; private set; } = null!;

        /// <summary>Gets the fee plan associated with this fee selection.</summary>
        public FeePlan FeePlan { get; private set; } = null!;

        /// <summary>Gets the collection of discounts applied to this fee selection.</summary>
        public ICollection<StudentFeeDiscount> AppliedDiscounts { get; private set; }
            = new List<StudentFeeDiscount>();

        /// <summary>Gets the collection of installments for this fee selection.</summary>
        public ICollection<StudentInstallment> Installments { get; private set; }
            = new List<StudentInstallment>();

        /// <summary>
        /// Prevents a default instance of the <see cref="StudentFeeSelection"/> class from being created.
        /// </summary>
        private StudentFeeSelection() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="StudentFeeSelection"/> class.
        /// </summary>
        /// <param name="studentId">The unique identifier of the student.</param>
        /// <param name="courseId">The unique identifier of the course.</param>
        /// <param name="feePlanId">The unique identifier of the fee plan.</param>
        /// <param name="yearlyFee">The yearly fee amount.</param>
        /// <param name="selectedDurationYears">The selected duration in years.</param>
        /// <param name="totalBeforeDiscount">The total fee before discounts.</param>
        /// <param name="totalDiscountPercent">The discount percentage applied.</param>
        /// <param name="totalDiscountAmount">The discount amount applied.</param>
        /// <param name="finalAmount">The final payable amount after discounts.</param>
        /// <param name="paymentType">The payment type.</param>
        /// <param name="totalInstallments">The total number of installments.</param>
        /// <param name="startDate">The start date of the fee plan.</param>
        /// <param name="endDate">The end date of the fee plan.</param>
        public StudentFeeSelection(
            Guid studentId,
            Guid courseId,
            Guid feePlanId,
            decimal yearlyFee,
            int selectedDurationYears,
            decimal totalBeforeDiscount,
            decimal totalDiscountPercent,
            decimal totalDiscountAmount,
            decimal finalAmount,
            PaymentType paymentType,
            int totalInstallments,
            DateTime startDate,
            DateTime endDate)
        {
            StudentId = studentId;
            CourseId = courseId;
            FeePlanId = feePlanId;
            YearlyFee = yearlyFee;
            SelectedDurationYears = selectedDurationYears;
            TotalBeforeDiscount = totalBeforeDiscount;
            TotalDiscountPercent = totalDiscountPercent;
            TotalDiscountAmount = totalDiscountAmount;
            FinalAmount = finalAmount;
            PaymentType = paymentType;
            TotalInstallments = totalInstallments;
            StartDate = startDate;
            EndDate = endDate;
            Status = FeeSelectionStatus.Confirmed;
            CreatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Recalculates the fee selection status based on installment payments.
        /// </summary>
        public void RefreshStatus()
        {
            if (!Installments.Any())
            {
                return;
            }

            var allPaid = Installments.All(i => i.Status == InstallmentStatus.Paid);
            var anyPaid = Installments.Any(i => i.Status == InstallmentStatus.Paid);

            if (allPaid)
            {
                Status = FeeSelectionStatus.Paid;
            }
            else if (anyPaid)
            {
                Status = FeeSelectionStatus.PartiallyPaid;
            }
            else
            {
                Status = FeeSelectionStatus.Confirmed;
            }

            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Cancels the fee selection, marking it as inactive.
        /// </summary>
        public void Cancel()
        {
            Status = FeeSelectionStatus.Cancelled;
            IsActive = false;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
