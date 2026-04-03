using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Winfocus.LMS.Application.DTOs;
using Winfocus.LMS.Application.DTOs.Common;
using Winfocus.LMS.Application.DTOs.Teacher;
using Winfocus.LMS.Application.DTOs.Common;

namespace Winfocus.LMS.Application.Interfaces
{
    /// <summary>
    /// ITeacherRegistrationService.
    /// </summary>
    public interface ITeacherRegistrationService
    {
        /// <summary>
        /// Creates a new teacher registration.
        /// </summary>
        /// <param name="request">Request DTO containing teacher details.</param>
        /// <returns>A <see cref="CommonResponse{T}"/> containing the created <see cref="TeacherRegistrationDto"/> on success or failure details.</returns>
        Task<CommonResponse<TeacherRegistrationDto>> CreateAsync(TeacherRegistrationRequest request);

        /// <summary>
        /// Gets a teacher registration by identifier.
        /// </summary>
        /// <param name="id">Teacher identifier.</param>
        /// <returns>A <see cref="CommonResponse{T}"/> containing the <see cref="TeacherRegistrationDto"/> if found; otherwise a failure response.</returns>
        Task<CommonResponse<TeacherRegistrationDto>> GetByIdAsync(Guid id);

        /// <summary>
        /// Retrieves all teacher registrations.
        /// </summary>
        /// <returns>A <see cref="CommonResponse{T}"/> containing a list of <see cref="TeacherRegistrationDto"/>.</returns>
        Task<CommonResponse<List<TeacherRegistrationDto>>> GetAllAsync();

        /// <summary>
        /// Gets filtered teacher registrations.
        /// </summary>
        /// <param name="request">Filter request.</param>
        /// <returns>Paged list of teacher DTOs.</returns>
        Task<PagedResult<TeacherRegistrationDto>> GetFilteredAsync(DTOs.Teacher.TeacherFilterRequest request);

        /// <summary>
        /// Updates an existing teacher registration.
        /// </summary>
        /// <param name="id">Identifier of the teacher to update.</param>
        /// <param name="request">Updated teacher data.</param>
        /// <returns>A <see cref="CommonResponse{T}"/> containing the updated <see cref="TeacherRegistrationDto"/> on success.</returns>
        Task<CommonResponse<TeacherRegistrationDto>> UpdateAsync(Guid id, TeacherRegistrationRequest request);

        /// <summary>
        /// Performs a soft delete for a teacher registration.
        /// </summary>
        /// <param name="id">Identifier of the teacher to soft delete.</param>
        /// <returns>A <see cref="CommonResponse{T}"/> indicating success (<c>true</c>) or failure (<c>false</c>).</returns>
        Task<CommonResponse<bool>> SoftDeleteAsync(Guid id);

        /// <summary>
        /// Confirms a teacher registration (changes status to Submitted).
        /// </summary>
        /// <param name="id">Identifier of the teacher to soft delete.</param>
        /// <returns>.</returns>
        Task<CommonResponse<bool>> TeacherConfirm(Guid id);

        /// <summary>
        /// Approves a teacher registration (changes status to Approved).
        /// </summary>
        /// <param name="id">Identifier of the teacher to soft delete.</param>
        /// <returns>.</returns>
        Task<CommonResponse<bool>> TeacherApprove(Guid id);

        /// <summary>
        /// Link a created user to a teacher registration (if supported).
        /// </summary>
        /// <param name="teacherId">teacherId.</param>
        /// <param name="userId">userId.</param>
        /// <returns>.</returns>
        Task LinkUserAsync(Guid teacherId, Guid userId);
    }
}
