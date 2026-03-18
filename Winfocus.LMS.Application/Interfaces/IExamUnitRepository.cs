using System;
using System.Collections.Generic;
using System.Text;
using Winfocus.LMS.Domain.Entities;

namespace Winfocus.LMS.Application.Interfaces
{
    /// <summary>
    /// IExamUnitRepository.
    /// </summary>
    public interface IExamUnitRepository
    {
        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>ExamUnit list.</returns>
        Task<IReadOnlyList<ExamUnit>> GetAllAsync();

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="SubjectID">The SubjectID.</param>
        /// <returns>ExamUnit.</returns>
        Task<ExamUnit?> GetByIdAsync(Guid id, Guid SubjectID);

        /// <summary>
        /// Adds the asynchronous.
        /// </summary>
        /// <param name="ExamUnit">The ExamUnit.</param>
        /// <returns>ExamUnit.</returns>
        Task<ExamUnit> AddAsync(ExamUnit ExamUnit);

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="ExamUnit">The ExamUnit.</param>
        /// <returns>task.</returns>
        Task<ExamUnit> UpdateAsync(ExamUnit ExamUnit);

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>task.</returns>
        Task<bool> DeleteAsync(Guid id);

        /// <summary>
        /// Existses the by code asynchronous.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="SubjectID">The SubjectID.</param>
        /// <returns>bool.</returns>
        Task<bool> ExistsByNameAsync(string name, Guid SubjectID);

        /// <summary>
        /// Gets centre by country, mode of study and state.
        /// </summary>
        /// <param name="SubjectID">SubjectID identifier.</param>
        /// <returns>Centre entity if found; otherwise null.</returns>
        Task<List<ExamUnit>> GetByFilterAsync(Guid? SubjectID);

        /// <summary>
        /// Gets queryable for filtering with full hierarchy.
        /// </summary>
        /// <returns>Queryable ExamUnit.</returns>
        IQueryable<ExamUnit> Query();
    }
}
