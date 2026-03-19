namespace Winfocus.LMS.Application.Interfaces
{
    using Winfocus.LMS.Application.DTOs;
    using Winfocus.LMS.Application.DTOs.DtpAdmin;

    /// <summary>
    /// IDtpAdminService.
    /// </summary>
    public interface IDtpAdminService
    {
        /// <summary>
        /// Gets column definitions from the active DTP registration form.
        /// </summary>
        /// <returns>List of columns with metadata for frontend table construction.</returns>
        Task<CommonResponse<OperatorColumnsResponseDto>> GetColumnsAsync();

        /// <summary>
        /// Gets paginated operator list with dynamic values.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Paginated list of operators with dynamic column data.</returns>
        Task<CommonResponse<OperatorListResponseDto>> GetOperatorsAsync(
            DtpOperatorFilterRequest request);

        /// <summary>
        /// Soft-deletes an operator registration.
        /// </summary>
        /// <param name="registrationId">The registration ID.</param>
        /// <param name="adminUserId">The admin user ID performing the deletion.</param>
        /// <returns>True if deletion was successful, false otherwise.</returns>
        Task<CommonResponse<bool>> DeleteOperatorAsync(Guid registrationId, Guid adminUserId);

        /// <summary>
        /// Toggles operator active/inactive status.
        /// </summary>
        /// <param name="registrationId">The registration ID.</param>
        /// <param name="adminUserId">The admin user ID performing the toggle.</param>
        /// <returns>
        /// New status of the operator after toggle.
        /// </returns>
        Task<CommonResponse<OperatorToggleResponseDto>> ToggleOperatorAsync(
            Guid registrationId, Guid adminUserId);

        /// <summary>
        ///
        /// </summary>
        /// <param name="registrationId"></param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        Task<CommonResponse<OperatorDetailDto>> GetOperatorDetailAsync(Guid registrationId);

        /// <summary>
        /// </summary>
        /// <param name="registrationId"></param>
        /// <param name="dto"></param>
        /// <param name="adminUserId"></param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        Task<CommonResponse<bool>> VerifyOperatorAsync(Guid registrationId, VerifyOperatorDto dto, Guid adminUserId);
    }
}
