using System;
using System.Collections.Generic;
using System.Text;
using Winfocus.LMS.Domain.Entities;

namespace Winfocus.LMS.Application.Interfaces
{
    /// <summary>
    /// IExamSubjectRepository.
    /// </summary>
    public interface IExamSubjectRepository
    {
        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>ExamSubject list.</returns>
        Task<IReadOnlyList<ExamSubject>> GetAllAsync();

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="gradeID">The gradeID.</param>
        /// <returns>ExamSubject.</returns>
        Task<ExamSubject?> GetByIdAsync(Guid id, Guid gradeID);

        /// <summary>
        /// Adds the asynchronous.
        /// </summary>
        /// <param name="ExamSubject">The ExamSubject.</param>
        /// <returns>ExamSubject.</returns>
        Task<ExamSubject> AddAsync(ExamSubject ExamSubject);

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="ExamSubject">The ExamSubject.</param>
        /// <returns>task.</returns>
        Task<ExamSubject> UpdateAsync(ExamSubject ExamSubject);

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
        /// <param name="gradeID">The gradeID.</param>
        /// <returns>bool.</returns>
        Task<bool> ExistsByNameAsync(string name, Guid gradeID);

        /// <summary>
        /// Gets centre by country, mode of study and state.
        /// </summary>
        /// <param name="gradeID">gradeID identifier.</param>
        /// <returns>Centre entity if found; otherwise null.</returns>
        Task<List<ExamSubject>> GetByFilterAsync(Guid? gradeID);

        /// <summary>
        /// Gets queryable for filtering with full hierarchy.
        /// </summary>
        /// <returns>Queryable ExamSubject.</returns>
        IQueryable<ExamSubject> Query();

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="gradeId">The identifier.</param>
        /// <returns>ExamSubject.</returns>
        Task<List<ExamSubject>> GetByGradeIdAsync(Guid gradeId);
    }
}
