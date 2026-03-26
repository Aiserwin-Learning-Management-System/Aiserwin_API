using System;
using System.Collections.Generic;
using System.Text;
using Winfocus.LMS.Domain.Entities;

namespace Winfocus.LMS.Application.Interfaces
{
    /// <summary>
    /// IExamSyllabusRepository.
    /// </summary>
    public interface IExamSyllabusRepository
    {
        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>ExamSyllabus list.</returns>
        Task<IReadOnlyList<ExamSyllabus>> GetAllAsync();

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="accademicYearId">The accademicYearId.</param>
        /// <returns>ExamSyllabus.</returns>
        Task<ExamSyllabus?> GetByIdAsync(Guid id, Guid accademicYearId);

        /// <summary>
        /// Adds the asynchronous.
        /// </summary>
        /// <param name="examSyllabus">The examSyllabus.</param>
        /// <returns>ExamSyllabus.</returns>
        Task<ExamSyllabus> AddAsync(ExamSyllabus examSyllabus);

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="examSyllabus">The examSyllabus.</param>
        /// <returns>task.</returns>
        Task<ExamSyllabus> UpdateAsync(ExamSyllabus examSyllabus);

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
        /// <param name="academicYearId">The academicYearId.</param>
        /// <returns>bool.</returns>
        Task<bool> ExistsByNameAsync(string name, Guid academicYearId);

        /// <summary>
        /// Gets centre by country, mode of study and state.
        /// </summary>
        /// <param name="academicYearId">academicYear identifier.</param>
        /// <returns>Centre entity if found; otherwise null.</returns>
        Task<List<ExamSyllabus>> GetByFilterAsync(
            Guid? academicYearId);

        /// <summary>
        /// Gets queryable for filtering with full hierarchy.
        /// </summary>
        /// <returns>Queryable ExamSyllabus.</returns>
        IQueryable<ExamSyllabus> Query();

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="yearId">The identifier.</param>
        /// <returns>ExamSyllabus.</returns>
        Task<List<ExamSyllabus>> GetByYearIdAsync(Guid yearId);
    }
}
