namespace Winfocus.LMS.Application.Interfaces
{
    using Winfocus.LMS.Domain.Entities;

    /// <summary>
    /// ICountryRepository.
    /// </summary>
    public interface ICountryRepository
    {
        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>Country.</returns>
        Task<IReadOnlyList<Country>> GetAllAsync();

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Country.</returns>
        Task<Country?> GetByIdAsync(Guid id);

        /// <summary>
        /// Adds the asynchronous.
        /// </summary>
        /// <param name="country">The country.</param>
        /// <returns>Country.</returns>
        Task<Country> AddAsync(Country country);

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="country">The country.</param>
        /// <returns>Country.</returns>
        Task<Country> UpdateAsync(Country country);

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>task.</returns>
        Task<bool> DeleteAsync(Guid id);

        /// <summary>
        /// Existses the by code asynchronous.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <returns>bool.</returns>
        Task<bool> ExistsByCodeAsync(string code);
    }
}
