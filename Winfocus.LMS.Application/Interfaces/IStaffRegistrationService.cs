namespace Winfocus.LMS.Application.Interfaces
{
    using Winfocus.LMS.Application.DTOs;
    using Winfocus.LMS.Application.DTOs.Common;
    using Winfocus.LMS.Application.DTOs.Registration;

    /// <summary>
    /// Orchestrates registration submission, validation, file uploads, and retrieval.
    /// </summary>
    public interface IStaffRegistrationService
    {
        /// <summary>
        /// Submits the asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns>
        /// The asynchronous.
        Task<CommonResponse<RegistrationResponseDto>> SubmitAsync(
            SubmitRegistrationRequest request, Guid userId);

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// The by identifier asynchronous.
        Task<CommonResponse<RegistrationDetailDto>> GetByIdAsync(Guid id);

        /// <summary>
        /// Gets the filtered asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        /// The filtered asynchronous.
        Task<CommonResponse<PagedResult<RegistrationResponseDto>>> GetFilteredAsync(
            StaffRegistrationFilterRequest request);
    }
}
