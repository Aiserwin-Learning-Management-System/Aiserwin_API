using System;
using System.Collections.Generic;
using System.Text;
using Winfocus.LMS.Application.DTOs;
using Winfocus.LMS.Application.DTOs.Masters;
using Winfocus.LMS.Application.DTOs.Students;

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
        Task DeleteAsync(Guid id);

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
        /// <returns>StudentDto.</returns>
        Task<IReadOnlyList<StudentDto>> GetFilteredAsync(
        Guid? countryId,
        Guid? stateId,
        Guid? modeId,
        Guid? centreId,
        Guid? batchId,
        Guid? gradeId,
        Guid? courseId,
        DateTime? startDate,
        DateTime? endDate,
        string? registrationStatus,
        string? searchText,
        int limit,
        int offset,
        string sortBy,
        string sortOrder);
    }
}
