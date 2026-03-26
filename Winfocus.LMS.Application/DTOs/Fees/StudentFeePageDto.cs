namespace Winfocus.LMS.Application.DTOs.Fees
{
    using Winfocus.LMS.Domain.Enums;

    /// <summary>
    /// Data Transfer Object (DTO) representing a student's fee page,
    /// including student details, grade, syllabus, scholarship, and fee listings.
    /// </summary>
    public class StudentFeePageDto
    {
        /// <summary>Gets or sets the unique identifier of the student.</summary>
        public Guid StudentId { get; set; }

        /// <summary>Gets or sets the name of the student.</summary>
        public string StudentName { get; set; } = string.Empty;

        /// <summary>Gets or sets the registration number of the student.</summary>
        public string RegistrationNumber { get; set; } = string.Empty;

        /// <summary>Gets or sets the unique identifier of the grade.</summary>
        public Guid GradeId { get; set; }

        /// <summary>Gets or sets the name of the grade.</summary>
        public string GradeName { get; set; } = string.Empty;

        /// <summary>Gets or sets the unique identifier of the syllabus.</summary>
        public Guid SyllabusId { get; set; }

        /// <summary>Gets or sets the name of the syllabus.</summary>
        public string SyllabusName { get; set; } = string.Empty;

        /// <summary>Gets or sets the scholarship percentage awarded to the student.</summary>
        public decimal ScholarshipPercent { get; set; }

        /// <summary>Gets or sets the collection of fee listings available for the student.</summary>
        public List<FeeListingRowDto> FeeListings { get; set; } = new();

        /// <summary>Gets or sets the unique identifier of the selected fee plan, if any.</summary>
        public Guid? SelectedFeePlanId { get; set; }
    }

    /// <summary>
    /// Represents a row in the fee listing, including course details,
    /// fee amounts, discounts, and installment information.
    /// </summary>
    public class FeeListingRowDto
    {
        /// <summary>Gets or sets the unique identifier of the fee plan.</summary>
        public Guid FeePlanId { get; set; }

        /// <summary>Gets or sets the unique identifier of the course.</summary>
        public Guid CourseId { get; set; }

        /// <summary>Gets or sets the name of the course.</summary>
        public string CourseName { get; set; } = string.Empty;

        /// <summary>Gets or sets the yearly fee amount for the course.</summary>
        public decimal YearlyFee { get; set; }

        /// <summary>Gets or sets the payment type (e.g., full payment, installment).</summary>
        public PaymentType PaymentType { get; set; }

        /// <summary>Gets or sets the duration of the course in years.</summary>
        public int DurationInYears { get; set; }

        /// <summary>Gets or sets the total discount percentage applied.</summary>
        public decimal TotalDiscountPercent { get; set; }

        /// <summary>Gets or sets the total fee before discounts.</summary>
        public decimal TotalBeforeDiscount { get; set; }

        /// <summary>Gets or sets the fee amount after discounts.</summary>
        public decimal FeeAfterDiscount { get; set; }

        /// <summary>Gets or sets the number of installments available.</summary>
        public int InstallmentCount { get; set; }

        /// <summary>Gets or sets the amount per installment.</summary>
        public decimal PerInstallment { get; set; }

        /// <summary>Gets or sets a value indicating whether this fee plan is selected.</summary>
        public bool IsSelected { get; set; }

        /// <summary>Gets or sets the collection of discounts applied to this fee plan.</summary>
        public List<DiscountBadgeDto> AppliedDiscounts { get; set; } = new();
    }

    /// <summary>
    /// Represents a discount badge applied to a fee plan,
    /// including the discount name and percentage.
    /// </summary>
    public class DiscountBadgeDto
    {
        /// <summary>Gets or sets the name of the discount.</summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>Gets or sets the discount percentage.</summary>
        public decimal Percent { get; set; }
    }
}
