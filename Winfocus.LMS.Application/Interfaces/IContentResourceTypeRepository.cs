using System;
using System.Collections.Generic;
using System.Text;
using Winfocus.LMS.Domain.Entities;

namespace Winfocus.LMS.Application.Interfaces
{
    /// <summary>
    /// IContentResourceTypeRepository.
    /// </summary>
    public interface IContentResourceTypeRepository
    {
        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>ContentResourceType list.</returns>
        Task<IReadOnlyList<ContentResourceType>> GetAllAsync();

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ContentResourceType.</returns>
        Task<ContentResourceType?> GetByIdAsync(Guid id);

        /// <summary>
        /// Adds the asynchronous.
        /// </summary>
        /// <param name="contentResourceType">The ContentResourceType.</param>
        /// <returns>ContentResourceType.</returns>
        Task<ContentResourceType> AddAsync(ContentResourceType contentResourceType);

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="contentResourceType">The ContentResourceType.</param>
        /// <returns>task.</returns>
        Task<ContentResourceType> UpdateAsync(ContentResourceType contentResourceType);

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
        /// <returns>bool.</returns>
        Task<bool> ExistsByNameAsync(string name);

        /// <summary>
        /// Gets queryable for filtering with full hierarchy.
        /// </summary>
        /// <returns>Queryable ContentResourceType.</returns>
        IQueryable<ContentResourceType> Query();
    }
}
