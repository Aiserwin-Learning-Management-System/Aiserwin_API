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
        /// <param name="countryid">The countryid.</param>
        /// <param name="stateid">The stateid.</param>
        /// <param name="centerid">The centerid.</param>
        /// <returns>StudentDto.</returns>
        Task<IReadOnlyList<StudentDto>> GetAllAsync(Guid? countryid, Guid? stateid, Guid? centerid);

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>StudentDto.</returns>
        Task<StudentDto?> GetByIdAsync(Guid id);

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="countryid">The countryid.</param>
        /// <param name="stateid">The stateid.</param>
        /// <param name="centerid">The centerid.</param>
        /// <returns>StudentDto.</returns>
        Task<StudentDto?> GetByIdsAsync(Guid id, Guid? countryid, Guid? stateid, Guid? centerid);

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="countryid">The countryid.</param>
        /// <param name="stateid">The stateid.</param>
        /// <param name="centerid">The centerid.</param>
        /// <returns>StudentDto.</returns>
        Task<StudentDto?> GetByUserIdsAsync(Guid id, Guid? countryid, Guid? stateid, Guid? centerid);

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

        /// <summary>
        /// Links the user asynchronous.
        /// </summary>
        /// <param name="studentId">The student identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        Task LinkUserAsync(Guid studentId, Guid userId);
    }
}
