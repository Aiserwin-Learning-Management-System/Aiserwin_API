using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Winfocus.LMS.Application.DTOs;
using Winfocus.LMS.Domain.Entities;

namespace Winfocus.LMS.Application.Interfaces
{
    /// <summary>
    /// Defines business operations for <see cref="Centre"/> entities.
    /// </summary>
    public interface ICentreService
    {
        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>CentreDto.</returns>
        Task<CommonResponse<List<CentreDto>>> GetAllAsync();

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>CentreDto.</returns>
        Task<CommonResponse<CentreDto>> GetByIdAsync(Guid id);

        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>CentreDto.</returns>
        Task<CommonResponse<CentreDto>> CreateAsync(CenterRequestDto request);

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="request">The request.</param>
        /// <returns>id.</returns>
        Task<CommonResponse<CentreDto>> UpdateAsync(Guid id, CenterRequestDto request);

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>id.</returns>
        Task<bool> DeleteAsync(Guid id);

        /// <summary>
        /// Gets centre by mode of study and state.
        /// </summary>
        /// <param name="modeofid">Mode of study identifier.</param>
        /// <param name="stateid">State identifier.</param>
        /// <returns>CentreDto if found; otherwise null.</returns>
        Task<CommonResponse<CentreDto>> GetByFilterAsync(Guid modeofid, Guid stateid);
    }
}
