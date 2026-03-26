using System;
using System.Collections.Generic;
using System.Text;
using Winfocus.LMS.Application.DTOs;
using Winfocus.LMS.Application.DTOs.Common;
using Winfocus.LMS.Application.DTOs.Registration;

namespace Winfocus.LMS.Application.Interfaces
{
    /// <summary>
    /// Defines business logic operations for managing registration forms.
    /// The service coordinates between controllers and repositories.
    /// </summary>
    public interface IRegistrationFormService
    {
        /// <summary>
        /// Creates a new registration form.
        /// </summary>
        /// <param name="dto">
        /// Data transfer object containing form information,
        /// selected groups, and standalone fields.
        /// </param>
        /// <returns>
        /// The unique identifier of the newly created registration form.
        /// </returns>
        Task<CommonResponse<Guid>> CreateAsync(CreateRegistrationFormDto dto);

        /// <summary>
        /// Retrieves a registration form by its identifier.
        /// </summary>
        /// <param name="id">
        /// The unique identifier of the registration form.
        /// </param>
        /// <returns>
        /// A <see cref="RegistrationFormResponseDto"/> containing
        /// the form details and section structure.
        /// </returns>
        Task<CommonResponse<RegistrationFormResponseDto>> GetByIdAsync(Guid id);

        /// <summary>
        /// Retrieves a registration form by staffcategoryid.
        /// </summary>
        /// <param name="staffcategoryid">
        /// The unique staffcategoryid of the registration form.
        /// </param>
        /// <returns>
        /// A <see cref="RegistrationFormResponseDto"/> containing
        /// the form details and section structure.
        /// </returns>
        Task<CommonResponse<RegistrationFormResponseDto>> GetByStaffCategoryAsync(Guid staffcategoryid);

        /// <summary>
        /// Retrieves all registration forms.
        /// </summary>
        /// <returns>
        /// A list of registration form summaries.
        /// </returns>
        Task<CommonResponse<List<RegistrationFormResponseDto>>> GetAllAsync();

        /// <summary>
        /// Updates an existing registration form.
        /// </summary>
        /// <param name="id">
        /// The identifier of the form to update.
        /// </param>
        /// <param name="dto">
        /// Updated form configuration.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        Task<CommonResponse<Guid>> UpdateAsync(Guid id, CreateRegistrationFormDto dto);

        /// <summary>
        /// Performs a soft delete on a registration form.
        /// </summary>
        /// <param name="id">
        /// The identifier of the form to delete.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous delete operation.
        /// </returns>
        Task<CommonResponse<bool>> DeleteAsync(Guid id);

        /// <summary>
        /// Gets the filtered asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        /// The filtered asynchronous.
        Task<CommonResponse<PagedResult<RegistrationFormResponseDto>>> GetFilteredAsync(
            StaffRegistrationFilterRequest request);

        /// <summary>
        /// Creates a new registration form.
        /// </summary>
        /// <param name="dto">
        /// Data transfer object containing form information,
        /// selected groups, and standalone fields.
        /// </param>
        /// <returns>
        /// The unique identifier of the newly created registration form.
        /// </returns>
        Task<CommonResponse<RegistrationFormResponseDto>> CreatePreview(CreateRegistrationFormDto dto);
    }
}
