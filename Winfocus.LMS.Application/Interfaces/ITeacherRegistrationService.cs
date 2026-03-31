using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Winfocus.LMS.Application.DTOs;
using Winfocus.LMS.Application.DTOs.Teacher;

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
        /// <returns>.</returns>
        Task<CommonResponse<TeacherRegistrationDto>> CreateAsync(TeacherRegistrationRequest request);

        /// <summary>
        /// Gets a teacher registration by identifier.
        /// </summary>
        /// <param name="id">Teacher identifier.</param>
        /// <returns>.</returns>
        Task<CommonResponse<TeacherRegistrationDto>> GetByIdAsync(Guid id);

        /// <summary>
        /// Retrieves all teacher registrations.
        /// </summary>
        /// <returns>.</returns>
        Task<CommonResponse<List<TeacherRegistrationDto>>> GetAllAsync();

        /// <summary>
        /// Updates an existing teacher registration.
        /// </summary>
        /// <param name="id">Identifier of the teacher to update.</param>
        /// <param name="request">Updated teacher data.</param>
        /// <returns>.</returns>
        Task<CommonResponse<TeacherRegistrationDto>> UpdateAsync(Guid id, TeacherRegistrationRequest request);

        /// <summary>
        /// Performs a soft delete for a teacher registration.
        /// </summary>
        /// <param name="id">Identifier of the teacher to soft delete.</param>
        /// <returns>.</returns>
        Task<CommonResponse<bool>> SoftDeleteAsync(Guid id);
    }
}
