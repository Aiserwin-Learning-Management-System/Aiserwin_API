using System;
using System.Collections.Generic;
using System.Text;
using Winfocus.LMS.Domain.Entities;

namespace Winfocus.LMS.Application.Interfaces
{
    /// <summary>
    /// Repository interface for managing <see cref="Exam"/> entities.
    /// Provides methods for CRUD operations and retrieving related exam data.
    /// </summary>
    public interface IExamRepository
    {
        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>Exam.</returns>
        Task<IReadOnlyList<Exam>> GetAllAsync();

        /// <summary>
        /// Retrieves an exam by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the exam.</param>
        /// <returns>
        /// A <see cref="Exam"/> object if found; otherwise, <c>null</c>.
        /// </returns>
        Task<Exam?> GetByIdAsync(Guid id);

        /// <summary>
        /// Adds a new exam to the data store.
        /// </summary>
        /// <param name="exam">The exam entity to be added.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task<Exam> AddAsync(Exam exam);

        /// <summary>
        /// Updates an existing exam in the data store.
        /// </summary>
        /// <param name="exam">The exam entity with updated values.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task<Exam> UpdateAsync(Exam exam);

        /// <summary>
        /// Deletes an existing exam from the data store.
        /// </summary>
        /// <param name="id">The exam identifier to be deleted.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task<bool> DeleteAsync(Guid id);

        /// <summary>
        /// Checks whether an exam exists in the data store.
        /// </summary>
        /// <param name="id">The unique identifier of the exam.</param>
        /// <returns>
        /// <c>true</c> if the exam exists; otherwise, <c>false</c>.
        Task<bool> ExistsAsync(Guid id);

        /// <summary>
        /// Gets questions associated with an exam.
        /// </summary>
        /// /// <param name="examId">The unique identifier of the exam.</param>
        /// <returns>.</returns>
        Task<List<ExamQuestion>> GetQuestionsForExamAsync(Guid examId);

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>exams.</returns>
        IQueryable<Exam> Query();
    }
}
