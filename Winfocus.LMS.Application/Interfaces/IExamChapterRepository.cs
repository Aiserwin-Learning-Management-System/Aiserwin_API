using System;
using System.Collections.Generic;
using System.Text;
using Winfocus.LMS.Domain.Entities;

namespace Winfocus.LMS.Application.Interfaces
{
    /// <summary>
    /// IExamUnitRepository.
    /// </summary>
    public interface IExamChapterRepository
    {
        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>ExamChapter list.</returns>
        Task<IReadOnlyList<ExamChapter>> GetAllAsync();

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="unitID">The unitID.</param>
        /// <returns>ExamChapter.</returns>
        Task<ExamChapter?> GetByIdAsync(Guid id, Guid unitID);

        /// <summary>
        /// Adds the asynchronous.
        /// </summary>
        /// <param name="examchapter">The examchapter.</param>
        /// <returns>ExamChapter.</returns>
        Task<ExamChapter> AddAsync(ExamChapter examchapter);

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="examchapter">The examchapter.</param>
        /// <returns>task.</returns>
        Task<ExamChapter> UpdateAsync(ExamChapter examchapter);

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
        /// <param name="unitid">The Unitid.</param>
        /// <returns>bool.</returns>
        Task<bool> ExistsByNameAsync(string name, Guid unitid);

        /// <summary>
        /// Gets centre by country, mode of study and state.
        /// </summary>
        /// <param name="unitid">Unitid identifier.</param>
        /// <returns>Centre entity if found; otherwise null.</returns>
        Task<List<ExamChapter>> GetByFilterAsync(Guid? unitid);

        /// <summary>
        /// Gets queryable for filtering with full hierarchy.
        /// </summary>
        /// <returns>Queryable ExamUnit.</returns>
        IQueryable<ExamChapter> Query();
    }
}
