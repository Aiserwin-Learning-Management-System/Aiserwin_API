// Application/DTOs/Fees/ConfirmFeeRequestDto.cs
namespace Winfocus.LMS.Application.DTOs.Fees
{
    /// <summary>
    /// Request DTO used when a student confirms their fee plan selection.
    /// This includes the chosen plan, duration, and acceptance of declaration terms.
    /// </summary>
    public sealed class ConfirmFeeRequestDto
    {
        /// <summary>Gets or sets the unique identifier of the student.</summary>
        public Guid StudentId { get; set; }

        /// <summary>Gets or sets the unique identifier of the selected fee plan.</summary>
        public Guid FeePlanId { get; set; }

        /// <summary>Gets or sets the start date of the fee plan.</summary>
        public DateTime StartDate { get; set; }

        /// <summary>Gets or sets the end date of the fee plan.</summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the student has accepted
        /// the declaration terms associated with the fee plan.
        /// </summary>
        public bool DeclarationAccepted { get; set; }
    }
}
