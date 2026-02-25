namespace Winfocus.LMS.Application.Interfaces
{
    using Winfocus.LMS.Application.DTOs;

    /// <summary>
    /// ICountryService.
    /// </summary>
    public interface ICountryService
    {
        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>CountryDto.</returns>
        Task<CommonResponse<List<CountryDto>>> GetAllAsync();

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>CountryDto.</returns>
        Task<CommonResponse<CountryDto>> GetByIdAsync(Guid id);

        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="userid">The userid.</param>
        /// <returns>CountryDto.</returns>
        Task<CommonResponse<CountryDto>> CreateAsync(CreateCountryRequest request, Guid userid);

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="request">The request.</param>
        /// <param name="userid">The userid.</param>
        /// <returns>id.</returns>
        Task<CommonResponse<CountryDto>> UpdateAsync(Guid id, CreateCountryRequest request, Guid userid);

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>id.</returns>
        Task<CommonResponse<bool>> DeleteAsync(Guid id);

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="name">The identifier.</param>
        /// <returns>CountryDto.</returns>
        Task<bool> ExistsByNameAsync(string name);
    }
}
