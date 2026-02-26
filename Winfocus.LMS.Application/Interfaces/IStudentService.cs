using System;
using System.Collections.Generic;
using System.Text;
using Winfocus.LMS.Application.DTOs;
using Winfocus.LMS.Application.DTOs.Common;
using Winfocus.LMS.Application.DTOs.Masters;
using Winfocus.LMS.Application.DTOs.Students;
using Winfocus.LMS.Domain.Enums;

namespace Winfocus.LMS.Application.Interfaces
{
    /// <summary>
    /// Defines business operations for <see cref="Student"/> entities.
    /// </summary>
    public interface IStudentService
    {
        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>StudentDto.</returns>
        Task<IReadOnlyList<StudentDto>> GetAllAsync();

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>StudentDto.</returns>
        Task<StudentDto?> GetByIdAsync(Guid id);

        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>StudentDto.</returns>
        Task<StudentDto> CreateAsync(StudentDto request);

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="request">The request.</param>
        /// <returns>id.</returns>
        Task UpdateAsync(Guid id, StudentDto request);

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>id.</returns>
        Task<CommonResponse<bool>> DeleteAsync(Guid id);

        /// <summary>
        /// Gets the filtered asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>StudentDto.</returns>
        Task<PagedResult<StudentDto>> GetFilteredAsync(StudentFilterRequest request);

        /// <summary>
        /// update the registration status.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>id.</returns>
        Task<CommonResponse<bool>> StudentConfirm(Guid id);

        /// <summary>
        /// update the registration status.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>id.</returns>
        Task<CommonResponse<bool>> StudentApprove(Guid id);
    }
}
