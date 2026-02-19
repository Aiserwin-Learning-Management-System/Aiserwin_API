using System;
using System.Collections.Generic;
using System.Text;
using Winfocus.LMS.Application.DTOs;
using Winfocus.LMS.Domain.Entities;
using Winfocus.LMS.Domain.Enums;

namespace Winfocus.LMS.Application.Interfaces
{
    /// <summary>
    /// Defines CRUD operations for <see cref="Student"/> entities.
    /// </summary>
    public interface IStudentRepository
    {
        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>Student.</returns>
        Task<IReadOnlyList<Student>> GetAllAsync();

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Student.</returns>
        Task<Student?> GetByIdAsync(Guid id);

        /// <summary>
        /// Adds the asynchronous.
        /// </summary>
        /// <param name="Student">The Student.</param>
        /// <returns>Student.</returns>
        Task<Student> AddAsync(Student Student);

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="student">The state.</param>
        /// <returns>Student.</returns>
        Task UpdateAsync(Student student);

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>task.</returns>
        Task<CommonResponse<bool>> DeleteAsync(Guid id);

        /// <summary>
        /// Gets the filtered asynchronous.
        /// </summary>
        /// <param name="countryId">The country identifier.</param>
        /// <param name="stateId">The state identifier.</param>
        /// <param name="modeId">The mode identifier.</param>
        /// <param name="centreId">The centre identifier.</param>
        /// <param name="batchId">The batch identifier.</param>
        /// <param name="gradeId">The grade identifier.</param>
        /// <param name="courseId">The course identifier.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="registrationStatus">The registration status.</param>
        /// <param name="searchText">The search text.</param>
        /// <param name="limit">The limit.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="sortBy">The sort by.</param>
        /// <param name="sortOrder">The sort order.</param>
        /// <returns>Student.</returns>
        Task<IReadOnlyList<Student>> GetFilteredAsync(
         Guid? countryId,
         Guid? stateId,
         Guid? modeId,
         Guid? centreId,
         Guid? batchId,
         Guid? gradeId,
         Guid? courseId,
         DateTime? startDate,
         DateTime? endDate,
         RegistrationStatus? registrationStatus,
         string? searchText,
         int limit,
         int offset,
         string sortBy,
         string sortOrder);

        /// <summary>
        /// update the registration status.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>task.</returns>
        Task<CommonResponse<bool>> StudentConfirm(Guid id);
    }
}
