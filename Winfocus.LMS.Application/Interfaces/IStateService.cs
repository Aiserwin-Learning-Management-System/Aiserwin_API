namespace Winfocus.LMS.Application.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Winfocus.LMS.Application.DTOs;
    using Winfocus.LMS.Application.DTOs.Common;
    using Winfocus.LMS.Application.DTOs.Masters;
    using Winfocus.LMS.Domain.Entities;

    /// <summary>
    /// Defines business operations for <see cref="State"/> entities.
    /// </summary>
    public interface IStateService
    {
        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>StateDto.</returns>
        Task<CommonResponse<List<StateDto>>> GetAllAsync();

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="countryId">The countryId.</param>
        /// <returns>StateDto.</returns>
        Task<CommonResponse<StateDto>> GetByIdAsync(Guid id, Guid countryId);

        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>StateDto.</returns>
        Task<CommonResponse<StateDto>> CreateAsync(CreateMasterStateRequest request);

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="request">The request.</param>
        /// <returns>id.</returns>
        Task<CommonResponse<StateDto>> UpdateAsync(Guid id, CreateMasterStateRequest request);

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="countryId">The countryId.</param>
        /// <returns>id.</returns>
        Task<CommonResponse<bool>> DeleteAsync(Guid id, Guid countryId);

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="countryid">The identifier.</param>
        /// <returns>StateDto.</returns>
        Task<CommonResponse<List<StateDto>>> GetByCountryIdAsync(Guid countryid);

        /// <summary>
        /// Gets the records filter.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="countryId">The countryId.</param>
        /// <returns>StateDto.</returns>
        Task<CommonResponse<PagedResult<StateDto>>> GetFilteredAsync(PagedRequest request, Guid countryId);
    }
}
