namespace Winfocus.LMS.Application.Interfaces
{
    using System;
    using System.Collections.Generic;
    using Winfocus.LMS.Domain.Entities;

    /// <summary>
    /// Provides methods for managing <see cref="Country"/> entities in the repository.
    /// </summary>
    public interface ICountryRepository
    {
        /// <summary>
        /// Gets all countries asynchronously.
        /// </summary>
        /// <returns>An enumerable collection of <see cref="Country"/>.</returns>
        Task<IEnumerable<Country>> GetAllAsync();

        /// <summary>
        /// Gets a country by its unique identifier asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the country.</param>
        /// <returns>The <see cref="Country"/> if found; otherwise, null.</returns>
        Task<Country?> GetByIdAsync(Guid id);

        /// <summary>
        /// Adds a new country asynchronously.
        /// </summary>
        /// <param name="country">The country to add.</param>
        /// <returns>The added <see cref="Country"/>.</returns>
        Task<Country> AddAsync(Country country);

        /// <summary>
        /// Updates an existing country asynchronously.
        /// </summary>
        /// <param name="country">The country to update.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous update operation.</returns>
        Task UpdateAsync(Country country);

        /// <summary>
        /// Deletes a country by its unique identifier asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the country to delete.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous delete operation.</returns>
        Task DeleteAsync(Guid id);
    }
}
