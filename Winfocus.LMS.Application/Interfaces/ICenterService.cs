using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Winfocus.LMS.Application.DTOs;
using Winfocus.LMS.Application.DTOs.Common;
using Winfocus.LMS.Application.DTOs.Masters;
using Winfocus.LMS.Domain.Entities;

namespace Winfocus.LMS.Application.Interfaces
{
    /// <summary>
    /// Defines business operations for <see cref="Centre"/> entities.
    /// </summary>
    public interface ICenterService
    {
        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>CentreDto.</returns>
        Task<CommonResponse<List<CenterDto>>> GetAllAsync();

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>CentreDto.</returns>
        Task<CommonResponse<CenterDto>> GetByIdAsync(Guid id);

        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>CentreDto.</returns>
        Task<CommonResponse<CenterDto>> CreateAsync(CenterRequestDto request);

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="request">The request.</param>
        /// <returns>id.</returns>
        Task<CommonResponse<CenterDto>> UpdateAsync(Guid id, CenterRequestDto request);

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>id.</returns>
        Task<CommonResponse<bool>> DeleteAsync(Guid id);

        /// <summary>
        /// Gets centre by mode of study and state.
        /// </summary>
        /// <param name="countryid">country identifier.</param>
        /// <param name="modeofid">Mode of study identifier.</param>
        /// <param name="stateid">State identifier.</param>
        /// <returns>CentreDto if found; otherwise null.</returns>
        Task<CommonResponse<List<CenterDto>>> GetByFilterAsync(Guid? countryid, Guid? modeofid, Guid? stateid);

        /// <summary>
        /// Gets filtered Center with pagination support.
        /// </summary>
        /// <param name="request">The paged request.</param>
        /// <returns>Paginated Center result.</returns>
        Task<CommonResponse<PagedResult<CenterDto>>> GetFilteredAsync(PagedRequest request);
    }
}
