using System;
using System.Collections.Generic;
using System.Text;
using Winfocus.LMS.Domain.Entities;

namespace Winfocus.LMS.Application.Interfaces
{
    /// <summary>
    /// Repository contract for managing <see cref="PageHeading"/> entities.
    /// Provides data access methods for retrieving and updating
    /// page heading information used in the admin interface.
    /// </summary>
    public interface IPageHeadingRepository
    {
        /// <summary>
        /// Retrieves all page headings from the data store.
        /// </summary>
        /// <returns>
        /// A list of <see cref="PageHeading"/> entities.
        /// </returns>
        Task<List<PageHeading>> GetAllAsync();

        /// <summary>
        /// Retrieves a specific page heading by its unique page key.
        /// </summary>
        /// <param name="pageKey">
        /// The unique identifier representing the page.
        /// </param>
        /// <returns>
        /// The matching <see cref="PageHeading"/> entity if found; otherwise <c>null</c>.
        /// </returns>
        Task<PageHeading?> GetByPageKeyAsync(string pageKey);

        /// <summary>
        /// Updates an existing page heading in the data store.
        /// </summary>
        /// <param name="pageHeading">
        /// The <see cref="PageHeading"/> entity containing updated heading information.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous update operation.
        /// </returns>
        Task<PageHeading> UpdateAsync(PageHeading pageHeading);
    }
}
