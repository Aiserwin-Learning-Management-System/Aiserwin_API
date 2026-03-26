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
        /// <returns>
        /// The asynchronous.
        Task<CommonResponse<RegistrationResponseDto>> SubmitAsync(
            SubmitRegistrationRequest request);

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

        /// <summary>
        /// Updates the status asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="dto">The dto.</param>
        /// <returns></returns>
        Task<CommonResponse<string>> UpdateStatusAsync(Guid id, UpdateRegistrationStatusDto dto);

        /// <summary>
        /// Links a user account to a staff registration after admin approval.
        /// Called after the auth system creates a user account for the approved operator.
        /// </summary>
        /// <param name="registrationId">The staff registration identifier.</param>
        /// <param name="userId">The newly created user account identifier.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task LinkUserAsync(Guid registrationId, Guid userId);
    }
}
