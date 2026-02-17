namespace Winfocus.LMS.Application.Interfaces
{
    using Winfocus.LMS.Application.DTOs.Fees;

    /// <summary>
    /// IFeeService.
    /// </summary>
    public interface IFeeService
    {
        /// <summary>
        /// Gets the fee page asynchronous.
        /// </summary>
        /// <param name="studentId">The student identifier.</param>
        /// <returns>Task&lt;FeePageResponseDto&gt;.</returns>
        Task<FeePageResponseDto> GetFeePageAsync(Guid studentId);

        /// <summary>
        /// Selects the fee asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Task&lt;FeeSummaryDto&gt;.</returns>
        Task<FeeSummaryDto> SelectFeeAsync(SelectFeeRequestDto request);
    }
}
