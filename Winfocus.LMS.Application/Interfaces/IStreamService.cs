using System;
using System.Collections.Generic;
using System.Text;
using Winfocus.LMS.Application.DTOs;
using Winfocus.LMS.Application.DTOs.Masters;

namespace Winfocus.LMS.Application.Interfaces
{
    /// <summary>
    /// IStreamService.
    /// </summary>
    public interface IStreamService
    {
        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>StreamDto.</returns>
        Task<IReadOnlyList<StreamDto>> GetAllAsync();

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>StreamDto.</returns>
        Task<StreamDto?> GetByIdAsync(Guid id);

        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>StreamDto.</returns>
        Task<StreamDto> CreateAsync(StreamRequest request);

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="request">The request.</param>
        /// <returns>id.</returns>
        Task UpdateAsync(Guid id, StreamRequest request);

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>id.</returns>
        Task DeleteAsync(Guid id);

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="gradeid">The identifier.</param>
        /// <returns>StreamDto.</returns>
        Task<StreamDto?> GetByGradeIdAsync(Guid gradeid);
    }
}
