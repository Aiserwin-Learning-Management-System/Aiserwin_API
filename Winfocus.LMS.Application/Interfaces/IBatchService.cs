using System;
using System.Collections.Generic;
using System.Text;
using Winfocus.LMS.Application.DTOs;
using Winfocus.LMS.Application.DTOs.Masters;

namespace Winfocus.LMS.Application.Interfaces
{
    /// <summary>
    /// Defines business operations for <see cref="Batch"/> entities.
    /// </summary>
    public interface IBatchService
    {
        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>BatchDto.</returns>
        Task<IReadOnlyList<BatchDto>> GetAllAsync();

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>BatchDto.</returns>
        Task<BatchDto?> GetByIdAsync(Guid id);

        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>BatchDto.</returns>
        Task<BatchDto> CreateAsync(BatchRequest request);

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="request">The request.</param>
        /// <returns>id.</returns>
        Task UpdateAsync(Guid id, BatchRequest request);

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>id.</returns>
        Task DeleteAsync(Guid id);

    }
}
