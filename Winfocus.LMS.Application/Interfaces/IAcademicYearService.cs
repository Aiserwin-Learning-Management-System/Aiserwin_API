using System;
using System.Collections.Generic;
using System.Text;
using Winfocus.LMS.Application.DTOs;
using Winfocus.LMS.Application.DTOs.Common;
using Winfocus.LMS.Application.DTOs.Masters;
using Winfocus.LMS.Domain.Entities;

namespace Winfocus.LMS.Application.Interfaces
{
    /// <summary>
    /// Defines business operations for <see cref="AcademicYear"/> entities.
    /// </summary>
    public interface IAcademicYearService
    {
        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>AcademicYearDto.</returns>
        Task<CommonResponse<List<AcademicYearDto>>> GetAllAsync();

        /// <summary>
        /// Gets the currently active academic year.
        /// </summary>
        /// <returns>AcademicYearDto.</returns>
        Task<AcademicYearDto?> GetCurrentAcademicYearAsync();

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>AcademicYearDto.</returns>
        Task<CommonResponse<AcademicYearDto>> GetByIdAsync(Guid id);

        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>AcademicYearDto.</returns>
        Task<CommonResponse<AcademicYearDto>> CreateAsync(AcademicYearRequest request);

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="request">The request.</param>
        /// <returns>id.</returns>
        Task<CommonResponse<AcademicYearDto>> UpdateAsync(Guid id, AcademicYearRequest request);

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>id.</returns>
        Task<CommonResponse<bool>> DeleteAsync(Guid id);

        /// <summary>
        /// Gets filtered academic year with pagination support.
        /// </summary>
        /// <param name="request">The paged request.</param>
        /// <returns>Paginated academic year result.</returns>
        Task<CommonResponse<PagedResult<AcademicYearDto>>> GetFilteredAsync(PagedRequest request);

    }
}
