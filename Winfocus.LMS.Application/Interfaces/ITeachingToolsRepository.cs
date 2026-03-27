using System;
using System.Collections.Generic;
using System.Text;
using Winfocus.LMS.Application.DTOs.Teacher;
using Winfocus.LMS.Domain.Entities;

namespace Winfocus.LMS.Application.Interfaces
{
    /// <summary>
    /// Defines business operations for <see cref="TeachingTools"/> entities.
    /// </summary>
    public interface ITeachingToolsRepository
    {
        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>TeachingToolsResponseDto.</returns>
        Task<IReadOnlyList<TeachingTools>> GetAllAsync();

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>TeachingToolsResponseDto.</returns>
        Task<TeachingTools?> GetByIdAsync(Guid id);

        /// <summary>
        /// Adds the asynchronous.
        /// </summary>
        /// <param name="tools">The TeachingTools.</param>
        /// <returns>TeachingToolsResponseDto.</returns>
        Task<TeachingTools> AddAsync(TeachingTools tools);

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="tools">The tools.</param>
        /// <returns>TeachingToolsResponseDto.</returns>
        Task<TeachingTools> UpdateAsync(TeachingTools tools);

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>task.</returns>
        Task<bool> DeleteAsync(Guid id);

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>TeachingToolsResponseDto.</returns>
        IQueryable<TeachingTools> Query();
    }
}
