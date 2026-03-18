using System;
using System.Collections.Generic;
using System.Text;
using Winfocus.LMS.Domain.Entities;

namespace Winfocus.LMS.Application.Interfaces
{
    /// <summary>
    /// IExamGradeRepository.
    /// </summary>
    public interface IExamGradeRepository
    {
        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>ExamGrade list.</returns>
        Task<IReadOnlyList<ExamGrade>> GetAllAsync();

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="syllabusId">The Syllabus.</param>
        /// <returns>ExamGrade.</returns>
        Task<ExamGrade?> GetByIdAsync(Guid id, Guid syllabusId);

        /// <summary>
        /// Adds the asynchronous.
        /// </summary>
        /// <param name="ExamGrade">The ExamGrade.</param>
        /// <returns>ExamGrade.</returns>
        Task<ExamGrade> AddAsync(ExamGrade ExamGrade);

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="ExamGrade">The ExamGrade.</param>
        /// <returns>task.</returns>
        Task<ExamGrade> UpdateAsync(ExamGrade ExamGrade);

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
        /// <param name="syllabusID">The syllabusID.</param>
        /// <returns>bool.</returns>
        Task<bool> ExistsByNameAsync(string name, Guid syllabusID);

        /// <summary>
        /// Gets centre by country, mode of study and state.
        /// </summary>
        /// <param name="SyllabusID">academicYear identifier.</param>
        /// <returns>Centre entity if found; otherwise null.</returns>
        Task<List<ExamGrade>> GetByFilterAsync(Guid? SyllabusID);

        /// <summary>
        /// Gets queryable for filtering with full hierarchy.
        /// </summary>
        /// <returns>Queryable ExamGrade.</returns>
        IQueryable<ExamGrade> Query();
    }
}
