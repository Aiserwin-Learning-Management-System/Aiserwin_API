namespace Winfocus.LMS.Application.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Winfocus.LMS.Domain.Entities;

    /// <summary>
    /// Defines business operations for <see cref="DoubtClearing"/> entities.
    /// </summary>
    public interface IDoubtClearingRepository
    {
        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>DoubtClearing.</returns>
        Task<IReadOnlyList<DoubtClearing>> GetAllAsync();

        /// <summary>
        /// Gets the academic year that contains the specified date.
        /// </summary>
        /// <param name="date">The date used to determine the doubt clearing.</param>
        /// <returns>The matching DoubtClearing if found; otherwise null.</returns>
        Task<DoubtClearing?> GetByDateAsync(DateTime date);

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>DoubtClearing .</returns>
        Task<DoubtClearing?> GetByIdAsync(Guid id);

        /// <summary>
        /// Adds the asynchronous.
        /// </summary>
        /// <param name="batch">The DoubtClearing.</param>
        /// <returns>AcademicYear.</returns>
        Task<DoubtClearing> AddAsync(DoubtClearing batch);

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="doubtclearing">The doubtclearing.</param>
        /// <returns>DoubtClearing.</returns>
        Task<DoubtClearing> UpdateAsync(DoubtClearing doubtclearing);

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>task.</returns>
        Task<bool> DeleteAsync(Guid id);

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="subjectid">The identifier.</param>
        /// <returns>DoubtClearing .</returns>
        Task<List<DoubtClearing>> GetBySubjectIdAsync(Guid subjectid);

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>doubtclearing .</returns>
        IQueryable<DoubtClearing> Query();
    }
}
