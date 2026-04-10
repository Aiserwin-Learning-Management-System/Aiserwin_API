using System;
using System.Collections.Generic;
using Winfocus.LMS.Domain.Entities;

namespace Winfocus.LMS.Application.Interfaces
{
    /// <summary>
    /// Repository contract for managing <see cref="ExamAccount"/> persistence operations.
    /// </summary>
    public interface IExamAccountRepository
    {
        /// <summary>
        /// Gets all active exam accounts.
        /// </summary>
        /// <returns>Read-only list of <see cref="ExamAccount"/> entities.</returns>
        Task<IReadOnlyList<ExamAccount>> GetAllAsync();

        /// <summary>
        /// Gets an exam account by identifier.
        /// </summary>
        /// <param name="id">The exam account identifier.</param>
        /// <returns>The exam account if found; otherwise <c>null</c>.</returns>
        Task<ExamAccount?> GetByIdAsync(Guid id);

        /// <summary>
        /// Adds a new exam account to persistence.
        /// </summary>
        /// <param name="entity">Entity to add.</param>
        /// <returns>The created entity.</returns>
        Task<ExamAccount> AddAsync(ExamAccount entity);

        /// <summary>
        /// Updates an existing exam account.
        /// </summary>
        /// <param name="entity">Entity with updated values.</param>
        /// <returns>The updated entity.</returns>
        Task<ExamAccount> UpdateAsync(ExamAccount entity);

        /// <summary>
        /// Soft-deletes an exam account.
        /// </summary>
        /// <param name="id">Identifier of the exam account.</param>
        /// <returns>True if deletion succeeded; otherwise false.</returns>
        Task<bool> DeleteAsync(Guid id);

        /// <summary>
        /// Provides a queryable for building filtered queries.
        /// </summary>
        /// <returns>IQueryable of <see cref="ExamAccount"/>.</returns>
        IQueryable<ExamAccount> Query();
    }
}
