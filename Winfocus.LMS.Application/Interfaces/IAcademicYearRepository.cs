using System;
using System.Collections.Generic;
using System.Text;
using Winfocus.LMS.Domain.Entities;

namespace Winfocus.LMS.Application.Interfaces
{
    /// <summary>
    /// Defines business operations for <see cref="AcademicYear"/> entities.
    /// </summary>
    public interface IAcademicYearRepository
    {
        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>BatchTimingMTF.</returns>
        Task<IReadOnlyList<AcademicYear>> GetAllAsync();

        /// <summary>
        /// Gets the academic year that contains the specified date.
        /// </summary>
        /// <param name="date">The date used to determine the academic year.</param>
        /// <returns>The matching academic year if found; otherwise null.</returns>
        Task<AcademicYear?> GetByDateAsync(DateTime date);

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Academic year.</returns>
        Task<AcademicYear?> GetByIdAsync(Guid id);

        /// <summary>
        /// Adds the asynchronous.
        /// </summary>
        /// <param name="batch">The AcademicYear.</param>
        /// <returns>AcademicYear.</returns>
        Task<AcademicYear> AddAsync(AcademicYear batch);

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="academicyear">The academicyear.</param>
        /// <returns>AcademicYear.</returns>
        Task<AcademicYear> UpdateAsync(AcademicYear academicyear);

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>task.</returns>
        Task<bool> DeleteAsync(Guid id);

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>academic year.</returns>
        IQueryable<AcademicYear> Query();
    }
}
