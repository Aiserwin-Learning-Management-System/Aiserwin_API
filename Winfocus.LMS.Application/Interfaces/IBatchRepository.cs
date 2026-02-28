using System;
using System.Collections.Generic;
using System.Text;
using Winfocus.LMS.Application.DTOs.Masters;
using Winfocus.LMS.Domain.Entities;

namespace Winfocus.LMS.Application.Interfaces
{
    /// <summary>
    /// Defines business operations for <see cref="Batch"/> entities.
    /// </summary>
    public interface IBatchRepository
    {
        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>Batch.</returns>
        Task<IReadOnlyList<Batch>> GetAllAsync();

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Batch.</returns>
        Task<Batch?> GetByIdAsync(Guid id);

        /// <summary>
        /// Adds the asynchronous.
        /// </summary>
        /// <param name="batch">The batch.</param>
        /// <returns>Batch.</returns>
        Task<Batch> AddAsync(Batch batch);

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="batch">The batch.</param>
        /// <returns>Batch.</returns>
        Task<Batch> UpdateAsync(Batch batch);

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>task.</returns>
        Task<bool> DeleteAsync(Guid id);

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>batches.</returns>
        IQueryable<Batch> Query();
    }
}
